using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Clients
{
    public class UdpClient : IWebClient
    {
        readonly System.Net.Sockets.UdpClient _client;
        private readonly ushort _port;
        private readonly string _ip;


        public UdpClient(string ip, ushort port)
        {
            _ip = ip;
            _port = port;

            _client = new System.Net.Sockets.UdpClient();
        }

        public void Update()
        {
        }

        public Task Send(byte[] sendBytes, int sendBytesLength)
        {
            if (sendBytes != null && _client != null)
            {
                _client.Send(sendBytes, sendBytesLength, _ip, _port);
            }
            return Task.CompletedTask;
        }

        public void OnDestroy()
        {
            _client.Dispose();
        }


        public void CloseConnection()
        {
        }
    }
}