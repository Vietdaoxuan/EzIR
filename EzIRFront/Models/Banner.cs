using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRFront.Models
{
    public class Banner
    {
      
        public int AbannerID { get; set; }

        public string Atitle { get; set; }

        public string AfileName { get; set; }

        public string Apath { get; set; }

        public string Aurl { get; set; }

        public string Acreateby { get; set; }

        public DateTime AcreateOn { get; set; }

        public string? AupdateBY { get; set; }

        public DateTime? AupdateOn { get; set; }

        public string Alogo { get; set; }
    }
}
