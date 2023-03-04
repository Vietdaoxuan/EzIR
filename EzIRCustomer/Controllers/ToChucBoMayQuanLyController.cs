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
using EzIRCustomer.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using EzIRCustomerAPI.Models;
using CoreLib.DataTableToObject.Mapping;
using Microsoft.Extensions.Localization;
using CommonLib.Interfaces;
using Microsoft.AspNetCore.Hosting;
using CoreLib.Entities.ViewModels;
using CoreLib.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Net.Http.Headers;
using CoreLib.Entities.ModelViews;
using EzIRCustomer.Authentication;

namespace EzIRCustomer.Controllers
{
    [RequiredLogin]
    public class ToChucBoMayQuanLyController : Controller
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

        public ToChucBoMayQuanLyController(IConfiguration configuration, IAppLogger appLogger, IMemoryCache cache, IHttpService httpService, IHtmlLocalizer<SharedResource> localizer, IStringLocalizer<SharedResource> sharedLocalizer, ICommon common, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, IErrorHandler errorHandler)
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
                // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                var checklogin = await CheckLoginSameTime();
                if (!checklogin)
                {
                    _errorHandler.WriteStringToFile("CheckLoginSameTime:", "ToChucBoMayQuanLy");
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
        [Route(LinkRoute.GET_TCBMQL)]
        public async Task<IActionResult> GetToChucBoMayQuanLy()
        {
            try
            {
                ToChucBoMayQuanLyViewModel toChucBoMayQuanLyViewModel = new ToChucBoMayQuanLyViewModel();
                var cpnyID = HttpContext.Session.GetInt32(SessionTypes.CPNY_ID);
                var username = HttpContext.Session.GetString(SessionTypes.USERNAME);
                //toChucBoMayQuanLyViewModel.OrgType = OrgType;
                toChucBoMayQuanLyViewModel.CpnyID = cpnyID;
                toChucBoMayQuanLyViewModel.UserLogin = username;
                var pram = "?" + toChucBoMayQuanLyViewModel.ToQueryString();

                var cResponseMessage = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.Get_ToChucBoMayQuanLy + pram);

                var responseObj = JsonConvert.DeserializeAnonymousType(cResponseMessage.Data.ToString(), new List<ToChucBoMayQuanLyModelView>()); ;
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
        [Route(LinkRoute.INSERT_TCBMQL)]
        public async Task<IActionResult> InsertToChucBoMayQuanLy(ToChucBoMayQuanLyViewModel toChucBoMayQuanLyViewModel, IFormFile file)
        {           
            try
            {
                try
                {
                    // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                    var checklogin = await CheckLoginSameTime();
                    if (!checklogin)
                    {
                        _errorHandler.WriteStringToFile("CheckLoginSameTime:", "InsertToChucBoMayQuanLy");
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
                toChucBoMayQuanLyViewModel.CpnyID = cpnyID;
                toChucBoMayQuanLyViewModel.UserLogin = username;
                toChucBoMayQuanLyViewModel.RoleCode = RoleCode.TO_CHUC_BO_MAY_QL;
                if (toChucBoMayQuanLyViewModel.OrgModelDesc == null)
                {
                    toChucBoMayQuanLyViewModel.OrgModelDescNote = "null";
                }
                else
                {
                    toChucBoMayQuanLyViewModel.OrgModelDescNote = toChucBoMayQuanLyViewModel.OrgModelDesc;
                }

                CResponseMessage cResponseMessage = new CResponseMessage { Code = -1, Message = "UnknowError" };

                if (toChucBoMayQuanLyViewModel == null)
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
                string mess = "- Xem thông tin phê duyệt: Tổng quan -> Tổ chức bộ máy quản lý";
                _emailSettings = new EmailSettings
                {
                    Mail = _EMAIL_SPECIALIST,
                    Message = string.Format("{0}<br />{1}<br />{2}<br />{3}", namevn, nameen, date, mess),
                    Subject = "Notification Cập nhật thông tin trong ngày của KH EzIR - Nguồn: " + company.AStockCode + ".EzIR"
                };

                toChucBoMayQuanLyViewModel.EmailSettings = _emailSettings;
                var toChucBoMayQuanLy = toChucBoMayQuanLyViewModel;
                {
                    var changeInfos = new List<ChangeInfo>();

                    var title = getTitleByMenuID(ConstMenuID.SO_DO_BO_MAY_QUAN_LY);

                    changeInfos.Add(new ChangeInfo
                    {
                        CpnyID = toChucBoMayQuanLy.CpnyID,
                        MenuID = ConstMenuID.SO_DO_BO_MAY_QUAN_LY,
                        Key = "ACONTENT",
                        Value = toChucBoMayQuanLy.OrgModelDesc +";"+ toChucBoMayQuanLy.OrgModelPath,
                        Status = 1,
                        KeyFunction = $"Tổ chức bộ máy Quản lý - {title}",
                        Function = "Tổ chức bộ máy Quản lý",
                        LevelID = "TQ",
                        DetailLevelID = "TCBMQL"
                    });

                    toChucBoMayQuanLy.EmailSettings = _emailSettings;

                    toChucBoMayQuanLy.ChangeInfos = changeInfos;
                    
                }

                if (!string.IsNullOrEmpty(toChucBoMayQuanLyViewModel.UserLogin) || !string.IsNullOrEmpty(toChucBoMayQuanLyViewModel.RoleCode))
                {
                    //lưu lại file ảnh trên thư mục
                    var result = await this._common.SaveFile(file, this._configuration["UploadOrgModelPath"], this._hostingEnvironment);
                    //var result = await this._common.SaveFile(file, this._configuration["UploadOrgModelPath"], this._hostingEnvironment, this._configuration);                
                    if (result.Code != 0)
                    {
                        toChucBoMayQuanLyViewModel.OrgModelPath = "";
                    }
                    else
                    {
                        toChucBoMayQuanLyViewModel.OrgModelPath = result.Data.ToString().Replace("Images\\", "").Replace(@"\", @"/").ToString();
                    }

                    var cResponse = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.Insert_ToChucBoMayQuanLy, toChucBoMayQuanLyViewModel);
                    cResponse.Message = this._sharedLocalizer[cResponse.Message];
                    return Json(cResponse);
                }

                var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

                return Json(response);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        [HttpPost]
        [Route(LinkRoute.UPDATE_TCBMQL)]
        public async Task<IActionResult> UpdateToChucBoMayQuanLy(ToChucBoMayQuanLyViewModel toChucBoMayQuanLyViewModel, IFormFile file)
        {
            try
            {
                try
                {
                    // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                    var checklogin = await CheckLoginSameTime();
                    if (!checklogin)
                    {
                        _errorHandler.WriteStringToFile("CheckLoginSameTime:", "UpdateToChucBoMayQuanLy");
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
                toChucBoMayQuanLyViewModel.CpnyID = cpnyID;
                toChucBoMayQuanLyViewModel.UserLogin = username;
                toChucBoMayQuanLyViewModel.RoleCode = RoleCode.TO_CHUC_BO_MAY_QL;

                if (toChucBoMayQuanLyViewModel.OrgModelDesc == null)
                {
                    toChucBoMayQuanLyViewModel.OrgModelDescNote = "null";
                }
                else
                {
                    toChucBoMayQuanLyViewModel.OrgModelDescNote = toChucBoMayQuanLyViewModel.OrgModelDesc;
                }


                CResponseMessage cResponseMessage = new CResponseMessage { Code = -1, Message = "UnknowError" };

                if (toChucBoMayQuanLyViewModel == null)
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
                string mess = "- Xem thông tin phê duyệt: Tổng quan -> Tổ chức bộ máy quản lý";
                _emailSettings = new EmailSettings
                {
                    Mail = _EMAIL_SPECIALIST,
                    Message = string.Format("{0}<br />{1}<br />{2}<br />{3}", namevn, nameen, date, mess),
                    Subject = "Notification Cập nhật thông tin trong ngày của KH EzIR - Nguồn: " + company.AStockCode + ".EzIR"
                }; toChucBoMayQuanLyViewModel.EmailSettings = _emailSettings;
                var toChucBoMayQuanLy = toChucBoMayQuanLyViewModel;
                {
                    var changeInfos = new List<ChangeInfo>();

                    var title = getTitleByMenuID(ConstMenuID.SO_DO_BO_MAY_QUAN_LY);

                    changeInfos.Add(new ChangeInfo
                    {
                        CpnyID = toChucBoMayQuanLy.CpnyID,
                        MenuID = ConstMenuID.SO_DO_BO_MAY_QUAN_LY,
                        Key = "ACONTENT",
                        Value = toChucBoMayQuanLy.OrgModelDesc + ";" + toChucBoMayQuanLy.OrgModelPath,
                        Status = 2,
                        KeyFunction = $"Tổ chức bộ máy Quản lý - {title}",
                        Function = "Tổ chức bộ máy Quản lý",
                        LevelID = "TQ",
                        DetailLevelID = "TCBMQL"
                    });

                    toChucBoMayQuanLy.EmailSettings = _emailSettings;

                    toChucBoMayQuanLy.ChangeInfos = changeInfos;

                }


                if (!string.IsNullOrEmpty(toChucBoMayQuanLyViewModel.UserLogin) && !string.IsNullOrEmpty(toChucBoMayQuanLyViewModel.RoleCode))
                {
                    ///lưu lại file ảnh trên thư mục

                    if (file != null)
                    {
                        var result = await this._common.SaveFile(file, this._configuration["UploadOrgModelPath"], this._hostingEnvironment);
                        if (result.Code != 0)
                        {
                            toChucBoMayQuanLyViewModel.OrgModelPath = "";
                        }
                        else
                        {
                            toChucBoMayQuanLyViewModel.OrgModelPath = result.Data.ToString().Replace("Images\\", "").Replace(@"\", @"/").ToString();
                        }
                    }

                    var cResponse = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.Update_ToChucBoMayQuanLy, toChucBoMayQuanLyViewModel);
                    cResponse.Message = this._sharedLocalizer[cResponse.Message];
                    return Json(cResponse);
                }

                var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

                return Json(response);
            }
            catch(Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
       
        }
        private string getTitleByMenuID(int? menuid)
        {
            switch (menuid)
            {
                case ConstMenuID.SO_DO_BO_MAY_QUAN_LY:
                    {
                        return "Sơ đồ bộ máy quản lý";
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
