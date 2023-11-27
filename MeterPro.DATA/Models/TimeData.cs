using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MeterPro.DATA.Models
{
    using MongoDB.Bson;
    using System.Text.Json.Serialization;

    public class TimeData
    {
        public ObjectId Id { get; set; }

        [JsonPropertyName("TempC")]
        public string? TempC { get; set; }

        [JsonPropertyName("TempB")]
        public string? TempB { get; set; }

        [JsonPropertyName("TempA")]
        public string? TempA { get; set; }

        [JsonPropertyName("PT")]
        public string? PT { get; set; }

        [JsonPropertyName("DO2")]
        public string? DO2 { get; set; }

        [JsonPropertyName("D01")]
        public string? D01 { get; set; }

        [JsonPropertyName("source")]
        public string? source { get; set; }

        [JsonPropertyName("UpInterval")]
        public string? UpInterval { get; set; }

        [JsonPropertyName("Pa")]
        public string? Pa { get; set; }

        [JsonPropertyName("Pb")]
        public string? Pb { get; set; }

        [JsonPropertyName("Pc")]
        public string? Pc { get; set; }

        [JsonPropertyName("EQC")]
        public string? EQC { get; set; }

        //[JsonPropertyName("id")]
        //public string? id { get; set; }

        [JsonPropertyName("Lg")]
        public string? Lg { get; set; }

        [JsonPropertyName("state")]
        public string? state { get; set; }

        [JsonPropertyName("EQL")]
        public string? EQL { get; set; }

        [JsonPropertyName("IN")]
        public string? IN { get; set; }

        [JsonPropertyName("EPID")]
        public string? EPID { get; set; }

        [JsonPropertyName("EPIF")]
        public string? EPIF { get; set; }

        [JsonPropertyName("EPIG")]
        public string? EPIG { get; set; }

        [JsonPropertyName("PFa")]
        public string? PFa { get; set; }

        [JsonPropertyName("EP")]
        public string? EP { get; set; }

        [JsonPropertyName("Ua")]
        public string? Ua { get; set; }

        [JsonPropertyName("PFc")]
        public string? PFc { get; set; }

        [JsonPropertyName("EPIJ")]
        public string? EPIJ { get; set; }

        [JsonPropertyName("Ub")]
        public string? Ub { get; set; }

        [JsonPropertyName("PFb")]
        public string? PFb { get; set; }

        [JsonPropertyName("Uc")]
        public string? Uc { get; set; }

        [JsonPropertyName("Qa")]
        public string? Qa { get; set; }

        [JsonPropertyName("Qb")]
        public string? Qb { get; set; }

        [JsonPropertyName("Qc")]
        public string? Qc { get; set; }

        [JsonPropertyName("EPIP")]
        public string? EPIP { get; set; }

        [JsonPropertyName("meterSn")]
        public string? meterSn { get; set; }

        [JsonPropertyName("Ia")]
        public string? Ia { get; set; }

        [JsonPropertyName("Ib")]
        public string? Ib { get; set; }

        [JsonPropertyName("Ic")]
        public string? Ic { get; set; }

        [JsonPropertyName("MEPIMD")]
        public string? MEPIMD { get; set; }

        [JsonPropertyName("timezone")]
        public string? timezone { get; set; }

        [JsonPropertyName("DI0")]
        public string? DI0 { get; set; }

        [JsonPropertyName("DI2")]
        public string? DI2 { get; set; }

        [JsonPropertyName("DI1")]
        public string? DI1 { get; set; }

        [JsonPropertyName("DI3")]
        public string? DI3 { get; set; }

        [JsonPropertyName("P")]
        public string? P { get; set; }

        [JsonPropertyName("Q")]
        public string? Q { get; set; }

        [JsonPropertyName("S")]
        public string? S { get; set; }

        [JsonPropertyName("Ubc")]
        public string? Ubc { get; set; }

        [JsonPropertyName("meterNo")]
        public string? meterNo { get; set; }

        [JsonPropertyName("timestamp")]
        public long? timestamp { get; set; }

        [JsonPropertyName("CreateTime")]
        public string? CreateTime { get; set; }

        [JsonPropertyName("msgid")]
        public long? msgid { get; set; }

        [JsonPropertyName("MEPIMDT")]
        public string? MEPIMDT { get; set; }

        [JsonPropertyName("Fr")]
        public string? Fr { get; set; }

        [JsonPropertyName("Sa")]
        public string? Sa { get; set; }

        [JsonPropertyName("Sb")]
        public string? Sb { get; set; }

        [JsonPropertyName("Sc")]
        public string? Sc { get; set; }

        [JsonPropertyName("CT")]
        public string? CT { get; set; }

        [JsonPropertyName("Uca")]
        public string? Uca { get; set; }

        [JsonPropertyName("PF")]
        public string? PF { get; set; }

        [JsonPropertyName("EPE")]
        public string? EPE { get; set; }

        [JsonPropertyName("gatewaySn")]
        public string? gatewaySn { get; set; }

        [JsonPropertyName("EPI")]
        public string? EPI { get; set; }

        [JsonPropertyName("Balance")]
        public string? Balance { get; set; }

        [JsonPropertyName("SwitchSta")]
        public string? SwitchSta { get; set; }
    }


  

    public class ReportedData
    {
        public int Rssi { get; set; }
        public  DeviceData DeviceData { get; set; }
    }
    public class DeviceData
    {
        [JsonPropertyName("State")]
        public string? State { get; set; }

        [JsonPropertyName("Ua")]
        public string? Ua { get; set; }

        [JsonPropertyName("Ub")]
        public string? Ub { get; set; }

        [JsonPropertyName("Uc")]
        public string? Uc { get; set; }

        [JsonPropertyName("Ia")]
        public string? Ia { get; set; }

        [JsonPropertyName("Ib")]
        public string? Ib { get; set; }

        [JsonPropertyName("Ic")]
        public string? Ic { get; set; }

        [JsonPropertyName("Pa")]
        public string? Pa { get; set; }

        [JsonPropertyName("Pb")]
        public string? Pb { get; set; }

        [JsonPropertyName("Pc")]
        public string? Pc { get; set; }

        [JsonPropertyName("P")]
        public string? P { get; set; }

        [JsonPropertyName("Qa")]
        public string? Qa { get; set; }

        [JsonPropertyName("Qb")]
        public string? Qb { get; set; }

        [JsonPropertyName("Qc")]
        public string? Qc { get; set; }

        [JsonPropertyName("PFa")]
        public string? PFa { get; set; }

        [JsonPropertyName("PFb")]
        public string? PFb { get; set; }

        [JsonPropertyName("PFc")]
        public string? PFc { get; set; }

        [JsonPropertyName("PF")]
        public string? PF { get; set; }

        [JsonPropertyName("PT")]
        public string? PT { get; set; }

        [JsonPropertyName("CT")]
        public string? CT { get; set; }

        [JsonPropertyName("EPI")]
        public string? EPI { get; set; }

        [JsonPropertyName("EPIJ")]
        public string? EPIJ { get; set; }

        [JsonPropertyName("EPIF")]
        public string? EPIF { get; set; }

        [JsonPropertyName("EPIP")]
        public string? EPIP { get; set; }

        [JsonPropertyName("EPIG")]
        public string? EPIG { get; set; }

        [JsonPropertyName("Balance")]
        public string? Balance { get; set; }

        [JsonPropertyName("BuyTimes")]
        public string? BuyTimes { get; set; }

        [JsonPropertyName("LoseTimes1")]
        public string? LoseTimes1 { get; set; }

        [JsonPropertyName("CommitTimes")]
        public string? CommitTimes { get; set; }

        [JsonPropertyName("ControlMode")]
        public string? ControlMode { get; set; }

        [JsonPropertyName("ControlMode1")]
        public string? ControlMode1 { get; set; }

        [JsonPropertyName("ControlMode2")]
        public string? ControlMode2 { get; set; }

        [JsonPropertyName("ControlMode3")]
        public string? ControlMode3 { get; set; }

        [JsonPropertyName("LoseMode")]
        public string? LoseMode { get; set; }

        [JsonPropertyName("LoseMode1")]
        public string? LoseMode1 { get; set; }

        [JsonPropertyName("LoseMode2")]
        public string? LoseMode2 { get; set; }

        [JsonPropertyName("LoseMode3")]
        public string? LoseMode3 { get; set; }

        [JsonPropertyName("TimeMode")]
        public string? TimeMode { get; set; }

        [JsonPropertyName("TimeMode1")]
        public string? TimeMode1 { get; set; }

        [JsonPropertyName("TimeMode2")]
        public string? TimeMode2 { get; set; }

        [JsonPropertyName("TimeMode3")]
        public string? TimeMode3 { get; set; }

        [JsonPropertyName("PrepaidSta")]
        public string? PrepaidSta { get; set; }

        [JsonPropertyName("PrepaidSta1")]
        public string? PrepaidSta1 { get; set; }

        [JsonPropertyName("PrepaidSta2")]
        public string? PrepaidSta2 { get; set; }

        [JsonPropertyName("PrepaidSta3")]
        public string? PrepaidSta3 { get; set; }

        [JsonPropertyName("SwitchSta1")]
        public string? SwitchSta1 { get; set; }

        [JsonPropertyName("SwitchSta2")]
        public string? SwitchSta2 { get; set; }

        [JsonPropertyName("SwitchSta3")]
        public string? SwitchSta3 { get; set; }

        [JsonPropertyName("SwitchSta")]
        public string? SwitchSta { get; set; }

        [JsonPropertyName("OweMoney")]
        public string? OweMoney { get; set; }

        [JsonPropertyName("AlarmA")]
        public string? AlarmA { get; set; }

        [JsonPropertyName("AlarmB")]
        public string? AlarmB { get; set; }

        [JsonPropertyName("MEPIMD")]
        public string? MEPIMD { get; set; }
    }


    public class UpdateMessage
    {
        public int MessageId { get; set; }
        public string Method { get; set; }
        public string SerialNumber { get; set; }
        public long Timestamp { get; set; }
        public long SendTime { get; set; }
        public int Version { get; set; }
        [JsonPropertyName("Reported")]
        public ReportedData Reported { get; set; }
    }

}
