mergeInto(LibraryManager.library, {
    ConnectWebSocket: function(urlPtr) {
        var url = UTF8ToString(urlPtr);
        if (typeof window.ws !== "undefined") {
            window.ws.close();
        }
        window.ws = new WebSocket(url);
        window.ws.onopen = function() { console.log("Connected to " + url); };
        window.ws.onmessage = function(evt) { console.log("Received: " + evt.data); };
        window.ws.onerror = function(evt) { console.error(evt); };
        window.ws.onclose = function() { console.log("Connection closed"); };
    },

    SendWebSocketMessage: function(msgPtr) {
        var message = UTF8ToString(msgPtr);
        if (window.ws && window.ws.readyState === WebSocket.OPEN) {
            window.ws.send(message);
        } else {
            console.warn("WebSocket not open");
        }
    }
});