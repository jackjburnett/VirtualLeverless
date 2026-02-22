using System.Collections.Generic;
using UnityEngine;

public class ButtonSpawner : MonoBehaviour
{
    public GameObject buttonPrefab; // Assign the prefab of your button in the Unity Editor
    public RectTransform panel; // Assign the panel where you want to spawn the buttons

    private readonly List<ButtonBehavior> _spawnedButtons = new();

    public void SpawnButton(string buttonFunction, GameObject udpServer, float x = 0, float y = 0)
    {
        if (buttonPrefab == null || panel == null)
        {
            Debug.LogError("ButtonPrefab or Panel is not assigned!");
            return;
        }

        // Instantiate the button prefab as a child of the panel
        var button = Instantiate(buttonPrefab, panel);

        // Get the ButtonBehavior component
        var buttonBehavior = button.GetComponent<ButtonBehavior>();
        if (buttonBehavior == null)
        {
            Debug.LogError("Spawned button is missing the ButtonBehavior script!");
            return;
        }

        // Set the button function value
        buttonBehavior.SetButtonFunction(buttonFunction);

        // Set the udp server game object
        buttonBehavior.udpSender = udpServer.GetComponent<SendViaUDP>();

        var buttonTransform = button.GetComponent<RectTransform>();
        buttonTransform.anchoredPosition = new Vector2(x, y); // ✅ Set position

        _spawnedButtons.Add(buttonBehavior); // Store button reference

        Debug.Log($"Button spawned with function: {buttonFunction}");
    }

    public List<ButtonBehavior> GetSpawnedButtons()
    {
        return _spawnedButtons; // Return the list of buttons
    }

    public void ClearAllButtons()
    {
        foreach (var button in _spawnedButtons) Destroy(button.gameObject);
        _spawnedButtons.Clear();
    }

    public void UpdateAllButtonSizes(float size)
    {
        foreach (var button in _spawnedButtons)
            if (button != null)
                button.SetButtonSize(size);
    }
}