using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities
{
    public class ManagerOrg
    {
        [DataNames("AMORGID")]
        public int AMOrgID { get; set; }

        [DataNames("AMORGCODE")]
        public string AMOrgCode { get; set; }

        [DataNames("AMORGDESC")]
        public string AMOrgDesc { get; set; }

        [DataNames("AMORGGROUPID")]
        public int AMOrgGroupID { get; set; }

        [DataNames("ABOLD")]
        public int ABold { get; set; }

        [DataNames("AMORGDESCEN")]
        public string AMOrgDescEN { get; set; }
    }
}
