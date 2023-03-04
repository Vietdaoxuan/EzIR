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
using EzIRSpecialist.Models.ViewModel.QuanLyDoanhNghiepCongBo;
using EzIRSpecialist.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace EzIRSpecialist.Controllers
{
    public class QuanLyBieuMauCBTTController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ITemplateContext _templateContext;
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

        public QuanLyBieuMauCBTTController(IConfiguration configuration, ITemplateContext templateContext, ICommonTypeContext commonTypeContext, IAppLogger appLogger, IMemoryCache cache, IStringLocalizer<SharedResource> sharedLocalizer, IHttpContextAccessor httpContextAccessor, IErrorHandler errorHandler, IHostingEnvironment hostingEnvironment, ICommon common)
        {
            _configuration = configuration;
            _templateContext = templateContext;
            _commonTypeContext = commonTypeContext;
            _appLogger = appLogger;
            _cache = cache;
            _sharedLocalizer = sharedLocalizer;
            _httpContextAccessor = httpContextAccessor;
            _errorHandler = errorHandler;
            _hostingEnvironment = hostingEnvironment;
            _common = common;
            _username = _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.USERNAME);
            _roleCode = RoleCode.QL_BIEU_MAU_CBTT;
            _UrlFileCommon = _configuration.GetSection("UrlFileCommon").Value;
        }

        [EzAuthorize(RoleCode.QL_BIEU_MAU_CBTT, view: true)]
        public async Task<IActionResult> Index()
        {
            ViewBag._UrlFileCommon = _UrlFileCommon + "/";
            List<CommonType> listCompanyType = new List<CommonType>();
            List<CommonType> listTemplateType = new List<CommonType>();
            var commonType = new CommonTypeViewModel
            {
                ListCategory = "3,7",
                UserLogin = _username,
                RoleCode = _roleCode
            };
            var listCommontype = await _commonTypeContext.Select(commonType);

            foreach (var obj in listCommontype.ToList())
            {
                if (obj.Category == ConstCommonTypes.NHOM_LOAI_HINH)
                {
                    listCompanyType.Add(obj);
                }
                if (obj.Category == ConstCommonTypes.NHOM_LOAI_BIEUMAU)
                {
                    listTemplateType.Add(obj);
                }
            }

            var viewmodel = new TemplateViewModel
            {
                listCompanyType = listCompanyType,
                listTemplateType = listTemplateType,
            };
            return View(viewmodel);
        }

        [EzAuthorize(RoleCode.QL_BIEU_MAU_CBTT, view: true)]
        public async Task<IActionResult> GetCompanyType(TemplateViewModel template)
        {
            List<CommonType> listCompanyType = new List<CommonType>();
            List<CommonType> listTemplateType = new List<CommonType>();
            var commonType = new CommonTypeViewModel
            {
                ListCategory = "3,7",
                UserLogin = _username,
                RoleCode = _roleCode
            };
            var listCommontype = await _commonTypeContext.Select(commonType);

            foreach (var obj in listCommontype.ToList())
            {
                if (obj.Category == ConstCommonTypes.NHOM_LOAI_HINH)
                {
                    listCompanyType.Add(obj);
                }
                if (obj.Category == ConstCommonTypes.NHOM_LOAI_BIEUMAU)
                {
                    listTemplateType.Add(obj);
                }
            }

            var viewmodel = new TemplateViewModel
            {
                listCompanyType = listCompanyType,
                listTemplateType = listTemplateType,
            };
            return Json(viewmodel);
        }

        [HttpGet]
        [Route(LinksRoute.GetBieuMauCBTT)]
        [EzAuthorize(RoleCode.QL_BIEU_MAU_CBTT, view: true)]
        public async Task<IActionResult> ListBieuMau(TemplateViewModel template)
         {
            template.UserLogin = _username;
            template.RoleCode = _roleCode;
            if (!string.IsNullOrEmpty(template.UserLogin) && !string.IsNullOrEmpty(template.RoleCode))
            {
                var data = await _templateContext.Select(template);
                return Json(data);
            }
            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }

        [HttpPost]
        [Route(LinksRoute.GetIndexBieumauCBTT)]
        [EzAuthorize(RoleCode.QL_BIEU_MAU_CBTT, view: true)]
        public  IActionResult InDexBieuMau(TemplateViewModel template)
        {
            template.UserLogin = _username;
            template.RoleCode = _roleCode;
            if (!string.IsNullOrEmpty(template.UserLogin) && !string.IsNullOrEmpty(template.RoleCode))
            {
                var data =  _templateContext.SelectIndex(template);
                return Json(data);
            }
            return null;
        }

        [HttpPost]
        [Route(LinksRoute.InsertBieuMauCBTT)]
        [EzAuthorize(RoleCode.QL_BIEU_MAU_CBTT, view: true, edit: true)]
        public async Task<IActionResult> InsertTemplate(TemplateViewModel template, IFormFile file)
        {
            if (template.CompanyTypelist == null)
            {
                return Json(new CResponseMessage { Code = -1, Message = "Chưa chọn loại hình doanh nghiệp" });
            }
            template.UserLogin = _username;
            template.RoleCode = _roleCode;
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            //string filePath = @"{yy}\{mm}\{dd}\{filaname}";

            String dy = datevalue.Day.ToString();
            String mn = datevalue.Month.ToString();
            String yy = datevalue.Year.ToString();

            if (Convert.ToInt32(dy) <= 9) dy = "0" + dy;
            if (Convert.ToInt32(mn) <= 9) mn = "0" + mn;


            if (!string.IsNullOrEmpty(template.UserLogin) && !string.IsNullOrEmpty(template.RoleCode))
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
                        
                        var result = await this._common.CopyFile(file, this._configuration["TemplatePath"], this._hostingEnvironment, this._configuration);
                        if (result.Message == "UPLOAD_FILE_SUCCESS")
                        {
                            template.FileName = Path.GetFileName(result.Data.ToString());
                            template.Path = this._configuration["PathFile"] + result.Data.ToString();
                            template.Url = this._configuration["UrlSaveFile"] + result.Data.ToString();
                            //this._configuration["TemplatePath"] + "/" + yy + "/" + mn + "/" + dy + "/" + template.FileName;

                            var cResponse = await _templateContext.Insert(template);
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
                    var responses = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermissionInsertTemplate"].Value };
                    return Json(responses);
                }

            }                

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermissionInsertTemplate"].Value };
            return Json(response);
        }

        // check quyền
        [HttpPost]
        [Route(LinksRoute.CheckUpdateBieuMauCBTT)]
        [EzAuthorize(RoleCode.QL_BIEU_MAU_CBTT, view: true, edit: true)]
        public async Task<IActionResult> UpdateCheck()
        {
            string CheckAccount = _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE);
            if (CheckAccount == "ADMIN" || CheckAccount == "FCF Admin Group")
            {
                var responses = new CResponseMessage { Code = 0, Message = "" };
                return Json(responses);
            }
            else
            {
                var responses = new CResponseMessage { Code = -1, Message = "Tài khoản đang dùng không có quyền sửa thông tin này" };
                return Json(responses);
            }
        }

        [HttpPost]
        [Route(LinksRoute.UpdateBieuMauCBTT)]
        [EzAuthorize(RoleCode.QL_BIEU_MAU_CBTT, view: true, edit: true)]
        public async Task<IActionResult> UpdateTemplate(TemplateViewModel template, IFormFile file)
        {
            if (template.CompanyTypelist == null)
            {
                return Json(new CResponseMessage { Code = -1, Message = "Chưa chọn loại hình doanh nghiệp" });
            }
            template.UserLogin = _username;
            template.RoleCode = _roleCode;
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

            String dy = datevalue.Day.ToString();
            String mn = datevalue.Month.ToString();
            String yy = datevalue.Year.ToString();

            if (Convert.ToInt32(dy) <= 9) dy = "0" + dy;
            if (Convert.ToInt32(mn) <= 9) mn = "0" + mn;


            if (!string.IsNullOrEmpty(template.UserLogin) && !string.IsNullOrEmpty(template.RoleCode))
            {
                //Chỉ TK admin mới sửa/ xóa được biểu mẫu. TK chuyên viên thường không có các chức năng này/ khi muốn sửa hay xóa thì hệ thống hiển thị thông báo không cho phép sửa/ xóa.
                string CheckAccount = _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE);
                if(CheckAccount == "ADMIN" || CheckAccount == "FCF Admin Group")
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
                            
                            var result = await this._common.CopyFile(file, this._configuration["TemplatePath"], this._hostingEnvironment, this._configuration);

                            if (result.Message == "UPLOAD_FILE_SUCCESS")
                            {
                                template.FileName = Path.GetFileName(result.Data.ToString());
                                template.Path = this._configuration["PathFile"] + result.Data.ToString();
                                template.Url = this._configuration["UrlSaveFile"] + result.Data.ToString();
                                //this._configuration["TemplatePath"] + "/" + yy + "/" + mn + "/" + dy + "/" + template.FileName;
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
                        template.FileName = template.OldFileName;
                        template.Path = template.OldPath;
                        template.Url = template.OldUrl;
                    }

                    var cResponse = await _templateContext.Update(template);
                    cResponse.Message = this._sharedLocalizer[cResponse.Message];
                    return Json(cResponse);
                }
                else
                {
                    var responses = new CResponseMessage { Code = -1, Message = "Tài khoản đang dùng không có quyền sửa thông tin này" };
                    return Json(responses);
                }
            }
               
            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }

        [HttpPost]
        [Route(LinksRoute.DeleteBieuMauCBTT)]
        [EzAuthorize(RoleCode.QL_BIEU_MAU_CBTT, view: true, edit: true)]
        public async Task<IActionResult> DeleteTemplate(TemplateViewModel template)
        {
            template.UserLogin = _username;
            template.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(template.UserLogin) && !string.IsNullOrEmpty(template.RoleCode))
            {
                //Chỉ TK admin mới sửa/ xóa được biểu mẫu. TK chuyên viên thường không có các chức năng này/ khi muốn sửa hay xóa thì hệ thống hiển thị thông báo không cho phép sửa/ xóa.
                string CheckAccount = _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE);
                if (CheckAccount == "ADMIN" || CheckAccount == "FCF Admin Group")
                {
                    var cResponse = await _templateContext.Delete(template);
                    cResponse.Message = this._sharedLocalizer[cResponse.Message];
                    return Json(cResponse);
                }
                else
                {
                    var responses = new CResponseMessage { Code = -1, Message = "Tài khoản đang dùng không có quyền xóa thông tin này" };
                    return Json(responses);
                }
                   
            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }

        //[HttpPost]
        //[EzAuthorize(RoleCode.QL_BIEU_MAU_CBTT, view: true, edit: true)]
        //public async Task<IActionResult> DownloadFile(TemplateViewModel template)
        //{
        //    template.UserLogin = _username;
        //    template.RoleCode = _roleCode;
            
        //    //string filePath = Path.Combine(this._hostingEnvironment.ContentRootPath, template.Url.Replace(@"/", @"\"));
        //    if (!string.IsNullOrEmpty(template.UserLogin) && !string.IsNullOrEmpty(template.RoleCode))
        //    {
        //        /*if (!System.IO.File.Exists(filePath))
        //        {
        //            return Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["FileIsNotExist"].Value });
        //        }
        //        var data = await _templateContext.Select(template);
        //        var ms = new MemoryStream(CheckUtils.ReadFile(filePath));*/
        //        //var fileDownload = CheckFile.ReadFile(template.Path);

        //        //string FileExtension = Path.GetFileName(filePath).Substring(Path.GetFileName(filePath).LastIndexOf('.') + 1).ToLower();
        //        string remoteUri = _configuration["UrlFileCommon"];
        //        string Filename = template.Path;
        //        string myStringWebResource = null;

        //        myStringWebResource = remoteUri + Filename;

        //        var filePath = myStringWebResource;

        //        var myClient = new WebClient();
        //        byte[] bytes = myClient.DownloadData(myStringWebResource);
        //        var type = Path.GetExtension(myStringWebResource).Replace(".", "");

        //        if (type == "doc" || type == "docx")
        //        {
        //            return File(bytes, "application/msword");
        //        }
        //        if (type == "pdf")
        //        {
        //            return File(bytes, "application/pdf");
        //        }
        //        if (type == "rar")
        //        {
        //            return File(bytes, "application/x-rar-compressed");
        //        }
        //        if (type == "zip")
        //        {
        //            return File(bytes, "application/x-zip-compressed");
        //        }
        //    }

        //    var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
        //    return Json(response);
        //}
    }
}
