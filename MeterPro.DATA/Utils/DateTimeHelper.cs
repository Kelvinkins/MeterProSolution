using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterPro.DATA.Utils
{
    public static class DateTimeHelper
    {
        public static bool HasNotReportedInLastTenMinutes(DateTime lastUpdated)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan timeDifference = currentTime - lastUpdated;
            if (timeDifference.TotalMinutes > 10)
            {
                return true; // Device has not reported in the last 10 minutes
            }

            return false; // Device has reported in the last 10 minutes
        }
    }
}
