using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterPro.DATA.AuthModels
{
    public class H5Json
    {
        public string? foo { get; set; } // Add the properties you need
    }

    public class Data
    {
        public bool? overseaFlag { get; set; }
        public string? userInfo { get; set; }
        public int? topAuthorizationFlag { get; set; }
        public int? useStrongPasswordFlag { get; set; }
        public object? triphase { get; set; }
        public bool? isAdmin { get; set; }
        public string? H5Config { get; set; }
        public string? token { get; set; }
        public bool? license { get; set; }
        public object? homepageUrl { get; set; }
        public string? logo_img_url { get; set; }
        public int? actionFlag { get; set; }
        //public H5Json H5Json { get; set; }
    }

    public class TokenResponse
    {
        public string? success { get; set; }
        public string? errorCode { get; set; }
        public string? errorMsg { get; set; }
        public Data? data { get; set; }
    }
}
