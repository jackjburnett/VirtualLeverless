using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
    public ThemeColours themeColours;
    public BackgroundManager backgroundManager;

    public TextMeshProUGUI[] texts;
    public Button[] buttons;
    public TextMeshProUGUI[] buttonTexts;

    private bool _isDarkMode;

    private void Start()
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
        var currentThemeColours =
            _isDarkMode ? themeColours.GetDarkThemeColours() : themeColours.GetLightThemeColours();

        // Update background colour
        if (currentThemeColours.TryGetValue("background", out var backgroundColour))
            backgroundManager.SetBackgroundColour(backgroundColour);

        // Update text colours
        if (currentThemeColours.TryGetValue("text", out var textColour))
            foreach (var text in texts)
                text.color = textColour;

        // Update button colors (example: changing only the normal color)
        if (currentThemeColours.TryGetValue("button", out var buttonColour))
            foreach (var button in buttons)
            {
                var colors = button.colors;
                colors.normalColor = buttonColour;
                colors.pressedColor = textColour;
                colors.selectedColor = textColour;
                // colors.disabledColor = disabledColor;
                colors.highlightedColor = buttonColour;
                button.colors = colors;
            }

        // Update button text colours
        if (!currentThemeColours.TryGetValue("border", out var borderColour)) return;
        foreach (var buttonText in buttonTexts) buttonText.color = borderColour;
    }
}