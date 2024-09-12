using UnityEngine;

public class SetPhysicalSize : MonoBehaviour
{
    public RectTransform targetRectTransform; // The UI element you want to resize
    public float physicalSizeMm = 30.0f; // Desired size in millimeters

    void Start()
    {
        SetSizeInMillimeters(targetRectTransform, physicalSizeMm);
    }

    void SetSizeInMillimeters(RectTransform rectTransform, float sizeMm)
    {
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform is not assigned.");
            return;
        }

        // Get the screen DPI
        float dpi = Screen.dpi;
        if (dpi == 0) // DPI might not be available on all devices
        {
            Debug.LogWarning("Screen DPI is not available. Using a default value of 96 DPI.");
            dpi = 96; // Default DPI value
        }

        // Convert millimeters to inches
        float sizeInches = sizeMm / 25.4f;

        // Calculate size in pixels based on DPI
        float sizePixels = sizeInches * dpi;

        // Convert pixels to Unity units
        // Calculate the canvas scale factor (for scaling UI elements)
        Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
        float canvasScaleFactor = (canvas != null) ? canvas.scaleFactor : 1.0f;

        // Assuming the UI element's reference resolution is the same as its size
        float sizeUnityUnits = sizePixels / (canvasScaleFactor * dpi / 25.4f);

        // Set the size of the RectTransform
        rectTransform.sizeDelta = new Vector2(sizeUnityUnits, sizeUnityUnits);

        Debug.Log($"Set {sizeMm} mm to {sizeUnityUnits} Unity units based on DPI {dpi}.");
    }
}
