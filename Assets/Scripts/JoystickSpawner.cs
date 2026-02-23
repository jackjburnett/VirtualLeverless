using System.Collections.Generic;
using UnityEngine;

public class JoystickSpawner : MonoBehaviour
{
    public GameObject joystickPrefab; // Assign the prefab of your joystick
    public RectTransform panel; // Panel to spawn joysticks in

    private readonly List<JoystickBehavior> _spawnedJoysticks = new();

    public void SpawnJoystick(string joystickFunction, GameObject udpServer, float x = 0, float y = 0, float size = 30f)
    {
        if (joystickPrefab == null || panel == null)
        {
            Debug.LogError("JoystickPrefab or Panel is not assigned!");
            return;
        }

        var joystick = Instantiate(joystickPrefab, panel);
        var joystickBehavior = joystick.GetComponent<JoystickBehavior>();
        if (joystickBehavior == null)
        {
            Debug.LogError("Spawned joystick is missing the JoystickBehavior script!");
            return;
        }

        joystickBehavior.SetJoystickFunction(joystickFunction);
        joystickBehavior.udpSender = udpServer.GetComponent<SendViaUDP>();

        var rectTransform = joystick.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(x, y);
        joystickBehavior.SetJoystickSize(size);

        _spawnedJoysticks.Add(joystickBehavior);
        Debug.Log($"Joystick spawned with function: {joystickFunction}");
    }

    public List<JoystickBehavior> GetSpawnedJoysticks()
    {
        return _spawnedJoysticks;
    }

    public void ClearAllJoysticks()
    {
        foreach (var joystick in _spawnedJoysticks)
            Destroy(joystick.gameObject);
        _spawnedJoysticks.Clear();
    }

    public void UpdateAllJoystickSizes(float size)
    {
        foreach (var joystick in _spawnedJoysticks)
            joystick.SetJoystickSize(size);
    }
}