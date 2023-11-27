using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterPro.DATA.AuthModels
{
    public class LoginModelV2
    {
        public int msgid { get; set; }
        public string? method { get; set; }
        public string? sn { get; set; }
        public int res { get; set; }
        public long timestamp { get; set; }

    }
}
