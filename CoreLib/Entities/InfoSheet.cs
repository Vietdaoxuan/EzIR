using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities
{
    public class InfoSheet
    {
        [DataNames("AINFOID")]
        public int? InfoID { get; set; }

        [DataNames("AID")]
        public int? ID { get; set; }

        [DataNames("ATITLE")]
        public string Title { get; set; }

        [DataNames("ACONTENT")]
        public string Content { get; set; }

        [DataNames("APARENTID")]
        public int? ParentID { get; set; }

        [DataNames("AMENUID")]
        public int? MenuID { get; set; }

        [DataNames("AHASCONTENT")]
        public int? HasContent { get; set; }

        [DataNames("AMODULENUMBER")]
        public int? ModuleNumber { get; set; }

        [DataNames("APOSTDATE")]
        public DateTime? PostDate { get; set; }

        [DataNames("AAUTHORID")]
        public int? AuthorID { get; set; }

        [DataNames("AMODIFIERID")]
        public int? ModifierID { get; set; }

        [DataNames("ACPNYID")]
        public int? CpnyID { get; set; }

        [DataNames("AISAPPROVED")]
        public int? IsApproved { get; set; }

        [DataNames("AGUIDELINES")]
        public int? GuideLines { get; set; }

        [DataNames("ALANGUAGE")]
        public string Language { get; set; }

        [DataNames("ANOTE")]
        public string Note { get; set; }

        [DataNames("ALEVELID")]
        public string Levelid { get; set; }

        [DataNames("AAPPROVE")]
        public int? Approve { get; set; }
    }
}
