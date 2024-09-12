using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ButtonSpawner : MonoBehaviour
{
    public GameObject buttonPrefab; // Assign your button prefab here
    public Canvas[] canvases;        // Array of canvases to check
    public bool isLocked = false;   // Lock state for button spawning
    
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
        // Check the lock before proceeding
        if (isLocked)
        {
            Debug.Log("Button spawning is currently locked.");
            return;
        }
        
        Vector2 clickPosition = Input.mousePosition;
        Vector2 localPoint = Vector2.zero;
        bool isUIElementUnderneath = false;

        // Check each canvas
        foreach (var canvas in canvases)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            if (canvasRect == null)
            {
                Debug.LogWarning("Canvas does not have a RectTransform component.");
                continue;
            }

            // Convert click position to the canvas's local space
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                clickPosition,
                canvas.worldCamera,
                out localPoint
            );

            // Set up pointer event data
            pointerEventData.position = clickPosition;
            raycastResults.Clear();
            raycasters[System.Array.IndexOf(canvases, canvas)].Raycast(pointerEventData, raycastResults);

            Debug.Log($"Checking canvas: {canvas.name}");
            foreach (var result in raycastResults)
            {
                Debug.Log($"Raycast hit: {result.gameObject.name}");
                // Check for any UI element, including Buttons and Panels
                if (result.gameObject.GetComponent<Graphic>() != null)
                {
                    isUIElementUnderneath = true;
                    break;
                }
            }

            if (isUIElementUnderneath)
            {
                Debug.Log("UI element found under the cursor.");
                break;
            }
        }

        // Spawn button only if there's no UI element underneath
        if (!isUIElementUnderneath)
        {
            // Assuming you want to spawn on the first canvas
            Canvas targetCanvas = canvases[0];
            if (targetCanvas == null)
            {
                Debug.LogWarning("The target canvas is null.");
                return;
            }

            GameObject newButton = Instantiate(buttonPrefab, targetCanvas.transform);
            RectTransform buttonRectTransform = newButton.GetComponent<RectTransform>();
            if (buttonRectTransform == null)
            {
                Debug.LogError("The button prefab does not have a RectTransform component.");
                return;
            }

            // Directly use the localPoint as the button position
            buttonRectTransform.anchoredPosition = localPoint;

            Debug.Log($"Button spawned at: {localPoint}");
        }
    }
}


    
    // Method to set the lock state
    public void SetLock(bool lockState)
    {
        isLocked = lockState;
    }

    // Optionally, you might want to toggle the lock state
    public void ToggleLock()
    {
        isLocked = !isLocked;
    }
}
