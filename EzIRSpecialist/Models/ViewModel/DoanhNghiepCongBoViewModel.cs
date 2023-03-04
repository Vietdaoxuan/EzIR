using CoreLib.Entities;
using EzIRSpecialist.Models.QuanLyTaiKhoan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ViewModel.QuanLyDoanhNghiepCongBo
{
    public class DoanhNghiepCongBoViewModel
    {
        public int? CompanyID { get; set; }

        public int? Sid { get; set; }

        public string StockCode { get; set; }

        public string ListNewID { get; set; }

        public string ListRegionID { get; set; }

        public string ListUsername { get; set; }

        public string Language { get; set; }

        public int? CompanyType { get; set; }

        public int? NewType { get; set; }

        public int? DocType { get; set; }

        public int? UserType { get; set; }

        public int? RegionID { get; set; }

        public int? NewID { get; set; }

        public int? AccountType { get; set; }

        public int? Year { get; set; }

        public string Expert { get; set; }

        public string UserPost { get; set; }

        public string Title { get; set; }

        public string Path { get; set; }

        public string Url { get; set; }

        public string OldUrl { get; set; }

        public string OldPath { get; set; }

        public string OldFileName { get; set; }

        public string FileName { get; set; }
        
        public string UserLogin { get; set; }

        public string RoleCode { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? DatePub { get; set; }

        public DateTime? ToDate { get; set; }

        public DateTime? DateCreate { get; set; }

        public int? IsDelete { get; set; }

        public int? IsBackDate { get; set; }

        public int? IsPublic { get; set; }

        public List<CommonType> listCompanyType { get; set; }

        public List<CommonType> listloaitl { get; set; }

        public List<CommonType> listloaitin { get; set; }

        public List<CommonType> listRegion { get; set; }

        public List<Company> listMCK { get; set; }
    }
}
