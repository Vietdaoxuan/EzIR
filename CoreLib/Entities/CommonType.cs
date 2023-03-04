using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLib.Entities
{
    public class CommonType
    {
        [DataNames("ATYPEID")]
        public int TypeID { get; set; }

        [DataNames("ATYPECODE")]
        public string TypeCode { get; set; }

        [DataNames("ATYPENAME")]
        public string TypeName { get; set; }

        [DataNames("ATYPENAMEEN")]
        public string TypeNameEN { get; set; }

        [DataNames("ATYPEDESCRIPTION")]
        public string TypeDescription { get; set; }

        [DataNames("AORD")]
        /// <summary>
        /// Ord : QuangKS điều chỉnh từ string => int
        /// Date: 2021-11-23
        /// </summary>
        public int Ord { get; set; }

        [DataNames("ACATEGORY")]
        public int Category { get; set; }

        [DataNames("APARENTID")]
        public int ParentID { get; set; }
    }
}
