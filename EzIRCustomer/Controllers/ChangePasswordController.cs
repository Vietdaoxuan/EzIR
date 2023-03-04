using CommonLib.Interfaces;
using CoreLib.Commons;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Interfaces;
using EzIRCustomer.Authentication;
using EzIRCustomer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomer.Controllers
{
    [RequiredLogin]
    public class ChangePasswordController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;
        private readonly IConfiguration _configuration;
        private readonly ICommon _common;
        private string _BASE_API_URL;
        private readonly IHttpService _httpService;

        public ChangePasswordController(
            IStringLocalizer<SharedResource> stringLocalizer,
             IHttpService httpService,
            IConfiguration configuration,
            ICommon common)
        {
            this._stringLocalizer = stringLocalizer;
            this._configuration = configuration;
            this._common = common;
            _httpService = httpService;
            _BASE_API_URL = _configuration.GetSection("ApiUrl").Value;
        }


        public IActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ValidateCaptchaCode(changePasswordViewModel.GCaptcha))
            {
                var response = new CResponseMessage(0, string.Empty);
                changePasswordViewModel.Username = HttpContext.Session.GetString(SessionTypes.USERNAME);
                changePasswordViewModel.Cpnyid = HttpContext.Session.GetInt32(SessionTypes.CPNY_ID);
                changePasswordViewModel.Email = HttpContext.Session.GetString(SessionTypes.EMAIL);
                //Set RoleCode
                _httpService.SetRoleControllerToHeader(RoleCode.DOI_MAT_KHAU);
                response = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.Change_Password, changePasswordViewModel);
                response.Message = this._stringLocalizer[response.Message].Value;
                return this.Json(response);
            }
            else
            {
                TempData.Add("Message", "IncorrectCaptcha");
                return RedirectToAction("Index", "ChangePassword");
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