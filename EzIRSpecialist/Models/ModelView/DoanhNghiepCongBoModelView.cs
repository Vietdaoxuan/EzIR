using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ModelView.QuanLyDoanhNghiepCongBo
{
    public class DoanhNghiepCongBoModelView
    {
        [DataNames("USERPOST")]
        public string UserPost { get; set; }

        [DataNames("EMAIL")]
        public string Email { get; set; }

        [DataNames("REGION")]
        public string Region { get; set; }

        [DataNames("DOCTYPE")]
        public string DocTypeText { get; set; }

        [DataNames("NEWSTYPE")]
        public string NewsTypeText { get; set; }

        [DataNames("ADOCTYPE")]
        public int? DocType { get; set; }

        [DataNames("ANEWTYPE")]
        public int? NewTypeCode { get; set; }

        [DataNames("ANEWID")]
        public int? NewID { get; set; }

        [DataNames("ASID")]  
        public int? SID { get; set; }

        [DataNames("ATITLE")]
        public string Title { get; set; }

        [DataNames("ADATEPUB")]
        public DateTime? DatePublic { get; set; }

        [DataNames("ADATECREATE")]
        public DateTime? DateCreate { get; set; }

        [DataNames("USERTYPE")]
        public int? UserType { get; set; }

        [DataNames("AEXPERT")]
        public string Expert { get; set; }

        [DataNames("COMPANYTYPE")]
        public int? CompanyType { get; set; }

        [DataNames("LTK")]
        public string AccountType { get; set; }

        [DataNames("AYEAR")]
        public string Year { get; set; }

        [DataNames("ASTOCKCODE")]
        public string StockCode { get; set; }

        [DataNames("AFILENAME")]
        public string FileName { get; set; }

        [DataNames("APATH")]
        public string Path { get; set; }

        [DataNames("AURL")]
        public string Url { get; set; }

        [DataNames("ACPNYID")]
        public string CompanyID { get; set; }

        [DataNames("ACREATEON")]
        public DateTime? CreateOn { get; set; }

    }
}
