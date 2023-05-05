using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChartServer
{
    public class ChartCach
    {
        private static ChartCach myInstance;
        public static IChartObserver Observer;

        private ChartCach()
        {

        }

        public static void Attach(IChartObserver observer)
        {
            Observer = observer;
        }

        public static ChartCach GetInstance()
        {
            if (myInstance == null)
            {
                myInstance = new ChartCach();
            }
            return myInstance;
        }

        public string MessageString
        {
            set
            {
                Observer.Notify(value);
            }
        }
    }
}
