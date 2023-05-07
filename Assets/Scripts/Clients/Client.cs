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
        private static JsonSerializerSettings settings;

        public string IP = "localhost";
        public ushort Port = 7000;
        public ConnectionType Type;
        public NullValueHandling NullValueHandling;
        public Formatting Formatting;
        public bool Simulation;

        private IWebClient client;

        private void Start()
        {
            settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting
            };

            switch (Type)
            {
                case ConnectionType.None:
                    break;
                case ConnectionType.TCP:
                    client = new TcpClient(IP, Port);
                    break;
                case ConnectionType.UDP:
                    client = new UdpClient(IP, Port);
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

        public async Task UpdateTrees(List<InputTree> createTrees, List<InputTree> updateTrees,
            List<InputTree> removeTrees)
        {
            List<InputTree>[] trees = new List<InputTree>[3]
            {
                createTrees, updateTrees, removeTrees
            };

            if ((createTrees != null && updateTrees != null && removeTrees != null) && (createTrees.Count > 0 ||updateTrees.Count > 0 || removeTrees.Count > 0))
            {
                string jsonString = JsonConvert.SerializeObject(trees, settings);
                if(Simulation){
                    jsonString = "1" + jsonString;
                }
                else{
                jsonString += "0" + jsonString;
                }
                try
                {
                    await client.Send(jsonString);
                    Debug.Log($"List of trees was sent {createTrees.Count}, {updateTrees.Count}, {removeTrees.Count})");
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
            }
        }
    }

    public enum ConnectionType
    {
        None,
        TCP,
        UDP
    }
}