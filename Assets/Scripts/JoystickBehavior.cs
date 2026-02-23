using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickBehavior : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public TMP_Text buttonText;          // Optional label on joystick
    public string joystickFunction;
    public float buttonSize = 30f;
    public float fontRatio = 0.5f;
    public SendViaUDP udpSender;

    private RectTransform _rectTransform;
    private Canvas _canvas;
    private Vector2 _inputDirection;
    private bool _isLocked = true; // default locked

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();

        // Try to find the UDP sender on the EventSystem
        var eventSystem = GameObject.Find("EventSystem");
        if (eventSystem != null)
            udpSender = eventSystem.GetComponent<SendViaUDP>();
        else
            Debug.LogError("EventSystem GameObject not found.");

        SetButtonSize(buttonSize);
        Lock(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isLocked)
        {
            UpdateInput(eventData);
            SendUDP();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isLocked)
        {
            UpdateInput(eventData);
            SendUDP();
        }
        else
        {
            // Move joystick around canvas if unlocked
            Vector2 delta = eventData.delta / _canvas.scaleFactor;
            _rectTransform.anchoredPosition += delta;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isLocked)
        {
            _inputDirection = Vector2.zero;
            SendUDP();
        }
    }

    private void UpdateInput(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            float x = Mathf.Clamp(localPoint.x / (_rectTransform.sizeDelta.x / 2), -1f, 1f);
            float y = Mathf.Clamp(localPoint.y / (_rectTransform.sizeDelta.y / 2), -1f, 1f);
            _inputDirection = new Vector2(x, y);
        }
    }

    private void SendUDP()
    {
        if (udpSender != null)
        {
            string message = $"{joystickFunction}:{_inputDirection.x:F2},{_inputDirection.y:F2}";
            udpSender.SendMessage(message);
        }
    }

    // Lock/unlock joystick for movement
    public void Lock(bool lockState)
    {
        _isLocked = lockState;
    }

    public void ToggleLock()
    {
        _isLocked = !_isLocked;
    }

    // Set joystick prefab size
    public void SetButtonSize(float size)
    {
        buttonSize = size;
        if (_rectTransform != null)
            _rectTransform.sizeDelta = new Vector2(size, size);
        if (buttonText != null)
            buttonText.fontSize = size * fontRatio;
    }

    // Set joystick type / label
    public void SetJoystickFunction(string functionName)
    {
        joystickFunction = functionName.ToUpper();

        if (buttonText == null) return;

        switch (joystickFunction)
        {
            case "LEFT_JOYSTICK":
                buttonText.text = "LJ";
                fontRatio = 0.5f;
                break;
            case "RIGHT_JOYSTICK":
                buttonText.text = "RJ";
                fontRatio = 0.5f;
                break;
            default:
                buttonText.text = joystickFunction;
                fontRatio = 0.8f;
                break;
        }

        SetButtonSize(buttonSize);
    }
}