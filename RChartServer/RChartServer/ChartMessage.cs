using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChartServer
{
    /// <summary>
    /// The chart message object
    /// </summary>
    public class ChartMessage : MarshalByRefObject
    {

        public ChartMessage()
        {

        }

        public void SetMessage(string message)
        {
            ChartCach.GetInstance().MessageString = message;
        }

        public event ReceivedHandler ServerToClient
        {
            add { m_receivedHandler += value; }
            remove { m_receivedHandler -= value; }
        }

        private ReceivedHandler m_receivedHandler;

        public void ToCient(string message)
        {
            if (m_receivedHandler != null)
            {
                m_receivedHandler(message);
            }
        }
    }

    public delegate void ReceivedHandler(string text);
}
