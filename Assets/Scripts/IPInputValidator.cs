using UnityEngine;
using TMPro; // Required for working with TMP_InputValidator
using System.Text.RegularExpressions;

[CreateAssetMenu(fileName = "IPInputValidator", menuName = "Input Validators/IP Address Validator")]
public class IPInputValidator : TMP_InputValidator
{
    // The regex pattern for a valid IPv4 address
    private readonly string _ipPattern =
        @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){0,3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)?$";

    public override char Validate(ref string text, ref int pos, char ch)
    {
        // Concatenate the current text with the new character
        string newText = text.Insert(pos, ch.ToString());

        // Check if the new text matches the partial IP pattern
        if (Regex.IsMatch(newText, _ipPattern))
        {
            // Update the text and position only if it's valid
            text = newText;
            pos++;
            return ch;
        }

        // If the character doesn't match the IP address format, reject it
        return '\0';
    }
}