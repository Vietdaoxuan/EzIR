using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Services;
using CoreLib.SharedKernel;
using EzIRFront.Models;
using EzIRFront.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EzIRFront.Controllers
{
    public class HomeDemoController : Controller
    {
        private readonly IConfiguration _config;
        //private readonly ILogger<HomeDemoController> _logger;//
        private IMemoryCache _cache;
        private IAppLogger _appLogger;
        private static readonly HttpClient client = new HttpClient();
        string baseUri = "";
        string URL = "";
        string imgURL = "";
        string logobanner = "";
        string urlpath = "";
        private readonly ISerilogProvider _serilogProvider;
        //private readonly IValidateApi _validateApi;
        private static WebApiConfigs _WebApiConfig = new WebApiConfigs();
        string tintucUri = "";
        string bantinUri = "";
        //string bantinirUri = "";
        private readonly IHttpContextAccessor _httpContextAccessor;


        public HomeDemoController(IHttpContextAccessor httpContextAccessor, IMemoryCache cache, IConfiguration config, IAppLogger appLogger, ISerilogProvider serilogProvider)//, ILogger<HomeController> logger
        {
            _cache = cache;
            _config = config;
            _appLogger = appLogger;
            this._serilogProvider = serilogProvider;
            config.GetSection("API").Bind(_WebApiConfig);
            baseUri = _WebApiConfig.BASE_URL;
            URL = _config.GetSection("UrlBanner").Value;
            imgURL = _config.GetSection("UrlImgHome").Value;
            logobanner = _config.GetSection("LogoBannerPath").Value;
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
            ViewBag.ImageURL = imgURL;
           

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

                table.ForEach(x => data.Add(new { astock_CODE = x.astock_CODE, name = $"{x.astock_CODE} - {x.astockname}", floor = x.aposT_TO, logopath = x.alogopath }));

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

        [HttpGet]
        public async Task<IActionResult> GetBanTinIR()
        {
            try
            {
                var cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
                if (cookieValue == "vn" || cookieValue == null)
                {
                    var requestUri = $"{tintucUri + WebApiConfigs.Link_WebApi_BanTinIRHomeVN}";
                    clsData banTin = new clsData();
                    var data = await CallNewsAPI.API_GetNew(requestUri);
                    banTin.Table1 = data.Data.Table1;
                    return Json(banTin.Table1);
                }
                else
                {
                    var requestUri = $"{tintucUri + WebApiConfigs.Link_WebApi_BanTinIRHomeEN}";
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

        }

        /// <summary>
        /// Link trang thông tin chung
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SelectStockCode(HomeViewModels homeViewModels)
        {
            var langValue = "";
            if (_httpContextAccessor.HttpContext.Request.QueryString.ToString().Contains("?culture=en-US"))
                langValue = "?culture=en-US";
            else
                langValue = "";
            return Redirect("/ThongTinDoanhNghiep/" + homeViewModels.astock_CODE + langValue);
        }

        [HttpGet]
        public async Task<IActionResult> GetLogoCompany()
        {
            //lấy logo công ty
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

                var data = new List<object>();

                table1.ForEach(x => data.Add(new { astock_CODE = x.astock_CODE, logopath = x.alogopath , awebsite = x.awebsite}));

                return Json(data);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
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
