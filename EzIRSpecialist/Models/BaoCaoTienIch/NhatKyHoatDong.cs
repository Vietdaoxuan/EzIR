using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.BaoCaoTienIch
{
    public class NhatKyHoatDong
    {
        [DataNames("AUserName")]
        public string Ausername { get; set; }
        
        [DataNames("Aloaitaikhoan")]
        public string Aloaitaikhoan { get; set; }

        [DataNames("ActionC")]
        public string ActionC { get; set; }

        [DataNames("Ausertype")]
        public string Ausertype { get; set; }

        [DataNames("Aaction")]
        public string Aaction { get; set; }

        [DataNames("Acreateon")]
        public DateTime Acreateon { get; set; }

        [DataNames("Anote")]
        public string Anote { get; set; }

        [DataNames("Astockcode")]
        public string Astockcode { get; set; }

        //[DataNames("FromDate")]
        //public string FromDate { get; set; }

        //[DataNames("ToDate")]
        //public string ToDate { get; set; }


    }
}
