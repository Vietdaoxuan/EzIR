using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.Admin
{
    public class Group
    {
        [DataNames("AGROUPCODE")]
        public string GroupCode { get; set; }

        [DataNames("AGROUPNAME")]
        public string GroupName { get; set; }
    }
}
