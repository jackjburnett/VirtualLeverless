using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickBehavior : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public TMP_Text joystickText; // Optional label on joystick
    public string joystickName;
    public float joystickSize = 30f;
    public float fontRatio = 0.5f;
    public SendViaUDP udpSender;
    private Canvas _canvas;
    private Vector2 _inputDirection;
    private bool _isLocked = true; // default locked

    private RectTransform _rectTransform;

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

        SetJoystickSize(joystickSize);
        Lock(false);
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
            var delta = eventData.delta / _canvas.scaleFactor;
            _rectTransform.anchoredPosition += delta;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isLocked)
        {
            UpdateInput(eventData);
            SendUDP();
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
            var x = Mathf.Clamp(localPoint.x / (_rectTransform.sizeDelta.x / 2), -1f, 1f);
            var y = Mathf.Clamp(localPoint.y / (_rectTransform.sizeDelta.y / 2), -1f, 1f);
            _inputDirection = new Vector2(x, y);
        }
    }

    private void SendUDP()
    {
        if (udpSender != null)
        {
            // Send in the format LEFT_JOYSTICK_x_y
            var message = $"{joystickName}_{_inputDirection.x:F2}_{_inputDirection.y:F2}";
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
    public void SetJoystickSize(float size)
    {
        joystickSize = size;
        if (_rectTransform != null)
            _rectTransform.sizeDelta = new Vector2(size, size);
        if (joystickText != null)
            joystickText.fontSize = size * fontRatio;
    }

    // Set joystick type / label
    public void SetJoystickFunction(string functionName)
    {
        joystickName = functionName.ToUpper();

        if (joystickText == null) return;

        switch (joystickName)
        {
            case "LEFT_JOYSTICK":
                joystickText.text = "LJ";
                fontRatio = 0.5f;
                break;
            case "RIGHT_JOYSTICK":
                joystickText.text = "RJ";
                fontRatio = 0.5f;
                break;
            default:
                joystickText.text = joystickName;
                fontRatio = 0.5f;
                break;
        }

        SetJoystickSize(joystickSize);
    }
}