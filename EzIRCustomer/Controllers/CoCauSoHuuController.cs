using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Interfaces;
using CoreLib.SharedKernel;
using EzIRCustomer.Authentication;
using EzIRCustomer.Commons;
using EzIRCustomerAPI.Models;
using EzIRCustomerAPI.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomer.Controllers
{
    [RequiredLogin]
    public class CoCauSoHuuController : Controller
    {
        private readonly IHtmlLocalizer<SharedResource> _localizer;
        private readonly IConfiguration _configuration;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IHttpService _httpService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IErrorHandler _errorHandler;
        private string _BASE_API_URL;
        private int? _CPNYID;
        private string _USERNAME;
        private string _EMAIL_SPECIALIST;
        private EmailSettings _emailSettings;

        public CoCauSoHuuController(IConfiguration configuration, IAppLogger appLogger, IMemoryCache cache, IHttpService httpService, IHtmlLocalizer<SharedResource> localizer, IHttpContextAccessor httpContextAccessor, IErrorHandler errorHandler)
        {
            _configuration = configuration;
            _appLogger = appLogger;
            _cache = cache;
            _httpService = httpService;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
            _errorHandler = errorHandler;
            _BASE_API_URL = _configuration.GetSection("ApiUrl").Value;
            _CPNYID = _httpContextAccessor.HttpContext.Session.GetInt32(SessionTypes.CPNY_ID);
            _USERNAME = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.USERNAME);
            _EMAIL_SPECIALIST = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.MAIL_SPECIALIST);
            _emailSettings = new EmailSettings { Mail = _EMAIL_SPECIALIST, Message = "Cập nhật Tổng quan -> Cơ cấu sở hữu!", Subject = "Cập nhật Tổng quan -> Cơ cấu sở hữu!" };
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                var checklogin = await CheckLoginSameTime();
                if (!checklogin)
                {
                    _errorHandler.WriteStringToFile("CheckLoginSameTime:", "GetCongTy");
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
        [Route(LinkRoute.GET_CCSH)]
        public async Task<IActionResult> GetCongTy(CoCauSoHuuViewModel coCauSoHuuViewModel)
        {
            try
            {

                //coCauSoHuuViewModel.CpnyID = _CPNYID;

                var pram = "?" + coCauSoHuuViewModel.ToQueryString();

                var cResponseMessage = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.GetCongTy + pram);

                var responseObj = JsonConvert.DeserializeObject<List<List<SubCompany>>>(cResponseMessage.Data.ToString());
                var languagecv = Request.Cookies[CookieTypes.LANGUAGE].Substring(3);
                if (languagecv == "US")
                {
                    foreach (var company in responseObj.ToList()[0])
                    {
                        /// công ty con
                        if (company.asubcompanyen != null)
                        {
                            company.asubcompanyname = company.asubcompanyen;
                        }
                        if(company.aaddressen != null)
                        {
                            company.aaddress = company.aaddressen;
                        }
                    }

                    foreach (var company in responseObj.ToList()[1])
                    {
                        /// công ty liên kết
                        if (company.asubcompanyen != null)
                        {
                            company.asubcompanyname = company.asubcompanyen;
                        }
                        if (company.aaddressen != null)
                        {
                            company.aaddress = company.aaddressen;
                        }
                    }
                }

                cResponseMessage.Data = responseObj;

                return Json(cResponseMessage.Data);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });

            }
        }

        [HttpGet]
        public async Task<IActionResult> GetNganh()
        {
            try
            {

                var cResponseMessage = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.Get_Ministry);
                var responseObj = JsonConvert.DeserializeObject<List<Ministry>>(cResponseMessage.Data.ToString());
                var languagecv = Request.Cookies[CookieTypes.LANGUAGE].Substring(3);
                if (languagecv == "US")
                {
                    foreach (var Morg in responseObj.ToList())
                        Morg.anamevn = Morg.anameen;
                }
                cResponseMessage.Data = responseObj;
                return Json(cResponseMessage.Data);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });

            }
         
        }

        [HttpGet]
        public async Task<IActionResult> GetSubCompanyType(CoCauSoHuuViewModel coCauSoHuuViewModel)
        {
            try
            {
                var cResponseMessage = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.Get_SubCompanyType + "?" + coCauSoHuuViewModel.ToQueryString());
                var responseObj = JsonConvert.DeserializeObject<List<SubCompany>>(cResponseMessage.Data.ToString());

                cResponseMessage.Data = responseObj;
                return Json(cResponseMessage.Data);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });

            }
        }

        [HttpPost]
        [Route(LinkRoute.SAVE_CCSH)]
        public async Task<IActionResult> SaveCCSH(CoCauSoHuuViewModel coCauSoHuuViewModel, CoCauSoHuu coCauSoHuu, string jsonSubCompany, string jsonSharehoder)
        {
            try
            {
                try
                {
                    // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                    var checklogin = await CheckLoginSameTime();
                    if (!checklogin)
                    {
                        _errorHandler.WriteStringToFile("CheckLoginSameTime:", "SaveCCSH");
                        Response.Cookies.Delete(CookieTypes.TOKEN);
                        HttpContext.Session.Clear();
                        return RedirectToAction("index", "login");
                    }

                } catch (Exception ex) {
                    _appLogger.ErrorLogger.LogError(ex);
                }
                var pram = "?" + coCauSoHuuViewModel.ToQueryString();
                var cResponseMessage = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.GetCongTy + pram);

                var responseObj = JsonConvert.DeserializeObject<List<List<SubCompany>>>(cResponseMessage.Data.ToString());


                var cpnyID = HttpContext.Session.GetInt32(SessionTypes.CPNY_ID);

                coCauSoHuu.UserName = _USERNAME;
                coCauSoHuu.RoleCode = RoleCode.CO_CAU_SO_HUU;
                List<SubCompany> lsSubCompany = null;
                List<ShareHolder> lsShareHolder = null;
                var languagecv = Request.Cookies[CookieTypes.LANGUAGE].Substring(3);

                if (!string.IsNullOrEmpty(jsonSubCompany) ||!string.IsNullOrEmpty(jsonSharehoder))
                {                 
                    lsSubCompany = JsonConvert.DeserializeAnonymousType(jsonSubCompany, new List<SubCompany>());
                    
                    lsShareHolder = JsonConvert.DeserializeAnonymousType(jsonSharehoder, new List<ShareHolder>());
                   
                    if(lsSubCompany != null && lsSubCompany.Count > 0)
                    {
                        foreach(var item in lsSubCompany)
                        {
                            item.UserName = _USERNAME;
                            item.RoleCode = RoleCode.CO_CAU_SO_HUU;

                            if (languagecv == "US")
                            {
                                foreach (var sub in responseObj.ToList()[0])
                                {
                                    if (sub.asubcompanyid == item.asubcompanyid)
                                    {
                                        item.aaddressen = item.aaddress;
                                        item.aaddress = sub.aaddress;
                                        item.asubcompanyen = item.asubcompanyname;
                                        item.asubcompanyname = sub.asubcompanyname;
                                    }
                                }

                                foreach (var sub in responseObj.ToList()[1])
                                {
                                    /// công ty liên kết
                                    if (sub.asubcompanyid == item.asubcompanyid)
                                    {
                                        item.aaddressen = item.aaddress;
                                        item.aaddress = sub.aaddressen;
                                        item.asubcompanyen = item.asubcompanyname;
                                        item.asubcompanyname = sub.asubcompanyname;
                                    }
                                }
                            }
                        } 
                    }
                    else if(lsShareHolder != null && lsShareHolder.Count > 0)
                    {
                        foreach (var item in lsShareHolder)
                        {
                            item.UserName = _USERNAME;
                            item.RoleCode = RoleCode.CO_CAU_SO_HUU;
                           
                        }
                    }
                   
                }

                /// nội  dung mail được gửi cho chuyên viên
                var pramCpny = $"?cpnyID={cpnyID}";

                var responseCompanies = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.GET_COMPANY + pramCpny);

                var companies = JsonConvert.DeserializeAnonymousType(responseCompanies.Data.ToString(), new List<CompanyEzSearchTemp>());
                var company = companies.FirstOrDefault();

                string namevn = "- Tên doanh nghiệp: " + company.AName_VN;
                string nameen = "- Mã Chứng khoán: " + company.AStockCode;
                string date = "- Thời gian thực hiện: " + DateTime.Now;
                string mess = "- Xem thông tin phê duyệt: Tổng quan -> Cơ cấu sở hữu";
                _emailSettings = new EmailSettings
                {
                    Mail = _EMAIL_SPECIALIST,
                    Message = string.Format("{0}<br />{1}<br />{2}<br />{3}", namevn, nameen, date, mess),
                    Subject = "Notification Cập nhật thông tin trong ngày của KH EzIR - Nguồn: " + company.AStockCode + ".EzIR"
                };


                var responseMessage = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.Insert_Company_ShareHolder, new CoCauSoHuu { ListSubCompany = lsSubCompany, ListShareHolder = lsShareHolder, emailSettings = _emailSettings } );
                
                responseMessage.Message = _localizer[responseMessage.Message].Value.ToString();

                return Json(responseMessage);

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
