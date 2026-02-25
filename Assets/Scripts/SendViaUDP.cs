using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using NativeWebSocket;

public class SendViaUDP : MonoBehaviour
{
    public TMP_InputField[] serverIPInputFields; // 4 fields for IP octets
    public TMP_InputField portInputField; // Port input

    public UnityEvent<string> onSendMessageRequested;
    private int _port;
    private string _serverIP;

    private WebSocket ws;

    private async void Start()
    {
        if (onSendMessageRequested == null)
            onSendMessageRequested = new UnityEvent<string>();

        onSendMessageRequested.AddListener(SendMessage);
    }

    private void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        ws?.DispatchMessageQueue();
#endif
    }

    private async void OnApplicationQuit()
    {
        if (ws != null)
            await ws.Close();
    }

    private bool IsValidIP(string[] ipParts)
    {
        foreach (var part in ipParts)
            if (!int.TryParse(part, out var value) || value < 0 || value > 255)
                return false;
        return true;
    }

    public async void SendMessage(string message)
    {
        // 1️⃣ Read IP from input fields
        var ipParts = new string[4];
        for (var i = 0; i < 4; i++)
            ipParts[i] = serverIPInputFields[i].text;

        _serverIP = string.Join(".", ipParts);

        if (!IsValidIP(ipParts) || !int.TryParse(portInputField.text, out _port))
        {
            Debug.LogError("Invalid IP address or port number");
            return;
        }

        var url = $"ws://{_serverIP}:{_port}";

        try
        {
            // 2️⃣ Connect if not connected
            if (ws == null || ws.State != WebSocketState.Open)
            {
                ws = new WebSocket(url);

                ws.OnOpen += () => Debug.Log("WebSocket connection opened!");
                ws.OnError += e => Debug.LogError($"WebSocket Error: {e}");
                ws.OnClose += e => Debug.Log("WebSocket closed!");
                ws.OnMessage += bytes =>
                {
                    var receivedMessage = Encoding.UTF8.GetString(bytes);
                    Debug.Log("Received: " + receivedMessage);
                };

                await ws.Connect();
            }

            // 3️⃣ Send the message
            await ws.SendText(message);
            Debug.Log($"Sent: {message} to {_serverIP}:{_port}");
        }
        catch (Exception e)
        {
            Debug.LogError($"WebSocket Exception: {e.Message}");
        }
    }
}