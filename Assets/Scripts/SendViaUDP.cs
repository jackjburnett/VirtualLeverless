using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class SendViaUDP : MonoBehaviour
{
    public TMP_InputField serverIPInputField;
    public TMP_InputField portInputField;

    [FormerlySerializedAs("OnSendMessageRequested")]
    public UnityEvent<string> onSendMessageRequested; // Event to trigger message sending

    private int _port = 8080; // UDP port number for communication
    private string _serverIP = "192.168.0.17"; // Replace with your server IP address

    private UdpClient _udpClient;

    private void Start()
    {
        // Create UdpClient
        _udpClient = new UdpClient();

        onSendMessageRequested.AddListener(SendMessage);
    }

    private void OnApplicationQuit()
    {
        // Close the UDP client when the application quits
        if (_udpClient != null) _udpClient.Close();
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
            var data = Encoding.ASCII.GetBytes(message);

            // Send message to the server
            _udpClient.Send(data, data.Length, _serverIP, _port);
            Debug.Log($"Sent {message} to {_serverIP}:{_port}");
        }
        catch (SocketException e)
        {
            Debug.LogError("SocketException: " + e.Message);
        }
        catch (Exception e) // Ensure to use System.Exception
        {
            Debug.LogError("Exception: " + e.Message);
        }
    }

    private bool IsValidIP(string ip)
    {
        // Basic IP address validation
        return IPAddress.TryParse(ip, out _);
    }
}