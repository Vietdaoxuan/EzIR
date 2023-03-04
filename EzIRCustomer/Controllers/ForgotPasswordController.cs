using CommonLib.Interfaces;
using CoreLib.Configs;
using CoreLib.Interfaces;
using EzIRCustomer.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomer.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private string BaseUrl { get; }
        private readonly ICommon _common;
        private readonly IConfiguration _configuration;
        private readonly IHttpService _httpService;
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public ForgotPasswordController(ICommon common,
           IConfiguration configuration,
           IStringLocalizer<SharedResource> stringLocalizer, IHttpService httpServices)
        {
            _common = common;
            _configuration = configuration;
            _stringLocalizer = stringLocalizer;
            _httpService = httpServices;
            BaseUrl = _configuration.GetSection("ApiUrl").Value;
        }
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(ForgotPasswordViewModel forgotPasswordModel)
        {
            if (ValidateCaptchaCode(forgotPasswordModel.GCaptcha)) 
            {
                // continue business logic
                _httpService.SetRoleControllerToHeader(RoleCode.FR_LOGIN);
                _httpService.SetUsernameControllerToHeader(forgotPasswordModel.Username);
                var data = await _httpService.ResponseMessageFromPostUrl(BaseUrl + ApiRoute.Forgot_Password, forgotPasswordModel);
                data.Message = _stringLocalizer[data.Message];
                return Json(data);

            }
            else
            {
                TempData.Add("Message", "IncorrectCaptcha");
                return RedirectToAction("Index", "ForgotPassword");
            }
           
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
    }
}
