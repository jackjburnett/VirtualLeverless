using TMPro;
using UnityEngine;

public class ResizeButtonHandler : MonoBehaviour
{
    public ButtonSpawner buttonSpawner;
    public TMP_InputField sizeInputField;

    public void OnResizeClicked()
    {
        if (float.TryParse(sizeInputField.text, out var size)) buttonSpawner.UpdateAllButtonSizes(size);
    }
}