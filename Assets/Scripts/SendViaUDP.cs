using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Text;

public class SendViaUDP : MonoBehaviour
{
    public Button sendButton;
    public Button sendButton2;
    private UdpClient _udpClient;
    private string _serverIP = "192.168.0.17"; // Replace with your PC's IP address
    private int _port = 8080; // _port number for UDP communication

    void Start()
    {
        // Create UdpClient
        _udpClient = new UdpClient();
        
        // Add listener to the button
        sendButton.onClick.AddListener(SendMessageToPC_A);
        sendButton2.onClick.AddListener(SendMessageToPC_A2);
    }

    void SendMessageToPC_A()
    {
        try
        {
            // Create message
            string message = "A_PRESS";
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
    
    void SendMessageToPC_A2()
    {
        try
        {
            // Create message
            string message = "A_RELEASE";
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