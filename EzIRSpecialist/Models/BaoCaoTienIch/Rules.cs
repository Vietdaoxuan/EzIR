using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.BaoCaoTienIch
{
    public class Rules
    {
        [DataNames("ARuleID")]
        public int ARuleID { get; set; }

        [DataNames("ACompanyType")]
        public int ACompanyType { get; set; }

        [DataNames("ADeadline")]
        public int ADeadline { get; set; }

        [DataNames("ADate")]
        public DateTime? ADate { get; set; }

        [DataNames("ADateExtend")]
        public DateTime? ADateExtend { get; set; }

        [DataNames("ADatelineExtend")]
        public int ADatelineExtend { get; set; }

        [DataNames("AQDCBTT")]
        public string Aqdcbtt { get; set; }

        [DataNames("ACCPLCBTT")]
        public string Accplcbtt { get; set; }

        [DataNames("ARulACreateByeID")]
        public string ACreateBy { get; set; }

        [DataNames("ACreateOn")]
        public DateTime? ACreateOn { get; set; }

        [DataNames("AUpdateBy")]
        public string AUpdateBy { get; set; }

        [DataNames("AUpdateOn")]
        public DateTime? AUpdateOn { get; set; }

        [DataNames("AloaiTin")]
        public string AloaiTin { get; set; }

        [DataNames("AloaiDoanhNghiep")]
        public string AloaiDoanhNghiep { get; set; }

        [DataNames("ADOCTYPE")]
        public int ADocType { get; set; }

        [DataNames("ANEWTYPE")]
        public int ANewType { get; set; }

        [DataNames("AOBJECT")]
        public int AObject { get; set; }

        [DataNames("ADEADLINE_GIAHAN")]
        public int ADeadline_Giahan { get; set; }

        [DataNames("ACATEGORY")]
        public int Acategory { get; set; }

        [DataNames("ATenBieuMauSo")]
        public string ATenBieuMauSo { get; set; }

        [DataNames("ABieuMauSo")]
        public int? ABieuMauSo { get; set; }

        [DataNames("ANIENDOBCTC")]
        public string ANienDoBCTC { get; set; }

        [DataNames("ATenNienDoBCTC")]
        public string ATenNienDoBCTC { get; set; }

        public string LoaiHinh { get; set; }

        [DataNames("ALoaiTaiLieu")]
        public string ATenLoaiTaiLieu { get; set; }

        [DataNames("ADoiTuongApDung")]
        public string ADoiTuongApDung { get; set; }
    }
}
