using UnityEngine;

public class LockAllJoysticks : MonoBehaviour
{
    public JoystickSpawner joystickSpawner;

    public void LockAll()
    {
        var joysticks = joystickSpawner.GetSpawnedJoysticks();

        if (joysticks.Count == 0)
        {
            Debug.LogWarning("No joysticks available to lock.");
            return;
        }

        foreach (var joystick in joysticks) joystick.Lock(true); // Lock all joysticks

        Debug.Log("All joysticks are now locked.");
    }
}