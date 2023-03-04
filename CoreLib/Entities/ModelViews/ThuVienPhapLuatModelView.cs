using CoreLib.DataTableToObject.Attrubutes;
using System;

namespace CoreLib.Entities.ModelViews
{
    public class ThuVienPhapLuatModelView
    {
        [DataNames("AID")]
        public int? id { get; set; }

        [DataNames("ATYPEDOC")]
        public int? typeDoc { get; set; }

        [DataNames("ACOMPANY")]
        public string? company { get; set; }

        [DataNames("ANO")]
        public string? no { get; set; }

        /// <summary>
        /// DatePub: Ngày ban hành
        /// </summary>
        [DataNames("ADATEPUB")]
        public DateTime? datePub { get; set; }

        [DataNames("ATEXTNOTE")]
        public string? textNote { get; set; }

        [DataNames("ADATEEFF")]
        public DateTime? dateEff { get; set; }

        [DataNames("APATH")]
        public string? path{ get; set; }

        [DataNames("ATYPENAME")]
        public string? typeName { get; set; }

        [DataNames("AFILENAME")]
        public string? fileName { get; set; }

        [DataNames("STT")]
        public int? stt { get; set; }
    }
}
