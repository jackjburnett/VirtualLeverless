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
    private Canvas _canvas;
    private bool _isLocked = true;

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

        _canvas = GetComponentInParent<Canvas>();
        Lock(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isLocked)
        {
            // Get the delta movement of the pointer
            var delta = eventData.delta / _canvas.scaleFactor;

            // Move the button to the new position
            _buttonTransform.anchoredPosition += delta;
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
        if (_isLocked) udpSender.SendMessage("A_RELEASE");
    }

    public void Lock(bool lockState)
    {
        _isLocked = lockState;
        GetComponent<Button>().interactable = !lockState;
    }

    public void ToggleLock()
    {
        Lock(!_isLocked);
    }
}