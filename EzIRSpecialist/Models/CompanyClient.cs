using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models
{
    public class CompanyClient
    {
        [DataNames("ACPNYID")]
        public int CompanyID { get; set; }

        [DataNames("ASTOCK_CODE")]
        public string StockCode { get; set; }

        [DataNames("AISDELETE")]
        public int IsDelete { get; set; }

        [DataNames("ACREATEBY")]
        public string CreateBy { get; set; }

        [DataNames("ACREATEON")]
        public DateTime? CreateOn { get; set; }

        [DataNames("AUPDATEBY")]
        public string UpdateBy { get; set; }

        [DataNames("AUPDATEON")]
        public DateTime? UpdateOn { get; set; }
    }
}
