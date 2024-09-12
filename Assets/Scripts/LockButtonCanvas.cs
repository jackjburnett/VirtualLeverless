using UnityEngine;
using UnityEngine.UI;

public class LockButtonCanvas : MonoBehaviour
{
    public Button lockButton;
    public Button unlockButton;
    public Button toggleLockButton;
    public ButtonSpawner buttonSpawner;

    private void Start()
    {
        if (lockButton != null) lockButton.onClick.AddListener(LockButtons);

        if (unlockButton != null) unlockButton.onClick.AddListener(UnlockButtons);

        if (toggleLockButton != null) toggleLockButton.onClick.AddListener(ToggleLock);
    }

    private void LockButtons()
    {
        if (buttonSpawner != null) buttonSpawner.SetLock(true);
    }

    private void UnlockButtons()
    {
        if (buttonSpawner != null) buttonSpawner.SetLock(false);
    }

    private void ToggleLock()
    {
        if (buttonSpawner != null) buttonSpawner.ToggleLock();
    }
}