using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models
{
    public class DSTinDangBackDate
    {
        [DataNames("ASTOCKCODE")]
        public string astockcode { get; set; }

        [DataNames("ANAME")]
        public string aname { get; set; }

        [DataNames("AVUNGMIEN")]
        public string avungmien { get; set; }

        [DataNames("ASANGIAODICH")]
        public string asangiaodich { get; set; }

        [DataNames("AEXPERT")]
        public string aexpert { get; set; }

        [DataNames("AREGIONID")]
        public int aregionid { get; set; }

        [DataNames("ATITLE")]
        public string atitle { get; set; }

        [DataNames("AYEAR")]
        public int ayear { get; set; }

        [DataNames("ACREATEBY")]
        public string acreateby { get; set; }

        [DataNames("ALOAITAILIEU")]
        public string aloaitailieu { get; set; }

        [DataNames("ALOAITIN")]
        public string aloaitin { get; set; }


        [DataNames("ANGAYDANGTIN")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime angaydangtin { get; set; }

        [DataNames("ANGAYDANGTINBANDAU")]
        public DateTime angaydangtinbandau { get; set; }
    }
}
