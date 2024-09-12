using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSpawner : MonoBehaviour
{
    public GameObject buttonPrefab; // Assign your button prefab here
    public Canvas[] canvases; // Array of canvases to check
    public bool isLocked; // Lock state for button spawning
    private PointerEventData _pointerEventData;

    private GraphicRaycaster[] _raycasters;
    private readonly List<RaycastResult> _raycastResults = new();

    private void Start()
    {
        // Ensure each canvas has a GraphicRaycaster component
        _raycasters = new GraphicRaycaster[canvases.Length];
        for (var i = 0; i < canvases.Length; i++)
        {
            _raycasters[i] = canvases[i].GetComponent<GraphicRaycaster>();
            if (_raycasters[i] == null) Debug.LogError($"Canvas {i} does not have a GraphicRaycaster component.");
        }

        _pointerEventData = new PointerEventData(EventSystem.current);
    }

    private void Update()
    {
        // Check the lock before proceeding
        if (isLocked)
        {
            Debug.Log("Button spawning is currently locked.");
            return;
        }

        // Handle multiple touches
        if (Input.touchCount > 0)
        {
            // Process the first touch (or any custom touch selection logic)
            var touch = Input.GetTouch(0);
            var touchPosition = touch.position;
            HandleTouch(touchPosition);
        }
        else if (Input.GetMouseButtonDown(0)) // Detect left mouse click or single touch on mobile
        {
            Vector2 clickPosition = Input.mousePosition;
            HandleTouch(clickPosition);
        }
    }

    private void HandleTouch(Vector2 position)
    {
        var isUIElementUnderneath = false;

        // Check each canvas
        foreach (var canvas in canvases)
        {
            var canvasRect = canvas.GetComponent<RectTransform>();
            if (canvasRect == null)
            {
                Debug.LogWarning("Canvas does not have a RectTransform component.");
                continue;
            }

            // Convert touch position to the canvas's local space
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                position,
                canvas.worldCamera,
                out var localPoint
            );

            // Set up pointer event data
            _pointerEventData.position = position;
            _raycastResults.Clear();
            _raycasters[Array.IndexOf(canvases, canvas)].Raycast(_pointerEventData, _raycastResults);

            foreach (var result in _raycastResults)
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

            // Spawn button only if there's no UI element underneath
            // Assuming you want to spawn on the first canvas
            var targetCanvas = canvases[0];
            if (targetCanvas == null)
            {
                Debug.LogWarning("The target canvas is null.");
                return;
            }

            var newButton = Instantiate(buttonPrefab, targetCanvas.transform);
            var buttonRectTransform = newButton.GetComponent<RectTransform>();
            if (buttonRectTransform == null)
            {
                Debug.LogError("The button prefab does not have a RectTransform component.");
                return;
            }

            // Directly use the localPoint as the button position
            buttonRectTransform.anchoredPosition = localPoint;

            Debug.Log($"Button spawned at: {localPoint}");
            break; // Exit after spawning the button
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