using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ModelView
{
    public class UserGroupModelView
    {
        [DataNames("AUSERNAME")]
        public string Username { get; set; }

        [DataNames("AACTIVE")]
        public int Active { get; set; }

        [DataNames("ACREATEBY")]
        public string CreateBy { get; set; }

        [DataNames("ACREATEON")]
        public DateTime CreateOn { get; set; }

        [DataNames("AFULLNAME")]
        public string FullName { get; set; }

        [DataNames("AEMAIL")]
        public string Email { get; set; }

        [DataNames("AREGIONID")]
        public string RegionID { get; set; }

        [DataNames("ISINGROUP")]
        public bool IsInGroup { get; set; }
    }
}
