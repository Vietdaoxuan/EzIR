using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Commons;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Entities.ModelViews;
using CoreLib.Interfaces;
using EzIRSpecialist.Authentication;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models;
using EzIRSpecialist.Models.ViewModel;

using EzIRSpecialist.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace EzIRSpecialist.Controllers
{
    public class ThuVienPhapLuatController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IThuVienPhapLuatContext _thuVienPhapLuatContext;
        private readonly ICommonTypeContext _commonTypeContext;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IErrorHandler _errorHandler;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ICommon _common;
        string _UrlFileCommon;

        private string _username { get; set; }
        private string _roleCode { get; set; }

        public ThuVienPhapLuatController(IConfiguration configuration, IThuVienPhapLuatContext thuVienPhapLuatContext, ICommonTypeContext commonTypeContext, IAppLogger appLogger, IMemoryCache cache, IStringLocalizer<SharedResource> sharedLocalizer, IHttpContextAccessor httpContextAccessor, IErrorHandler errorHandler, IHostingEnvironment hostingEnvironment, ICommon common)
        {
            _configuration = configuration;
            _thuVienPhapLuatContext = thuVienPhapLuatContext;
            _commonTypeContext = commonTypeContext;
            _appLogger = appLogger;
            _cache = cache;
            _sharedLocalizer = sharedLocalizer;
            _httpContextAccessor = httpContextAccessor;
            _errorHandler = errorHandler;
            _hostingEnvironment = hostingEnvironment;
            _common = common;
            _username = _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.USERNAME);
            _roleCode = RoleCode.THU_VIEN_PHAP_LUAT;
            _UrlFileCommon = _configuration.GetSection("UrlFileCommon").Value;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag._UrlFileCommon = _UrlFileCommon + "/";

            var commonType = new CommonTypeViewModel
            {
                ListCategory = "20",
                UserLogin = _username,
                RoleCode = _roleCode
            };
            List<CommonType> listTypeDoc = (List<CommonType>)await _commonTypeContext.Select(commonType);

            var viewmodel = new ThuVienPhapLuatViewModel
            {
                listTypeDoc = listTypeDoc,

            };
            return View(viewmodel);

        }

        [HttpGet]
        [Route(LinksRoute.GetThuVienPhapLuat)]
        [EzAuthorize(RoleCode.THU_VIEN_PHAP_LUAT, view: true)]
        public async Task<IActionResult> ListThuVienPhapLuat(ThuVienPhapLuatViewModel thuVien)
        {
            thuVien.UserLogin = _username;
            thuVien.RoleCode = _roleCode;
            if (!string.IsNullOrEmpty(thuVien.UserLogin) && !string.IsNullOrEmpty(thuVien.RoleCode))
            {

                if (thuVien.YearPub == null)
                {
                    thuVien.YearPub = 0;
                }
                if (thuVien.YearEff == null)
                {
                    thuVien.YearEff = 0;
                }

                var data = await _thuVienPhapLuatContext.Select(thuVien);

                return Json(data.OrderByDescending(yB => yB.DatePub).ThenByDescending(yE => yE.DateEff).ToList());
            }
            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }

        [HttpPost]
        [Route(LinksRoute.UpdateThuVienPhapLuat)]
        [EzAuthorize(RoleCode.THU_VIEN_PHAP_LUAT, view: true, edit: true)]
        public async Task<IActionResult> UpdateLibo(ThuVienPhapLuatViewModel thuVien, IFormFile file)
        {

            thuVien.UserLogin = _username;
            thuVien.RoleCode = _roleCode;
            //String sDate = DateTime.Now.ToString();
            //DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            //String dy = datevalue.Day.ToString();
            //String mn = datevalue.Month.ToString();
            //String yy = datevalue.Year.ToString();

            //if (Convert.ToInt32(dy) <= 9) dy = "0" + dy;
            //if (Convert.ToInt32(mn) <= 9) mn = "0" + mn;


            if (!string.IsNullOrEmpty(thuVien.UserLogin) && !string.IsNullOrEmpty(thuVien.RoleCode))
            {
                //Chỉ TK admin mới sửa/ xóa được biểu mẫu. TK chuyên viên thường không có các chức năng này/ khi muốn sửa hay xóa thì hệ thống hiển thị thông báo không cho phép sửa/ xóa.
                string CheckAccount = _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE);
                if (CheckAccount == "ADMIN" || CheckAccount == "FCF Admin Group")
                {
                    string[] validFileType = { "doc", "docx", "rar", "pdf", "zip" };
                    if (file != null)
                    {
                        string FileExtension = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();
                        if (file.Length > 30 * 1024 * 1024)
                        {
                            return Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["FileIsOver30MB"].Value });
                        }
                        if (validFileType.Contains(FileExtension))
                        {
                            //var result = await this._common.SaveBieuMau(file, this._configuration["TemplatePath"], this._hostingEnvironment.ContentRootPath);

                            var result = await this._common.CopyFile(file, this._configuration["LiboPath"], this._hostingEnvironment, this._configuration);

                            if (result.Message == "UPLOAD_FILE_SUCCESS")
                            {
                                thuVien.FileName = Path.GetFileName(result.Data.ToString());
                                thuVien.Path = this._configuration["PathFile"] + result.Data.ToString();
                                thuVien.Url = this._configuration["UrlSaveFile"] + result.Data.ToString();
                                //this._configuration["TemplatePath"] + "/" + yy + "/" + mn + "/" + dy + "/" + thuVien.FileName;
                            }
                            else
                            {
                                return Json(new CResponseMessage { Code = -1, Message = "Lỗi không tải lên được file" });
                            }
                        }
                        else
                        {
                            return Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["FileIsNotAccepted"].Value });
                        }


                    }

                    else
                    {
                        thuVien.FileName = thuVien.OldFileName;
                        thuVien.Path = thuVien.OldPath;
                        thuVien.Url = thuVien.OldUrl;
                    }

                    var cResponse = await _thuVienPhapLuatContext.Update(thuVien);
                    cResponse.Message = this._sharedLocalizer[cResponse.Message];
                    return Json(cResponse);
                }
                else
                {
                    var responses = new CResponseMessage { Code = -1, Message = "Tài khoản đang dùng không có quyền sửa tài liệu này" };
                    return Json(responses);
                }
            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }

        //[HttpPost]
        //[EzAuthorize(RoleCode.THU_VIEN_PHAP_LUAT, view: true, edit: true)]
        //public async Task<IActionResult> DownloadFile(ThuVienPhapLuatViewModel thuVien)
        //{
        //    thuVien.UserLogin = _username;
        //    thuVien.RoleCode = _roleCode;

        //    //string filePath = Path.Combine(this._hostingEnvironment.ContentRootPath, thuVien.Url.Replace(@"/", @"\"));
        //    if (!string.IsNullOrEmpty(thuVien.UserLogin) && !string.IsNullOrEmpty(thuVien.RoleCode))
        //    {
        //        /*if (!System.IO.File.Exists(filePath))
        //        {
        //            return Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["FileIsNotExist"].Value });
        //        }
        //        var data = await _DoanhNghiepCongBoContext.Select(thuVien);
        //        var ms = new MemoryStream(CheckUtils.ReadFile(filePath));*/
        //        //var fileDownload = CheckFile.ReadFile(template.Path);

        //        //string FileExtension = Path.GetFileName(filePath).Substring(Path.GetFileName(filePath).LastIndexOf('.') + 1).ToLower();
        //        string remoteUri = _configuration["UrlFileCommon"];
        //        string Filename = thuVien.Path;
        //        string myStringWebResource = null;

        //        myStringWebResource = remoteUri + Filename;

        //        var filePath = myStringWebResource;
        //        var myClient = new WebClient();
        //        try
        //        {
        //            byte[] bytes = myClient.DownloadData(myStringWebResource);

        //            var type = Path.GetExtension(myStringWebResource).Replace(".", "");

        //            if (type == "doc" || type == "docx")
        //            {
        //                return File(bytes, "application/msword");
        //            }
        //            if (type == "pdf")
        //            {
        //                return File(bytes, "application/pdf");
        //            }
        //            if (type == "rar")
        //            {
        //                return File(bytes, "application/x-rar-compressed");
        //            }
        //            if (type == "zip")
        //            {
        //                return File(bytes, "application/x-zip-compressed");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return this.Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["File không tồn tại"].Value });
        //        }

        //    }

        //    var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
        //    return Json(response);
        //}

        [HttpPost]
        [Route(LinksRoute.DeleteThuVienPhapLuat)]
        [EzAuthorize(RoleCode.THU_VIEN_PHAP_LUAT, view: true, edit: true)]
        public async Task<IActionResult> DeleteLibo(ThuVienPhapLuatViewModel thuVien)
        {
            thuVien.UserLogin = _username;
            thuVien.RoleCode = _roleCode;


            if (!string.IsNullOrEmpty(thuVien.UserLogin) && !string.IsNullOrEmpty(thuVien.RoleCode))
            {
                //Chỉ TK admin mới sửa/ xóa được biểu mẫu. TK chuyên viên thường không có các chức năng này/ khi muốn sửa hay xóa thì hệ thống hiển thị thông báo không cho phép sửa/ xóa.
                string CheckAccount = _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE);
                if (CheckAccount == "ADMIN" || CheckAccount == "FCF Admin Group")
                {
                    var cResponse = await _thuVienPhapLuatContext.Delete(thuVien);
                    cResponse.Message = this._sharedLocalizer[cResponse.Message];
                    return Json(cResponse);
                }
                else
                {
                    var responses = new CResponseMessage { Code = -1, Message = "Tài khoản đang dùng không có quyền xóa tài liệu này" };
                    return Json(responses);
                }
            }
            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }


        [HttpPost]
        [Route(LinksRoute.InsertThuVienPhapLuat)]
        [EzAuthorize(RoleCode.THU_VIEN_PHAP_LUAT, view: true, edit: true)]
        public async Task<IActionResult> InsertLibo(ThuVienPhapLuatViewModel thuVien, IFormFile file)
        {

            thuVien.UserLogin = _username;
            thuVien.RoleCode = _roleCode;



            if (!string.IsNullOrEmpty(thuVien.UserLogin) && !string.IsNullOrEmpty(thuVien.RoleCode))
            {
                //Chỉ TK admin mới sửa/ xóa được biểu mẫu. TK chuyên viên thường không có các chức năng này/ khi muốn sửa hay xóa thì hệ thống hiển thị thông báo không cho phép sửa/ xóa.
                string CheckAccount = _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE);
                if (CheckAccount == "ADMIN" || CheckAccount == "FCF Admin Group")
                {
                    if (file == null)
                    {
                        return Json(new CResponseMessage { Code = -1, Message = "Chưa chọn File" });
                    }
                    string[] validFileType = { "doc", "docx", "rar", "pdf", "zip" };
                    string FileExtension = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();

                    if (file.Length > 30 * 1024 * 1024)
                    {
                        return Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["FileIsOver30MB"].Value });
                    }
                    if (validFileType.Contains(FileExtension) && file.Length < 30 * 1024 * 1024)
                    {
                        //var result = await this._common.SaveBieuMau(file, this._configuration["TemplatePath"], this._hostingEnvironment.ContentRootPath);

                        var result = await this._common.CopyFile(file, this._configuration["LiboPath"], this._hostingEnvironment, this._configuration);
                        if (result.Message == "UPLOAD_FILE_SUCCESS")
                        {
                            thuVien.FileName = Path.GetFileName(result.Data.ToString());
                            thuVien.Path = this._configuration["PathFile"] + result.Data.ToString();
                            thuVien.Url = this._configuration["UrlSaveFile"] + result.Data.ToString();
                            //this._configuration["TemplatePath"] + "/" + yy + "/" + mn + "/" + dy + "/" + thuVien.FileName;

                            var cResponse = await _thuVienPhapLuatContext.Insert(thuVien);
                            cResponse.Message = this._sharedLocalizer[cResponse.Message];
                            return Json(cResponse);
                        }
                        else
                        {
                            return Json(new CResponseMessage { Code = -1, Message = "Lỗi không tải lên được file" });
                        }
                    }
                    else
                    {
                        return Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["FileIsNotAccepted"].Value });
                    };
                }
                else
                {
                    var responses = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
                    return Json(responses);
                }

            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }

        [HttpGet]
        [Route(LinksRoute.GetThuVienPhapLuatID)]
        [EzAuthorize(RoleCode.THU_VIEN_PHAP_LUAT, view: true)]
        public async Task<IActionResult> GetVanBanByID(ThuVienPhapLuatViewModel thuVien)
        {
            thuVien.UserLogin = _username;
            thuVien.RoleCode = _roleCode;
            if (!string.IsNullOrEmpty(thuVien.UserLogin) && !string.IsNullOrEmpty(thuVien.RoleCode))
            {
                var data = await _thuVienPhapLuatContext.SelectIndex(thuVien);
                return Json(data.ToList());
            }
            return null;
        }

    }
}
