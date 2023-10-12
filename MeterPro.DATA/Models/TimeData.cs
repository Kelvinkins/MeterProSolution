using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MeterPro.DATA.Models
{
    public class TimeData
    {

        public string? TempC { get; set; }
        public string? TempB { get; set; }
        public string? TempA { get; set; }
        public string? PT { get; set; }
        public string? DO2 { get; set; }
        public string? D01 { get; set; }
        public string? source { get; set; }
        public string? UpInterval { get; set; }
        public string? Pa { get; set; }
        public string? Pb { get; set; }
        public string? Pc { get; set; }

        //        "EQC": "12.84",
        //"id": "1132",
        //"Lg": "0",
        //"state": "ONLINE",
        //"EQL": "0",
        //"IN": "1.98",
        //"EPID": "0.392",
        //"EPIF": "0",
        //"EPIG": "0",
        //"PFa": "0.951",
        //"EP": "36.34",
        public string? EQC { get; set; }
        public string? id { get; set; }
        public string? Lg { get; set; }
        public string? state { get; set; }
        public string? EQL { get; set; }
        public string? IN { get; set; }
        public string? EPID { get; set; }
        public string? EPIF { get; set; }
        public string? EPIG { get; set; }
        public string? PFa { get; set; }
        public string? EP { get; set; }


        //"Ua": "235",
        //"PFc": "1",
        //"EPIJ": "0",
        //"Ub": "235.9",
        //"PFb": "1",
        //"Uc": "235.8",
        //"Qa": "-0.128",
        //"Qb": "0",
        //"Qc": "0",
        //"EPIP": "0",
        //"meterSn": "12212122890001",
        //"Uab": "407.8",
        //"Ia": "1.98",
        public string? Ua { get; set; }
        public string? PFc { get; set; }
        public string? EPIJ { get; set; }
        public string? Ub { get; set; }
        public string? PFb { get; set; }
        public string? Uc { get; set; }
        public string? Qa { get; set; }
        public string? Qb { get; set; }
        public string? Qc { get; set; }
        public string? EPIP { get; set; }
        public string? meterSn { get; set; }
        public string? Ia { get; set; }
        //        "Ib": "0",
        //"Ic": "0",
        //"MEPIMD": "0.407",
        //"timezone": "8",
        //"DI0": "0",
        //"DI2": "0",
        //"DI1": "0",
        //"DI3": "0",
        //"P": "0.394",
        public string? Ib { get; set; }
        public string? Ic { get; set; }
        public string? MEPIMD { get; set; }
        public string? timezone { get; set; }
        public string? DI0 { get; set; }
        public string? DI2 { get; set; }
        public string? DI1 { get; set; }
        public string? DI3 { get; set; }
        public string? P { get; set; }

        //"Q": "-0.13",
        //"S": "0.416",
        //"Ubc": "408.5",
        //"meterNo": "12212122890001_12212122890001",
        //"timestamp": 1672021200000,
        //"CreateTime": "2022-12-26 10:20:00",
        //"msgid": 767178432396455936,
        //"MEPIMDT": "2022-12-23 12:50",
        //"Fr": "50.02",
        //"Sa": "0.414",
        //"Sb": "0",

        public string? Q { get; set; }
        public string? S { get; set; }
        public string? Ubc { get; set; }
        public string? meterNo { get; set; }
        public string? timestamp { get; set; }
        public string? CreateTime { get; set; }
        public string? msgid { get; set; }
        public string? MEPIMDT { get; set; }
        public string? Fr { get; set; }
        public string? Sa { get; set; }
        public string? Sb { get; set; }
        //        "Sc": "0",
        //"CT": "1",
        //"Uca": "407.7",
        //"PF": "0.949",
        //"EPE": "0",
        //"gatewaySn": "12212122890001",
        //"EPI": "36.34
        public string? Sc { get; set; }
        public string? CT { get; set; }
        public string? Uca { get; set; }
        public string? PF { get; set; }
        public string? EPE { get; set; }
        public string? gatewaySn { get; set; }
        public string? EPI { get; set; }


    }
}
