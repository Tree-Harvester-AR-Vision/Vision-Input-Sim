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
        Task UpdateTrees(List<InputTree> inputTrees, List<InputTree> list, List<InputTree> inputTrees1);
        void Update();
    }
}
