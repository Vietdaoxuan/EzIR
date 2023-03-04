using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ModelView.QLTKModelView
{
    public class DoanhNghiepModelView
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

        [DataNames("ATYPENAME")]
        public string TypeName { get; set; }

        [DataNames("AEXPERT")]
        public string Expert { get; set; }

        [DataNames("ANAME")]
        public string CompanyName { get; set; }

        [DataNames("ALEVEL")]
        public int? Level { get; set; }

        [DataNames("AROLECODE")]
        public string RoleCode { get; set; }

        [DataNames("LISTROLENAME")]
        public string ListRoleName { get; set; }

        [DataNames("LISTROLECODE")]
        public string ListRoleCode { get; set; }

        [DataNames("ASTATUS")]
        public int? RoleStatus { get; set; }

        [DataNames("ACPNYSTATUS")]
        public int? CompanyStatus { get; set; }

        [DataNames("ACPNYSTATUSSTRING")]
        public string CompanyStatusString { get; set; }

        [DataNames("COMPANYNOTE")]
        public string CompanyNote { get; set; }

        [DataNames("AMaCK")]
        public string StockName { get; set; }

        [DataNames("AROLENAME")]
        public string RoleName { get; set; }

        [DataNames("AISDELETE")]
        public string IsDelete { get; set; }

        [DataNames("AFISCALYEAR")]
        public DateTime? FiscYear { get; set; }

        public string RoleCodeHtml { get; set; }

        public int? FiscYearDay { get; set; }

        public int? FiscYearMonth { get; set; }

        [DataNames("ANIENDOBCTC")]
        public int? NiendoBCTC { get; set; }

        [DataNames("NIENDOBCTC")]
        public string NiendoBCTCstring { get; set; }

    }
}
