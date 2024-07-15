using UnityEngine;

public class StaticCameraSetup : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        AdjustCamera();
    }

    void AdjustCamera()
    {
        // Ensure the camera is orthographic
        if (mainCamera.orthographic)
        {
            // Calculate the camera size based on the screen height and desired unit size
            float screenHeightInUnits = 10f; // Adjust this based on your game's unit scale
            mainCamera.orthographicSize = screenHeightInUnits / 2;

            // Position the camera so the bottom left corner is at (0, 0)
            float cameraHeight = mainCamera.orthographicSize * 2;
            float cameraWidth = cameraHeight * mainCamera.aspect;

            // Set the camera position to center based on the size and keep the bottom left at (0, 0)
            mainCamera.transform.position = new Vector3(cameraWidth / 2, mainCamera.orthographicSize, -10);
        }
        else
        {
            Debug.LogWarning("This script is designed for orthographic cameras only.");
        }
    }
}
