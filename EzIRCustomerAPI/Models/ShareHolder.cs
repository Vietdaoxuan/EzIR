using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.Models
{
    public class ShareHolder
    {
        /// <summary>
        /// Cổ đông lớn
        /// </summary>
        [DataNames("ACPNYID")]
        public int? acpnyid { get; set; }

        [DataNames("ASHNAME")]
        public string ashname { get; set; }

        [DataNames("P_ACURSHARENO")]
        public int acurshareno { get; set; }

        [DataNames("P_ACURSHARERATE")]
        public double acursharerate { get; set; }

        [DataNames("ASHERID")]
        public int asherid { get; set; }

        [DataNames("AAPPROVE")]
        public int aapprove { get; set; }

        [DataNames("ANOTE")]
        public string anote { get; set; }
        
        [DataNames("AORDER")]
        public int? aorder { get; set; }

        public string RoleCode { get; set; }

        public string UserName { get; set; }

    }
}
