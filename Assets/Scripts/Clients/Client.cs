using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Clients
{
    public class Client : MonoBehaviour
    {
        

        public string ip = "localhost";
        public ushort port = 7000;
        public ConnectionType type;
        public NullValueHandling nullValueHandling;
        public Formatting format;
        
        private IWebClient _client;
        private JsonSerializerSettings _settings;

        private void Start()
        {
            switch (type)
            {
                case ConnectionType.None:
                    break;
                case ConnectionType.TCP:
                    _client = new TcpClient(ip, port);
                    break;
                case ConnectionType.UDP:
                    _client = new UdpClient(ip, port);
                    break;
            }
        
            _settings = new JsonSerializerSettings()
            {
                NullValueHandling = nullValueHandling,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = format
            };
        }

        private void Update()
        {
            if (_client != null)
            {
                _client.Update();
            }
        }

        private void OnApplicationQuit()
        {
            if (_client != null)
            {
                _client.CloseConnection();
            }
        }

        public IWebClient GetClient()
        {
            return _client;
        }

        public async Task UpdateTrees(
            List<InputTree> createTrees,
            List<InputTree> updateTrees,
            List<InputTree> removeTrees)
        {
            List<InputTree>[] trees = new List<InputTree>[3]
            {
                createTrees, updateTrees, removeTrees
            };

            string jsonString = JsonConvert.SerializeObject(trees, _settings);

            try
            {
                Byte[] sendBytes = Encoding.ASCII.GetBytes(jsonString);
                _client.Send(sendBytes, sendBytes.Length);
                //SendSequence("List__" + jsonString);
                Debug.Log("List of trees was sent");
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
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
