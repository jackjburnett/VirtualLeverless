using UnityEngine;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine.Events;

public class SendViaUDP : MonoBehaviour
{
    public TMP_InputField serverIPInputField;
    public TMP_InputField portInputField;
    
    public UnityEvent<string> OnSendMessageRequested; // Event to trigger message sending
    
    private UdpClient _udpClient;
    private string _serverIP = "192.168.0.17"; // Replace with your server IP address
    private int _port = 8080; // UDP port number for communication

    void Start()
    {
        // Create UdpClient
        _udpClient = new UdpClient();
        
        OnSendMessageRequested.AddListener(SendMessage);
    }

    public new void SendMessage(string message)
    {
        // Update serverIP and port from the input fields
        _serverIP = serverIPInputField.text;
        if (!int.TryParse(portInputField.text, out _port) || !IsValidIP(_serverIP))
        {
            Debug.LogError("Invalid IP address or port number");
            return;
        }

        try
        {
            // Create message
            byte[] data = Encoding.ASCII.GetBytes(message);

            // Send message to the server
            _udpClient.Send(data, data.Length, _serverIP, _port);
            Debug.Log($"Sent {message} to {_serverIP}:{_port}");
        }
        catch (SocketException e)
        {
            Debug.LogError("SocketException: " + e.Message);
        }
        catch (System.Exception e) // Ensure to use System.Exception
        {
            Debug.LogError("Exception: " + e.Message);
        }
    }
    
    private bool IsValidIP(string ip)
    {
        // Basic IP address validation
        return System.Net.IPAddress.TryParse(ip, out _);
    }

    private void OnApplicationQuit()
    {
        // Close the UDP client when the application quits
        if (_udpClient != null)
        {
            _udpClient.Close();
        }
    }
}