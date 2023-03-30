using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.Clients;
using Newtonsoft.Json;
using UnityEngine;

namespace Clients
{
    public class TcpClient : IWebClient
    {
        readonly ClientWebSocket _socket;

        Task _connected;
        bool _specified;
        private readonly JsonSerializerSettings _settings;

        public TcpClient(string ip, ushort port, JsonSerializerSettings settings)
        {
            _settings = settings;

            _socket = new ClientWebSocket();
            _specified = false;
            _connected = _socket.ConnectAsync(new Uri($"ws://{ip}:{port}"), CancellationToken.None);
        }

        public async void Update() {
            if (_connected.IsCompleted && !_specified) {
                Debug.Log("Connection Complete!");
                _specified = true;
                await Send("T");
            }
        }

        private async Task Send(string data) {
            await _socket.SendAsync(Encoding.UTF8.GetBytes(data), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task UpdateTrees(
            List<InputTree> createTrees, 
            List<InputTree> updateTrees, 
            List<InputTree> removeTrees) {


            List<InputTree>[] trees = new List<InputTree>[3]
            {
                createTrees, updateTrees, removeTrees
            };

            string jsonString = JsonConvert.SerializeObject(trees, _settings);

            try
            {
                await Send("List__" + jsonString);
                Debug.Log("List of trees was sent");
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        public void CloseConnection()
        {
            _connected = _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Application closes", CancellationToken.None);
        }
    }
}
