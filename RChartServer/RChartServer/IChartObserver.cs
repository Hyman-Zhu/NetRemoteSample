using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RChartServer
{
    public interface IChartObserver
    {
        void Notify(string text);
    }
}
