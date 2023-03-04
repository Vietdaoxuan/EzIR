using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.Models.ModelViews
{
    public class CustomerModelView
    {
        [DataNames("AUSERNAME")]
        public string Username { get; set; }

        [DataNames("APASSWORD")]
        public string Password { get; set; }

        [DataNames("AACTIVE")]
        public bool? Active { get; set; }

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

        [DataNames("APHONE")]
        public string Phone { get; set; }

        [DataNames("ANOTE")]
        public string Note { get; set; }

        [DataNames("ACPNYID")]
        public int CpnyID { get; set; }

        [DataNames("ASTOCKCODE")]
        public string StockCode { get; set; }

        [DataNames("STOCKNAME")]
        public string StockName { get; set; }

        [DataNames("STOCKNAMEEN")]
        public string StockNameEn { get; set; }

        [DataNames("ACOMPANYTYPE")]
        public int CompanyType { get; set; }

        [DataNames("COMPANYTYPE")]
        public string CompanyTypeName { get; set; }

        [DataNames("ATYPENAMEEN")]
        public string CompanyTypeNameEN { get; set; }

        [DataNames("ANAME")]
        public string AMaCK { get; set; }

        [DataNames("EMAIL_SPECIALIST")]
        public string EmailSpecialist { get; set; }

        [DataNames("EMAIL_CC")]
        public string EmailSpecialistCC { get; set; }

        [DataNames("EXPERTNAME")]
        public string ExpertName { get; set; }

        [DataNames("EXPERTPHONE")]
        public string ExpertPhone { get; set; }
    }
}
