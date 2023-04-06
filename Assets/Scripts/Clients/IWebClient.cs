using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Clients
{
    public interface IWebClient
    {
        void CloseConnection();
        void Update();
        Task Send(string jsonString);
    }
}
