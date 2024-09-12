using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Text;

public class SendViaUDP : MonoBehaviour
{
    public Button sendButton;
    private UdpClient _udpClient;
    private string _serverIP = "192.168.0.17"; // Replace with your PC's IP address
    private int _port = 8080; // _port number for UDP communication

    void Start()
    {
        // Create UdpClient
        _udpClient = new UdpClient();
        
        // Add listener to the button
        sendButton.onClick.AddListener(SendMessageToPC);
    }

    void SendMessageToPC()
    {
        try
        {
            // Create message
            string message = "SPACEBAR";
            byte[] data = Encoding.ASCII.GetBytes(message);

            // Send message to the server
            _udpClient.Send(data, data.Length, _serverIP, _port);
            Debug.Log("Sent message to PC: " + message);
        }
        catch (SocketException e)
        {
            Debug.Log("SocketException: " + e);
        }
    }

    private void OnApplicationQuit()
    {
        // Close the UDP client when the application quits
        _udpClient.Close();
    }
}