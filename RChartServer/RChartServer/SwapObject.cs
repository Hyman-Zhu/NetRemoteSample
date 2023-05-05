using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChartServer
{
    public class SwapObject : MarshalByRefObject
    {
        public override object InitializeLifetimeService()
        {
            return null;
        }

        private ReceivedHandler m_received;

        public event ReceivedHandler SwapServerToClient
        {
            add { m_received += value; }
            remove { m_received -= value; }
        }

        public void ToClient(string message)
        {
            if (m_received != null)
            {
                m_received(message);
            }
        }
    }
}
