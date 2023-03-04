using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities
{
    public class CompanyType
    {
        [DataNames("ATYPE")]
        public int AType { get; set; }

        [DataNames("ADESC")]
        public string ADesc { get; set; }
    }
}
