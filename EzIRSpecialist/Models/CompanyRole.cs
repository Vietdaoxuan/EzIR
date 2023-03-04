using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models
{
    public class CompanyRole
    {
        [DataNames("ACPNYID")]
        public int CompanyID { get; set; }

        [DataNames("AROLECODE")]
        public string RoleCompany { get; set; }

        [DataNames("ASTATUS")]
        public bool? Status { get; set; }

        [DataNames("ACREATEBY")]
        public string CreateBy { get; set; }

        [DataNames("ACREATEON")]
        public DateTime? CreateOn { get; set; }

        [DataNames("AUPDATEBY")]
        public string UpdateBy { get; set; }

        [DataNames("AUPDATEON")]
        public DateTime? UpdateOn { get; set; }
    }
}
