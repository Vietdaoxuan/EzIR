using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ViewModel
{
    public class RoleViewModel
    {
        [DataNames("AROLECODE")]
        public string RoleCode { get; set; }

        [DataNames("AROLENAME")]
        public string RoleName { get; set; }

        [DataNames("AROLETYPE")]
        public string RoleType { get; set; }

        [DataNames("AORD")]
        public int? Ord { get; set; }
    }
}
