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
using CoreLib.Entities.ViewModels;
using CoreLib.Interfaces;
using EzIRCustomer.Authentication;
using EzIRCustomer.Commons;
using EzIRCustomer.Utility;
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
    public class ThongTinChungController : Controller
    {
        private readonly IHtmlLocalizer<SharedResource> _localizer;
        private readonly IConfiguration _configuration;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IHttpService _httpService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ICommon _common;
        private readonly IErrorHandler _errorHandler;
        private string _BASE_API_URL;
        private int? _CPNYID;
        private string _USERNAME;
        private string _LANGUAGE;
        private string _EMAIL_SPECIALIST;
        private EmailSettings _emailSettings;
        private string _Link_Show_Logo = "";

        public ThongTinChungController(IConfiguration configuration, IAppLogger appLogger, IMemoryCache cache, ICommon common, IHttpService httpService, IHostingEnvironment hostingEnvironment, IHtmlLocalizer<SharedResource> localizer, IHttpContextAccessor httpContextAccessor, IErrorHandler errorHandler)
        {
            _configuration = configuration;
            _appLogger = appLogger;
            _cache = cache;
            _httpService = httpService;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _common = common;
            _errorHandler = errorHandler;
            _BASE_API_URL = _configuration.GetSection("ApiUrl").Value;
            _CPNYID = _httpContextAccessor.HttpContext.Session.GetInt32(SessionTypes.CPNY_ID);
            _USERNAME = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.USERNAME);
            _EMAIL_SPECIALIST = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.MAIL_SPECIALIST);
            _Link_Show_Logo = _configuration.GetSection("UploadPath").Value;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                var checklogin = await CheckLoginSameTime();
                if (!checklogin)
                {
                    _errorHandler.WriteStringToFile("CheckLoginSameTime:", "ThongTinChung");
                    HttpContext.Session.Clear();

                    return RedirectToAction("index", "login");
                }

            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
            }

            ViewBag._Link_Show_Logo = _Link_Show_Logo + "/";
            return View();
        }

        [HttpGet]
        [Route(LinkRoute.GET_TTC)]
        public async Task<IActionResult> GetTTC()
        {
            try
            {

                ViewBag._Link_Show_Logo = _Link_Show_Logo+"/" ;
                _LANGUAGE = Request.Cookies[CookieTypes.LANGUAGE]?.ToUpper()?.Split('-')[1] ?? "";

                var pramCpny = $"?cpnyID={_CPNYID}";
                var pramDevelop = $"?cpnyID={_CPNYID}&status=&language={_LANGUAGE}";

                // Thông tin công ty
                var responseCompanies = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.GET_COMPANY + pramCpny);
                var companies = JsonConvert.DeserializeAnonymousType(responseCompanies.Data.ToString(), new List<CompanyEzSearchTemp>());
                var company = companies.FirstOrDefault();
                _appLogger.InfoLogger.LogInfo("ALogoPath" + _Link_Show_Logo + "/"+ company.ALogoPath);
                if (_LANGUAGE == "US")
                {
                    company.NameVN = company.NameEN;
                    company.AHeadOffice = company.AHeadOfficeEN;
                    company.ASummary = company.ASummary_EN;
                }

                // Các mốc lịch sử
                var responseDevelopProgresses = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.GET_DEVELOP_PROGRESS + pramDevelop);
                var developProgresses = JsonConvert.DeserializeAnonymousType(responseDevelopProgresses.Data.ToString(), new List<DevelopProgress>());
                
                var obj = new
                {
                    company,
                    developProgresses
                };

                return Json(obj);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = _localizer["InternalServerError"].Value });
            }

        }

        [HttpPost]
        [Route(LinkRoute.SAVE_TTC)]
        public async Task<ActionResult> SaveTTC(CompanyEzSearchTemp companyEzSearchTemp, string jsonDevelopProgresses)
        {
            try
            {
                try
                {
                    // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                    var checklogin = await CheckLoginSameTime();
                    if (!checklogin)
                    {
                        _errorHandler.WriteStringToFile("CheckLoginSameTime:", "SaveTTC");
                        HttpContext.Session.Clear();

                        return RedirectToAction("index", "login");
                    }

                }
                catch (Exception ex)
                {
                    _appLogger.ErrorLogger.LogError(ex);
                }

                _LANGUAGE = Request.Cookies[CookieTypes.LANGUAGE]?.ToUpper()?.Split('-')[1] ?? "";
                companyEzSearchTemp.Language = _LANGUAGE;
                companyEzSearchTemp.Username = _USERNAME;
                companyEzSearchTemp.RoleCode = RoleCode.THONG_TIN_CHUNG;
                List<DevelopProgress> developProgress = new List<DevelopProgress>();



                if (!string.IsNullOrEmpty(jsonDevelopProgresses))
                {
                    developProgress = JsonConvert.DeserializeAnonymousType(jsonDevelopProgresses, new List<DevelopProgress>());

                    if (developProgress != null || developProgress.Count > 0)
                    {
                        int i = 0;
                        foreach (var item in developProgress)
                        {
                            item.Language = _LANGUAGE;
                            item.Username = _USERNAME;
                            item.RoleCode = RoleCode.THONG_TIN_CHUNG;
                            //item.EventID = i+1; i += 1;
                        }
                    }
                }


                /// nội  dung mail được gửi cho chuyên viên

                var pramCpny = $"?cpnyID={_CPNYID}";

                var responseCompanies = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.GET_COMPANY + pramCpny);

                var companies = JsonConvert.DeserializeAnonymousType(responseCompanies.Data.ToString(), new List<CompanyEzSearchTemp>());
                var company = companies.FirstOrDefault();

                string namevn = "- Tên doanh nghiệp: " + company.AName_VN;
                string nameen = "- Mã Chứng khoán: " + company.AStockCode;
                string date = "- Thời gian thực hiện: " + DateTime.Now;
                string mess = "- Xem thông tin phê duyệt: Tổng quan -> Thông tin chung";
                _emailSettings = new EmailSettings
                {
                    Mail = _EMAIL_SPECIALIST,
                    Message = string.Format("{0}<br />{1}<br />{2}<br />{3}", namevn, nameen, date, mess),
                    Subject = "Notification Cập nhật thông tin trong ngày của KH EzIR - Nguồn: " + company.AStockCode +".EzIR"
                };

                // Lưu file image nếu ImageFile != null 
                if (companyEzSearchTemp.ImageFile != null)
                {
                    var messCheck = companyEzSearchTemp.ImageFile.CheckValidFileImage();

                    if (companyEzSearchTemp.ImageFile.GetSizeFile() > 2) return Json(new CResponseMessage { Code = -1, Message = _localizer["ImageMaxSize"].Value });
                    if (companyEzSearchTemp.ImageFile.FileName.Length > 30) return Json(new CResponseMessage { Code = -1, Message = _localizer["FileNameLarge"].Value });

                    if (!string.IsNullOrEmpty(messCheck))
                    {
                        return Json(new CResponseMessage { Code = -1, Message = _localizer[messCheck].Value });
                    }
                    else
                    {
                        //lưu lại file ảnh trên thư mục
                        var result = await _common.SaveFile(companyEzSearchTemp.ImageFile, _Link_Show_Logo + "/"+ _configuration["UploadLogoPath"], _hostingEnvironment);

                        if (result.Code != 0)
                        {
                            companyEzSearchTemp.ALogoPath= "";
                        }
                        else
                        {
                            companyEzSearchTemp.ALogoPath = result.Data.ToString().Replace("Images\\","").Replace(@"\", @"/").ToString();
                        }

                        companyEzSearchTemp.ALogoImage = await companyEzSearchTemp.ImageFile.GetBytes();
                        companyEzSearchTemp.ImageFile = null;
                    }
                }

                var responseMessage = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.INSERT_COMPANY_DEVELOP_PROGRESS
                                                                                    , new CompanyInfo
                                                                                    {
                                                                                        Company = companyEzSearchTemp,
                                                                                        DevelopProgresses = developProgress,
                                                                                        EmailSettings = _emailSettings
                                                                                    }
                                                                                    );

                responseMessage.Message = _localizer[responseMessage.Message].Value;
                return Json(responseMessage);                
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = _localizer["InternalServerError"].Value });
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
