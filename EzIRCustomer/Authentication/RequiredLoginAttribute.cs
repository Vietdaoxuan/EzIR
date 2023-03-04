using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Interfaces;
using CoreLib.SharedKernel;
using EzIRCustomer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomer.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RequiredLoginAttribute : Attribute, IAuthorizationFilter
    {
        private IStringLocalizer<SharedResource> _stringLocalizer;
        public RequiredLoginAttribute()
        {
            

        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            this._stringLocalizer = (IStringLocalizer<SharedResource>)context.HttpContext.RequestServices.GetService(typeof(IStringLocalizer<SharedResource>));

            // nếu cookie chưa có giá trị của ngôn ngữ thì set ngôn ngữ là tiếng việt
            if (!context.HttpContext.Request.Cookies.ContainsKey(CookieRequestCultureProvider.DefaultCookieName))
            {
                context.HttpContext.Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("vi-VN")),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            }
           
            try
            {

                if (context.HttpContext.Session.GetString(SessionTypes.TOKEN) == null
                    )
                {

                    // Kiểm tra nếu cookies rỗng thì không gán

                    if (context.HttpContext.Request.Cookies[CookieTypes.TOKEN] != null
                        && context.HttpContext.Request.Cookies[CookieTypes.USERNAME] != null
                        && context.HttpContext.Request.Cookies[CookieTypes.FULL_NAME] != null
                        && context.HttpContext.Request.Cookies[CookieTypes.EMAIL] != null
                        && context.HttpContext.Request.Cookies[CookieTypes.CPNY_ID] != null
                        && context.HttpContext.Request.Cookies[CookieTypes.STOCK_CODE] != null
                         && context.HttpContext.Request.Cookies[CookieTypes.STOCK_NAME] != null
                        && context.HttpContext.Request.Cookies[CookieTypes.COMPANY_TYPE] != null
                         && context.HttpContext.Request.Cookies[CookieTypes.COMPANY_TYPE_NAME] != null
                        && context.HttpContext.Request.Cookies[CookieTypes.MAIL_SPECIALIST] != null
                          )
                    {
                        context.HttpContext.Session.SetString(SessionTypes.TOKEN, context.HttpContext.Request.Cookies[CookieTypes.TOKEN]);
                        context.HttpContext.Session.SetString(SessionTypes.SEED, context.HttpContext.Request.Cookies[CookieTypes.SEED]);
                        context.HttpContext.Session.SetString(SessionTypes.USERNAME, context.HttpContext.Request.Cookies[CookieTypes.USERNAME]);
                        context.HttpContext.Session.SetString(SessionTypes.FULL_NAME, context.HttpContext.Request.Cookies[CookieTypes.FULL_NAME]);
                        context.HttpContext.Session.SetString(SessionTypes.EMAIL, context.HttpContext.Request.Cookies[CookieTypes.EMAIL]);
                        context.HttpContext.Session.SetString(SessionTypes.CPNY_ID, context.HttpContext.Request.Cookies[CookieTypes.CPNY_ID]);
                        context.HttpContext.Session.SetString(SessionTypes.STOCK_CODE, context.HttpContext.Request.Cookies[CookieTypes.STOCK_CODE]);
                        context.HttpContext.Session.SetString(SessionTypes.STOCK_NAME, context.HttpContext.Request.Cookies[CookieTypes.STOCK_NAME]);
                        context.HttpContext.Session.SetString(SessionTypes.COMPANY_TYPE, context.HttpContext.Request.Cookies[CookieTypes.COMPANY_TYPE]);
                        context.HttpContext.Session.SetString(SessionTypes.COMPANY_TYPE_NAME, context.HttpContext.Request.Cookies[CookieTypes.COMPANY_TYPE_NAME]);
                        context.HttpContext.Session.SetString(SessionTypes.MAIL_SPECIALIST, context.HttpContext.Request.Cookies[CookieTypes.MAIL_SPECIALIST]);

                        
                    }
                    else  //Nếu trong cooikes cũng bằng null thì nhảy ra trang đăng nhập
                    {
                        context.HttpContext.Session.Clear();
                        context.Result = new RedirectToActionResult("index", "Login", null);
                    }
                }

              

            }
            catch (Exception e)
            {
                var errorHandler = (IErrorHandler)context.HttpContext.RequestServices.GetService(typeof(IErrorHandler));
                errorHandler.WriteToFile(e);
            }
        }
    }
}
