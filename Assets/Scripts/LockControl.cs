using UnityEngine;
using UnityEngine.UI;

public class LockControl : MonoBehaviour
{
    public Button lockButton;
    public Button unlockButton;
    public Button toggleLockButton;
    public ButtonSpawner buttonSpawner;

    void Start()
    {
        if (lockButton != null)
        {
            lockButton.onClick.AddListener(LockButtons);
        }

        if (unlockButton != null)
        {
            unlockButton.onClick.AddListener(UnlockButtons);
        }

        if (toggleLockButton != null)
        {
            toggleLockButton.onClick.AddListener(ToggleLock);
        }
    }

    void LockButtons()
    {
        if (buttonSpawner != null)
        {
            buttonSpawner.SetLock(true);
        }
    }

    void UnlockButtons()
    {
        if (buttonSpawner != null)
        {
            buttonSpawner.SetLock(false);
        }
    }

    void ToggleLock()
    {
        if (buttonSpawner != null)
        {
            buttonSpawner.ToggleLock();
        }
    }
}