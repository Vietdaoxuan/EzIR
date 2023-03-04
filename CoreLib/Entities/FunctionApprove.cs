using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities
{
    public class FunctionApprove
    {
        [DataNames("acpnyid")]
        public int? CpnyID { get; set; }

        [DataNames("amenuid")]
        public int? MenuID { get; set; }

        [DataNames("afunction")]
        public string Function { get; set; }

        [DataNames("astatus")]
        public int? Status { get; set; }

        [DataNames("alevelid")]
        public string LevelID { get; set; }

        [DataNames("adetaillevelid")]
        public string DetailLevelID { get; set; }
    }
}
