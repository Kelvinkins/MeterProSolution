using MeterPro.DATA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MeterPro.DATA.CommandModels
{
    public class Command
    {
        [JsonPropertyName("meterSn")]
        public string? MeterSn { get; set; }
        [JsonPropertyName("gatewaySn")]

        public string? GatewaySn { get; set; }
        [JsonPropertyName("method")]
        public string? Method { get; set; }
        [JsonPropertyName("value")]
        public Value? Value { get; set; }
        public ShutOffBy? ShutOffBy { get; set; }
    }
    public class Value
    {
        [JsonPropertyName("ForceSwitch")]
        public int? ForceSwitch { get; set; }
    }

    public class LoginModel
    {
        public string? LoginName { get; set; }
        public string? PassWord { get; set; }
    }
}
