using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Entities.ModelViews;
using CoreLib.Entities.ViewModels;
using CoreLib.Interfaces;
using CoreLib.SharedKernel;
using CoreLib.ViewModel;
using EzIRCustomer.Commons;
using EzIRCustomerAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace EzIRCustomer.Controllers
{
    public class ThuVienPhapLuatController : Controller
    {
        private readonly IHtmlLocalizer<SharedResource> _localizer;
        private readonly IConfiguration _configuration;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private string _BASE_API_URL;
        private readonly IHttpService _httpService;    
        private readonly ICommon _common;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IErrorHandler _errorHandler;

        string LinkURL = "";

        public ThuVienPhapLuatController(IConfiguration configuration, IAppLogger appLogger, IMemoryCache cache, IHttpService httpService, IHtmlLocalizer<SharedResource> localizer, IStringLocalizer<SharedResource> sharedLocalizer, ICommon common, IHostingEnvironment hostingEnvironment, IErrorHandler errorHandler)
        {
            _configuration = configuration;
            _appLogger = appLogger;
            _cache = cache;
            _common = common;
            _sharedLocalizer = sharedLocalizer;
            _hostingEnvironment = hostingEnvironment;
            _httpService = httpService;
            _localizer = localizer;
            _errorHandler = errorHandler;
            _BASE_API_URL = _configuration.GetSection("ApiUrl").Value;
            LinkURL = _configuration.GetSection("UrlFileCommon").Value;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                var checklogin = await CheckLoginSameTime();
                if (!checklogin)
                {
                    _errorHandler.WriteStringToFile("CheckLoginSameTime:", "ThuVienPhapLuat");
                    HttpContext.Session.Clear();

                    return RedirectToAction("index", "login");
                }

            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
            }

            ViewBag.LinkURL = LinkURL + "/";

            List<CommonType> listTemplateType = new List<CommonType>();
            var commonType = new CommonTypeViewModel
            {
                ListCategory = "20",
            };

            var pram = "?" + commonType.ToQueryString();
            var cResponseMessage = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.CommonCategory + pram);
            var responseObj = JsonConvert.DeserializeAnonymousType(cResponseMessage.Data.ToString(), new List<CoreLib.Entities.CommonType>());
           
            foreach (var obj in responseObj.ToList())
            {
                if (obj.Category == ConstCommonTypes.NHOM_LOAI_VAN_BAN)
                {
                    listTemplateType.Add(obj);
                }
            }

            var viewmodel = new TemplateViewModel
            {
                listTemplateType = listTemplateType,
            };

            var languagetemp = Request.Cookies[CookieTypes.LANGUAGE].Substring(3);
            if (languagetemp == "US")
            {
                foreach (var TemplateType in listTemplateType.ToList())
                    TemplateType.TypeName = TemplateType.TypeNameEN;
            }

            return View(viewmodel);         
        }

        [HttpGet]
        [Route(LinkRoute.GET_THUVIENPHAPLUAT)]
        public async Task<IActionResult> GetListThuVienPhapLuat(ThuVienPhapLuatViewModel thuvienphapluatviewmodel)
        {
            try
            {

                //var pram = "?" + infoSheetViewModel.ToQueryString();

                //var cResponseMessage = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.GET_INFOSHEET + pram);

                //var responseObj = JsonConvert.DeserializeAnonymousType(cResponseMessage.Data.ToString(), new List<InfoSheet>());

                //responseObj = responseObj.Where(a => a.MenuID == ConstMenuID.TAM_NHIN || a.MenuID == ConstMenuID.CHIEN_LUOC).ToList();

                //cResponseMessage.Data = responseObj;

                ////ViewBag.Data = cResponseMessage;

                //return Json(cResponseMessage);
                //thuvienphapluatviewmodel.TypeDoc = cpnyID;
                //templateViewModel.CompanyType = companyType;

                var pram = "?" + thuvienphapluatviewmodel.ToQueryString();

                var cResponseMessage = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.GetThuVienPhapLuat + pram);
                var responseObj = JsonConvert.DeserializeAnonymousType(cResponseMessage.Data.ToString(), new List<ThuVienPhapLuatModelView>()); ;



                //var responseObj = JsonConvert.DeserializeObject<List<List<ThuVienPhapLuatModelView>>>(cResponseMessage.Data.ToString());
                cResponseMessage.Data = responseObj;
                //var languagenews = Request.Cookies[CookieTypes.LANGUAGE].Substring(3);
                //if (languagenews == "US")
                //{
                //    foreach (var Templatelist in responseObj.ToList())
                //        foreach (var Template in Templatelist.ToList())
                //        {
                //            Template.TypeName = Template.TypeNameEN;
                //        }

                //}

                return Json(responseObj.OrderByDescending(yB => yB.datePub).ThenByDescending(yE => yE.dateEff));             

            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = _localizer["InternalServerError"].Value });
            }

        }

        [HttpPost]
        [Route(LinkRoute.DOWNLOAD_TVPL)]
        public async Task<IActionResult> DownloadFile(ThuVienPhapLuatViewModel thuvienphapluatviewmodel)
        {
            try
            {
                try
                {
                    // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                    var checklogin = await CheckLoginSameTime();
                    if (!checklogin)
                    {
                        _errorHandler.WriteStringToFile("CheckLoginSameTime:", "ThuVienPhapLuat_DownloadFile");
                        HttpContext.Session.Clear();

                        return RedirectToAction("index", "login");
                    }

                }
                catch (Exception ex)
                {
                    _appLogger.ErrorLogger.LogError(ex);
                }

                string _UserLogin = HttpContext.Session.GetString(SessionTypes.USERNAME);                
                string _RoleCode = RoleCode.THU_VIEN_PHAP_LUAT;

                if (!string.IsNullOrEmpty(_UserLogin) && !string.IsNullOrEmpty(_RoleCode))
                {                                     
                    string Url = thuvienphapluatviewmodel.Path;
                    string myStringWebResource = null;
                    
                    myStringWebResource = LinkURL + Url;

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
            catch (Exception ex)
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
