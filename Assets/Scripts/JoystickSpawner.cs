using System.Collections.Generic;
using System.Linq;
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
        joystickBehavior.webSocketSender = udpServer.GetComponent<SendViaWebSocket>();

        var rectTransform = joystick.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(x, y);
        joystickBehavior.SetJoystickSize(size);

        RegisterJoystick(joystickBehavior);
        Debug.Log($"Joystick spawned with function: {joystickFunction}");
    }

    private void RegisterJoystick(JoystickBehavior joystick)
    {
        if (!_spawnedJoysticks.Contains(joystick))
            _spawnedJoysticks.Add(joystick);
    }

    public void UnregisterJoystick(JoystickBehavior joystick)
    {
        _spawnedJoysticks.Remove(joystick);
    }

    public List<JoystickBehavior> GetSpawnedJoysticks()
    {
        return _spawnedJoysticks;
    }

    public void ClearAllJoysticks()
    {
        foreach (var joystick in _spawnedJoysticks)
            joystick.DeleteJoystick();
        _spawnedJoysticks.Clear();
    }

    public void UpdateAllJoystickSizes(float size)
    {
        foreach (var joystick in _spawnedJoysticks.Where(joystick => joystick != null))
            joystick.SetJoystickSize(size);
    }
}