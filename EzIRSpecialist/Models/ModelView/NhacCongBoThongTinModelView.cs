using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ModelView
{
    public class NhacCongBoThongTinModelView
    {

         [DataNames("ALOAIDOANHNGHIEP")]
         public string ALoaiDoanhNghiep { get; set; }

         [DataNames("ASTOCKCODE")]
         public string AStockCode { get; set; }

         [DataNames("ACCPLCBTT")]
         public string accplcbtt { get; set; }

         [DataNames("ATITLE")]
         public string ATitle { get; set; }

         [DataNames("AEMAIL")]
         public string AEmail { get; set; }

         [DataNames("ANOTE")]
         public string Anote { get; set; }

         [DataNames("ANEWTYPE")]
         public string ANewType { get; set; }

         [DataNames("LOAITIN")]
         public string LoaiTin { get; set; }

         [DataNames("AENDTIME")]
         public DateTime? AEndTime { get; set; }

         [DataNames("ANAME")]
         public string Aname { get; set; }

         [DataNames("ACPNYID")]
         public int AAcpnyid { get; set; }

         [DataNames("ADATENOW")]
         public string ADateNow { get; set; }

        [DataNames("AQDCBTT")]
        public string AqdCBTT { get; set; }

        [DataNames("ASTT")]
        public string Astt { get; set; }

      
    }


}
