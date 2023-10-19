using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterPro.DATA.CommandModels
{
    public class Command
    {
        public string? MeterSn { get; set; }
        public string? GatewaySn { get; set; }
        public string? Method { get; set; }
        public Value? Value { get; set; }
    }
    public class Value
    {
        public string? ForceSwitch { get; set; }
    }

    public class LoginModel
    {
        public string? LoginName { get; set; }
        public string? Password { get; set; }
    }
}
