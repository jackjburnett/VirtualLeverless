using UnityEngine;

public class ButtonSpawner : MonoBehaviour
{
    public GameObject buttonPrefab; // Assign the prefab of your button in the Unity Editor
    public RectTransform panel; // Assign the panel where you want to spawn the button

    public void SpawnButton(string buttonFunction, GameObject udpServer)
    {
        // Instantiate the button prefab as a child of the panel
        var button = Instantiate(buttonPrefab, panel);

        // Get the ButtonBehavior component
        var buttonBehavior = button.GetComponent<ButtonBehavior>();

        // Set the button function value
        buttonBehavior.buttonFunction = buttonFunction;

        // Set the udp server game object
        buttonBehavior.udpSender = udpServer.GetComponent<SendViaUDP>();
    }
}