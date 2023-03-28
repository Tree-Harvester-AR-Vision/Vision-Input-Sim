using System;
using System.Text;
using System.Threading;
using System.Collections;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using Newtonsoft.Json;

public class WebSocket : MonoBehaviour
{

    private static JsonSerializerSettings settings = new JsonSerializerSettings()
    {
        NullValueHandling = NullValueHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        Formatting = Formatting.None
    };

    public string IP = "localhost";
    public ushort Port = 7000;

    static ClientWebSocket socket;

    Task Connected;
    bool specified;

    private void Start() {
        
        socket = new ClientWebSocket();
        specified = false;

        Connected = socket.ConnectAsync(new Uri($"ws://{IP}:{Port}"), CancellationToken.None);
    }

    private void OnApplicationQuit()
    {
        Connected = socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Application closes", CancellationToken.None);
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


        List<InputTree>[] trees = new List<InputTree>[3]
        {
            createTrees, updateTrees, removeTrees
        };

        string jsonString = JsonConvert.SerializeObject(trees, settings);

        await Send(socket, "List__" + jsonString);
        Debug.Log("List of trees was sent");


        //foreach(InputTree tree in createTrees) {
        //    await Send(socket, "Create" + tree.JsonSerialize());
        //    Debug.Log("Sent creation data");
        //}

        //foreach(InputTree tree in updateTrees) {
        //    await Send(socket, "Update" + tree.JsonSerialize());
        //    Debug.Log("Sent update data");
        //}

        //foreach(InputTree tree in removeTrees) {
        //    Debug.Log("removing");
        //    await Send(socket, "Remove" + tree.JsonSerialize());
        //    Debug.Log("Sent remove data");
        //}
    }
}
