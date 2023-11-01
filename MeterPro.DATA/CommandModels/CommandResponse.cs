using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterPro.DATA.CommandModels
{
    public class CommandResponse
    {
      
        public string? Success { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMsg { get; set; }
        public dynamic? data { get; set; }

    }
}
