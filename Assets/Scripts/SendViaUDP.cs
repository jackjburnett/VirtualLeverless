using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using NativeWebSocket;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

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

    private void OnApplicationQuit()
    {
        if (ws != null)
            StartCoroutine(CloseWebSocketCoroutine());
    }

    private IEnumerator CloseWebSocketCoroutine()
    {
        var closeTask = ws.Close();
        while (!closeTask.IsCompleted)
            yield return null;

        ws = null;
    }

    private async void SendMessage(string message)
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
        var url = $"ws://{serverDomainInput.text}";
#endif

        try
        {
            // Only create and connect once
            if (ws == null)
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

                await ws.Connect(); // wait for connection to open
            }

            // Wait until the socket is open before sending
            while (ws.State != WebSocketState.Open)
                await Task.Yield();

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