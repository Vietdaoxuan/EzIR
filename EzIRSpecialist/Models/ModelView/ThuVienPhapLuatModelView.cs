using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ModelView
{
    public class ThuVienPhapLuatModelView
    {
        [DataNames("AID")]
        public string ID { get; set; }

        [DataNames("ATYPENAME")]
        public string TypeName { get; set; }

        [DataNames("ATYPEDOC")]
        public int? TypeDoc { get; set; }

        [DataNames("ACOMPANY")]
        public string Company { get; set; }

        [DataNames("ANO")]
        public string No { get; set; }

        [DataNames("ADATEPUB")]
        public DateTime DatePub { get; set; }

        [DataNames("ATEXTNOTE")]
        public string TextNote { get; set; }

        [DataNames("ADATEEFF")]
        public DateTime DateEff { get; set; }

        [DataNames("AFILENAME")]
        public string FileName { get; set; }

        [DataNames("APATH")]
        public string Path { get; set; }

       
    }
}
