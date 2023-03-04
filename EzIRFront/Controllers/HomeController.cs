using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EzIRFront.Models;
using Microsoft.Extensions.Caching.Memory;
using EzIRFront.GetDataFromDS;
using CoreLib.Entities;
using Microsoft.Extensions.Configuration;
using System.Data;
using DataServiceLib.Utilities;
using Microsoft.Extensions.Logging;
using CommonLib.Interfaces.Logger;
using CommonLib.Interfaces;
using CoreLib.Configs;
using System.Net.Http;
using System.Threading.Tasks;
using CoreLib.SharedKernel;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;
using CoreLib.Interfaces;
using CoreLib.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using EzIRFront.Models.ViewModels;

namespace EzIRFront.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        //private readonly ILogger<HomeController> _logger;//
        private IMemoryCache _cache;
        private IAppLogger _appLogger;
        private static readonly HttpClient client = new HttpClient();
        string baseUri = "";
        string URL = "";
        string logobanner = "";
        string urlpath = "";
        private readonly ISerilogProvider _serilogProvider;
        //private readonly IValidateApi _validateApi;
        private static WebApiConfigs _WebApiConfig = new WebApiConfigs();
        string tintucUri = "";
        //string bantinirUri = "";
        private readonly IHttpContextAccessor _httpContextAccessor;


        public HomeController(IHttpContextAccessor httpContextAccessor, IMemoryCache cache, IConfiguration config, IAppLogger appLogger, ISerilogProvider serilogProvider)//, ILogger<HomeController> logger
        {            
            _cache = cache;
            _config = config;
            _appLogger = appLogger;
            this._serilogProvider = serilogProvider;
            config.GetSection("API").Bind(_WebApiConfig);
            baseUri = _WebApiConfig.BASE_URL;
            URL = _config.GetSection("UrlBanner").Value;
            logobanner = _config.GetSection("LogoBannerPath").Value ;
            //urlpath = _config.GetSection("SpecialistPath").Value;
            tintucUri = _WebApiConfig.API_TinTuc;
            //bantinirUri = _WebApiConfig.API_TinTuc;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// lấy thông tin banner 
        /// lấy ảnh trong folder LogoBanner
        /// </summary>
        /// <returns></returns>
      
        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any, NoStore = false)]
      
        public async Task<IActionResult> Index()
        {
            try
            {
                
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Banner_List}";

                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Banner = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Banner) as JObject;

                var jArr = jObj.GetValue("table");

                var table = jArr.ToObject<List<Banner>>();
                ViewBag.ListBanner = table;
                ViewBag.LogoBanner = "data:image/jpeg;base64,";
                ViewBag.Url = URL;

            }
            catch
            {

            }
            
            try
            {
                var requestUri1 = $"{baseUri + WebApiConfigs.Link_WebApi_logo_List}";

                var response1 = await HttpRequestFactory.Get(requestUri1);

                this._serilogProvider.Logger.Debug($"Status:" + response1.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company1 = response1.Content.ReadAsStringAsync().Result;

                var jObj1 = JsonConvert.DeserializeObject(Data_Company1) as JObject;

                var jArr1 = jObj1.GetValue("table");

                var table1 = jArr1.ToObject<List<CompanyService>>();

                ViewBag.ListLogo = table1;
 

            }

            catch (Exception ex)
            {
               
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        ////api/companyinfo/listcompany
        /// <summary>
        /// Lấy thông tin mã chứng khoán, công ty
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<IActionResult> GetStockCode()
        {
            
            try
            {
                string language = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());

           

                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_StockCode + "?Language=" + language.ToUpper()}";

                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống
             
                var Data_Company = response.Content.ReadAsStringAsync().Result;
          
                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");

                var table = jArr.ToObject<List<CompanyService>>();

                var data = new List<object>();

                table.ForEach(x => data.Add(new { astock_CODE = x.astock_CODE, nameSearch = $"{x.astock_CODE} - {x.astockname}" }));

                return Json(data);
   
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetTinTuc()
        { 
            try
            {
               var cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
                if (cookieValue == "vn" || cookieValue == null)
                {
                    var requestUri = $"{tintucUri + WebApiConfigs.Link_WebApi_TinTucHomeVN}";
                    clsData tinTuc = new clsData();
                    var data = await CallNewsAPI.API_GetNew(requestUri);
                    tinTuc.Table1 = data.Data.Table1;
                   
                    return Json(tinTuc.Table1);
                }
                else
                {
                    var requestUri = $"{tintucUri + WebApiConfigs.Link_WebApi_TinTucHomeEN}";
                    clsData tinTuc = new clsData();
                    var data = await CallNewsAPI.API_GetNew(requestUri);
                    tinTuc.Table1 = data.Data.Table1;
                    return Json(tinTuc.Table1);
                }             
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }

        }

        /*[HttpGet]
        public async Task<IActionResult> GetBanTinIR()
        {
            try
            {
                var cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
                if (cookieValue == "vn" || cookieValue == null)
                {
                    var requestUri = $"{bantinirUri + WebApiConfigs.Link_WebApi_BanTinIRHomeVN}";
                    clsData banTin = new clsData();
                    var data = await CallNewsAPI.API_GetNew(requestUri);
                    banTin.Table1 = data.Data.Table1;
                    return Json(banTin.Table1);
                }
                else
                {
                    var requestUri = $"{bantinirUri + WebApiConfigs.Link_WebApi_BanTinIRHomeEN}";
                    clsData banTin = new clsData();
                    var data = await CallNewsAPI.API_GetNew(requestUri);
                    banTin.Table1 = data.Data.Table1;
                    return Json(banTin.Table1);
                }
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }

        }*/


        /// <summary>
        /// Link trang thông tin chung
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SelectStockCode(HomeViewModels homeViewModels)
        {
            var langValue ="" ;
            if (_httpContextAccessor.HttpContext.Request.QueryString.ToString().Contains("?culture=en-US"))
                langValue = "?culture=en-US";
            else
                langValue = "";
            return Redirect("/ThongTinDoanhNghiep/" + homeViewModels.astock_CODE + langValue);
        }


        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {

            //Response.Cookies.Append(
            //    CookieRequestCultureProvider.DefaultCookieName,
            //    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            //    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), SameSite = SameSiteMode.Lax }
            //);

            //SetCookies(CookieTypes.LANGUAGE, culture, 40);
            if (returnUrl.Contains("?culture=en-US"))
                returnUrl = returnUrl.Replace("?culture=en-US", "");
            else
                returnUrl = returnUrl + "?culture=en-US";
            return Redirect(returnUrl);
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


    }
}
