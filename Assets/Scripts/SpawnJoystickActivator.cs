using UnityEngine;
using UnityEngine.Serialization;

public class SpawnJoystickActivator : MonoBehaviour
{
    // Reference to the JoystickSpawner script attached to the panel
    public JoystickSpawner joystickSpawner;

    // Reference to the SendViaUDP script attached to the EventSystem
    [FormerlySerializedAs("sendViaUDP")] public SendViaWebSocket sendViaWebSocket;

    // The joystick function value (e.g., LEFT_JOYSTICK or RIGHT_JOYSTICK)
    public string joystickBehaviorValue;

    // Called when the UI button is clicked
    public void OnButtonClick()
    {
        if (joystickSpawner != null && sendViaWebSocket != null)
            // Spawn joystick at default position (0,0)
            joystickSpawner.SpawnJoystick(joystickBehaviorValue, sendViaWebSocket.gameObject);
        else
            Debug.LogError("JoystickSpawner or SendViaUDP reference is missing!");
    }
}