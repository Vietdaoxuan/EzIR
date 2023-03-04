using CoreLib.Entities;
using EzIRSpecialist.Models.BaoCaoTienIch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ViewModel
{
    public class RulesViewModel
    {
        public string UserName { get; set; }
        public string RoleCode { get; set; }

        public IEnumerable<CommonType> ListLoaiTaiLieu { get; set; }

        public IEnumerable<CommonType> ListLoaiHinhDN { get; set; }

        public IEnumerable<CommonType> ListTin { get; set; }

        public IEnumerable<CommonType> ListNienDoBCTC { get; set; }

        public IEnumerable<CommonType> ListDoiTuongAD { get; set; }

        public int? ARuleID { get; set; }

        //Loại hình
        public int? ACompanyType { get; set; }

        //Loại Tài liệu
        public int? ADocType { get; set; }

        //Loại tin
        public int? ANewType { get; set; }

        //Số ngày nhắc công bố thông tin
        public int? ADeadline { get; set; }

        //Ngày tính từ thời điểm
        public DateTime? ADate { get; set; }

        //Số ngày gia hạn nhắc công bố thông tin
        public int? ADatelineExtend { get; set; }

        public int? ADeadline_Giahan { get; set; }

        //Ngày tính từ thời điểm ra hạn
        public DateTime? ADateExtend { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FormDate { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ToDate { get; set; }

        //Quy định công bố thông tin
        public string Aqdcbtt { get; set; }

        //Căn cứ pháp lý công bố thông tin
        public string Accplcbtt { get; set; }

        //Đối tượng áp dụng
        public int? AObject { get; set; }

        public string ACreateBy { get; set; }

        public DateTime? ACreateOn { get; set; }

        public string AUpdateBy { get; set; }

        public DateTime? AUpdateOn { get; set; }

        public string AloaiTaiLieu { get; set; }

        public string AloaiDoanhNghiep { get; set; }

        public string ADoiTuongApDung { get; set; }

        public Rules Rules { get; set; }

        public string Acategory { get; set; }

        public string LoaiHinhIDs { get; set; }

        public int? NienDo { get; set; }

        public int? BieuMauSo { get; set; }

        public string StrADate { get; set; }

        public string StrADateExtend { get; set; }

        public int LoaiBieuMau { get; set; }
    }
}
