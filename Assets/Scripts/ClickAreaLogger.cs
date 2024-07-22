using UnityEngine;

public class ClickAreaLogger : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        Debug.developerConsoleVisible = true;
        if (!Input.GetMouseButtonDown(0)) return;
        // Get mouse position in screen coordinates
        var mousePosition = Input.mousePosition;

        // Convert screen coordinates to world coordinates (assuming z = 10 units from camera)
        if (!_camera) return;
        var worldPosition = _camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));

        // Convert world coordinates to centimeters (assuming 1 unit = 1 mm)
        var xCentimeters = worldPosition.x * 10; // 1 unit = 1 mm
        var yCentimeters = worldPosition.y * 10; // 1 unit = 1 mm

        // Log the position in centimeters
        Debug.LogError("Click Position (cm): X = " + xCentimeters.ToString("F2") + ", Y = " + yCentimeters.ToString("F2"));
    }
    
}
