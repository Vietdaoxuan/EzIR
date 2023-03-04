using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Commons
{
    public static class LinksRoute
    {
        public const string GET_GROUP = "/get-group";
        public const string INSERT_GROUP = "/insert-group";
        public const string UPDATE_GROUP = "/update-group";
        public const string DELETE_GROUP = "/delete-group";

        public const string GET_ROLE_GROUP = "/get-role-group";
        public const string INSERT_ROLE_GROUP = "/insert-role-group";

        public const string GET_USER = "/get-user";

        public const string GET_USER_GROUP = "/get-user-group";
        public const string INSERT_USER_GROUP = "/insert-user-group";
        public const string DELETE_USER_GROUP = "/delete-user-group";

        //link chuyên viên
        public const string InsertChuyenVien = "/insert-chuyenvien";
        public const string GetChuyenVien = "/get-chuyenvien";
        public const string GetChuyenVienUpdate = "/get-chuyenvienupdate";
        public const string UpdateChuyenVien = "/update-chuyenvien";
        public const string DeleteChuyenVien = "/delete-chuyenvien";

        //link tk doanh nghiệp
        public const string GetTKDoanhNghiep = "/get-tkdoanhnghiep";
        public const string GetDoanhNghiepUpdate = "/get-doanhnghiepupdate";
        public const string GetMCKTKDoanhNghiep = "/get-mcktkdoanhnghiep";
        public const string InsertTKDoanhNghiep = "/insert-tkdoanhnghiep";
        public const string SendMailTKDoanhNghiep = "/sendmail-tkdoanhnghiep";
        public const string UpdateTKDoanhNghiep = "/update-tkdoanhnghiep";
        public const string DeleteTKDoanhNghiep = "/delete-tkdoanhnghiep";

        //link doanh nghiệp công bố
        public const string GetCongBoDoanhNghiep = "/get-doanhnghiepcongbo";
        public const string InsertCongBoDoanhNghiep = "/insert-doanhnghiepcongbo";
        public const string UpdateCongBoDoanhNghiep = "/update-doanhnghiepcongbo";
        public const string DeleteCongBoDoanhNghiep = "/delete-doanhnghiepcongbo";

        //api CBTT
        public const string API_INSERT_NEW = "/insertnews";
        public static string API_DELETE_NEW = "/deletenews";
        public static string API_UPDATE_NEW = "/updatenews";

        //link biểu mẫu CBTT
        public const string GetBieuMauCBTT = "/get-bieumaucbtt";
        public const string GetIndexBieumauCBTT = "/get-indexbieumaubtt";
        public const string InsertBieuMauCBTT = "/insert-bieumaucbtt";
        public const string UpdateBieuMauCBTT = "/update-bieumaucbtt"; 
        public const string CheckUpdateBieuMauCBTT = "/CheckUpdateBieuMauCBTT";
        public const string DeleteBieuMauCBTT = "/delete-bieumaucbtt";

        //link thư viện pháp luật
        public const string GetThuVienPhapLuat = "/get-thuvienphapluat";
        public const string InsertThuVienPhapLuat = "/insert-thuvienphapluat";
        public const string UpdateThuVienPhapLuat = "/update-thuvienphapluat";
        public const string DeleteThuVienPhapLuat = "/delete-thuvienphapluat";
        public const string GetThuVienPhapLuatID = "/get-thuvienphapluatid";

        //link doanh nghiệp
        public const string GetDoanhNghiep = "/get-doanhnghiep";
        public const string GetCustomerByExpert = "/get-customer-by-expert";
        public const string GetAllDoanhnghiep = "/get-all-doanhnghiep";
        public const string InsertDoanhNghiep = "/insert-doanhnghiep";
        public const string UpdateDoanhNghiep = "/update-doanhnghiep";
        public const string UpdateRoleDoanhNghiep = "/update-roledoanhnghiep";
        public const string UpdateCompanyClient = "/update-comnpanyclient";
        public const string DeleteDoanhNghiep = "/delete-doanhnghiep";

        //link quản lý banner
        public const string GetBanner = "/get-banner";
        public const string InsertBanner = "/insert-banner";
        public const string UpdateBanner = "/update-banner";
        public const string DeleteBanner = "/delete-banner";

        //link nhật ký hoạt động
        public const string GetAction = "/get-action";

        //link Commontype
        public const string GetCommonType = "/get-commontype";
        public const string InsertCommonType = "/insert-commontype";
        public const string UpdateCommonType = "/update-commontype";
        public const string DeleteCommonType = "/delete-commontype";

        ///Quản lý Rules
        public const string GetRules= "/get-rules";
        public const string GetNewsType = "/get-newstype";
        public const string InsertRules = "/insert-rules";
        public const string UpdateRules = "/update-rules";
        public const string DeleteRules = "/delete-rules";

        //Danh sách tin đăng theo khách hàng
        public const string GetListNews = "/get-listnews";

        //Danh sách tin đăng BackDate 
        public const string GetListNewsBackDate = "/get-listnewsbackdate";

        //link nhật ký hoạt động
        public const string GetJobsStatus = "/get-jobsstatus";

    }
}
