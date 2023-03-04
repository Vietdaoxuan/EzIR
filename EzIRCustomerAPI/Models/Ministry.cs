using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.Models
{
    public class Ministry
    {
        /// <summary>
        ///Ngành, lĩnh vực
        /// </summary>
        [DataNames("AMINISTRYID")]
        public int aministryid { get; set; }

        [DataNames("ANAMEVN")]
        public string anamevn { get; set; }

        [DataNames("ANAMEEN")]
        public string anameen { get; set; }

        [DataNames("APARENTID")]
        public int aparentid { get; set; }

        public string UserName { get; set; }

        public string RoleCode { get; set; }

    }
}
