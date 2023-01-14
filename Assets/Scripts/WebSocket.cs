using System;
using System.Text;
using System.Threading;
using System.Collections;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class WebSocket : MonoBehaviour
{

    ClientWebSocket socket;

    CancellationTokenSource source = new CancellationTokenSource();
    CancellationToken token;

    Task Connected;
    bool specified;

    void Start() {
        
        socket = new ClientWebSocket();
        token = source.Token;
        specified = false;

        Connected = socket.ConnectAsync(new Uri("ws://localhost:8000"), token);
    }

    void Update() {
        if (Connected.IsCompleted && !specified) {
            Debug.Log("Connection Complete!");
            specified = true;
            Send(socket, "T");
        }
    }

    static async Task Send(ClientWebSocket socket, string data) {
        await socket.SendAsync(Encoding.UTF8.GetBytes(data), WebSocketMessageType.Text, true, CancellationToken.None);
    }
}
