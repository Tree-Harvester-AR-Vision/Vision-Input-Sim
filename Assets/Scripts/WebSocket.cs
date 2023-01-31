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

        Connected = socket.ConnectAsync(new Uri("ws://localhost:8000"), CancellationToken.None);
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

    public static async void UpdateTrees(List<InputTree> trees, List<InputTree> removeTrees) {
        int i = 0;
        foreach(InputTree tree in trees) {
            if (tree.boundingBox.Center != Vector3.zero) {
                i++;
            }
            await Send(socket, "Update");
            await Send(socket, tree.JsonSerialize());
        }

        foreach(InputTree tree in removeTrees) {
            if (tree.boundingBox.Center != Vector3.zero) {
                i++;
            }
            await Send(socket, "Remove");
            await Send(socket, tree.JsonSerialize());
        }
    }
}
