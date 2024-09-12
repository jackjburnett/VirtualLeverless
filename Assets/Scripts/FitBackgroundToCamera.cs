using UnityEngine;

public class FitBackgroundToCamera : MonoBehaviour
{
    private void Start()
    {
        var camera = Camera.main;
        if (camera != null && camera.orthographic)
        {
            // Calculate the sprite size based on the camera's orthographic size and aspect ratio
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                var cameraHeight = 2f * camera.orthographicSize;
                var cameraWidth = cameraHeight * camera.aspect;

                // Set the sprite's size to match the camera's view
                var spriteSize = new Vector2(cameraWidth, cameraHeight);
                transform.localScale = new Vector3(spriteSize.x / spriteRenderer.bounds.size.x,
                    spriteSize.y / spriteRenderer.bounds.size.y, 1f);
            }
        }
    }
}