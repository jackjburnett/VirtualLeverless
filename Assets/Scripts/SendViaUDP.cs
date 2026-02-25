using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using NativeWebSocket;

public class SendViaUDP : MonoBehaviour
{
    [Header("Server Domain Input (e.g., example.trycloudflare.com)")]
    public TMP_InputField serverDomainInput;

    public UnityEvent<string> onSendMessageRequested;

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

    public async void SendMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(serverDomainInput.text))
        {
            Debug.LogError("Server domain is empty!");
            return;
        }

        // Prepend protocol depending on platform
#if UNITY_WEBGL && !UNITY_EDITOR
        string url = $"wss://{serverDomainInput.text}";
#else
        string url = $"ws://{serverDomainInput.text}";
#endif

        try
        {
            // Connect if not connected
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

            // Send the message
            await ws.SendText(message);
            Debug.Log($"Sent: {message} to {url}");
        }
        catch (Exception e)
        {
            Debug.LogError($"WebSocket Exception: {e.Message}");
        }
    }
}