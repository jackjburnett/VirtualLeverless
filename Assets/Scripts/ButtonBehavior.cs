using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public TMP_Text buttonText;
    public SendViaWebSocket webSocket;
    public ButtonSpawner spawner;

    private string _buttonFunction;
    private float _buttonSize = 30f;
    private RectTransform _buttonTransform;
    private Canvas _canvas;
    private float _fontRatio = 0.8f;
    private float _hapticStrength = 0.5f;
    private bool _isHapticEnabled;
    private bool _isLocked;

    private void Awake()
    {
        // Check if the EventSystem GameObject exists
        var eventSystem = GameObject.Find("EventSystem");
        if (eventSystem != null)
            webSocket = eventSystem.GetComponent<SendViaWebSocket>();
        else
            Debug.LogError("EventSystem GameObject not found.");

        // Get the RectTransform component
        _buttonTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        SetButtonSize(_buttonSize);
        Lock(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isLocked) return;
        // Get the delta movement of the pointer
        var delta = eventData.delta / _canvas.scaleFactor;

        // Move the button to the new position
        _buttonTransform.anchoredPosition += delta;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_isLocked) return;
        // Send a message over WebSocket with the button function name and "_PRESS" appended
        if (webSocket != null)
        {
            webSocket.onSendMessageRequested.Invoke(_buttonFunction + "_PRESS");
            if (_isHapticEnabled) HapticManager.TriggerHaptic(_hapticStrength);
        }
        else
        {
            Debug.LogError("webSocket is null.");
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_isLocked) return;
        // Send a message over WebSocket with the button function name and "_RELEASE" appended
        if (webSocket != null)
            webSocket.onSendMessageRequested.Invoke(_buttonFunction + "_RELEASE");
        else
            Debug.LogError("webSocket is null.");
    }

    public void Lock(bool lockState)
    {
        _isLocked = lockState;
        GetComponent<Button>().interactable = !lockState;
    }

    public void ToggleLock()
    {
        Lock(!_isLocked);
        _isLocked = !_isLocked;
    }

    public bool GetLockState()
    {
        return _isLocked;
    }

    public void SetButtonFunction(string functionName)
    {
        _buttonFunction = functionName;

        // Assign text based on function name
        switch (_buttonFunction.ToUpper())
        {
            case "DPAD_LEFT":
                buttonText.text = "←";
                break;
            case "DPAD_RIGHT":
                buttonText.text = "→";
                break;
            case "DPAD_UP":
                buttonText.text = "↑";
                break;
            case "DPAD_DOWN":
                buttonText.text = "↓";
                break;
            case "LEFT_SHOULDER":
                buttonText.text = "LB";
                _fontRatio = 0.5f;
                break;
            case "RIGHT_SHOULDER":
                buttonText.text = "RB";
                _fontRatio = 0.5f;
                break;
            case "LEFT_TRIGGER":
                buttonText.text = "LT";
                _fontRatio = 0.5f;
                break;
            case "RIGHT_TRIGGER":
                buttonText.text = "RT";
                _fontRatio = 0.5f;
                break;
            case "LEFT_THUMB":
                buttonText.text = "LJ";
                _fontRatio = 0.5f;
                break;
            case "RIGHT_THUMB":
                buttonText.text = "RJ";
                _fontRatio = 0.5f;
                break;
            case "BACK":
                buttonText.text = "<";
                break;
            case "START":
                buttonText.text = "=";
                break;
            default:
                buttonText.text = _buttonFunction;
                break;
        }

        SetButtonSize(_buttonSize);
    }

    public string GetButtonFunction()
    {
        return _buttonFunction;
    }

    public void SetButtonSize(float size)
    {
        if (_buttonTransform != null)
        {
            _buttonSize = size;
            _buttonTransform.sizeDelta = new Vector2(size, size);
            buttonText.fontSize = size * _fontRatio;
        }
        else
        {
            Debug.LogError("Button RectTransform is null.");
        }
    }

    public float GetFontRatio()
    {
        return _fontRatio;
    }

    public float GetButtonSize()
    {
        return _buttonSize;
    }

    public void SetHapticStrength(float strength)
    {
        _hapticStrength = strength;
    }

    public float GetHapticStrength()
    {
        return _hapticStrength;
    }

    public void SetHapticEnabled(bool haptic)
    {
        _isHapticEnabled = haptic;
    }

    public bool GetHapticEnabled()
    {
        return _isHapticEnabled;
    }

    public void DeleteButton()
    {
        spawner.UnregisterButton(this);
        Destroy(gameObject);
    }
}