using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ModelView
{
    public class RoleGroupModelView
    {
        [DataNames("AROLENAME")]
        public string RoleName { get; set; }

        [DataNames("AROLETYPE")]
        public string RoleType { get; set; }

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
