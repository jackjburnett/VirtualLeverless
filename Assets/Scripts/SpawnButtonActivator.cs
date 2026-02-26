using UnityEngine;

public class SpawnButtonActivator : MonoBehaviour
{
    // Reference to the ButtonSpawner script attached to the panel
    public ButtonSpawner buttonSpawner;

    // Reference to the SendViaUDP script attached to the EventSystem
    public SendViaWebSocket sendViaWebSocket;

    // Reference to the button behavior value
    public string buttonBehaviorValue;

    // Called when the button is clicked
    public void OnButtonClick()
    {
        // Call the SpawnButton method of the ButtonSpawner script
        buttonSpawner.SpawnButton(buttonBehaviorValue, sendViaWebSocket.gameObject);
    }
}