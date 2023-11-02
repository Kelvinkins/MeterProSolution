using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterPro.DATA.Models
{
    public class Subscription
    {
        public double? SubscriptionValue { get; set; }
        public double? InitialValue { get; set; }
        public double? Balance { get; set; }
        public string? MeterSn { get; set; }
        public DateTime? DateActivated { get; set; }


    }

    public class SubscriptionHistory
    {
        public double? SubscriptionValue { get; set; }
        public double? InitialValue { get; set; }
        public double? Balance { get; set; }
        public string? MeterSn { get; set; }
        public DateTime? DateActivated { get; set; }
        public DateTime? DateDeactivated { get; set; }


    }
}
