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
    }
}
