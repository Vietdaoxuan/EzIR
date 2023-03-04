using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Commons;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using EzIRCustomer.Models.ViewModels;

namespace EzIRCustomer.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHtmlLocalizer<SharedResource> _localizer;
        private readonly IConfiguration _configuration;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private string _BASE_API_URL;
        private readonly IHttpService _httpService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly ICommon _common;

        public LoginController(IConfiguration configuration, IAppLogger appLogger, IMemoryCache cache, IHttpService httpService, IHtmlLocalizer<SharedResource> localizer, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _appLogger = appLogger;
            _cache = cache;
            _httpService = httpService;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
            _BASE_API_URL = _configuration.GetSection("ApiUrl").Value;
        }

        public IActionResult Index()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);            
            return View();
        }

        public async Task<IActionResult> Login(string username, string password, string captchaCode)
        {
            string errorMessage = "UnknowError";
            int errorCode = -1;
            
            // Validate Username, Password
            if (Validate(username, password, out errorCode, out errorMessage))
            {                
                // Validate Captcha Code
                if (ValidateCaptchaCode(captchaCode)) //Captcha.ValidateCaptchaCode(captchaCode, HttpContext)
                {
                    // continue business logic
                    try
                    {
                        var ip = Response.HttpContext.Connection.RemoteIpAddress.ToString();
                        var browser = Request.Headers["User-Agent"].ToString();
                        var seed = PasswordGenerator.GetRandomPassword();
                        
                        var loginModel = new { Username = username, Password = password, IP = ip, Browser = browser, Seed = seed};
                        var infoSheetViewModel = new InfoSheetViewModel();

                        CResponseMessage cResponseMessage = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.GET_TOKEN, loginModel);

                        if (cResponseMessage.Code == 0 && cResponseMessage.Data != null)
                        {
                            var definition = new
                            {
                                Token = "",
                                Seed = "",
                                UserName = "",
                                FullName = "",
                                Email = "",
                                Phone = "",
                                CpnyID = 0,
                                StockCode = "",
                                StockName = "",
                                StockNameEn = "",
                                CompanyType = 0,
                                CompanyTypeName = "",
                                CompanyTypeNameEN = "",
                                EmailSpecialist = "",
                                EmailSpecialistCC = "",
                                ExpertName = "",
                                ExpertPhone = ""
                            };

                            var responseObj = JsonConvert.DeserializeAnonymousType(cResponseMessage.Data.ToString(), definition);

                            if(string.IsNullOrEmpty(responseObj.Token))
                            {
                                TempData.Add("Message", "InternalServerError");
                                SetCookies(CookieTypes.LANGUAGE, "vi-VN", 10080);
                                DeleteCookies();

                                return RedirectToAction("Index", "Login");
                            }

                            //lưu dữ liệu vào cookie
                            SetCookies(CookieTypes.TOKEN, responseObj.Token, 60);
                            SetCookies(CookieTypes.SEED, seed, 10080);
                            SetCookies(CookieTypes.USERNAME, responseObj.UserName ?? "", 60);
                            SetCookies(CookieTypes.FULL_NAME, responseObj.FullName ?? "", 60);
                            SetCookies(CookieTypes.EMAIL, responseObj.Email ?? "", 60);                            
                            SetCookies(CookieTypes.CPNY_ID, responseObj.CpnyID.ToString() ?? "", 60);
                            SetCookies(CookieTypes.STOCK_CODE, responseObj.StockCode ?? "", 60);
                            SetCookies(CookieTypes.STOCK_NAME, responseObj.StockName ?? "", 60);
                            SetCookies(CookieTypes.STOCK_NAMEEN, responseObj.StockName ?? "", 60);
                            SetCookies(CookieTypes.COMPANY_TYPE, responseObj.CompanyType.ToString() ?? "", 60);
                            SetCookies(CookieTypes.COMPANY_TYPE_NAME, responseObj.CompanyTypeName ?? "", 60);
                            SetCookies(CookieTypes.COMPANY_TYPE_NAMEEN, responseObj.CompanyTypeNameEN ?? "", 60);
                            SetCookies(CookieTypes.MAIL_SPECIALIST, responseObj.EmailSpecialist ?? "", 60);
                            SetCookies(CookieTypes.MAIL_SPECIALISTCC, responseObj.EmailSpecialistCC ?? "", 60);
                            SetCookies(CookieTypes.EXPERT_NAME, responseObj.ExpertName ?? "", 60);
                            SetCookies(CookieTypes.EXPERT_PHONE, responseObj.ExpertPhone ?? "", 60);
                            SetCookies(CookieTypes.LANGUAGE, "vi-VN", 10080);

                            _appLogger.InfoLogger.LogInfo("-----");
                            _appLogger.InfoLogger.LogInfo("TOKEN: " + responseObj.Token);
                            _appLogger.InfoLogger.LogInfo("TOKEN: " + HttpContext.Request.Cookies[CookieTypes.TOKEN]);
                            _appLogger.InfoLogger.LogInfo("SEED: " + HttpContext.Request.Cookies[CookieTypes.SEED]);
                            _appLogger.InfoLogger.LogInfo("USERNAME: " + HttpContext.Request.Cookies[CookieTypes.USERNAME]);
                            _appLogger.InfoLogger.LogInfo("FULL_NAME: " + HttpContext.Request.Cookies[CookieTypes.FULL_NAME]);
                            _appLogger.InfoLogger.LogInfo("EMAIL: " + HttpContext.Request.Cookies[CookieTypes.EMAIL]);
                            _appLogger.InfoLogger.LogInfo("CPNY_ID: " + HttpContext.Request.Cookies[CookieTypes.CPNY_ID]);
                            _appLogger.InfoLogger.LogInfo("STOCK_CODE: " + HttpContext.Request.Cookies[CookieTypes.STOCK_CODE]);
                            _appLogger.InfoLogger.LogInfo("STOCK_NAME: " + HttpContext.Request.Cookies[CookieTypes.STOCK_NAME]);
                            _appLogger.InfoLogger.LogInfo("STOCK_NAMEEN: " + HttpContext.Request.Cookies[CookieTypes.STOCK_NAMEEN]);
                            _appLogger.InfoLogger.LogInfo("COMPANY_TYPE: " + HttpContext.Request.Cookies[CookieTypes.COMPANY_TYPE]);
                            _appLogger.InfoLogger.LogInfo("COMPANY_TYPE_NAME: " + HttpContext.Request.Cookies[CookieTypes.COMPANY_TYPE_NAME]);
                            _appLogger.InfoLogger.LogInfo("COMPANY_TYPE_NAMEEN: " + HttpContext.Request.Cookies[CookieTypes.COMPANY_TYPE_NAMEEN]);
                            _appLogger.InfoLogger.LogInfo("MAIL_SPECIALIST: " + HttpContext.Request.Cookies[CookieTypes.MAIL_SPECIALIST]);
                            _appLogger.InfoLogger.LogInfo("MAIL_SPECIALIST: " + HttpContext.Request.Cookies[CookieTypes.MAIL_SPECIALISTCC]);
                            _appLogger.InfoLogger.LogInfo("EXPERT_NAME: " + HttpContext.Request.Cookies[CookieTypes.EXPERT_NAME]);
                            _appLogger.InfoLogger.LogInfo("IP: " + HttpContext.Request.Cookies[CookieTypes.MAIL_SPECIALIST]);
                            _appLogger.InfoLogger.LogInfo("BROWSER: " + HttpContext.Request.Cookies[CookieTypes.MAIL_SPECIALIST]);
                            _appLogger.InfoLogger.LogInfo("-----");

                            //lưu dữ liệu vào session
                            HttpContext.Session.SetString(SessionTypes.TOKEN, responseObj.Token);
                            HttpContext.Session.SetString(SessionTypes.USERNAME, responseObj.UserName ?? "");
                            HttpContext.Session.SetString(SessionTypes.FULL_NAME, responseObj.FullName ?? "");
                            HttpContext.Session.SetString(SessionTypes.EMAIL, responseObj.Email ?? "");
                            HttpContext.Session.SetString(SessionTypes.PHONE, responseObj.Phone ?? "");
                            HttpContext.Session.SetInt32(SessionTypes.CPNY_ID, responseObj.CpnyID);
                            HttpContext.Session.SetString(SessionTypes.STOCK_NAME, responseObj.StockName ?? "");
                            HttpContext.Session.SetString(SessionTypes.STOCK_NAMEEN, responseObj.StockNameEn ?? "");
                            HttpContext.Session.SetString(SessionTypes.STOCK_CODE, responseObj.StockCode ?? "");
                            HttpContext.Session.SetInt32(SessionTypes.COMPANY_TYPE, responseObj.CompanyType);
                            HttpContext.Session.SetString(SessionTypes.COMPANY_TYPE_NAME, responseObj.CompanyTypeName ?? "");
                            HttpContext.Session.SetString(SessionTypes.COMPANY_TYPE_NAMEEN, responseObj.CompanyTypeNameEN ?? "");
                            HttpContext.Session.SetString(SessionTypes.MAIL_SPECIALIST, responseObj.EmailSpecialist ?? "");
                            HttpContext.Session.SetString(SessionTypes.MAIL_SPECIALISTCC, responseObj.EmailSpecialistCC ?? "");
                            HttpContext.Session.SetString(SessionTypes.EXPERT_NAME, responseObj.ExpertName ?? "");
                            HttpContext.Session.SetString(SessionTypes.EXPERT_PHONE, responseObj.ExpertPhone ?? "");

                            // IHttpContextAccessor(nếu dùng IHttpContextAccessor để set session thì sẽ dùng được trong Injection)
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.TOKEN, responseObj.Token);
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.SEED, seed ?? "");
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.USERNAME, responseObj.UserName ?? "");
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.FULL_NAME, responseObj.FullName ?? "");
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.EMAIL, responseObj.Email ?? "");
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.PHONE, responseObj.Phone ?? "");
                            _httpContextAccessor.HttpContext.Session.SetInt32(SessionTypes.CPNY_ID, responseObj.CpnyID);
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.STOCK_NAME, responseObj.StockName ?? "");
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.STOCK_NAMEEN, responseObj.StockNameEn ?? "");
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.STOCK_CODE, responseObj.StockCode ?? "");
                            _httpContextAccessor.HttpContext.Session.SetInt32(SessionTypes.COMPANY_TYPE, responseObj.CompanyType);
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.COMPANY_TYPE_NAME, responseObj.CompanyTypeName ?? "");
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.COMPANY_TYPE_NAMEEN, responseObj.CompanyTypeNameEN ?? "");
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.MAIL_SPECIALIST, responseObj.EmailSpecialist ?? "");
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.MAIL_SPECIALISTCC, responseObj.EmailSpecialistCC ?? "");
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.EXPERT_NAME, responseObj.ExpertName ?? "");
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.EXPERT_PHONE, responseObj.ExpertPhone ?? "");

                            //Lưu dữ liệu IP và UserAgent, Seed vào session
                            HttpContext.Session.SetString(SessionTypes.IP, Response.HttpContext.Connection.RemoteIpAddress.ToString());
                            HttpContext.Session.SetString(SessionTypes.BROWSER, Request.Headers["User-Agent"].ToString());
                            HttpContext.Session.SetString(SessionTypes.SEED, seed.ToString());

                            Random random = new Random();
                            int num = random.Next(100000);

                            SetCookies(SessionTypes.LOGINCOOKIES, num.ToString(), 40);

                            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new List<Claim>() {
                                new Claim(ClaimTypes.Name,responseObj.UserName),
                                new Claim(ClaimTypes.Role,responseObj.UserName)
                            }, CookieAuthenticationDefaults.AuthenticationScheme);

                            ClaimsPrincipal user = new ClaimsPrincipal();
                            user.AddIdentity(claimsIdentity);

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                            user,
                                                            new AuthenticationProperties()
                                                            {
                                                                AllowRefresh = true,
                                                                ExpiresUtc = DateTime.Now.AddMinutes(1440),
                                                                //IsPersistent = true
                                                            });
                        }
                        else
                        {
                            TempData.Add("Message", cResponseMessage.Message);
                        }

                        ViewBag.Data = cResponseMessage;

                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        _appLogger.ErrorLogger.LogError(ex);

                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                        HttpContext.Session.Clear();

                        // set lại cookies
                        Response.Cookies.Append(
                            CookieRequestCultureProvider.DefaultCookieName,
                            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("vi-VN")),
                            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), SameSite = SameSiteMode.Lax }
                        );

                        TempData.Add("Message", "InternalServerError");
                        SetCookies(CookieTypes.LANGUAGE, "vi-VN", 10080);
                        DeleteCookies();

                        return RedirectToAction("Index", "Login");
                    }

                }
                else
                {
                    TempData.Add("Message", "IncorrectCaptcha");
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                TempData.Add("Message", errorMessage);                
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear();
            
            // set lại cookies
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("vi-VN")),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), SameSite = SameSiteMode.Lax }
            );

            SetCookies(CookieTypes.LANGUAGE, "vi-VN", 10080);
            DeleteCookies();

            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), SameSite = SameSiteMode.Lax }
            );

            SetCookies(CookieTypes.LANGUAGE, culture, 10080);

            return LocalRedirect(returnUrl);
        }

        public IActionResult GetCaptchaImage()
        {
            int width = 100;
            int height = 36;
            var captchaCode = Captcha.GenerateCaptchaCode();
            var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
            
            SetCookies(CookieTypes.CAPTCHA_CODE, result.CaptchaCode, 24);

            Stream s = new MemoryStream(result.CaptchaByteData);
            return new FileStreamResult(s, "image/png");
        }

        public bool ValidateCaptchaCode(string userInputCaptcha)
        {
            try
            {               
                if (string.IsNullOrEmpty(userInputCaptcha) || string.IsNullOrEmpty(Request.Cookies[CookieTypes.CAPTCHA_CODE]))
                    return false;

                var isValid = userInputCaptcha.Equals(Request.Cookies[CookieTypes.CAPTCHA_CODE]);                
                Response.Cookies.Delete(CookieTypes.CAPTCHA_CODE);

                return isValid;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Validate Username, Password
        private bool Validate(string username, string password, out int errorCode, out string errorMess)
        {
            errorCode = 0;
            errorMess = null;

            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    errorMess = "UsernameIsNotToEmpty";
                    errorCode = -1;
                    return false;
                }

                if (!CheckUtils.ContainsUnicodeCharacter(username))
                {
                    errorMess = "IncorrectUsernameOrPassword";
                    errorCode = -1;
                    return false;
                }

                if (string.IsNullOrEmpty(password))
                {
                    errorMess = "PasswordIsNotToEmpty";
                    errorCode = -1;
                    return false;
                }
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                throw;
            }
            return true;
        }

        // Set Cookies
        private void SetCookies(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            option.SameSite = SameSiteMode.Strict;
            option.HttpOnly = true;

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(key, value, option);
        }

        // Delete Cookies
        private void DeleteCookies()
        {
            Response.Cookies.Delete(CookieTypes.TOKEN);
            Response.Cookies.Delete(CookieTypes.LOGINCOOKIES);
            Response.Cookies.Delete(CookieTypes.USERNAME);
            Response.Cookies.Delete(CookieTypes.FULL_NAME);
            Response.Cookies.Delete(CookieTypes.EMAIL);
            Response.Cookies.Delete(CookieTypes.CPNY_ID);
            Response.Cookies.Delete(CookieTypes.STOCK_CODE);
            Response.Cookies.Delete(CookieTypes.STOCK_NAME);
            Response.Cookies.Delete(CookieTypes.STOCK_NAMEEN);
            Response.Cookies.Delete(CookieTypes.COMPANY_TYPE);
            Response.Cookies.Delete(CookieTypes.COMPANY_TYPE_NAME);
            Response.Cookies.Delete(CookieTypes.COMPANY_TYPE_NAMEEN);
            Response.Cookies.Delete(CookieTypes.MAIL_SPECIALIST);
            Response.Cookies.Delete(CookieTypes.SEED);
            Response.Cookies.Delete(CookieTypes.IP);
            Response.Cookies.Delete(CookieTypes.BROWSER);
        }
    }
}
