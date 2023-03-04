using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Commons;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Interfaces;
using CoreLib.SharedKernel;
using EzIRCustomer.Authentication;
using EzIRCustomer.Commons;
using EzIRCustomer.Models.ViewModels;
using EzIRCustomerAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EzIRCustomer.Controllers
{
    [RequiredLogin]
    public class ThiTruongKhachHangDoiThuController : Controller
    {
        private readonly IHtmlLocalizer<SharedResource> _localizer;
        private readonly IConfiguration _configuration;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IHttpService _httpService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommon _common;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IErrorHandler _errorHandler;
        private string _BASE_API_URL;
        private int? _CPNYID;
        private string _USERNAME;
        private string _EMAIL_SPECIALIST;
        private EmailSettings _emailSettings;
        private string _LANGUAGE;

        public ThiTruongKhachHangDoiThuController(IConfiguration configuration, IAppLogger appLogger, IMemoryCache cache, IHostingEnvironment hostingEnvironment, ICommon common, IHttpService httpService, IHtmlLocalizer<SharedResource> localizer, IHttpContextAccessor httpContextAccessor, IErrorHandler errorHandler)
        {
            _configuration = configuration;
            _appLogger = appLogger;
            _cache = cache;
            _httpService = httpService;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
            _common = common;
            _hostingEnvironment = hostingEnvironment;
            _errorHandler = errorHandler;
            _BASE_API_URL = _configuration.GetSection("ApiUrl").Value;
            _CPNYID = _httpContextAccessor.HttpContext.Session.GetInt32(SessionTypes.CPNY_ID);
            _USERNAME = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.USERNAME);
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
                    _errorHandler.WriteStringToFile("CheckLoginSameTime:", "ThiTruongKhachHangDoiThu");
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
        [Route(LinkRoute.GET_TTKHDT)]
        public async Task<IActionResult> GetTtkhdt(InfoSheetViewModel infoSheetViewModel)
        {
            try
            {
                _LANGUAGE = Request.Cookies[CookieTypes.LANGUAGE]?.ToUpper()?.Split('-')[1] ?? "";

                infoSheetViewModel.CpnyID = _CPNYID;
                infoSheetViewModel.Language = _LANGUAGE;

                var pram = "?" + infoSheetViewModel.ToQueryString();

                var cResponseMessage = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.GET_INFOSHEET + pram);

                var responseObj = JsonConvert.DeserializeAnonymousType(cResponseMessage.Data.ToString(), new List<InfoSheet>());

                responseObj = responseObj.Where(a => a.MenuID == ConstMenuID.NGUYEN_LIEU_DAU_VAO_NCC
                                                    || a.MenuID == ConstMenuID.CO_CAU_DOANH_THU
                                                    || a.MenuID == ConstMenuID.SAN_PHAM_THAY_THE
                                                    || a.MenuID == ConstMenuID.DOI_THU_CANH_TRANH
                                                    || a.MenuID == ConstMenuID.THI_TRUONG_KHACH_HANG
                                                    || a.MenuID == ConstMenuID.THI_TRUONG_KHAC_HANG_DOI_THU_KHAC
                                                    && a.Approve != 3).ToList();

                cResponseMessage.Data = responseObj;

                return Json(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
            
        }


        [HttpPost]
        [Route(LinkRoute.SAVE_TTKHDT)]
        public async Task<IActionResult> SaveTtkhdt(List<InfoSheetViewModel> infoSheetViewModels)
        {
            try
            {
                // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                var checklogin = await CheckLoginSameTime();
                if (!checklogin)
                {
                    _errorHandler.WriteStringToFile("CheckLoginSameTime:", "SaveTtkhdt");
                    HttpContext.Session.Clear();

                    return RedirectToAction("index", "login");
                }

            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
            }

            CResponseMessage cResponseMessage = new CResponseMessage { Code = -1, Message = "UnknowError" };
            _LANGUAGE = Request.Cookies[CookieTypes.LANGUAGE]?.ToUpper()?.Split('-')[1] ?? "";

            if (infoSheetViewModels == null || infoSheetViewModels.Count < 0)
            {
                cResponseMessage.Message = "InvalidInputData";
                return Json(cResponseMessage);
            }


            var pramCpny = $"?cpnyID={_CPNYID}";

            var responseCompanies = await _httpService.ResponseMessageFromGetUrl(_BASE_API_URL + ApiRoute.GET_COMPANY + pramCpny);

            var companies = JsonConvert.DeserializeAnonymousType(responseCompanies.Data.ToString(), new List<CompanyEzSearchTemp>());
            var company = companies.FirstOrDefault();

            //_emailSettings = new EmailSettings { Mail = _EMAIL_SPECIALIST, 
            //    Message = "+ Tên doanh nghiệp:"+ company.AName_VN+
            //              "\n+ Mã Chứng khoán:"+ company.AStockCode+
            //              "\n+ Thời gian thực hiện:"+ DateTime.Now+
            //              "\n+ Xem thông tin phê duyệt: Kinh doanh->Tầm nhìn Chiến lược"
            //    ,Subject = "Notification - Cập nhật thông tin trên giao diện EzIR - " + company.AStockCode };

            string namevn = "- Tên doanh nghiệp: " + company.AName_VN;
            string nameen = "- Mã Chứng khoán: " + company.AStockCode;
            string date = "- Thời gian thực hiện: " + DateTime.Now;
            string mess = "- Xem thông tin phê duyệt: Kinh doanh->Thị trường/Khách hàng/Đối thủ ";
            _emailSettings = new EmailSettings 
                {       Mail = _EMAIL_SPECIALIST, 
                        Message = string.Format("{0}<br />{1}<br />{2}<br />{3}", namevn, nameen, date, mess),
                        Subject = "Notification Cập nhật thông tin trong ngày của KH EzIR - Nguồn: " + company.AStockCode + ".EzIR"
            };

            foreach (var infoSheet in infoSheetViewModels)
            {

                var changeInfos = new List<ChangeInfo>();

                var title = getTitleByMenuID(infoSheet.MenuID);

                changeInfos.Add(new ChangeInfo
                {
                    CpnyID = infoSheet.CpnyID,
                    MenuID = infoSheet.MenuID,
                    Key = "ACONTENT",
                    Value = infoSheet.Content,
                    Status = (infoSheet.ID == 0 || infoSheet.ID == null) ? 1 : 2,
                    KeyFunction = $"Thị trường khách hàng đối thủ - {title}",
                    Function = "Thị trường/Khách hàng/Đối thủ",
                    LevelID = "KT",
                    DetailLevelID = "TTKHDT"
                });

                infoSheet.EmailSettings = _emailSettings;
                infoSheet.ChangeInfos = changeInfos;
                infoSheet.Username = _USERNAME;
                infoSheet.RoleCode = RoleCode.THI_TRUONG_KHACH_HANG_DOI_THU;
                infoSheet.Title = title;
                infoSheet.Language = _LANGUAGE;

            }
            _httpService.SetRoleControllerToHeader(RoleCode.THI_TRUONG_KHACH_HANG_DOI_THU);
            _httpService.SetUsernameControllerToHeader(_USERNAME);

            var cResponse = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.INSERT_INFOSHEET, infoSheetViewModels);

            cResponse.Message = _localizer[cResponse.Message].Value;

            return Json(cResponse);
        }

        [HttpPut]
        [Route(LinkRoute.EDIT_TTKHDT)]
        public async Task<IActionResult> EditTtkhdt(List<InfoSheetViewModel> infoSheetViewModels)
        {
            try
            {
                // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                var checklogin = await CheckLoginSameTime();
                if (!checklogin)
                {
                    _errorHandler.WriteStringToFile("CheckLoginSameTime:", "EditTtkhdt");
                    HttpContext.Session.Clear();

                    return RedirectToAction("index", "login");
                }

            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
            }

            CResponseMessage cResponseMessage = new CResponseMessage { Code = -1, Message = "UnknowError" };

            if (infoSheetViewModels == null || infoSheetViewModels.Count < 0)
            {
                cResponseMessage.Message = "InvalidInputData";
                return Json(cResponseMessage);
            }

            _httpService.SetRoleControllerToHeader(RoleCode.THI_TRUONG_KHACH_HANG_DOI_THU);
            _httpService.SetUsernameControllerToHeader(_USERNAME);

            var cResponse = await _httpService.ResponseMessageFromPostUrl(_BASE_API_URL + ApiRoute.GET_INFOSHEET, infoSheetViewModels);

            return Json(cResponse);
        }

        [HttpPost]
        [Route(LinkRoute.UPLOAD_IMAGE_TTKHDT)]
        public async Task<ActionResult> UploadImage(IFormFile upload)
        {
            try
            {
                // Kiểm tra xem có 2 user đăng nhập trên 2 thiết bị thì out user đầu tiên ra.
                var checklogin = await CheckLoginSameTime();
                if (!checklogin)
                {
                    _errorHandler.WriteStringToFile("CheckLoginSameTime:", "ThiTruongKhachHangDoiThu_UploadImage");
                    HttpContext.Session.Clear();

                    return RedirectToAction("index", "login");
                }

            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
            }

            var checkResult = upload.CheckValidFileImage();

            if (!string.IsNullOrEmpty(checkResult))
            {
                var errorMessage = _localizer[checkResult].ToString();

                dynamic errorResult = JsonConvert.DeserializeObject("{ 'uploaded': 0, 'error': { 'message': \"" + errorMessage + "\"}}");

                return Json(errorResult);
            }

            //lưu lại file ảnh trên thư mục
            var url = string.Empty;
            var fileName = string.Empty;
            var successMessage = string.Empty;

            var result = await _common.SaveFile(upload, _configuration["UploadCKImagePath"], _hostingEnvironment);

            if (result.Code != 0)
            {
                url = "";
                fileName = "";
            }
            else
            {
                url = result.Data.ToString().Replace(@"\", @"/").ToString();
                fileName = url.Split('/')[2];
            }

            return Json(new { uploaded = true, fileName, url, error = new { message = successMessage } });
        }
        private string getTitleByMenuID(int? menuid)
        {
            switch (menuid)
            {
                case ConstMenuID.NGUYEN_LIEU_DAU_VAO_NCC:
                    {
                        return "Thị trường khách hàng đối thủ - Nguyên liệu đầu vào, Nhà cung cấp";
                    }
                case ConstMenuID.CO_CAU_DOANH_THU:
                    {
                        return "Thị trường khách hàng đối thủ - Cơ cấu doanh thu";
                    }
                case ConstMenuID.SAN_PHAM_THAY_THE:
                    {
                        return "Thị trường khách hàng đối thủ - Sản phẩm thay thế";
                    }
                case ConstMenuID.DOI_THU_CANH_TRANH:
                    {
                        return "Thị trường khách hàng đối thủ - Đối thủ cạnh tranh";
                    }
                case ConstMenuID.THI_TRUONG_KHACH_HANG:
                    {
                        return "Thị trường khách hàng đối thủ - Thị trường khách hàng";
                    }
                case ConstMenuID.THI_TRUONG_KHAC_HANG_DOI_THU_KHAC:
                    {
                        return "Thị trường khách hàng đối thủ - Thị trường khách hàng đối thủ khác";
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
