using UnityEngine;

public class UnlockAllButtons : MonoBehaviour
{
    public ButtonSpawner buttonSpawner;

    public void UnlockAll()
    {
        var buttons = buttonSpawner.GetSpawnedButtons();

        if (buttons.Count == 0)
        {
            Debug.LogWarning("No buttons available to unlock.");
            return;
        }

        foreach (var button in buttons) button.Lock(false); // Unlock all buttons


        Debug.Log("All buttons are now unlocked.");
    }
}