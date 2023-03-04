using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.Admin
{
    public class UserGroup
    {
        [DataNames("AUSERNAME")]
        public string Username { get; set; }

        [DataNames("AGROUPCODE")]
        public string GroupCode { get; set; }

        [DataNames("ACREATEBY")]
        public string CreateBy { get; set; }

        [DataNames("ACREATEON")]
        public DateTime? CreateOn { get; set; }
    }
}
