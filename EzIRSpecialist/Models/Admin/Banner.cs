using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.Admin
{
    public class Banner
    {
        [DataNames("ABANNERID")]
        public int AbannerID { get; set; }

        [DataNames("ATITLE")]
        public string Atitle { get; set; }

        [DataNames("AFILENAME")]
        public string AfileName { get; set; }

        [DataNames("APATH")]
        public string Apath { get; set; }

        [DataNames("AURL")]
        public string Aurl { get; set; }

        [DataNames("ACREATEBY")]
        public string Acreateby { get; set; }

        [DataNames("ACREATEON")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime AcreateOn { get; set; }

        [DataNames("AUPDATEBY")]
        public string AupdateBY { get; set; }

        [DataNames("AUPDATEON")]
        public DateTime AupdateOn { get; set; }

        [DataNames("ALOGO")]
        public byte[] Alogo { get; set; }

        public string imagelogo { get; set; }
    }
}
