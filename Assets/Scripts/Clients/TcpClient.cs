using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Clients
{
    public class TcpClient : IWebClient
    {
        readonly ClientWebSocket _socket;

        Task _connected;
        bool _specified;

        public TcpClient(string ip, ushort port)
        {

            _socket = new ClientWebSocket();
            _specified = false;
            _connected = _socket.ConnectAsync(new Uri($"ws://{ip}:{port}"), CancellationToken.None);
        }

        public async void Update() {
            if (_connected.IsCompleted && !_specified) {
                Debug.Log("Connection Complete!");
                _specified = true;
                byte[] data = Encoding.ASCII.GetBytes("T");
                await Send(data, data.Length);
            }
        }

        public async Task Send(byte[] sendBytes, int sendBytesLength)
        {
            await _socket.SendAsync(sendBytes, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public void CloseConnection()
        {
            _connected = _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Application closes", CancellationToken.None);
        }
    }
}
