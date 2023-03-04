using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ModelView.QLTKModelView
{
    public class QLTKDoanhNghiepModelView
    {
        [DataNames("AUSERNAME")]
        public string Username { get; set; }

        [DataNames("APASSWORD")]
        public string Password { get; set; }

        [DataNames("AACTIVE")]
        public int? Active { get; set; }

        [DataNames("ACREATEBY")]
        public string CreateBy { get; set; }

        [DataNames("ACREATEON")]
        public DateTime? CreateOn { get; set; }

        [DataNames("AUPDATEBY")]
        public string UpdateBy { get; set; }

        [DataNames("AUPDATEON")]
        public DateTime? UpdateOn { get; set; }

        [DataNames("AFULLNAME")]
        public string FullName { get; set; }

        [DataNames("AEMAIL")]
        public string Email { get; set; }

        [DataNames("ACPNYID")]
        public int? CompanyID { get; set; }

        [DataNames("APHONE")]
        public string Phone { get; set; }

        [DataNames("ANOTE")]
        public string Note { get; set; }

        [DataNames("ASTOCKCODE")]
        public string StockCode { get; set; }

        [DataNames("ACOMPANYTYPE")]
        public int? CompanyType { get; set; }

        [DataNames("COMPANYTYPE")]
        public string CompanyTypeName { get; set; }

        [DataNames("AEXPERT")]
        public string Expert { get; set; }

        [DataNames("ANAME")]
        public string CompanyName { get; set; }

        [DataNames("ACTIVE")]
        public string ActiveName { get; set; }
    }
}
