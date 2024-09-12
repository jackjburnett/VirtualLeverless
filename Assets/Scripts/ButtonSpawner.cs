using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSpawner : MonoBehaviour
{
    public GameObject buttonPrefab; // Assign your button prefab here
    public Canvas canvas; // Single canvas to check
    public Transform buttonManager; // The parent object where buttons should be instantiated
    public bool isLocked; // Lock state for button spawning
    private readonly List<RaycastResult> _raycastResults = new();
    private PointerEventData _pointerEventData;

    private GraphicRaycaster _raycaster;

    private void Start()
    {
        if (canvas == null)
        {
            Debug.LogError("Canvas is not assigned.");
            return;
        }

        // Ensure the canvas has a GraphicRaycaster component
        _raycaster = canvas.GetComponent<GraphicRaycaster>();
        if (_raycaster == null) Debug.LogError("The canvas does not have a GraphicRaycaster component.");

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

        // Convert touch position to the canvas's local space
        var canvasRect = canvas.GetComponent<RectTransform>();
        if (canvasRect == null)
        {
            Debug.LogWarning("Canvas does not have a RectTransform component.");
            return;
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
        _raycaster.Raycast(_pointerEventData, _raycastResults);

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
            return; // Stop if there's a UI element underneath
        }

        // Spawn button only if there's no UI element underneath
        var newButton = Instantiate(buttonPrefab, buttonManager); // Instantiate button as a child of ButtonManager
        var buttonRectTransform = newButton.GetComponent<RectTransform>();
        if (buttonRectTransform == null)
        {
            Debug.LogError("The button prefab does not have a RectTransform component.");
            return;
        }

        // Set the button's position in the local space of the button manager
        buttonRectTransform.anchoredPosition = localPoint;
        Debug.Log($"Button spawned at: {localPoint}");
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