using System;
using System.Text;
using System.Threading;
using System.Collections;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class WebSocket : MonoBehaviour {

    static ClientWebSocket socket;

    Task Connected;
    bool specified;

    private void Start() {
        
        socket = new ClientWebSocket();
        specified = false;

        Connected = socket.ConnectAsync(new Uri("ws://localhost:7000"), CancellationToken.None);
    }

    private async void Update() {
        if (Connected.IsCompleted && !specified) {
            Debug.Log("Connection Complete!");
            specified = true;
            await Send(socket, "T");
        }
    }

    private static async Task Send(ClientWebSocket socket, string data) {
        await socket.SendAsync(Encoding.UTF8.GetBytes(data), WebSocketMessageType.Text, true, CancellationToken.None);
    }

    public static async Task UpdateTrees(
        List<InputTree> createTrees, 
        List<InputTree> updateTrees, 
        List<InputTree> removeTrees) {

        foreach(InputTree tree in createTrees) {
            await Send(socket, "Create" + tree.JsonSerialize());
        }

        foreach(InputTree tree in updateTrees) {
            await Send(socket, "Update" + tree.JsonSerialize());
        }

        foreach(InputTree tree in removeTrees) {
            Debug.Log("removing");
            await Send(socket, "Remove" + tree.JsonSerialize());
        }
    }
}
