using UnityEngine;

public class LockAllButtons : MonoBehaviour
{
    public ButtonSpawner buttonSpawner;

    public void LockAll()
    {
        var buttons = buttonSpawner.GetSpawnedButtons();

        if (buttons.Count == 0)
        {
            Debug.LogWarning("No buttons available to lock.");
            return;
        }

        foreach (var button in buttons) button.Lock(true); // Lock all buttons

        Debug.Log("All buttons are now locked.");
    }
}