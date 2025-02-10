using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ButtonData
{
    public string function;
    public float x;
    public float y;
}

[Serializable]
public class ButtonLayout
{
    public List<ButtonData> buttons = new();
}

public class SaveButtonLayout : MonoBehaviour
{
    public ButtonSpawner buttonSpawner;
    public RectTransform panel; // Assign the panel in the inspector

    private string GetFilePath()
    {
        return Path.Combine(Application.persistentDataPath, "buttonLayout.json");
    }

    public void SaveLayout()
    {
        var buttons = buttonSpawner.GetSpawnedButtons();

        if (buttons.Count == 0)
        {
            Debug.LogWarning("No buttons available to save.");
            return;
        }

        var layout = new ButtonLayout();

        foreach (var button in buttons)
        {
            var buttonTransform = button.GetComponent<RectTransform>();

            // Convert position to local space relative to the panel
            var localPosition = buttonTransform.anchoredPosition;

            layout.buttons.Add(new ButtonData
            {
                function = button.buttonFunction,
                x = localPosition.x,
                y = localPosition.y
            });
        }

        var json = JsonUtility.ToJson(layout, true); // Pretty print
        var path = Path.Combine(Application.persistentDataPath, "buttonLayout.json");

        File.WriteAllText(path, json);

        Debug.Log($"Layout saved to: {path}");
    }

    public void LoadLayout()
    {
        var path = GetFilePath();

        if (!File.Exists(path))
        {
            Debug.LogError("Save file not found!");
            return;
        }

        var json = File.ReadAllText(path);
        var layout = JsonUtility.FromJson<ButtonLayout>(json);

        if (layout == null || layout.buttons.Count == 0)
        {
            Debug.LogError("Invalid or empty layout data!");
            return;
        }

        // Clear existing buttons before loading new ones
        buttonSpawner.ClearAllButtons();

        foreach (var buttonData in layout.buttons)
            // ✅ Now we pass X, Y position directly
            buttonSpawner.SpawnButton(buttonData.function, buttonSpawner.gameObject, buttonData.x, buttonData.y);

        Debug.Log("Layout loaded successfully!");
    }
}