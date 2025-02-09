using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public TMP_Text buttonText;
    public string buttonFunction;
    public SendViaUDP udpSender;

    private RectTransform _buttonTransform;
    private bool _isLocked;

    private void Awake()
    {
        // Check if the EventSystem GameObject exists
        var eventSystem = GameObject.Find("EventSystem");
        if (eventSystem != null)
            udpSender = eventSystem.GetComponent<SendViaUDP>();
        else
            Debug.LogError("EventSystem GameObject not found.");

        // Get the RectTransform component
        _buttonTransform = GetComponent<RectTransform>();

        // Set the button text to the button function
        buttonText.text = buttonFunction;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isLocked)
        {
            // Move the button to the new position
            var newPosition = eventData.position;

            // Clamp the new position to the bounds of the canvas
            var canvas = GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                var canvasRect = canvas.pixelRect;
                newPosition.x = Mathf.Clamp(newPosition.x, canvasRect.xMin, canvasRect.xMax);
                newPosition.y = Mathf.Clamp(newPosition.y, canvasRect.yMin, canvasRect.yMax);
            }

            _buttonTransform.anchoredPosition = newPosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isLocked)
        {
            // Send a message over UDP with the button function name and "_PRESS" appended
            if (udpSender != null)
                udpSender.SendMessage(buttonFunction + "_PRESS");
            else
                Debug.LogError("udpSender is null.");
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void Lock(bool lockState)
    {
        _isLocked = lockState;
        GetComponent<Button>().interactable = !lockState;

        // Update the button text to reflect the lock state
        buttonText.text = lockState ? "Locked" : buttonFunction;
    }

    public void ToggleLock()
    {
        Lock(!_isLocked);
    }
}