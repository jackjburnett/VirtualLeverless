using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ThemeColours
{
    public Dictionary<string, Color> DarkThemeColours = new();
    public Dictionary<string, Color> LightThemeColours = new();

    public ThemeColours()
    {
        // Light theme Colours
        LightThemeColours.Add("background", HexToColor("#FBFEF9"));
        LightThemeColours.Add("text", HexToColor("#D63230"));
        LightThemeColours.Add("button", HexToColor("#5E6973"));
        LightThemeColours.Add("border", HexToColor("#000000"));

        // Dark theme Colours
        DarkThemeColours.Add("background", HexToColor("#5E6973"));
        DarkThemeColours.Add("text", HexToColor("#FBFEF9"));
        DarkThemeColours.Add("button", HexToColor("#D63230"));
        DarkThemeColours.Add("border", HexToColor("#000000"));
    }

    private Color HexToColor(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out var color);
        return color;
    }
}