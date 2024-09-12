using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ButtonSpawner : MonoBehaviour
{
    public GameObject buttonPrefab; // Assign your button prefab here
    public Canvas canvas;           // The canvas where you want to place the buttons

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private List<RaycastResult> raycastResults = new List<RaycastResult>();

    void Start()
    {
        // Get the GraphicRaycaster component from the Canvas
        raycaster = canvas.GetComponent<GraphicRaycaster>();
        pointerEventData = new PointerEventData(EventSystem.current);
    }

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
            Vector2 offset = new Vector2(canvasSize.x / 2, canvasSize.y / 2);

            // Apply the offset to the localPoint (Add offset instead of subtracting)
            Vector2 adjustedPosition = localPoint + offset;

            // Set up pointer event data
            pointerEventData.position = clickPosition;
            raycastResults.Clear();
            raycaster.Raycast(pointerEventData, raycastResults);

            bool isUIElementUnderneath = false;
            foreach (var result in raycastResults)
            {
                // Check for Buttons or Panels (or other UI components)
                if (result.gameObject.GetComponent<Button>() != null)
                {
                    isUIElementUnderneath = true;
                    break;
                }
            }

            // Spawn button only if there's no UI element underneath
            if (!isUIElementUnderneath)
            {
                GameObject newButton = Instantiate(buttonPrefab, canvas.transform);
                RectTransform buttonRectTransform = newButton.GetComponent<RectTransform>();
                buttonRectTransform.anchoredPosition = adjustedPosition;
            }
        }
    }
}
