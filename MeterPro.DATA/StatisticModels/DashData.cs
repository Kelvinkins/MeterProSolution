using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterPro.DATA.StatisticModels
{
    public class DashData
    {
        public int AllMetersCount { get; set; }
        public int ConnectedMetersCount { get; set; }
        public int DisconnectedMetersCount { get; set; }
        public int PowerOutMeters { get; set; }

    }
}
