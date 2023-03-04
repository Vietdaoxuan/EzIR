using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities.ModelViews
{
    public class ToChucBoMayQuanLyModelView
    {
        [DataNames("AID")]
        public int? ID { get; set; }

        [DataNames("AORGMODELID")]
        public int? OrgModelID { get; set; }

        [DataNames("ACPNYID")]
        public int? CpnyID { get; set; }

        [DataNames("AORGMODELDESC")]
        public string OrgModelDesc { get; set; }

        [DataNames("AORGMODELPATH")]
        public string OrgModelPath { get; set; }

        [DataNames("AORGTYPE")]
        public int? OrgType { get; set; }

        [DataNames("ANOTE")]
        public string NOTE { get; set; }

        [DataNames("AAPPROVE")]
        public int? APPROVE { get; set; }
    }
}
