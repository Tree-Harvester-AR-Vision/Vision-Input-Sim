using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clients
{
    public interface IWebClient
    {
        void CloseConnection();
        void Update();
        Task Send(byte[] sendBytes, int sendBytesLength);
    }
}
