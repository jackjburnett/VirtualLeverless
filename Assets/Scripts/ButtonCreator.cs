using UnityEngine;
using UnityEngine.UI;

public class ButtonSpawner : MonoBehaviour
{
    public GameObject buttonPrefab; // Assign your button prefab here
    public Canvas canvas;           // The canvas where you want to place the buttons

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse click or single touch on mobile
        {
            Vector2 clickPosition = Input.mousePosition;

            // Convert click position to the canvas's local space
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, 
                clickPosition, 
                canvas.worldCamera, 
                out localPoint
            );

            // Calculate the offset based on half the screen width and height
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            Vector2 canvasSize = canvasRect.sizeDelta; // Canvas size in pixels

            // Calculate offset
            Vector2 offset = new Vector2(-canvasSize.x / 2, canvasSize.y / 2);

            // Apply the offset to the localPoint
            Vector2 adjustedPosition = localPoint - offset;

            // Instantiate the button prefab at the adjusted position
            GameObject newButton = Instantiate(buttonPrefab, canvas.transform);

            // Position the button in the canvas using anchoredPosition
            RectTransform buttonRectTransform = newButton.GetComponent<RectTransform>();
            buttonRectTransform.anchoredPosition = adjustedPosition;

            // Optionally, adjust the button size or other properties if needed
        }
    }
}