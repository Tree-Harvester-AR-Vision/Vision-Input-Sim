using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Clients;
using Newtonsoft.Json;
using Unity.Networking.Transport;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Net.Sockets;
using System.Text;


namespace Clients
{
    public class UdpClient : IWebClient
    {
        readonly System.Net.Sockets.UdpClient _client;
        private readonly JsonSerializerSettings _settings;
        private readonly ushort _port;
        private readonly string _ip;
        

        public UdpClient(string ip, ushort port, JsonSerializerSettings settings)
        {
            _settings = settings;
            _ip = ip;
            _port = port;

            _client = new System.Net.Sockets.UdpClient();
        }

        public void Update()
        {
            
        }

        public Task Send(string jsonString)
        {
            Debug.Log(jsonString);
            Byte[] sendBytes = Encoding.ASCII.GetBytes(jsonString);
            return _client.SendAsync(sendBytes, sendBytes.Length, _ip, _port);
        }


        //        public void Update() {
  //          m_Driver.ScheduleUpdate().Complete();
//
  //          if (!m_Connection.IsCreated) {
   //             Debug.LogError("Something went wrong during connect");
   //             return;
  //          }
//
        //    if (!SendStage) {
        //        StartupSequence();
        //    } else { SendSequence(); }
        //}

        public void OnDestroy() {
            _client.Dispose();
        }
        

        public void CloseConnection()
        {
            
        }
    }
}
