using TMPro;
using UnityEngine;

public class ResizeJoystickHandler : MonoBehaviour
{
    public JoystickSpawner joystickSpawner; // Assign your JoystickSpawner in the Inspector
    public TMP_InputField sizeInputField; // Assign the input field for size

    public void OnResizeClicked()
    {
        if (float.TryParse(sizeInputField.text, out var size)) joystickSpawner.UpdateAllJoystickSizes(size);
    }
}