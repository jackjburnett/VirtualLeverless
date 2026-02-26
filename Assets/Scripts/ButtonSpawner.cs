using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonSpawner : MonoBehaviour
{
    public GameObject buttonPrefab; // Assign the prefab of your button in the Unity Editor
    public RectTransform panel; // Assign the panel where you want to spawn the buttons

    private readonly List<ButtonBehavior> _spawnedButtons = new();

    public void SpawnButton(string buttonFunction, GameObject udpServer, float x = 0, float y = 0, float size = 30f)
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
        buttonBehavior.webSocket = udpServer.GetComponent<SendViaWebSocket>();

        // Set the spawner
        buttonBehavior.spawner = this;

        var buttonTransform = button.GetComponent<RectTransform>();
        buttonTransform.anchoredPosition = new Vector2(x, y);
        buttonBehavior.SetButtonSize(size);

        RegisterButton(buttonBehavior); // Store button reference

        Debug.Log($"Button spawned with function: {buttonFunction}");
    }

    private void RegisterButton(ButtonBehavior button)
    {
        if (!_spawnedButtons.Contains(button))
            _spawnedButtons.Add(button);
    }

    public void UnregisterButton(ButtonBehavior button)
    {
        _spawnedButtons.Remove(button);
    }

    public List<ButtonBehavior> GetSpawnedButtons()
    {
        return _spawnedButtons; // Return the list of buttons
    }

    public void ClearAllButtons()
    {
        for (var i = _spawnedButtons.Count - 1; i >= 0; i--) _spawnedButtons[i].DeleteButton();
        _spawnedButtons.Clear();
    }

    public void UpdateAllButtonSizes(float size)
    {
        foreach (var button in _spawnedButtons.Where(button => button != null))
            button.SetButtonSize(size);
    }
}