using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Commons;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Entities.ViewModels;
using CoreLib.Interfaces;
using CoreLib.SharedKernel;
using EzIRCustomer.Authentication;
using EzIRCustomer.Commons;
using EzIRCustomerAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EzIRCustomer.Controllers
{
    [RequiredLogin]
    public class ThanhPhanLanhDaoController : Controller
    {
        private readonly IHtmlLocalizer<SharedResource> _localizer;
        private readonly IConfiguration _configuration;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private string _BASE_API_URL;
        private readonly IHttpService _httpService;
        private string _USERNAME;
        private readonly ICommon _common;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IErrorHandler _errorHandler;
        private string _EMAIL_SPECIALIST;
        private EmailSettings _emailSettings;
        public ThanhPhanLanhDaoController(IConfiguration configuration, IAppLogger appLogger, IMemoryCache cache, IHttpService httpService, IHtmlLocalizer<SharedResource> localizer, IStringLocalizer<SharedResource> sharedLocalizer, ICommon common, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, IErrorHandler errorHandler)
        {
            _configuration = configuration;
            _appLogger = appLogger;
            _cache = cache;
            _common = common;
            _sharedLocalizer = sharedLocalizer;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _httpService = httpService;
            _localizer = localizer;
            _errorHandler = errorHandler;
            _BASE_API_URL = _configuration.GetSection("ApiUrl").Value;
            _EMAIL_SPECIALIST = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.MAIL_SPECIALIST);
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                try
                {
                    // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                    var checklogin = await CheckLoginSameTime();
                    if (!checklogin)
                    {
                        _errorHandler.WriteStringToFile("CheckLoginSameTime:", "ThanhPhanLanhDao");
                        HttpContext.Session.Clear();

                        return RedirectToAction("index", "login");
                    }

                }
                catch (Exception ex)
                {
                    _appLogger.ErrorLogger.LogError(ex);
                }

                var cResponseMessage = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.Get_KnowLedgeLevel);
                var responseObj = JsonConvert.DeserializeAnonymousType(cResponseMessage.Data.ToString(), new List<KnowLedgeLevel>());
                cResponseMessage.Data = responseObj;
                var languagecv = Request.Cookies[CookieTypes.LANGUAGE].Substring(3);
                if (languagecv == "US")
                {
                    foreach (var Morg in responseObj.ToList())
                        Morg.LEVELDESC = Morg.LEVELDESCEN;
                };
                var viewmodel = new KnowLedgeLevelViewModel
                {
                    ListKnowLedgeLevel = responseObj,

                };                

                return View(viewmodel);
            }
            catch(Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = _localizer["InternalServerError"].Value });
            }
           
        }

        [HttpGet]
        [Route(LinkRoute.GET_LIST_CHUC_VU)]
        public async Task<IActionResult> GetListChuvu()
        {
            try
            {
                var cResponseMessage = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.Get_Chucvu);
                var responseObj = JsonConvert.DeserializeAnonymousType(cResponseMessage.Data.ToString(), new List<ManagerOrg>());
                var languagecv = Request.Cookies[CookieTypes.LANGUAGE].Substring(3);
                if (languagecv == "US")
                {
                    foreach (var Morg in responseObj.ToList())
                        Morg.AMOrgDesc = Morg.AMOrgDescEN;
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

        [HttpGet]
        [Route(LinkRoute.GET_LIST_MANAGER)]
        public async Task<IActionResult> GetListManager(ManagerViewModel managerViewModel)
        {
            try
            {
                var cpnyID = HttpContext.Session.GetInt32(SessionTypes.CPNY_ID);
                managerViewModel.CPNYID = cpnyID;
                var pram = "?" + managerViewModel.ToQueryString();

                var cResponseMessage = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.Get_Manager + pram);

                var responseObj = JsonConvert.DeserializeAnonymousType(cResponseMessage.Data.ToString(), new List<Manager>()); ;
                cResponseMessage.Data = responseObj;
                var languagecv = Request.Cookies[CookieTypes.LANGUAGE].Substring(3);
                if (languagecv == "US")
                {
                    foreach (var Morg in responseObj.ToList())
                    {
                        Morg.LISTMANAGERORGOFMANAGER = Morg.LISTMANAGERORGOFMANAGEREN;
                        Morg.KNOWLEDGESPECIALLEVEL = Morg.KNOWLEDGESPECIALLEVELEN;
                    }
                       

                }
                //var sortedList = responseObj.OrderBy(q => q.MNGERID).ToList();
                var sortedList = responseObj.ToList();
                return Json(responseObj);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = _localizer["InternalServerError"].Value });
            }
            
        }
        [HttpPost]
        [Route(LinkRoute.SAVE_LIST_MANAGER)]
        public async Task<IActionResult> SaveManager(ManagerViewModel managerViewModel, IFormFile file)
         {                      
                       
            try
            {
                try
                {
                    // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                    var checklogin = await CheckLoginSameTime();
                    if (!checklogin)
                    {
                        _errorHandler.WriteStringToFile("CheckLoginSameTime:", "SaveManager");
                        HttpContext.Session.Clear();

                        return RedirectToAction("index", "login");
                    }

                }
                catch (Exception ex)
                {
                    _appLogger.ErrorLogger.LogError(ex);
                }

                var cpnyID = HttpContext.Session.GetInt32(SessionTypes.CPNY_ID);
                var username = HttpContext.Session.GetString(SessionTypes.USERNAME);
                managerViewModel.CPNYID = cpnyID;
                managerViewModel.UserLogin = username;
                managerViewModel.RoleCode = RoleCode.THANH_PHAN_LANH_DAO;

                CResponseMessage cResponseMessage = new CResponseMessage { Code = -1, Message = "UnknowError" };

                if (managerViewModel == null)
                {
                    cResponseMessage.Message = "InvalidInputData";
                    return Json(cResponseMessage);
                }

                /// nội  dung mail được gửi cho chuyên viên

                var pramCpny = $"?cpnyID={cpnyID}";

                var responseCompanies = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.GET_COMPANY + pramCpny);

                var companies = JsonConvert.DeserializeAnonymousType(responseCompanies.Data.ToString(), new List<CompanyEzSearchTemp>());
                var company = companies.FirstOrDefault();

                string namevn = "- Tên doanh nghiệp: " + company.AName_VN;
                string nameen = "- Mã Chứng khoán: " + company.AStockCode;
                string date = "- Thời gian thực hiện: " + DateTime.Now;
                string mess = "- Xem thông tin phê duyệt: Tổng quan -> Thành phần lãnh đạo";
                _emailSettings = new EmailSettings
                {
                    Mail = _EMAIL_SPECIALIST,
                    Message = string.Format("{0}<br />{1}<br />{2}<br />{3}", namevn, nameen, date, mess),
                    Subject = "Notification Thêm mới tin trong ngày của KH EzIR - Nguồn: " + company.AStockCode + ".EzIR"
                };

                managerViewModel.EmailSettings = _emailSettings;
                var manager = managerViewModel;
                { 
                   
                    manager.EmailSettings = _emailSettings;

                }
                
                if (file != null)
                {
                    string[] validFileType = { "doc", "docx", "rar", "pdf" };
                    string FileExtension = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();
                    if (file.Length > 30 * 1024 * 1024)
                    {
                        return Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["FileIsOver30MB"].Value });
                    }
                    if (validFileType.Contains(FileExtension) && file.Length < 30 * 1024 * 1024)
                    {
                        //var result = await this._common.CopyFile(file, this._configuration["UploadCVPath"], this._hostingEnvironment, this._configuration);
                        var result = await this._common.SaveFile(file, this._configuration["UploadCVPath"], this._hostingEnvironment);
                        if (result.Message == "UPLOAD_FILE_SUCCESS")
                        {
                            managerViewModel.CVPATH = Path.Combine(this._configuration["UploadCVPath"], Path.GetFileName(result.Data.ToString()));

                            var cResponse = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.Save_Manager, managerViewModel);
                            cResponse.Message = this._sharedLocalizer[cResponse.Message];
                            return Json(cResponse);
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
                    var cResponse = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.Save_Manager, managerViewModel);
                    cResponse.Message = this._sharedLocalizer[cResponse.Message];
                    return Json(cResponse);
                }
                
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }


        [HttpPost]
        [Route(LinkRoute.UPDATE_LIST_MANAGER)]
        public async Task<IActionResult> UpdateManager(ManagerViewModel managerViewModel, IFormFile file)
        {
            try
            {
                try
                {
                    // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                    var checklogin = await CheckLoginSameTime();
                    if (!checklogin)
                    {
                        _errorHandler.WriteStringToFile("CheckLoginSameTime:", "UpdateManager");
                        HttpContext.Session.Clear();

                        return RedirectToAction("index", "login");
                    }

                }
                catch (Exception ex)
                {
                    _appLogger.ErrorLogger.LogError(ex);
                }

                var cpnyID = HttpContext.Session.GetInt32(SessionTypes.CPNY_ID);
                var username = HttpContext.Session.GetString(SessionTypes.USERNAME);
                managerViewModel.CPNYID = cpnyID;
                managerViewModel.UserLogin = username;
                managerViewModel.RoleCode = RoleCode.THANH_PHAN_LANH_DAO;


                CResponseMessage cResponseMessage = new CResponseMessage { Code = -1, Message = "UnknowError" };

                if (managerViewModel == null)
                {
                    cResponseMessage.Message = "InvalidInputData";
                    return Json(cResponseMessage);
                }

                var pramCpny = $"?cpnyID={cpnyID}";

                var responseCompanies = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.GET_COMPANY + pramCpny);

                var companies = JsonConvert.DeserializeAnonymousType(responseCompanies.Data.ToString(), new List<CompanyEzSearchTemp>());
                var company = companies.FirstOrDefault();

                string namevn = "- Tên doanh nghiệp: " + company.AName_VN;
                string nameen = "- Mã Chứng khoán: " + company.AStockCode;
                string date = "- Thời gian thực hiện: " + DateTime.Now;
                string mess = "- Xem thông tin phê duyệt: Tổng quan -> Thành phần lãnh đạo";
                _emailSettings = new EmailSettings
                {
                    Mail = _EMAIL_SPECIALIST,
                    Message = string.Format("{0}<br />{1}<br />{2}<br />{3}", namevn, nameen, date, mess),
                    Subject = "Notification Thêm mới tin trong ngày của KH EzIR - Nguồn: " + company.AStockCode + ".EzIR"
                };
                managerViewModel.EmailSettings = _emailSettings;
                var manager = managerViewModel;
                {

                    manager.EmailSettings = _emailSettings;

                }

                string[] validFileType = { "doc", "docx", "rar", "pdf" };
                if (file != null)
                {
                    string FileExtension = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();
                    if (file.Length > 30 * 1024 * 1024)
                    {
                        return Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["FileIsOver30MB"].Value });
                    }
                    if (validFileType.Contains(FileExtension))
                    {
                        //var result = await this._common.CopyFile(file, this._configuration["UploadCVPath"], this._hostingEnvironment, this._configuration);
                        var result = await this._common.SaveFile(file, this._configuration["UploadCVPath"], this._hostingEnvironment);
                        if (result.Message == "UPLOAD_FILE_SUCCESS")
                        {
                            managerViewModel.CVPATH = Path.Combine( this._configuration["UploadCVPath"], Path.GetFileName(result.Data.ToString()));
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
                    var cResponse = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.Update_Manage, managerViewModel);
                    cResponse.Message = this._sharedLocalizer[cResponse.Message];
                    return Json(cResponse);
                }
                else
                {
                    var cResponse = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.Update_Manage, managerViewModel);
                    cResponse.Message = this._sharedLocalizer[cResponse.Message];
                    return Json(cResponse);
                }
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }

            
        }

        [HttpPost]
        [Route(LinkRoute.DOWNLOAD_CV)]
        public async Task<IActionResult> DownloadFile(ManagerViewModel managerViewModel)
        {
            try
            {
                try
                {
                    // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                    var checklogin = await CheckLoginSameTime();
                    if (!checklogin)
                    {
                        _errorHandler.WriteStringToFile("CheckLoginSameTime:", "TamNhinChienLuoc_DownloadFile");
                        HttpContext.Session.Clear();

                        return RedirectToAction("index", "login");
                    }

                }
                catch (Exception ex)
                {
                    _appLogger.ErrorLogger.LogError(ex);
                }

                managerViewModel.UserLogin = HttpContext.Session.GetString(SessionTypes.USERNAME);
                managerViewModel.CPNYID = HttpContext.Session.GetInt32(SessionTypes.CPNY_ID);
                managerViewModel.RoleCode = RoleCode.THANH_PHAN_LANH_DAO;

                if (!string.IsNullOrEmpty(managerViewModel.UserLogin) && !string.IsNullOrEmpty(managerViewModel.RoleCode))
                {
                    var ms = new MemoryStream(CheckUtils.ReadFile(managerViewModel.CVPATH));

                    string FileExtension = Path.GetFileName(managerViewModel.CVPATH).Substring(Path.GetFileName(managerViewModel.CVPATH).LastIndexOf('.') + 1).ToLower();

                    if (FileExtension == "doc" || FileExtension == "docx")
                    {
                        return File(ms, "application/msword");
                    }
                    if (FileExtension == "pdf")
                    {
                        return File(ms, "application/pdf");
                    }
                    if (FileExtension == "rar")
                    {
                        return File(FileExtension, "application/x-rar-compressed");
                    }
                    if (FileExtension == "zip")
                    {
                        return File(ms, "application/x-zip-compressed");
                    }
                }

                var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
                return Json(response);
            }
            catch(Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
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
