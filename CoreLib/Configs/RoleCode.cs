using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Configs
{
    public class RoleCode
    {
        /// <summary>
        /// Chuyên viên
        /// </summary>

        // Quản lý tài khoản chuyên viên
        public const string ACCOUNT_CHUYENVIEN = "QLTKCV";

        // Quản lý tài khoản doanh nghiệp
        public const string ACCOUNT_DOANH_NGHIEP = "QLTKDN";

        // Quản lý doanh nghiệp
        public const string QL_DOANH_NGHIEP = "QLDN";

        // Quản lý doanh nghiệp công bố
        public const string QL_DOANH_NGHIEP_CONG_BO = "QLDNCB";

        // Quản lý biểu mẫu công bố thông tin
        public const string QL_BIEU_MAU_CBTT = "QLBMCBTT";

        // Quản lý Rules
        public const string QL_RULES = "QLR";

        // Đổi mật khẩu
        public const string DOI_MAT_KHAU = "DMK";

        // Nhật ký hoạt động
        public const string NHAT_KY_HOAT_DONG = "NKHD";

        // Nhắc công bố thông tin
        public const string NHAC_CBTT = "NCBTT";

        // Home của Chuyên viên
        public const string HOME = "HOME";

        // Support của Chuyên viên
        public const string SUPPORT = "SUPPORT";

        // Quản lý banner
        public const string QUAN_LY_BANNER = "QLB";

        // Quản lý nhóm
        public const string QUAN_LY_NHOM = "QLN";

        // Phân nhóm người dùng
        public const string PHAN_NHOM_NGUOI_DUNG = "PNND";

        // Phân quyền cho nhóm
        public const string PHAN_QUYEN_CHO_NHOM = "PQCN";

        // Commontype Chuyên viên
        public const string COMMONTYPE = "COMMONTYPE";

        //Báo cáo danh sách tin đăng  theo khách hàng
        public const string DS_TIN_DANG_THEO_KHACH_HANG = "DSTDTKH";

        //Báo cáo danh sách tin đăng backdate
        public const string DS_TIN_DANG_BACKDATE = "DSTDBD";

        // Xem thông tin phê duyệt
        public const string THONG_TIN_PHE_DUYET = "TTPD";

        // Phê duyệt thông tin
        public const string PHE_DUYET_THONG_TIN = "PDTT";

        // thư viện pháp luật
        public const string THU_VIEN_PHAP_LUAT = "TVPL";

        /// <summary>
        /// Khách hàng
        /// </summary>

        // Công bố thông tin
        public const string CBTT = "CBTT";

        // Danh mục biểu mẫu
        public const string DMBM = "DMBM";

        // Thông tin chung
        public const string THONG_TIN_CHUNG = "TTC";

        // Tầm nhìn chiến lựợc
        public const string TAM_NHIN_CHIEN_LUOC = "TNCL";

        // Tổ chức bộ máy quản lý
        public const string TO_CHUC_BO_MAY_QL = "TCBMQL";

        // Thành phần lãnh đạo
        public const string THANH_PHAN_LANH_DAO = "TPLD";

        // Cơ cấu sở hữu
        public const string CO_CAU_SO_HUU = "CCSH";

        // Thị trường khách hàng đối thủ
        public const string THI_TRUONG_KHACH_HANG_DOI_THU = "TTKHDT";

        // Trình độ công nghệ
        public const string TRINH_DO_CONG_NGHE = "TDCN";

        // Năng lực quản lý
        public const string NANG_LUC_QUAN_LY = "NLQL";

        // Thông tin dự án đầu tư
        public const string THONG_TIN_DU_AN_DAU_TU = "TTDADT";

        // Phân tích swot
        public const string PHAN_TICH_SWOT = "PTS";

        // Vị thế doanh nghiệp
        public const string VI_THE_DOANH_NGHIEP = "VTDN";

        //Quên mật khẩu       
        public const string FR_LOGIN = "QMK";

        //Trạng thái Jobs đồng bộ
        public const string JOBS_STATUS = "JS";

        //Tổng quan
        public const string TONG_QUAN = "TQ_TQ";

        public const string TONG_QUAN_THONG_TIN_CHUNG= "TQ_TTC";

        public const string TONG_QUAN_TO_CHUC_BO_MAY_QUAN_LY = "TQ_TCBMQL";

        public const string TONG_QUAN_CO_CAU_SO_HUU = "TQ_CCSH";

        public const string TONG_QUAN_TAM_NHIN_CHIEN_LUOC = "TQ_TNCL";

        public const string TONG_QUAN_THANH_PHAN_LANH_DAO = "TQ_TPLD";

        //Lưu trữ
        public const string LUU_TRU = "LT_LT";

        //Kinh doanh

        public const string KINH_DOANH = "KD_KD";

        public const string KINH_DOANH_THI_TRUONG_KHACH_HANG_DOI_THU = "KD_TTKHDT";

        public const string KINH_DOANH_TRINH_DO_CONG_NGHE = "KD_TDCN";

        public const string KINH_DOANH_NANG_LUC_QUAN_LY = "KD_NLQL";

        public const string KINH_DOANH_THONG_TIN_DU_AN_DAU_TU= "KD_TTDADT";

        public const string KINH_DOANH_PHAN_TICH_SWOT = "KD_PTSWOT";

        public const string KINH_DOANH_VI_THE_DOANH_NGHIEP = "KD_VTDN";

        public const string TAI_CHINH_KE_HOACH_TAI_CHINH = "TC_KHTC";

        public const string PHAN_TICH_CUA_FPTS = "PT_FPTS";

        //Tài chính
        public const string TAI_CHINH = "TC_TC";

        public const string TAI_CHINH_HE_SO_TAI_CHINH = "TC_HSTC";

        public const string TAI_CHINH_CAN_DOI_KE_TOAN = "TC_CDKT";

        public const string TAI_CHINH_KET_QUA_HOAT_DONG_KINH_DOANH = "TC_KQHDKD";

        public const string TAI_CHINH_ICF = "TC_ICF";

        public const string TAI_CHINH_DCF = "TC_DCF";

        public const string DOI_THOAI_DOANH_NGHIEP = "DT_DTDN";
 
        //Cổ phiếu
        public const string CO_PHIEU = "CP_CP";

        public const string LICH_SU_TANG_VON_TRA_CO_TUC = "CP_LSTVTCT";

        public const string CO_PHIEU_LICH_SU_GIAO_DICH = "CP_LSGD";

        public const string CO_PHIEU_DO_THI_KY_THUAT = "CP_DTKT";

        


    }
}
