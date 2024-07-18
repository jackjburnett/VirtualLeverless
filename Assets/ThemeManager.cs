using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ThemeManager : MonoBehaviour
{
    public ThemeColours themeColours;
    public Image background;
    public TextMeshProUGUI[] texts;
    public Button[] buttons;

    private bool _isDarkMode;

    void Start()
    {
        // Initialize the theme based on saved preferences
        // Assuming PlayerPrefs is used to save the user's preference
        _isDarkMode = PlayerPrefs.GetInt("DarkMode", 0) == 1;
        UpdateTheme();
    }

    public void ToggleTheme()
    {
        _isDarkMode = !_isDarkMode;
        PlayerPrefs.SetInt("DarkMode", _isDarkMode ? 1 : 0);
        UpdateTheme();
    }

    private void UpdateTheme()
    {
        Dictionary<string, Color> currentThemeColours = _isDarkMode ? themeColours.DarkThemeColours : themeColours.LightThemeColours;

        // Update background colour
        if (currentThemeColours.TryGetValue("background", out var backgroundColour))
        {
            background.color = backgroundColour;
        }

        // Update text colours
        foreach (TextMeshProUGUI text in texts)
        {
            if (currentThemeColours.TryGetValue("text", out var textColour))
            {
                text.color = textColour;
            }
        }

        // Update button colors (example: changing only the normal color)
        foreach (Button button in buttons)
        {
            if (currentThemeColours.TryGetValue("button", out var buttonColour))
            {
                ColorBlock colors = button.colors;
                colors.normalColor = buttonColour;
                button.colors = colors;
            }
        }

        // You can extend this to update other UI elements as needed
    }
}
