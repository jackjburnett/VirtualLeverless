using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ButtonBehavior : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public GameObject optionsPanel; // Assign the panel to open here
    public bool isLocked; // Lock state for button behavior
    public TMP_Text buttonText;
    public string buttonFunction = "?";
    public SendViaUDP udpSender;

    private RectTransform _rectTransform;
    private Canvas _canvas;
    private Vector2 _startPosition;
    private float _pointerDownTime;
    private bool _isDragging;
    private const float HoldThreshold = 0.5f; // Time in seconds to differentiate a hold from a tap

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = FindObjectOfType<Canvas>(); // Find the Canvas in the scene

        if (_canvas == null)
        {
            Debug.LogError("No Canvas found in the scene.");
        }
        else
        {
            _startPosition = _rectTransform.anchoredPosition;
        }

        // Ensure the panel starts hidden
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isLocked) {
            udpSender.SendMessage(buttonFunction+"_PRESS"); 
        }else{
            _pointerDownTime = Time.time;
            _isDragging = false; // Reset dragging flag
        }
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
                _canvas.GetComponent<RectTransform>(), // Use Canvas RectTransform
                eventData.position,
                _canvas.worldCamera,
                out localPoint
            );

            _rectTransform.anchoredPosition = localPoint;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isLocked){
            udpSender.SendMessage(buttonFunction+"_RELEASE");
        }
        else{
            float timeHeld = Time.time - _pointerDownTime;
            if (!_isDragging && timeHeld < HoldThreshold)
            {
                // Toggle options panel only if it is a tap
                if (optionsPanel != null){
                    optionsPanel.SetActive(!optionsPanel.activeSelf);
                }
            }
        }
            
        // Optional: Snap back to start position or handle release logic
        // _rectTransform.anchoredPosition = _startPosition;
    }
     
     // Method to lock or unlock the button 
    public void Lock(bool lockState)
    {
        isLocked = lockState;
        // Optionally, you could also visually indicate the lock state here
        // For example, you might change the button color or disable its interactions
    }
}
