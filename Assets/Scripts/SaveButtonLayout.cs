using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class ButtonData
{
    public string function;
    public float x;
    public float y;
    public float size;
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

    private string GetMostRecentLayoutFile()
    {
        var directory = Application.persistentDataPath;
        var layoutFiles = Directory.GetFiles(directory, "ButtonLayout_*.json");
        
        if (layoutFiles.Length == 0)
            return null;
            
        return layoutFiles.OrderByDescending(File.GetLastWriteTime).First();
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
                y = localPosition.y,
                size = button.buttonSize
            });
        }

        var json = JsonUtility.ToJson(layout, true); // Pretty print
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        var fileName = $"ButtonLayout_{timestamp}.json";
        var path = Path.Combine(Application.persistentDataPath, fileName);

        File.WriteAllText(path, json);

        Debug.Log($"Layout saved to: {path}");
    }

    public void LoadLayout()
    {
        var path = GetMostRecentLayoutFile();

        if (path == null)
        {
            Debug.LogError("No layout save files found!");
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
            buttonSpawner.SpawnButton(buttonData.function, buttonSpawner.gameObject, buttonData.x, buttonData.y, buttonData.size);

        Debug.Log("Layout loaded successfully!");
    }
}