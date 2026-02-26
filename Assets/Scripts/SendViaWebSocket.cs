using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using NativeWebSocket;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SendViaWebSocket : MonoBehaviour
{
    public TMP_InputField serverDomainInput;
    public UnityEvent<string> onSendMessageRequested;

    private Task _connectionTask;
    private WebSocket _ws;

    private void Start()
    {
        onSendMessageRequested ??= new UnityEvent<string>();
        onSendMessageRequested.AddListener(msg => _ = SendWebSocketMessage(msg));
    }

    private void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        _ws?.DispatchMessageQueue();
#endif
    }

    private void OnApplicationQuit()
    {
        if (_ws != null)
            StartCoroutine(CloseWebSocketCoroutine());
    }

    private IEnumerator CloseWebSocketCoroutine()
    {
        var closeTask = _ws.Close();
        while (!closeTask.IsCompleted)
            yield return null;

        _ws = null;
    }

    private async Task EnsureConnected()
    {
        if (_ws == null)
        {
            _ws = new WebSocket($"wss://{serverDomainInput.text}");

            _ws.OnOpen += () => Debug.Log("WebSocket connection opened!");
            _ws.OnError += e => Debug.LogError($"WebSocket Error: {e}");
            _ws.OnClose += _ => Debug.Log("WebSocket closed!");
            _ws.OnMessage += bytes =>
            {
                var receivedMessage = Encoding.UTF8.GetString(bytes);
                Debug.Log("Received: " + receivedMessage);
            };

            _connectionTask = _ws.Connect();
        }

        if (_connectionTask != null)
            await _connectionTask;
    }

    private async Task SendWebSocketMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(serverDomainInput.text))
        {
            Debug.LogError("Server domain is empty!");
            return;
        }

        try
        {
            await EnsureConnected();

            if (_ws.State == WebSocketState.Open)
            {
                await _ws.SendText(message);
                Debug.Log($"Sent: {message}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"WebSocket Exception: {e.Message}");
        }
    }
}