using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public GameObject optionsPanel; // Assign the panel to open here
    public bool isLocked; // Lock state for button behavior

    private RectTransform _rectTransform;
    private Canvas _canvas;
    private Vector2 _startPosition;
    private float _pointerDownTime;
    private bool _isDragging;
    private const float HoldThreshold = 0.5f; // Time in seconds to differentiate a hold from a tap

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _startPosition = _rectTransform.anchoredPosition;

        // Ensure the panel starts hidden
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isLocked) return; // Exit early if the button is locked

        _pointerDownTime = Time.time;
        _isDragging = false; // Reset dragging flag
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isLocked) return; // Exit early if the button is locked

        _isDragging = true; // Set dragging flag when drag starts

        // Move the button
        if (_canvas != null)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                eventData.position,
                _canvas.worldCamera,
                out localPoint
            );

            _rectTransform.anchoredPosition = localPoint;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isLocked) return; // Exit early if the button is locked

        float timeHeld = Time.time - _pointerDownTime;

        if (!_isDragging && timeHeld < HoldThreshold)
        {
            // Toggle options panel only if it is a tap
            if (optionsPanel != null)
            {
                optionsPanel.SetActive(!optionsPanel.activeSelf);
            }
        }

        // Optional: Snap back to start position or handle release logic
        // _rectTransform.anchoredPosition = _startPosition;
    }
}