using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLib.Entities.ViewModels
{
    public class CongBoThongTinViewModel
    {
        public int? CompanyID { get; set; }

        public int? Sid { get; set; }

        public string StockCode { get; set; }

        public string ListNewID { get; set; }

        public string ListRegionID { get; set; }

        public string ListUsername { get; set; }

        public int? CompanyType { get; set; }

        public int? NewType { get; set; }

        public int? DocType { get; set; }

        public int? UserType { get; set; }

        public int? RegionID { get; set; }

        public int? NewID { get; set; }

        public int? AccountType { get; set; }

        public int? Year { get; set; }

        public string Expert { get; set; }

        public string Title { get; set; }

        public string Path { get; set; }

        public string Url { get; set; }

        public int? Language { get; set; }

        public string OldFileName { get; set; }

        public string OldUrl { get; set; }

        public string OldPath { get; set; }

        public string FileName { get; set; }

        public string UserLogin { get; set; }
        public string CreateBy { get; set; }

        public string RoleCode { get; set; }

        public string FromDate { get; set; }

        public DateTime? DatePub { get; set; }

        public string ToDate { get; set; }

        public int? IsDelete { get; set; }

        public int? IsBackDate { get; set; }

        public List<CommonType> listloaitl { get; set; }

        public List<CommonType> listloaitin { get; set; }

        public EmailSettings emailSettings { get; set; }

    }
}
