using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomer.Commons
{
    public static class LinkRoute
    {
        // Phân tích SWOT
        public const string SAVE_SWOT = "/save-swot";
        public const string UPLOAD_IMAGE = "/upload-image";
        public const string GET_SWOT = "/get-sowt";
        public const string EDIT_SWOT = "/edit-sowt";
        
        // Công Bố Thông Tin
        public const string GetTinCongBo = "/Get-Tin-Cong-Bo";
        public const string InsertTinCongBo = "/Insert-Tin-Cong-Bo";
        public const string UpdateTincongBo = "/Update-Tin-Cong-Bo";
        public const string DeleteTincongBo = "/Delete-Tin-Cong-Bo";
        public const string DownloadTincongBo = "/Download-Tin-Cong-Bo";

        //api CBTT
        public const string API_INSERT_NEW = "/insertnews";
        public const string API_UPDATE_NEW = "/updatenews";
        public static string API_DELETE_NEW = "/deletenews";

        // Danh mục biểu mẫu
        public const string GetBieuMau = "/Get-Bieu-Mau";
        public const string DownloadBieuMau = "/Download-Bieu-Mau";

        //Trinh do cong nghe
        public const string SAVE_TDCN = "/save-tdcn";
        public const string UPLOAD_IMAGE_TDCN = "/upload-image-tdcn";
        public const string GET_TDCN = "/get-tdcn";
        public const string EDIT_TDCN = "/edit-tdcn";

        //Năng lực quản lý
        public const string SAVE_NLQL = "/save-nlql";
        public const string UPLOAD_IMAGE_NLQL = "/upload-image3-nlql";
        public const string GET_NLQL = "/get-nlql";
        public const string EDIT_NLQL = "/edit-nlql";

        //Thông tin dự án đầu tư
        public const string SAVE_TTDADT = "/save-ttdadt";
        public const string UPLOAD_IMAGE_TTDADT = "/upload-image-ttdadt";
        public const string GET_TTDADT = "/get-ttdadt";
        public const string EDIT_TTDADT = "/edit-ttdadt";

        //Vị thế doanh nghiệp
        public const string SAVE_VTDN = "/save-vtdn";
        public const string UPLOAD_IMAGE_VTDN = "/upload-image-vtdn";
        public const string GET_VTDN = "/get-vtdn";
        public const string EDIT_VTDN = "/edit-vtdn";

        //Tầm nhìn chiến lược
        public const string SAVE_TNCL = "/save-tncl";
        public const string UPLOAD_IMAGE_TNCL = "/upload-image-tncl";
        public const string GET_TNCL = "/get-tncl";
        public const string EDIT_TNCL = "/edit-tncl";


        //Cơ cấu sỡ hữu
        public const string GET_CCSH = "/get-ccsh";
       
        public const string SAVE_CCSH = "/save-ccsh";

        // Thông tin chung
        public const string GET_TTC = "/get-thong-tin-chung";
        public const string UPLOAD_LOGO = "/upload-logo";
        public const string SAVE_TTC = "/luu-thong-tin-chung";

        // Tổ chức bộ máy quản lý
        public const string GET_TCBMQL = "/get-to-chuc-bo-may-quan-ly";
        public const string INSERT_TCBMQL = "/insert-to-chuc-bo-may-quan-ly";
        public const string UPDATE_TCBMQL = "/update-to-chuc-bo-may-quan-ly";

        //Thành phần lãnh đạo
        public const string GET_LIST_CHUC_VU = "/get-danh-sach-chuc-vu";
        public const string GET_LIST_MANAGER = "/get-list-manager";
        public const string SAVE_LIST_MANAGER = "/save-list-manager";
        public const string UPDATE_LIST_MANAGER = "/update-list-manager";
        public const string DOWNLOAD_CV = "/Download-CV";

        //Thị trường khách hàng đối thủ
        public const string SAVE_TTKHDT = "/save-ttkhdt";
        public const string UPLOAD_IMAGE_TTKHDT = "/upload-image-ttkhdt";
        public const string GET_TTKHDT = "/get-ttkhdt";
        public const string EDIT_TTKHDT = "/edit-ttkhdt";

        //Thư viện pháp luật
        public const string GET_THUVIENPHAPLUAT = "/get-tvpl";
        public const string DOWNLOAD_TVPL = "/Download-tvpl";
    }
}
