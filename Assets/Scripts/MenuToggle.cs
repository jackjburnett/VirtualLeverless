using UnityEngine;
using UnityEngine.UI;

public class MenuToggle : MonoBehaviour
{
    public GameObject deviceSettingsPanel;  // Assign the panel containing menu buttons in the Inspector
    public GameObject controllerMenuPanel;  // Assign the panel containing menu buttons in the Inspector

    private bool _isDeviceVisible;
    private bool _isControllerVisible;

    private void Start()
    {
        // Ensure the menu is hidden initially
        deviceSettingsPanel.SetActive(false);
        controllerMenuPanel.SetActive(false);
    }
}