using UnityEngine;

public class UnlockAllJoysticks : MonoBehaviour
{
    public JoystickSpawner joystickSpawner;

    public void UnlockAll()
    {
        var joysticks = joystickSpawner.GetSpawnedJoysticks();

        if (joysticks.Count == 0)
        {
            Debug.LogWarning("No joysticks available to unlock.");
            return;
        }

        foreach (var joystick in joysticks) joystick.Lock(false); // Unlock all joysticks

        Debug.Log("All joysticks are now unlocked.");
    }
}