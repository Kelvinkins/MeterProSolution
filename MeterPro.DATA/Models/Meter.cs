﻿using MeterPro.DATA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterPro.DATA.Models
{
    public class Meter
    {
        public string? MeterSn { get; set; }
        public string? gatewaySn { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Status { get; set; }
        public DateTime? DateEnrolled { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string? PowerStatus { get; set; }
        public decimal TotalUsageAccum { get; set; }
        public decimal Balance { get; set; }
        public ShutOffBy? ShutOffBy { get; set; }
        public string? Owner { get; set; }
        public string? Network { get; set; }




    }
}
