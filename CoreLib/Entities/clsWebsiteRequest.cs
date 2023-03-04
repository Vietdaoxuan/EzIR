using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities
{
    public class clsWebsiteRequest
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public NewsWebsiteAPI Data { get; set; }
    }
}
