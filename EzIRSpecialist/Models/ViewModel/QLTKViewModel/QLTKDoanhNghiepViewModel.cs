using CoreLib.Entities;
using EzIRSpecialist.Models.ModelView.QLTKModelView;
using EzIRSpecialist.Models.QuanLyTaiKhoan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ViewModel.QLTKViewModel
{
    public class QLTKDoanhNghiepViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public int? Active { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateOn { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public int? CompanyID { get; set; }

        public string Phone { get; set; }

        public string Note { get; set; }

        public string StockCode { get; set; }

        public int? CompanyType { get; set; }

        public string Expert { get; set; }

        public string CompanyName { get; set; }

        public string CompanyNameEN { get; set; }

        public string UserLogin { get; set; }

        public string RoleCode { get; set; }

        public List<CommonType> listStatus { get; set; }

        public List<CommonType> listNienDo { get; set; }
        
        public List<CommonType> listCompanyType { get; set; }

        public List<ChuyenVienModelView> listChuyenVien { get; set; }

        public List<Company> listMCK { get; set; }

        public List<CompanyEzSearchTemp> listMCKEzSearch { get; set; }

    }
}
