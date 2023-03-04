using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Commons;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Interfaces;
using CoreLib.SharedKernel;
using EzIRCustomer.Authentication;
using EzIRCustomer.Commons;
using EzIRCustomer.Models.ViewModels;
using EzIRCustomerAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace EzIRCustomer.Controllers
{
    [RequiredLogin]
    public class TamNhinChienLuocController: Controller
    {
        private readonly IHtmlLocalizer<SharedResource> _localizer;
        private readonly IConfiguration _configuration;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IHttpService _httpService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommon _common;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IErrorHandler _errorHandler;
        private string _BASE_API_URL;
        private int? _CPNYID;
        private string _USERNAME;
        private string _EMAIL_SPECIALIST;
        private EmailSettings _emailSettings;
        private string _LANGUAGE;

        public TamNhinChienLuocController(IConfiguration configuration, IAppLogger appLogger, IMemoryCache cache, IHostingEnvironment hostingEnvironment, ICommon common, IHttpService httpService, IHtmlLocalizer<SharedResource> localizer, IHttpContextAccessor httpContextAccessor, IErrorHandler errorHandler)
        {
            _configuration = configuration;
            _appLogger = appLogger;
            _cache = cache;
            _httpService = httpService;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
            _common = common;
            _hostingEnvironment = hostingEnvironment;
            _errorHandler = errorHandler;
            _BASE_API_URL = _configuration.GetSection("ApiUrl").Value;
            _CPNYID = _httpContextAccessor.HttpContext.Session.GetInt32(SessionTypes.CPNY_ID);
            _USERNAME = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.USERNAME);
            _EMAIL_SPECIALIST = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.MAIL_SPECIALIST);
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                var checklogin = await CheckLoginSameTime();
                if (!checklogin)
                {
                    _errorHandler.WriteStringToFile("CheckLoginSameTime:", "TamNhinChienLuoc");
                    HttpContext.Session.Clear();

                    return RedirectToAction("index", "login");
                }

            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
            }

            return View();
        }
        [HttpGet]
        [Route(LinkRoute.GET_TNCL)]
        public async Task<IActionResult> GetTNCL(InfoSheetViewModel infoSheetViewModel)
        {
            try
            {
                //_LANGUAGE = Request.Cookies[CookieTypes.LANGUAGE]?.ToUpper()?.Split('-')[1] ?? "";

                infoSheetViewModel.CpnyID = _CPNYID;
                //infoSheetViewModel.Language = _LANGUAGE;

                var pram = "?" + infoSheetViewModel.ToQueryString();

                var cResponseMessage = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.GET_INFOSHEET + pram);

                var responseObj = JsonConvert.DeserializeAnonymousType(cResponseMessage.Data.ToString(), new List<InfoSheet>());

                responseObj = responseObj.Where(a => a.MenuID == ConstMenuID.TAM_NHIN || a.MenuID == ConstMenuID.CHIEN_LUOC).ToList();

                cResponseMessage.Data = responseObj;

                //ViewBag.Data = cResponseMessage;

                return Json(cResponseMessage);
            }
            catch(Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = _localizer["InternalServerError"].Value });
            }
            
        }
        [HttpPost]
        [Route(LinkRoute.SAVE_TNCL)]
        public async Task<IActionResult> SaveTNCL(List<InfoSheetViewModel> infoSheetViewModels)
        {
            try
            {
                try
                {
                    // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                    var checklogin = await CheckLoginSameTime();
                    if (!checklogin)
                    {
                        _errorHandler.WriteStringToFile("CheckLoginSameTime:", "SaveTNCL");
                        HttpContext.Session.Clear();

                        return RedirectToAction("index", "login");
                    }

                }
                catch (Exception ex)
                {
                    _appLogger.ErrorLogger.LogError(ex);
                }

                CResponseMessage cResponseMessage = new CResponseMessage { Code = -1, Message = "UnknowError" };

                //_LANGUAGE = Request.Cookies[CookieTypes.LANGUAGE]?.ToUpper()?.Split('-')[1] ?? "";

                if (infoSheetViewModels == null || infoSheetViewModels.Count < 0)
                {
                    cResponseMessage.Message = "InvalidInputData";
                    return Json(cResponseMessage);
                }

                /// nội  dung mail được gửi cho chuyên viên
                var pramCpny = $"?cpnyID={_CPNYID}";

                var responseCompanies = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.GET_COMPANY + pramCpny);

                var companies = JsonConvert.DeserializeAnonymousType(responseCompanies.Data.ToString(), new List<CompanyEzSearchTemp>());
                var company = companies.FirstOrDefault();

                string namevn = "- Tên doanh nghiệp: " + company.AName_VN;
                string nameen = "- Mã Chứng khoán: " + company.AStockCode;
                string date = "- Thời gian thực hiện: " + DateTime.Now;
                string mess = "- Xem thông tin phê duyệt: Tổng quan -> Tầm nhìn chiến lược";
                _emailSettings = new EmailSettings
                {
                    Mail = _EMAIL_SPECIALIST,
                    Message = string.Format("{0}<br />{1}<br />{2}<br />{3}", namevn, nameen, date, mess),
                    Subject = "Notification Cập nhật thông tin trong ngày của KH EzIR - Nguồn: " + company.AStockCode + ".EzIR"
                };

                foreach (var infoSheet in infoSheetViewModels)
                {
                    var changeInfos = new List<ChangeInfo>();

                    var title = getTitleByMenuID(infoSheet.MenuID);

                    changeInfos.Add(new ChangeInfo
                    {
                        CpnyID = infoSheet.CpnyID,
                        MenuID = infoSheet.MenuID,
                        Key = "ACONTENT",
                        Value = infoSheet.Content,
                        Status = (infoSheet.ID == 0 || infoSheet.ID == null) ? 1 : 2,
                        KeyFunction = $"Tầm nhìn - Chiến lược - {title}",
                        Function = "Tầm nhìn chiến lược",
                        LevelID = "TQ",
                        DetailLevelID = "TNCL"
                    });

                    infoSheet.EmailSettings = _emailSettings;

                    infoSheet.ChangeInfos = changeInfos;
                    infoSheet.Username = _USERNAME;
                    infoSheet.RoleCode = RoleCode.TAM_NHIN_CHIEN_LUOC;
                    infoSheet.Title = title;
                    //infoSheet.Language = _LANGUAGE;
                }

                _httpService.SetRoleControllerToHeader(RoleCode.TAM_NHIN_CHIEN_LUOC);
                _httpService.SetUsernameControllerToHeader(_USERNAME);

                var cResponse = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.INSERT_INFOSHEET, infoSheetViewModels);

                cResponse.Message = _localizer[cResponse.Message].Value;

                return Json(cResponse);
            }
            catch(Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = _localizer["InternalServerError"].Value });
            }
            
        }

        /*[HttpPut]
        [Route(LinkRoute.EDIT_TNCL)]
        public async Task<IActionResult> EditSwot(List<InfoSheetViewModel> infoSheetViewModels)
        {
            CResponseMessage cResponseMessage = new CResponseMessage { Code = -1, Message = "UnknowError" };

            if (infoSheetViewModels == null || infoSheetViewModels.Count < 0)
            {
                cResponseMessage.Message = "InvalidInputData";
                return Json(cResponseMessage);
            }

            _httpService.SetRoleControllerToHeader(RoleCode.TAM_NHIN_CHIEN_LUOC);
            _httpService.SetUsernameControllerToHeader(_USERNAME);

            var cResponse = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.UPDATE_INFOSHEET, infoSheetViewModels);

            return Json(cResponse);
        }*/

        [HttpPost]
        [Route(LinkRoute.UPLOAD_IMAGE_TNCL)]
        public async Task<ActionResult> UploadImage(IFormFile upload)
        {
            try
            {
                // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                var checklogin = await CheckLoginSameTime();
                if (!checklogin)
                {
                    _errorHandler.WriteStringToFile("CheckLoginSameTime:", "TamNhinChienLuoc_UploadImage");
                    HttpContext.Session.Clear();

                    return RedirectToAction("index", "login");
                }

            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
            }

            var checkResult = upload.CheckValidFileImage();

            if (!string.IsNullOrEmpty(checkResult))
            {
                var errorMessage = _localizer[checkResult].ToString();

                dynamic errorResult = JsonConvert.DeserializeObject("{ 'uploaded': 0, 'error': { 'message': \"" + errorMessage + "\"}}");

                return Json(errorResult);
            }

            //lưu lại file ảnh trên thư mục
            var url = string.Empty;
            var fileName = string.Empty;
            var successMessage = string.Empty;

            var result = await _common.SaveFile(upload, _configuration["UploadCKImagePath"], _hostingEnvironment);

            if (result.Code != 0)
            {
                url = "";
                fileName = "";
            }
            else
            {
                url = result.Data.ToString().Replace(@"\", @"/").ToString();
                fileName = url.Split('/')[2];
            }

            return Json(new { uploaded = true, fileName, url, error = new { message = successMessage } });
        }
        //public async Task<ActionResult> UploadImage(IFormFile upload)
        //{
        //    try
        //    {
        //        var checkResult = upload.CheckValidFileImage();

        //        if (upload.Length <= 0)
        //        {
        //            checkResult = "File không hợp lệ";
        //        }

        //        if (!string.IsNullOrEmpty(checkResult))
        //        {
        //            var errorMessage = checkResult;

        //            dynamic errorResult = JsonConvert.DeserializeObject("{ 'uploaded': 0, 'error': { 'message': \"" + errorMessage + "\"}}");

        //            return Json(errorResult);
        //        }

        //        // lấy tên ngẫu nhiên
        //        var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();

        //        // vì wwwroot là thư mục tĩnh nên chỉ lấy path đằng sau
        //        var url = await upload.GetSavedFilePath($"wwwroot/images/CKEditorImages", fileName);

        //        url = ".." + url.Substring(7);

        //        var successMessage = string.Empty;

        //        return Json(new { uploaded = true, fileName, url, error = new { message = successMessage } });
        //    }
        //    catch(Exception ex)
        //    {
        //        _appLogger.ErrorLogger.LogError(ex);
        //        return Json(new CResponseMessage { Code = -3, Message = _localizer["InternalServerError"].Value });
        //    }        
        //}
        private string getTitleByMenuID(int? menuid)
        {
            switch (menuid)
            {
                case ConstMenuID.TAM_NHIN:
                    {
                        return "Tầm nhìn";
                    }
                case ConstMenuID.CHIEN_LUOC:
                    {
                        return "Chiến lược";
                    }

                default:
                    return "";
            }
        }

        //Kiểm tra cổ đông login 2 trình duyệt
        public async Task<bool> CheckLoginSameTime()
        {
            LoginModel loginModel = new LoginModel()
            {
                Username = HttpContext.Session.GetString(SessionTypes.USERNAME),
                Seed = HttpContext.Session.GetString(SessionTypes.SEED),
            };

            CResponseMessage cResponseMessage = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.GET_SEED, loginModel);

            if (cResponseMessage.Code != 0)
            {
                return false;
            }
            return true;
        }
    }
}
