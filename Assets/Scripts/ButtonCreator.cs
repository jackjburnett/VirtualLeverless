using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ButtonSpawner : MonoBehaviour
{
    public GameObject buttonPrefab; // Assign your button prefab here
    public Canvas[] canvases;        // Array of canvases to check

    private GraphicRaycaster[] raycasters;
    private PointerEventData pointerEventData;
    private List<RaycastResult> raycastResults = new List<RaycastResult>();

    void Start()
    {
        // Ensure each canvas has a GraphicRaycaster component
        raycasters = new GraphicRaycaster[canvases.Length];
        for (int i = 0; i < canvases.Length; i++)
        {
            raycasters[i] = canvases[i].GetComponent<GraphicRaycaster>();
            if (raycasters[i] == null)
            {
                Debug.LogError($"Canvas {i} does not have a GraphicRaycaster component.");
            }
        }
        pointerEventData = new PointerEventData(EventSystem.current);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse click or single touch on mobile
        {
            Vector2 clickPosition = Input.mousePosition;
            Vector2 adjustedPosition = Vector2.zero;
            bool isUIElementUnderneath = false;

            // Check each canvas
            foreach (var canvas in canvases)
            {
                RectTransform canvasRect = canvas.GetComponent<RectTransform>();
                Vector2 canvasSize = canvasRect.sizeDelta; // Canvas size in pixels

                // Convert click position to the canvas's local space
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect,
                    clickPosition,
                    canvas.worldCamera,
                    out localPoint
                );

                // Calculate the offset based on the canvas size
                Vector2 offset = new Vector2(canvasSize.x / 2, -canvasSize.y / 2);

                // Apply the offset to the localPoint
                adjustedPosition = localPoint + offset;

                // Debugging
                Debug.Log($"Click Position: {clickPosition}");
                Debug.Log($"Local Point: {localPoint}");
                Debug.Log($"Canvas Size: {canvasSize}");
                Debug.Log($"Offset: {offset}");
                Debug.Log($"Adjusted Position: {adjustedPosition}");

                // Set up pointer event data
                pointerEventData.position = clickPosition;
                raycastResults.Clear();
                raycasters[System.Array.IndexOf(canvases, canvas)].Raycast(pointerEventData, raycastResults);

                foreach (var result in raycastResults)
                {
                    // Check for any UI element, including Buttons and Panels
                    if (result.gameObject.GetComponent<Graphic>() != null)
                    {
                        isUIElementUnderneath = true;
                        break;
                    }
                }

                if (isUIElementUnderneath)
                {
                    break;
                }
            }

            // Spawn button only if there's no UI element underneath
            if (!isUIElementUnderneath)
            {
                // Assuming you want to spawn on the first canvas
                Canvas targetCanvas = canvases[0];
                GameObject newButton = Instantiate(buttonPrefab, targetCanvas.transform);
                RectTransform buttonRectTransform = newButton.GetComponent<RectTransform>();
                // Apply the adjusted position to the button
                buttonRectTransform.anchoredPosition = adjustedPosition;
            }
        }
    }
}
