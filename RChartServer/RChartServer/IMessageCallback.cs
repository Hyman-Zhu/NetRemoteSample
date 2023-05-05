using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChartServer
{
    public interface IMessageCallback
    {
        void SendMessageBack(string message);
    }
}
