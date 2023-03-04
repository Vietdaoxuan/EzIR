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
    public class DanhMucBieuMauController : Controller
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
        private readonly IErrorHandler _errorHandler;

        string _UrlFileCommon;
        public DanhMucBieuMauController(IConfiguration configuration, IAppLogger appLogger, IMemoryCache cache, IHttpService httpService, IHtmlLocalizer<SharedResource> localizer, IStringLocalizer<SharedResource> sharedLocalizer, ICommon common, IHostingEnvironment hostingEnvironment, IErrorHandler errorHandler)
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
            _UrlFileCommon = _configuration.GetSection("UrlFileCommon").Value;
        }
        public async Task<IActionResult> Index(int? value)
        {
            try
            {
                ViewBag._UrlFileCommon = _UrlFileCommon + "/";

                try
                {
                    // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                    var checklogin = await CheckLoginSameTime();
                    if (!checklogin)
                    {
                        _errorHandler.WriteStringToFile("CheckLoginSameTime:", "DanhMucBieuMau");
                        HttpContext.Session.Clear();

                        return RedirectToAction("index", "login");
                    }

                }
                catch (Exception ex)
                {
                    _appLogger.ErrorLogger.LogError(ex);
                }

                List<CommonType> listTemplateType = new List<CommonType>();
                var commonType = new CommonTypeViewModel
                {
                    ListCategory = "7",
                };
                var pram = "?" + commonType.ToQueryString();
                var cResponseMessage = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.CommonCategory + pram);
                var responseObj = JsonConvert.DeserializeAnonymousType(cResponseMessage.Data.ToString(), new List<CoreLib.Entities.CommonType>());
                if (value == null)

                    foreach (var obj in responseObj.ToList())
                    {
                        if (obj.Category == ConstCommonTypes.NHOM_LOAI_BIEUMAU)
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
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = _localizer["InternalServerError"].Value });
            }
            
        }

        [HttpGet]
        [Route(LinkRoute.GetBieuMau)]
        public async Task<IActionResult> ListBieuMau(TemplateViewModel templateViewModel)
        {            
            try
            {
                try
                {
                    // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                    var checklogin = await CheckLoginSameTime();
                    if (!checklogin)
                    {
                        _errorHandler.WriteStringToFile("CheckLoginSameTime:", "ListBieuMau");
                        HttpContext.Session.Clear();

                        return RedirectToAction("index", "login");
                    }

                }
                catch (Exception ex)
                {
                    _appLogger.ErrorLogger.LogError(ex);
                }

                var cpnyID = HttpContext.Session.GetInt32(SessionTypes.CPNY_ID);
                var companyType = HttpContext.Session.GetInt32(SessionTypes.COMPANY_TYPE);
                if(companyType != null)
                {
                    templateViewModel.CompanyID = cpnyID;
                    templateViewModel.CompanyType = companyType;

                    var pram = "?" + templateViewModel.ToQueryString();

                    var cResponseMessage = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.Get_DanhMucBieuMau + pram);

                    var responseObj = JsonConvert.DeserializeObject<List<List<TemplateModelView>>>(cResponseMessage.Data.ToString());
                    cResponseMessage.Data = responseObj;
                    var languagenews = Request.Cookies[CookieTypes.LANGUAGE].Substring(3);
                    if (languagenews == "US")
                    {
                        foreach (var Templatelist in responseObj.ToList())
                            foreach (var Template in Templatelist.ToList())
                            {
                                Template.TypeName = Template.TypeNameEN;
                            }

                    }
                    return Json(responseObj);
                }
                else
                {
                    return Json(new CResponseMessage { Code = -3, Message = _localizer["InternalServerError"].Value });
                }
                
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = _localizer["InternalServerError"].Value });
            }
            
        }

        [HttpPost]
        [Route(LinkRoute.DownloadBieuMau)]
        public async Task<IActionResult> DownloadFile(TemplateViewModel templateViewModel)
        {
            try
            {
                try
                {
                    // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                    var checklogin = await CheckLoginSameTime();
                    if (!checklogin)
                    {
                        _errorHandler.WriteStringToFile("CheckLoginSameTime:", "DanhMucBieuMau_DownloadFile");
                        HttpContext.Session.Clear();

                        return RedirectToAction("index", "login");
                    }

                }
                catch (Exception ex)
                {
                    _appLogger.ErrorLogger.LogError(ex);
                }

                templateViewModel.UserLogin = HttpContext.Session.GetString(SessionTypes.USERNAME);
                templateViewModel.CompanyID = HttpContext.Session.GetInt32(SessionTypes.CPNY_ID);
                templateViewModel.RoleCode = RoleCode.DMBM;


                //string filePath = Path.Combine(this._hostingEnvironment.ContentRootPath, templateViewModel.Path.Replace(@"/", @"\"));

                if (!string.IsNullOrEmpty(templateViewModel.UserLogin) && !string.IsNullOrEmpty(templateViewModel.RoleCode))
                {
                    /*if (!System.IO.File.Exists(filePath))
                    {
                        return Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["FileIsNotExist"].Value });
                    }
                    var ms = new MemoryStream(CheckUtils.ReadFile(filePath));
                    string FileExtension = Path.GetFileName(filePath).Substring(Path.GetFileName(filePath).LastIndexOf('.') + 1).ToLower();*/

                    string remoteUri = _configuration["UrlFileCommon"];
                    string Url = templateViewModel.Path;
                    string myStringWebResource = null;

                    myStringWebResource = remoteUri + Url;

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
            catch(Exception ex)
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
