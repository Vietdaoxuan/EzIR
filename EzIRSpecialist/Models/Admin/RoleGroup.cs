using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.Admin
{
    public class RoleGroup
    {
        [DataNames("AROLECODE")]
        public string RoleCode { get; set; }

        [DataNames("AGROUPCODE")]
        public string GroupCode { get; set; }

        [DataNames("AVIEW")]
        public bool? View { get; set; }

        [DataNames("AEDIT")]
        public bool? Edit { get; set; }

        [DataNames("ADELETE")]
        public bool? Delete { get; set; }

        [DataNames("ASPECIAL")]
        public bool? Special { get; set; }
    }
}
