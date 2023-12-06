using MeterPro.DATA.Enums;
using MongoDB.Driver.Core.WireProtocol.Messages;
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


    public class SwitchCommand

    {
        [JsonPropertyName("method")]
        public string? Method { get; set; }

        [JsonPropertyName("msgid")]
        public string? msgid { get; set; }
        [JsonPropertyName("payload")]
        public Payload? payload { get; set; }
        [JsonPropertyName("sn")]
        public string? sn { get; set; }
        [JsonPropertyName("timestamp")]
        public long timestamp { get; set; }
    }

    public class Payload
    {
        [JsonPropertyName("addr")]
        public string? addr { get; set; }
        [JsonPropertyName("do1")]
        public int do1 { get; set; }
        [JsonPropertyName("meterName")]
        public string? meterName { get; set; }
        [JsonPropertyName("method")]
        public string? method { get; set; }
        [JsonPropertyName("ForceSwitch")]
        public int ForceSwitch { get; set; }

    }
   
}
