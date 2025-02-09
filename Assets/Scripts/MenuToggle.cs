using UnityEngine;

public class MenuToggle : MonoBehaviour
{
    public GameObject deviceSettingsPanel; // Assign the panel containing menu buttons in the Inspector
    public GameObject controllerMenuPanel; // Assign the panel containing menu buttons in the Inspector
    public GameObject buttonMenuPanel; // Assign the panel containing menu buttons in the Inspector
    private bool _isButtonVisible;
    private bool _isControllerVisible;
    private bool _isDeviceVisible;

    private void Start()
    {
        // Ensure the menus are hidden initially
        deviceSettingsPanel.SetActive(false);
        controllerMenuPanel.SetActive(false);
        buttonMenuPanel.SetActive(false);
        _isDeviceVisible = false;
        _isControllerVisible = false;
        _isButtonVisible = false;
    }

    public void ToggleDevices()
    {
        if (!_isDeviceVisible)
        {
            deviceSettingsPanel.SetActive(true);
            _isDeviceVisible = true;
        }
        else
        {
            deviceSettingsPanel.SetActive(false);
            _isDeviceVisible = false;
        }
    }

    public void ToggleController()
    {
        if (!_isControllerVisible)
        {
            controllerMenuPanel.SetActive(true);
            _isControllerVisible = true;
        }
        else
        {
            controllerMenuPanel.SetActive(false);
            _isControllerVisible = false;
        }
    }

    public void ToggleButtons()
    {
        if (!_isButtonVisible)
        {
            buttonMenuPanel.SetActive(true);
            _isButtonVisible = true;
        }
        else
        {
            buttonMenuPanel.SetActive(false);
            _isButtonVisible = false;
        }
    }

    public void CloseAll()
    {
        deviceSettingsPanel.SetActive(false);
        controllerMenuPanel.SetActive(false);
        buttonMenuPanel.SetActive(false);
        _isDeviceVisible = false;
        _isControllerVisible = false;
        _isButtonVisible = false;
    }
}