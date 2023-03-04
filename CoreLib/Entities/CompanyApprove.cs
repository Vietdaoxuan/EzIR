using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities
{
    public class CompanyApprove
    {
        [DataNames("acpnyid")]
        public int? CpnyID { get; set; }

        [DataNames("namevn")]
        public string NameVN { get; set; }
    }
}
