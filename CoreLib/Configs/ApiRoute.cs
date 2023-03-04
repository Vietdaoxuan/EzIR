using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Configs
{
    public static class ApiRoute
    {
        // login
        public const string GET_TOKEN = "/login/get-token";

        // check seed
        public const string GET_SEED = "/login/get-seed";

        // changepassword
        public const string Change_Password = "/ChangePassword";

        //forgotpassword
        public const string Forgot_Password = "/ForgotPassword";
        // infosheet
        public const string GET_INFOSHEET = "/infosheet/get-infosheet";
        public const string INSERT_INFOSHEET = "/infosheet/insert-infosheet";
        public const string UPDATE_INFOSHEET = "/infosheet/update-infosheet";

        // Công bố thông tin
        public const string Get_CongBoThongTin = "/CBTT/GetCongBoThongTin";
        public const string Insert_TinCongBo = "/CBTT/Insert-CongBoThongTin";
        public const string Update_TinCongBo = "/CBTT/Update-CongBoThongTin";
        public const string Delete_TinCongBo = "/CBTT/Delete-CongBoThongTin";

        // Danh mục biểu mẫu
        public const string Get_DanhMucBieuMau = "/DMBM/GetDanhMucBieuMau";

        // Tổ chức bộ máy quản lý
        public const string Get_ToChucBoMayQuanLy = "/TCBMQL/GetToChucBoMay";
        public const string Insert_ToChucBoMayQuanLy = "/TCBMQL/Insert-ToChucBoMay";
        public const string Update_ToChucBoMayQuanLy = "/TCBMQL/Update-ToChucBoMay";

        // Thành phần lãnh đạo
        public const string Get_KnowLedgeLevel = "/TPLD/GetKnowLedgeLevel";
        public const string Get_Chucvu = "/TPLD/GetChucvu";
        public const string Get_Manager = "/TPLD/GetManager";
        public const string Save_Manager = "/TPLD/Insert-Manager";
        public const string Update_Manage = "/TPLD/Update-Manager";

        // CommonType
        public const string CommonCategory = "/common/CommonCategory";

        // Company
        public const string GET_COMPANY = "/company/get-info-company";

        // Role Company
        public const string SPEZIR_ROLE_GET_BY_COMPANY = "/company/get-role-company";
        // Develop Progress - Các mốc lịch sử
        public const string GET_DEVELOP_PROGRESS = "/company/get-info-develop-progress";
        public const string INSERT_COMPANY_DEVELOP_PROGRESS = "/company/insert-company-develop-progress";

        // Cơ cấu sỡ hữu
        // Công ty con/ công ty liên kết
        public const string GetCongTy = "/subcompany/get-info-subcompany";
        public const string Get_Ministry = "/ministry/get-info-ministry";
        public const string Get_SubCompanyType = "/subcompany/get-info-subcompanytype";
        public const string Insert_Company_ShareHolder = "/subcompany/insert-company-shareholder";

        //Thư viện pháp luật
        public const string GetThuVienPhapLuat = "/TVPL/get-info-liboflaw";
    }
}
