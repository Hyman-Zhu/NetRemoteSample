using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RChartServer
{
    /// <summary>
    /// The chart message body
    /// </summary>
    public class MessageBody
    {
        public MessageBody(string text, string time, HorizontalAlignment location)
        {
            Text = text;
            Time = time;
            Location = location;
        }

        public string Text { get; set; }

        public string Time { get; set; }

        public HorizontalAlignment Location { get; set; }
    }
}
