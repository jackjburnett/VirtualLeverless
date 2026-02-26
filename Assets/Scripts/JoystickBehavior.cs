using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickBehavior : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public TMP_Text joystickText;
    public SendViaWebSocket webSocketSender;
    public JoystickSpawner spawner;

    private Canvas _canvas;
    private float _fontRatio = 0.5f;
    private Vector2 _inputDirection;
    private bool _isLocked;
    private string _joystickFunction;
    private float _joystickSize = 30f;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();

        // Try to find the UDP sender on the EventSystem
        var eventSystem = GameObject.Find("EventSystem");
        if (eventSystem != null)
            webSocketSender = eventSystem.GetComponent<SendViaWebSocket>();
        else
            Debug.LogError("EventSystem GameObject not found.");

        SetJoystickSize(_joystickSize);
        Lock(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isLocked)
        {
            UpdateInput(eventData);
            SendWebSocket();
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
        if (!_isLocked) return;
        UpdateInput(eventData);
        SendWebSocket();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isLocked)
        {
            _inputDirection = Vector2.zero;
            SendWebSocket();
        }
    }

    private void UpdateInput(PointerEventData eventData)
    {
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform, eventData.position, eventData.pressEventCamera, out var localPoint)) return;
        var x = Mathf.Clamp(localPoint.x / (_rectTransform.sizeDelta.x / 2), -1f, 1f);
        var y = Mathf.Clamp(localPoint.y / (_rectTransform.sizeDelta.y / 2), -1f, 1f);
        _inputDirection = new Vector2(x, y);
    }

    private void SendWebSocket()
    {
        if (webSocketSender == null) return;
        // Send in the format LEFT/RIGHT_JOYSTICK_x_y
        var message = $"{_joystickFunction}_{_inputDirection.x:F2}_{_inputDirection.y:F2}";
        webSocketSender.onSendMessageRequested.Invoke(message);
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

    public bool GetLockState()
    {
        return _isLocked;
    }

    // Set joystick prefab size
    public void SetJoystickSize(float size)
    {
        _joystickSize = size;
        if (_rectTransform != null)
            _rectTransform.sizeDelta = new Vector2(size, size);
        if (joystickText != null)
            joystickText.fontSize = size * _fontRatio;
    }

    public float GetJoystickSize()
    {
        return _joystickSize;
    }

    public void SetFontRatio(float ratio)
    {
        _fontRatio = ratio;
    }

    public float GetFontRatio()
    {
        return _fontRatio;
    }

    // Set joystick type / label
    public void SetJoystickFunction(string functionName)
    {
        _joystickFunction = functionName.ToUpper();

        if (joystickText == null) return;

        joystickText.text = _joystickFunction switch
        {
            "LEFT_JOYSTICK" => "LJ",
            "RIGHT_JOYSTICK" => "RJ",
            _ => _joystickFunction
        };

        _fontRatio = 0.5f;

        SetJoystickSize(_joystickSize);
    }

    public string GetJoystickFunction()
    {
        return _joystickFunction;
    }

    public void DeleteJoystick()
    {
        spawner.UnregisterJoystick(this);
        Destroy(gameObject);
    }
}