using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterPro.DATA.Models
{
    public class Subscription
    {
        public decimal SubscriptionValue { get; set; }
        public decimal InitialValue { get; set; }
        public decimal Balance { get; set; }
        public string? MeterSn { get; set; }
        public DateTime? DateActivated { get; set; }


    }

    public class SubscriptionHistory
    {
        public decimal SubscriptionValue { get; set; }
        public decimal InitialValue { get; set; }
        public decimal Balance { get; set; }
        public string? MeterSn { get; set; }
        public DateTime? DateActivated { get; set; }
        public DateTime? DateDeactivated { get; set; }


    }
}
