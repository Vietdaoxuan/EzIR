using CoreLib.Entities;
using EzIRSpecialist.Models.BaoCaoTienIch;
using EzIRSpecialist.Models.QuanLyTaiKhoan;
using EzIRSpecialist.Models.ModelView.QLTKModelView;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ViewModel
{
    public class NhacCongBoThongTinViewModel
    {
        public string AStockCode { get; set; }

        public string FileTemplate { get; set; }

        public List<CommonType> ListLoaiDN { get; set; }

        public List<Company>  ListMaCK { get; set; }

        public List<ChuyenVienModelView> ListChuyenVien { get; set; }

        public NhacCongBoThongTin nhacCongBoThongTin { get; set; }

        public IEnumerable<CommonType> ListLoaiHinhDN { get; set; }

        public IEnumerable<CommonType> ListNienDoBCTC { get; set; }

        public IEnumerable<CommonType> ListDoiTuongAD { get; set; }

        public string TypeID { get; set; }
        public string UserName { get; set; }

        public string RoleCode { get; set; }

        public int? ALoaiDoanhNghiep { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FormDate { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ToDate { get; set; }

        public string EmailContent { get; set; }

        public string Subject { get; set; }

        public int ListCompanyIdMail { get; set; }

        [DisplayFormat(DataFormatString = "{dd/MM/yyyy}")]
        public DateTime AEndTime { get; set; }

        public int ACompanyType { get; set; }

        public string Aname { get; set; }

        public string AExpert { get; set; }

        public int? Alevel { get; set; }
        public int? AniendoBCTC { get; set; }

        public string AObject { get; set; }
    }
}
