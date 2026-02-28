using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ThemeColours
{
    private Dictionary<string, Color> _darkThemeColours = new();
    private Dictionary<string, Color> _lightThemeColours = new();

    // TODO: Load from file if present
    public ThemeColours()
    {
        // Light theme Colours
        _lightThemeColours.Add("background", HexToColor("#FBFEF9"));
        _lightThemeColours.Add("text", HexToColor("#D63230"));
        _lightThemeColours.Add("button", HexToColor("#5E6973"));
        _lightThemeColours.Add("border", HexToColor("#000000"));

        // Dark theme Colours
        _darkThemeColours.Add("background", HexToColor("#5E6973"));
        _darkThemeColours.Add("text", HexToColor("#FBFEF9"));
        _darkThemeColours.Add("button", HexToColor("#D63230"));
        _darkThemeColours.Add("border", HexToColor("#000000"));
    }

    private Color HexToColor(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out var color);
        return color;
    }

    public void SetDarkThemeBackground(string colour)
    {
        _darkThemeColours["background"] = HexToColor(colour);
    }

    public void SetDarkThemeText(string colour)
    {
        _darkThemeColours["text"] = HexToColor(colour);
    }

    public void SetDarkThemeButton(string colour)
    {
        _darkThemeColours["button"] = HexToColor(colour);
    }

    public Dictionary<string, Color> GetDarkThemeColours()
    {
        return _darkThemeColours;
    }

    public void SetLightThemeBackground(string colour)
    {
        _lightThemeColours["background"] = HexToColor(colour);
    }

    public void SetLightThemeText(string colour)
    {
        _lightThemeColours["text"] = HexToColor(colour);
    }

    public void SetLightThemeButton(string colour)
    {
        _lightThemeColours["button"] = HexToColor(colour);
    }

    public Dictionary<string, Color> GetLightThemeColours()
    {
        return _lightThemeColours;
    }
}