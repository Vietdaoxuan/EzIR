using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommonLib.Interfaces.Logger;
using CoreLib.Commons;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Interfaces;
using CoreLib.SharedKernel;
using EzIRCustomer.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.Extensions.Localization;
using CommonLib.Interfaces;
using Microsoft.AspNetCore.Hosting;
using CoreLib.Entities.ViewModels;
using CoreLib.ViewModel;
using EzIRCustomer.Authentication;
using System.Net;
using EzIRCustomerAPI.Models;

namespace EzIRCustomer.Controllers.CongBoThongTin
{
    [RequiredLogin]
    public class CongBoThongTinController : Controller
    {
        private readonly IHtmlLocalizer<SharedResource> _localizer;
        private readonly IConfiguration _configuration;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private string _BASE_API_URL;
        private readonly IHttpService _httpService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommon _common;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IErrorHandler _errorHandler;
        private string _EMAIL_SPECIALIST;
        private string _EMAIL_SPECIALISTCC;
        private EmailSettings _emailSettings;
        private EmailSettings _emailSettingDeletes;
        string _stockCode = "";
        string _UrlFileCommon;
        public CongBoThongTinController(IConfiguration configuration, IAppLogger appLogger, IMemoryCache cache, IHttpService httpService, IHttpContextAccessor httpContextAccessor, IHtmlLocalizer<SharedResource> localizer, IStringLocalizer<SharedResource> sharedLocalizer, ICommon common, IHostingEnvironment hostingEnvironment, IErrorHandler errorHandler)
        {
            _configuration = configuration;
            _appLogger = appLogger;
            _cache = cache;
            _common = common;
            _sharedLocalizer = sharedLocalizer;
            _hostingEnvironment = hostingEnvironment;
            _httpService = httpService;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _errorHandler = errorHandler;
            _BASE_API_URL = _configuration.GetSection("ApiUrl").Value;
            _EMAIL_SPECIALIST = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.MAIL_SPECIALIST);
            _EMAIL_SPECIALISTCC = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.MAIL_SPECIALISTCC);
            _stockCode = _httpContextAccessor.HttpContext.Session.GetString("StockCode");
            _emailSettings = new EmailSettings { Mail = _EMAIL_SPECIALIST, Cc = _EMAIL_SPECIALISTCC, Message = "", Subject = "Notification Thêm mới tin - CBTT trong ngày của KH EzIR - Nguồn: " + _stockCode + ".EzCBTT"};
            _emailSettingDeletes = new EmailSettings { Mail = _EMAIL_SPECIALIST, Cc = _EMAIL_SPECIALISTCC, Message = "", Subject = "Notification Xóa tin CBTT trong ngày của Khách hàng EzIR - Nguồn: " + _stockCode + ".EzCBTT"};
            _UrlFileCommon = _configuration.GetSection("UrlFileCommon").Value;
        }
        public async Task<IActionResult> Index(int? value)
        {
            ViewBag._UrlFileCommon = _UrlFileCommon + "/";
            try
            {
                try
                {
                    // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                    var checklogin = await CheckLoginSameTime();
                    if (!checklogin)
                    {
                        _errorHandler.WriteStringToFile("CheckLoginSameTime:", "CongBoThongTin");
                        HttpContext.Session.Clear();

                        return RedirectToAction("index", "login");
                    }

                }
                catch (Exception ex)
                {
                    _appLogger.ErrorLogger.LogError(ex);
                }
                List<CommonType> listloaitl = new List<CommonType>();
                List<CommonType> listloaitin = new List<CommonType>();
                var commonType = new CommonTypeViewModel
                {
                    ListCategory = "1,3,4,11"
                };

                var pram = "?" + commonType.ToQueryString();
                var cResponseMessage = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.CommonCategory + pram);
                var responseObj = JsonConvert.DeserializeAnonymousType(cResponseMessage.Data.ToString(), new List<CommonType>());


                if (value == null)
                {
                    foreach (var obj in responseObj.ToList())
                    {
                        switch (obj.Category)
                        {
                            case ConstCommonTypes.NHOM_TAI_LIEU:
                                if (obj.Ord < 11) listloaitl.Add(obj);
                                break;
                            case ConstCommonTypes.NHOM_BAO_CAO_TAI_CHINH:
                                listloaitin.Add(obj);
                                break;
                            case ConstCommonTypes.NHOM_DIEU_LE:
                                listloaitin.Add(obj);
                                break;
                            case ConstCommonTypes.NHOM_BAN_CAO_HACH:
                                listloaitin.Add(obj);
                                break;
                            case ConstCommonTypes.NHOM_BAO_CAO_THUONG_NIEN:
                                listloaitin.Add(obj);
                                break;
                            case ConstCommonTypes.NHOM_BAO_CAO_TINH_HINH_QUAN_TRI:
                                listloaitin.Add(obj);
                                break;
                            case ConstCommonTypes.NHOM_TAI_LIEU_DHDCD:
                                listloaitin.Add(obj);
                                break;
                            case ConstCommonTypes.NHOM_NGHI_QUYET_DHDCD:
                                listloaitin.Add(obj);
                                break;
                            case ConstCommonTypes.NHOM_NGHI_QUYET_HDQT:
                                listloaitin.Add(obj);
                                break;
                            case ConstCommonTypes.NHOM_BAN_TIN_IR:
                                listloaitin.Add(obj);
                                break;
                            case ConstCommonTypes.NHOM_TIN_KHAC:
                                listloaitin.Add(obj);
                                break;
                            default:
                                break;

                        }
                    }                  

                    var languagenews = Request.Cookies[CookieTypes.LANGUAGE].Substring(3);
                    if (languagenews == "US")
                    {
                        foreach (var DocType in listloaitl.ToList())
                            DocType.TypeName = DocType.TypeNameEN;
                    }

                    var viewmodel = new CongBoThongTinViewModel
                    {

                        listloaitl = listloaitl,
                        listloaitin = listloaitin

                    };
                    return View(viewmodel);
                }
                else
                {
                    foreach (var listin in responseObj.ToList())
                    {
                        if (listin.ParentID == value)

                            listloaitin.Add(listin);

                    }

                    var languagenews = Request.Cookies[CookieTypes.LANGUAGE].Substring(3);
                    if (languagenews == "US")
                    {
                        foreach (var NewType in listloaitin.ToList())
                            NewType.TypeName = NewType.TypeNameEN;
                    }

                    return Json(listloaitin);
                }
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = _localizer["InternalServerError"].Value });
            }            
        }
        [HttpGet]
        [Route(LinkRoute.GetTinCongBo)]
        public async Task<IActionResult> GetCongBo(CongBoThongTinViewModel congBoThongTinViewModel)
        {
            try
            {
                var cpnyID = HttpContext.Session.GetInt32(SessionTypes.CPNY_ID);
                var username = HttpContext.Session.GetString(SessionTypes.USERNAME);
                congBoThongTinViewModel.CompanyID = cpnyID;
                congBoThongTinViewModel.UserLogin = username;
                var pram = "?" + congBoThongTinViewModel.ToQueryString();
                var cResponseMessage = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.Get_CongBoThongTin + pram);
                var responseObj = JsonConvert.DeserializeAnonymousType(cResponseMessage.Data.ToString(), new List<TinCongBo>()); ;
                var languagenews = Request.Cookies[CookieTypes.LANGUAGE].Substring(3);
                if (languagenews == "US")
                {
                    foreach (var DocType in responseObj.ToList())
                    {
                        DocType.atypename = DocType.atypenameen;
                        DocType.anewtypename = DocType.anewtypenameen;
                    }
                        
                }
                cResponseMessage.Data = responseObj;               
                return Json(responseObj);
            }
            catch(Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = _localizer["InternalServerError"].Value });
            }
            
        }
        [HttpPost]
        [Route(LinkRoute.InsertTinCongBo)]
        public async Task<IActionResult> InsertCongBo(CongBoThongTinViewModel congBoThongTinViewModels, IFormFile file, string languagenews)
        {
            try
            {
                try
                {
                    // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                    var checklogin = await CheckLoginSameTime();
                    if (!checklogin)
                    {
                        _errorHandler.WriteStringToFile("CheckLoginSameTime:", "InsertCongBo");
                        HttpContext.Session.Clear();

                        return RedirectToAction("index", "login");
                    }

                }
                catch (Exception ex)
                {
                    _appLogger.ErrorLogger.LogError(ex);
                }
                var cookieValue = Request.Cookies[CookieTypes.LANGUAGE];
                if (cookieValue == "vi-VN")
                {
                    congBoThongTinViewModels.Language = 1;
                    languagenews = "VN";
                }
                else if (cookieValue == "en-US")
                {
                    congBoThongTinViewModels.Language = 2;
                    languagenews = "EN";
                }
                congBoThongTinViewModels.UserLogin = HttpContext.Session.GetString(SessionTypes.USERNAME);
                congBoThongTinViewModels.CompanyID = HttpContext.Session.GetInt32(SessionTypes.CPNY_ID);
                congBoThongTinViewModels.StockCode = HttpContext.Session.GetString(SessionTypes.STOCK_CODE).ToString();
                congBoThongTinViewModels.emailSettings = this._emailSettings;


                congBoThongTinViewModels.DatePub = DateTime.Now;
                string formatters = "MM/dd/yyyy hh:mm:ss tt";
                string newtrackerIDs = congBoThongTinViewModels.DatePub?.ToString(formatters);

                congBoThongTinViewModels.RoleCode = RoleCode.CBTT;
                String sDate = DateTime.Now.ToString();
                DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

                String dy = datevalue.Day.ToString();
                String mn = datevalue.Month.ToString();
                String yy = datevalue.Year.ToString();

                string[] validFileType = { "doc", "docx", "rar", "pdf", "zip" };
                string solutionPath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).FullName, this._configuration["SpecialistPath"]);
                this._appLogger.InfoLogger.LogInfo(solutionPath);
                if (file == null)
                {
                    return Json(new CResponseMessage { Code = -1, Message = "Chưa chọn File" });
                }
                string FileExtension = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();               
                if (file.Length > 30 * 1024 * 1024)
                {
                    return Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["FileIsOver30MB"].Value });
                }
                if (validFileType.Contains(FileExtension) && file.Length < 30 * 1024 * 1024)
                {
                    var result = await this._common.CopyFile(file, this._configuration["NewsPath"], this._hostingEnvironment, this._configuration);
                    if (result.Message == "UPLOAD_FILE_SUCCESS")
                    {

                        congBoThongTinViewModels.FileName = Path.GetFileName(result.Data.ToString());
                        congBoThongTinViewModels.Path = this._configuration["UrlSaveFile"] + result.Data.ToString();
                        congBoThongTinViewModels.Url = this._configuration["UrlSaveFile"] + result.Data.ToString();
                        //this._configuration["NEWS_URL_DEST"] + yy + "/" + mn + "/" + dy + "/" + congBoThongTinViewModels.FileName;

                        //Nếu loại tài liệu là bản tin IR hoặc báo cáo phân tích doanh nghiệp thì chỉ insert vào db EzIR
                        // Quangks edit: KHông có DocType = 111 = > Xóa code 2021-11-2021
                        if (congBoThongTinViewModels.DocType == 10)
                        {
                            var cResponse = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.Insert_TinCongBo, congBoThongTinViewModels);
                            cResponse.Message = this._sharedLocalizer[cResponse.Message];
                            return Json(cResponse);
                        }

                        //Gán thông tin News bên CBTT
                        NewsWebsiteAPI newsWebsiteAPI = new NewsWebsiteAPI()
                        {
                            DatePub = DateTime.Now.ToString("MM/dd/yyyy HH:mm"),
                            DocTypeID = congBoThongTinViewModels.DocType ?? default(int),
                            Title = congBoThongTinViewModels.Title,
                            Language = languagenews,
                            ReportYear = congBoThongTinViewModels.Year.ToString(),
                            URL = congBoThongTinViewModels.Url,
                            NewsID = congBoThongTinViewModels.NewID ?? default(int),
                            UserName = congBoThongTinViewModels.UserLogin,
                            Stock_Code = congBoThongTinViewModels.StockCode,
                        };

                        //Call API CBTT insert db CBTT
                        var cResponseMessage = await _httpService.CallAPIWebsite(newsWebsiteAPI, this._configuration["API_CBTT"], LinkRoute.API_INSERT_NEW);
                        if (cResponseMessage.Code == 0 && cResponseMessage.Data.NewsID != -1)
                        {
                            congBoThongTinViewModels.Sid = cResponseMessage.Data.NewsID;
                            var cResponse = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.Insert_TinCongBo, congBoThongTinViewModels);

                            //Nếu insert news vào db EzIR lỗi thì xóa dữ liệu vừa insert bên CBTT
                            if (cResponse.Code != 0)
                            {
                                var cResponseMessageDelete = await _httpService.CallAPIWebsite(newsWebsiteAPI, this._configuration["API_CBTT"], LinkRoute.API_DELETE_NEW);
                            }

                            cResponse.Message = this._sharedLocalizer[cResponse.Message];
                            return Json(cResponse);
                        }
                        else
                        {
                            return Json(new CResponseMessage { Code = -1, Message = "Lỗi khi thêm tin mới!" });
                        }

                    }
                    else
                    {
                        return Json(result);
                    }
                }
                else
                {
                    return Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["FileIsNotAccepted"].Value });
                }
                
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = _localizer["InternalServerError"].Value });
            }
        }
        [HttpPost]
        [Route(LinkRoute.UpdateTincongBo)]
        public async Task<IActionResult> UpdateCongBo(CongBoThongTinViewModel congBoThongTinViewModels, IFormFile file, string languagenews)
        {
            try
            {
                try
                {
                    // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                    var checklogin = await CheckLoginSameTime();
                    if (!checklogin)
                    {
                        _errorHandler.WriteStringToFile("CheckLoginSameTime:", "UpdateCongBo");
                        HttpContext.Session.Clear();

                        return RedirectToAction("index", "login");
                    }

                }
                catch (Exception ex)
                {
                    _appLogger.ErrorLogger.LogError(ex);
                }
                var cookieValue = Request.Cookies[CookieTypes.LANGUAGE];
                if (cookieValue == "vi-VN")
                {
                    congBoThongTinViewModels.Language = 1;
                    languagenews = "VN";
                }
                else if (cookieValue == "en-US")
                {
                    congBoThongTinViewModels.Language = 2;
                    languagenews = "EN";
                }
                congBoThongTinViewModels.UserLogin = HttpContext.Session.GetString(SessionTypes.USERNAME);
                congBoThongTinViewModels.CompanyID = HttpContext.Session.GetInt32(SessionTypes.CPNY_ID);
                congBoThongTinViewModels.StockCode = HttpContext.Session.GetString(SessionTypes.STOCK_CODE);
                congBoThongTinViewModels.IsBackDate = 0;
                var pram = "?" + "NewID=" + congBoThongTinViewModels.NewID.ToString();
                var cResponseMessage = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.Get_CongBoThongTin + pram);
                var responseObj = JsonConvert.DeserializeAnonymousType(cResponseMessage.Data.ToString(), new List<TinCongBo>());

                congBoThongTinViewModels.DatePub = DateTime.Now;
                string formatters = "MM/dd/yyyy hh:mm:ss tt";
                string newtrackerIDs = congBoThongTinViewModels.DatePub?.ToString(formatters);

                //congBoThongTinViewModels.DatePub = responseObj.FirstOrDefault().aDatePub;
                String sDate = DateTime.Now.ToString();
                DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

                clsWebsiteRequest clsWebsiteRequest;

                String dy = datevalue.Day.ToString();
                String mn = datevalue.Month.ToString();
                String yy = datevalue.Year.ToString();

                congBoThongTinViewModels.RoleCode = RoleCode.CBTT;
                string[] validFileType = { "doc", "docx", "rar", "pdf","zip" };

                //Gán thông tin News bên CBTT
                NewsWebsiteAPI newsWebsiteAPI = new NewsWebsiteAPI()
                { 
                    DatePub = "",
                    DocTypeID = congBoThongTinViewModels.DocType ?? default(int),
                    Title = congBoThongTinViewModels.Title,
                    Language = languagenews,
                    ReportYear = congBoThongTinViewModels.Year.ToString(),
                    //URL = congBoThongTinViewModels.Url,
                    NewsID = congBoThongTinViewModels.Sid ?? default(int),
                    UserName = congBoThongTinViewModels.UserLogin,
                    Stock_Code = congBoThongTinViewModels.StockCode,
                };
                string solutionPath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).FullName,this._configuration["SpecialistPath"]);
                this._appLogger.InfoLogger.LogInfo(solutionPath);
                if (file != null)
                {
                    string FileExtension = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();
                    if (file.Length > 30 * 1024 * 1024)
                    {
                        return Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["FileIsOver30MB"].Value });
                    }
                    if (validFileType.Contains(FileExtension))
                    {
                        //var result = await this._common.SaveBieuMau(file, this._configuration["UploadStaticFilePath"], solutionPath);
                        var result = await this._common.CopyFile(file, this._configuration["NewsPath"], this._hostingEnvironment, this._configuration);
                        if (result.Message == "UPLOAD_FILE_SUCCESS")
                        {
                            congBoThongTinViewModels.FileName = Path.GetFileName(result.Data.ToString());
                            congBoThongTinViewModels.Path = this._configuration["UrlSaveFile"] + result.Data.ToString();
                            congBoThongTinViewModels.Url = this._configuration["UrlSaveFile"] + result.Data.ToString();
                            //this._configuration["NEWS_URL_DEST"] + yy + "/" + mn + "/" + dy + "/" + congBoThongTinViewModels.FileName;
                            newsWebsiteAPI.URL = congBoThongTinViewModels.Url;
                            if (congBoThongTinViewModels.DocType == 10 || congBoThongTinViewModels.DocType == 111)
                            {
                                var cResponse = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.Update_TinCongBo, congBoThongTinViewModels);
                                cResponse.Message = this._sharedLocalizer[cResponse.Message];
                                return Json(cResponse);
                            }

                            //Call API CBTT update db CBTT
                            clsWebsiteRequest = await _httpService.CallAPIWebsite(newsWebsiteAPI, this._configuration["API_CBTT"], LinkRoute.API_UPDATE_NEW);
                        }
                        else
                        {
                            return Json(result);
                        }
                    }
                    else
                    {
                        return Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["FileIsNotAccepted"].Value });
                    }
                }
                else 
                {
                    congBoThongTinViewModels.FileName = congBoThongTinViewModels.OldFileName;
                    congBoThongTinViewModels.Path = congBoThongTinViewModels.OldPath;
                    congBoThongTinViewModels.Url = congBoThongTinViewModels.OldUrl;
                    newsWebsiteAPI.URL = congBoThongTinViewModels.Url;

                    if (congBoThongTinViewModels.DocType == 10 || congBoThongTinViewModels.DocType == 111)
                    {
                        var cResponse = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.Update_TinCongBo, congBoThongTinViewModels);
                        cResponse.Message = this._sharedLocalizer[cResponse.Message];
                        return Json(cResponse);
                    }
                    //Call API CBTT update db CBTT
                    clsWebsiteRequest = await _httpService.CallAPIWebsite(newsWebsiteAPI, this._configuration["API_CBTT"], LinkRoute.API_UPDATE_NEW);
                }
                if (clsWebsiteRequest.Code == 0 && clsWebsiteRequest.Data.NewsID != -1)
                {
                    var cResponse = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.Update_TinCongBo, congBoThongTinViewModels);
                    //Nếu updte news trong EzIR lỗi thì update dữ liệu cũ vào db CBTT
                    if (cResponse.Code != 0)
                    {
                        var prams = "?" + "NewID=" + congBoThongTinViewModels.NewID.ToString();
                        var cResponseMessages = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.Get_CongBoThongTin + prams);
                        var oldNewsData = JsonConvert.DeserializeAnonymousType(cResponseMessages.Data.ToString(), new List<TinCongBo>());
              
                        var listOldData = oldNewsData.FirstOrDefault();

                        //gán dữ liệu cũ vào newsWebsiteAPI
                        newsWebsiteAPI.DatePub = DateTime.Parse(listOldData.aDatePub.ToString()).ToString("MM/dd/yyyy");
                        newsWebsiteAPI.DocTypeID = listOldData.aDocType ?? default(int);
                        newsWebsiteAPI.Title = listOldData.aTitle;
                        newsWebsiteAPI.Language = languagenews;
                        newsWebsiteAPI.ReportYear = listOldData.ayear.ToString();
                        newsWebsiteAPI.URL = listOldData.aurl;
                        newsWebsiteAPI.NewsID = listOldData.asid ?? default(int);
                        newsWebsiteAPI.UserName = congBoThongTinViewModels.UserLogin;
                        newsWebsiteAPI.Stock_Code = listOldData.aStockCode;

                        //Call API CBTT delete db CBTT
                        var cResponseMessageDelete = await _httpService.CallAPIWebsite(newsWebsiteAPI, this._configuration["API_CBTT"], LinkRoute.API_UPDATE_NEW);
                    }
                    cResponse.Message = this._sharedLocalizer[cResponse.Message];
                    return Json(cResponse);
                }
                else
                {
                    return Json(new CResponseMessage { Code = -1, Message = "Lỗi khi update tin mới!" });
                }
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = _localizer["InternalServerError"].Value });
            }
        }
        [HttpPost]
        [Route(LinkRoute.DeleteTincongBo)]
        public async Task<IActionResult> DeleteCongBo(CongBoThongTinViewModel congBoThongTinViewModels)
        {
            try
            {
                try
                {
                    // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                    var checklogin = await CheckLoginSameTime();
                    if (!checklogin)
                    {
                        _errorHandler.WriteStringToFile("CheckLoginSameTime:", "DeleteCongBo");
                        HttpContext.Session.Clear();

                        return RedirectToAction("index", "login");
                    }

                }
                catch (Exception ex)
                {
                    _appLogger.ErrorLogger.LogError(ex);
                }
                congBoThongTinViewModels.UserLogin = HttpContext.Session.GetString(SessionTypes.USERNAME);
                congBoThongTinViewModels.RoleCode = RoleCode.CBTT;

                NewsWebsiteAPI newsWebsiteAPI = new NewsWebsiteAPI()
                {
                    NewsID = congBoThongTinViewModels.Sid ?? default(int),
                    UserName = congBoThongTinViewModels.UserLogin,
                };

                if (!string.IsNullOrEmpty(congBoThongTinViewModels.UserLogin) && !string.IsNullOrEmpty(congBoThongTinViewModels.RoleCode))
                {
                    var pram = "?" + "NewID=" + congBoThongTinViewModels.NewID.ToString();
                    var cResponseMessage = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.Get_CongBoThongTin + pram);
                    var data = JsonConvert.DeserializeAnonymousType(cResponseMessage.Data.ToString(), new List<TinCongBo>());

                    congBoThongTinViewModels.Url = data.FirstOrDefault().aurl;
                    congBoThongTinViewModels.StockCode = HttpContext.Session.GetString(SessionTypes.STOCK_CODE).ToString();
                    congBoThongTinViewModels.Title = data.FirstOrDefault().aTitle;
                    congBoThongTinViewModels.emailSettings = this._emailSettingDeletes;

                    if (data.FirstOrDefault().aDocType == 10 || data.FirstOrDefault().aDocType == 111)
                    {
                        var cResponse = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.Delete_TinCongBo, congBoThongTinViewModels);
                        cResponse.Message = this._sharedLocalizer[cResponse.Message];
                        return Json(cResponse);
                    }

                    //Call API CBTT delete db CBTT
                    var cResponseMessageDelete = await _httpService.CallAPIWebsite(newsWebsiteAPI, this._configuration["API_CBTT"], LinkRoute.API_DELETE_NEW);
                    if (cResponseMessageDelete.Code == 0)
                    {
                        var cResponse = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.Delete_TinCongBo, congBoThongTinViewModels);
                        cResponse.Message = this._sharedLocalizer[cResponse.Message];
                        return Json(cResponse);
                    }

                }

                var response = new CResponseMessage { Code = -1, Message = "Lỗi khi xóa tin !" };
                return Json(response);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = _localizer["InternalServerError"].Value });
            }         
        }
        [HttpPost]
        [Route(LinkRoute.DownloadTincongBo)]
        public async Task<IActionResult> DownloadFile(CongBoThongTinViewModel congBoThongTinViewModel)
        {
            try
            {
                try
                {
                    // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                    var checklogin = await CheckLoginSameTime();
                    if (!checklogin)
                    {
                        _errorHandler.WriteStringToFile("CheckLoginSameTime:", "DownloadFile");
                        HttpContext.Session.Clear();

                        return RedirectToAction("index", "login");
                    }

                }
                catch (Exception ex)
                {
                    _appLogger.ErrorLogger.LogError(ex);
                }
                congBoThongTinViewModel.UserLogin = HttpContext.Session.GetString(SessionTypes.USERNAME);
                congBoThongTinViewModel.CompanyID = HttpContext.Session.GetInt32(SessionTypes.CPNY_ID);
                congBoThongTinViewModel.RoleCode = RoleCode.CBTT;

                if (!string.IsNullOrEmpty(congBoThongTinViewModel.UserLogin) && !string.IsNullOrEmpty(congBoThongTinViewModel.RoleCode))
                {
                   /* if (!System.IO.File.Exists(congBoThongTinViewModel.Path))
                    {
                        return Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["FileIsNotExist"].Value });
                    }

                    var ms = new MemoryStream(CheckUtils.ReadFile(congBoThongTinViewModel.Path));

                    string FileExtension = Path.GetFileName(congBoThongTinViewModel.Path).Substring(Path.GetFileName(congBoThongTinViewModel.Path).LastIndexOf('.') + 1).ToLower();*/


                    string remoteUri = _configuration["UrlFileCommon"];
                    string Url = congBoThongTinViewModel.Path;
                    string myStringWebResource = null;

                    myStringWebResource = remoteUri + Url;

                    var filePath = myStringWebResource;

                    var myClient = new WebClient();
                    byte[] bytes = myClient.DownloadData(myStringWebResource);
                    var type = Path.GetExtension(myStringWebResource).Replace(".", "");

                    if (type == "doc" || type == "docx")
                    {
                        return File(bytes, "application/msword");
                    }
                    if (type == "pdf")
                    {
                        return File(bytes, "application/pdf");
                    }
                    if (type == "rar")
                    {
                        return File(bytes, "application/x-rar-compressed");
                    }
                    if (type == "zip")
                    {
                        return File(bytes, "application/x-zip-compressed");
                    }
                }

                var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
                return Json(response);
            }
            catch(Exception ex)
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
