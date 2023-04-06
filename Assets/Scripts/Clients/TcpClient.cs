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

        public Task Send(string data) {
            return _socket.SendAsync(Encoding.UTF8.GetBytes(data), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public void CloseConnection()
        {
            _connected = _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Application closes", CancellationToken.None);
        }
    }
}
