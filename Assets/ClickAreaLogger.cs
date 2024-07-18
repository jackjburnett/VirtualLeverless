using UnityEngine;

public class ClickAreaLogger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Get mouse position in screen coordinates
            Vector3 mousePosition = Input.mousePosition;

            // Convert screen coordinates to world coordinates (assuming z = 10 units from camera)
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));

            // Convert world coordinates to centimeters (assuming 1 unit = 1 mm)
            float xCentimeters = worldPosition.x * 10; // 1 unit = 1 mm
            float yCentimeters = worldPosition.y * 10; // 1 unit = 1 mm

            // Log the position in centimeters
            Debug.Log("Click Position (cm): X = " + xCentimeters.ToString("F2") + ", Y = " + yCentimeters.ToString("F2"));
        }
    }
    
}
