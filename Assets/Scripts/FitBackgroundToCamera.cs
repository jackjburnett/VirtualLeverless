using UnityEngine;

public class FitBackgroundToCamera : MonoBehaviour
{
    void Start()
    {
        Camera camera = Camera.main;
        if (camera.orthographic)
        {
            // Calculate the sprite size based on the camera's orthographic size and aspect ratio
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                float cameraHeight = 2f * camera.orthographicSize;
                float cameraWidth = cameraHeight * camera.aspect;

                // Set the sprite's size to match the camera's view
                Vector2 spriteSize = new Vector2(cameraWidth, cameraHeight);
                transform.localScale = new Vector3(spriteSize.x / spriteRenderer.bounds.size.x, spriteSize.y / spriteRenderer.bounds.size.y, 1f);
            }
        }
    }
}