using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.BaoCaoTienIch
{
    public class NhacCongBoThongTin
    {
        [DataNames("ALOAIDOANHNGHIEP")]
        public string ALoaiDoanhNghiep { get; set; }

        [DataNames("ASTOCKCODE")]
        public string AStockCode { get; set; }

        [DataNames("ACCPLCBTT")]
        public string accplcbtt { get; set; }

        [DataNames("AQDCBTT")]
        public string Aqdcbtt { get; set; }

        [DataNames("ATITLE")]
        public string ATitle { get; set; }

        [DataNames("AEMAIL")]
        public string AEmail { get; set; }

        [DataNames("ANOTE")]
        public string Anote { get; set; }

        [DataNames("ANEWTYPE")]
        public string ANewType { get; set; }

        [DataNames("ADOCTYPE")]
        public string ADocType { get; set; }

        [DataNames("LOAITIN")]
        public string LoaiTin   { get; set; }

        [DisplayFormat(DataFormatString = "{dd/MM/yyyy}")]
        [DataNames("AENDTIME")]
        public DateTime AEndTime { get; set; }

        [DataNames("ANAME")]
        public string Aname { get; set; }

        [DataNames("ACPNYID")]
        public int AAcpnyid { get; set; }


        [DataNames("ADATENOW")]
        public string ADateNow { get; set; }

        [DataNames("ADAY")]
        public string ADay { get; set; }

        [DataNames("AMONTH")]
        public string AMonth { get; set; }

        [DataNames("AYEAR")]
        public string AYear { get; set; }

        [DataNames("ACC")]
        public string Acc { get; set; }

        [DataNames("ACOMPANYTYPE")]
        public int acompanytype { get; set;}

        [DataNames("ANAMECPNY")]
        public string anamecpny { get; set; }

        [DataNames("ARULEID")]
        public string aruleid { get; set; }

        [DataNames("adateextend")]
        public DateTime AdateExtend { get; set; }

    }
}
