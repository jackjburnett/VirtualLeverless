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
public class JoystickData
{
    public string function;
    public float x;
    public float y;
    public float size;
}

[Serializable]
public class ControlsLayout
{
    public List<ButtonData> buttons = new();
    public List<JoystickData> joysticks = new();
}

public class SaveButtonLayout : MonoBehaviour
{
    public ButtonSpawner buttonSpawner;
    public JoystickSpawner joystickSpawner;

    private static string GetMostRecentLayoutFile()
    {
        var directory = Application.persistentDataPath;
        var layoutFiles = Directory.GetFiles(directory, "ControlsLayout_*.json");

        return layoutFiles.Length == 0 ? null : layoutFiles.OrderByDescending(File.GetLastWriteTime).First();
    }

    public void SaveLayout()
    {
        var layout = new ControlsLayout();

        // Save buttons
        var buttons = buttonSpawner.GetSpawnedButtons();
        foreach (var button in buttons)
        {
            var rect = button.GetComponent<RectTransform>();
            layout.buttons.Add(new ButtonData
            {
                function = button.GetButtonFunction(),
                x = rect.anchoredPosition.x,
                y = rect.anchoredPosition.y,
                size = button.GetButtonSize()
            });
        }

        // Save joysticks
        var joysticks = joystickSpawner.GetSpawnedJoysticks();
        foreach (var joystick in joysticks)
        {
            var rect = joystick.GetComponent<RectTransform>();
            layout.joysticks.Add(new JoystickData
            {
                function = joystick.GetJoystickFunction(),
                x = rect.anchoredPosition.x,
                y = rect.anchoredPosition.y,
                size = joystick.GetJoystickSize()
            });
        }

        var json = JsonUtility.ToJson(layout, true);
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        var fileName = $"ControlsLayout_{timestamp}.json";
        var path = Path.Combine(Application.persistentDataPath, fileName);

        File.WriteAllText(path, json);
        Debug.Log($"Controls layout saved to: {path}");
    }

    public void LoadLayout()
    {
        var path = GetMostRecentLayoutFile();

        if (path == null)
        {
            Debug.LogError("No controls layout save files found!");
            return;
        }

        var json = File.ReadAllText(path);
        var layout = JsonUtility.FromJson<ControlsLayout>(json);

        if (layout == null)
        {
            Debug.LogError("Invalid controls layout data!");
            return;
        }

        // Clear existing controls
        buttonSpawner.ClearAllButtons();
        joystickSpawner.ClearAllJoysticks();

        // Spawn buttons
        foreach (var data in layout.buttons)
            buttonSpawner.SpawnButton(data.function, buttonSpawner.gameObject, data.x, data.y, data.size);

        // Spawn joysticks
        foreach (var data in layout.joysticks)
            joystickSpawner.SpawnJoystick(data.function, joystickSpawner.gameObject, data.x, data.y, data.size);

        Debug.Log("Controls layout loaded successfully!");
    }
}