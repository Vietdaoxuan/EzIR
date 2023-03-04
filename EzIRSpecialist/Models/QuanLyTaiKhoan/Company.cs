using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.QuanLyTaiKhoan
{
    public class Company
    {
        [DataNames("ACPNYID")]
        public int? CompanyID { get; set; }

        [DataNames("ASTOCKCODE")]
        public string StockCode { get; set; }

        [DataNames("AEXCHANGE")]
        public string Exchange { get; set; }

        [DataNames("ACOMPANYTYPE")]
        public int? CompanyType { get; set; }

        [DataNames("ATYPENAME")]
        public string TypeName { get; set; }

        [DataNames("ANAME")]
        public string Name { get; set; }

        [DataNames("ANAMEEN")]
        public string NameEn { get; set; }

        [DataNames("AEXPERT")]
        public string Expert { get; set; }

        [DataNames("AFISCALYEAR")]
        public DateTime? FiscalYear { get; set; }

        [DataNames("ALEVEL")]
        public int? Level { get; set; }

        [DataNames("ANOTE")]
        public string Note { get; set; }

        [DataNames("ACREATEBY")]
        public string CreateBy { get; set; }

        [DataNames("ACREATEON")]
        public DateTime? CreateOn { get; set; }

        [DataNames("AUPDATEBY")]
        public string UpdateBy { get; set; }

        [DataNames("AUPDATEON")]
        public DateTime? UpdateOn { get; set; }

        [DataNames("AMaCK")]
        public string AMack { get; set; }
    }
}
