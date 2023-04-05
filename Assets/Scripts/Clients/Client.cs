using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Clients;
using UnityEngine;
using TcpClient = Clients.TcpClient;
using UdpClient = Clients.UdpClient;

namespace Assets.Scripts.Clients
{
    public class Client : MonoBehaviour
    {
        internal static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None
        };

        public string IP = "localhost";
        public ushort Port = 7000;
        public ConnectionType Type;

        private IWebClient client;

        private void Start()
        {
            switch (Type)
            {
                case ConnectionType.None:
                    break;
                case ConnectionType.TCP:
                    client = new TcpClient(IP, Port, settings);
                    break;
                case ConnectionType.UDP:
                    client = new UdpClient(IP, Port, settings);
                    break;
            }
        }

        private void Update()
        {
            client.Update();
        }

        private void OnApplicationQuit()
        {
            if (client != null)
            {
                client.CloseConnection();
            }
        }

        public IWebClient GetClient()
        {
            return client;
        }

        public async Task UpdateTrees(List<InputTree> inputTrees, List<InputTree> list, List<InputTree> inputTrees1)
        {
            client.UpdateTrees(inputTrees, list, inputTrees);
        }
    }

    public enum ConnectionType
    {
        None,
        TCP,
        UDP
    }
}
