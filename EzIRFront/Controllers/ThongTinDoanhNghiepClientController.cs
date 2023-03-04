using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Entities.ViewModels;
using CoreLib.Interfaces;
using CoreLib.Services;
using CoreLib.SharedKernel;

using EzIRFront.Models;
using EzIRFront.Models.ModelViews;
using EzIRFront.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EzIRFront.Controllers
{
    /// <summary>
    /// Thông tin site nhúng khách hàng ezirfront.fpts.com.vn/thongtindoanhnghiepclient/mack
    /// vd: ezirfront.fpts.com.vn/thongtindoanhnghiepclient/fts
    /// </summary>
    public class ThongTinDoanhNghiepClientController : Controller
    {
        private readonly IConfiguration _config;
        //private readonly ILogger<HomeController> _logger;
        private IMemoryCache _cache;
        private IAppLogger _appLogger;
        private readonly IErrorHandler _errorHandler;
        private static readonly HttpClient client = new HttpClient();
        string baseUri = "";
        private readonly ISerilogProvider _serilogProvider;
        //private readonly IValidateApi _validateApi;
        private static WebApiConfigs _WebApiConfig = new WebApiConfigs();
        string tintucUri = "";
        string tinTucLink = "";
        string tinTucDNLink = "";
        string Imgbm = "";
        string ImgbmLogo = "";
        string UrlFile = "";
        string CkeditorImgUrl = "";
        string protocol = "";

        private readonly IHtmlLocalizer<SharedResource> _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ThongTinDoanhNghiepClientController(IErrorHandler errorHandler, IHttpContextAccessor httpContextAccessor, IMemoryCache cache, IConfiguration config, IAppLogger appLogger, ISerilogProvider serilogProvider, IHtmlLocalizer<SharedResource> localizer)

        {
            _errorHandler = errorHandler;
            _cache = cache;
            _config = config;
            _appLogger = appLogger;
            this._serilogProvider = serilogProvider;
            config.GetSection("API").Bind(_WebApiConfig);
            baseUri = _WebApiConfig.BASE_URL;
            tintucUri = _WebApiConfig.API_TinTuc;
            _localizer = localizer;
            tinTucDNLink = "&pageSize=8&selectedPage=1&cbtt=0&from=01-01-1970&to=01-01-3000&newsType=1";
            tinTucLink = "&pageSize=8&selectedPage=1&cbtt=1&from=01-01-1970&to=01-01-3000&newsType=1";
            Imgbm = _config.GetSection("UrlImg").Value;
            ImgbmLogo = _config.GetSection("UrlImgLogo").Value;
            UrlFile = _config.GetSection("UrlFile").Value;
            CkeditorImgUrl = _config.GetSection("CkeditorImgUrl").Value;
            protocol = _config.GetSection("ProtocolStatus").Value;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<IActionResult> Index(string astocK_CODE)           
        {
            string body = string.Empty;
            try
            {

           
            ViewBag.Protocol = protocol;
            astocK_CODE = astocK_CODE.ToUpper();

            var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_StockCode + "?StockCode=" + astocK_CODE}";
            var response = await HttpRequestFactory.Get(requestUri);
            this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống
            var Data_Company = response.Content.ReadAsStringAsync().Result;
            var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;
            var jArr = jObj.GetValue("table");
            var table = jArr.ToObject<List<CompanyService>>();

            var cpnyId = table.Find(x => x.acpnyid == x.acpnyid).acpnyid;

            // lấy các menu của doanh nghiệp được phân quyền 

            var requestUri1 = $"{baseUri + WebApiConfigs.LINK_WEBAPI_ENTERPRISE_LIST + "?CompanyID=" + cpnyId}";

            var response1 = await HttpRequestFactory.Get(requestUri1);

            this._serilogProvider.Logger.Debug($"Status:" + response1.StatusCode.ToString());  // Ghi log hệ thống
         
            var data_enterprise = response1.Content.ReadAsStringAsync().Result;

            var jObj1 = JsonConvert.DeserializeObject(data_enterprise) as JObject;

            var jArr1 = jObj1.GetValue("table");

                //Láy file html hiển thị công ty chưa gán link ezirfront.fpts.com.vn

                //using streamreader for reading my htmltemplate   
                using (StreamReader reader = new StreamReader(Path.Combine("StaticFiles", "role_ezirfront.html")))
                {
                    body = reader.ReadToEnd();
                }

                var table1 = jArr1.ToObject<List<QuanLyDoanhNghiep>>();
 
                try
                {
                
                    var linklienket = table1.FirstOrDefault().aisdelete;

                    if (linklienket.ToString() == "1")
                    {
                        var listrole = table1.FirstOrDefault().listrolecode;
                        ViewBag.ListRole = listrole;
                    }

                    else
                    {
                        return Content(body, "text/html");
                    }
                }
                catch(Exception)
                {
                    return Content(body, "text/html");
                }

                return View();
            }

            catch (Exception ex)
            {
                _errorHandler.WriteStringToFile("error Index client", ex.ToString());
                ex.ToString();
                return Content(body, "text/html");
            }
        }


        /// <summary>
        /// Thông tin chung
        /// </summary>
        /// <returns></returns>
       
        public IActionResult ThongTinChung()
        {
            var cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            TempData["tintucLanguage"] = cookieValue;
            ViewBag.Imgbm = Imgbm + "/";
            ViewBag.ImgbmLogo = ImgbmLogo + "/";
            ViewBag.Protocol = protocol;
            return ViewComponent("Thongtinchung");
        }


        /// <summary>
        /// Láy thông tin công ty
        /// </summary>
        /// <param name="stock_Code"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ThongTin(string stock_Code)
        {
            
                var link = Request.Path;
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_StockCode + "?StockCode=" + stock_Code}";
                var response = await HttpRequestFactory.Get(requestUri);
                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống
                var Data_Company = response.Content.ReadAsStringAsync().Result;
                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;
                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<CompanyService>>();
                return Json(table);
  
        }

        /// <summary>
        /// tầm nhìn chiến lược
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> TamNhinChienLuoc()
        {
            var cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Imgbm = Imgbm;
            ViewBag.Language = cookieValue;
            return this.ViewComponent("TamNhinChienLuoc");
        }
        /// <summary>
        /// thành phần lãnh đạo
        /// </summary>
        /// <returns></returns>
        public IActionResult ThanhPhanLanhDao()
        {
            ViewBag.Imgbm = Imgbm + "/";
            return this.ViewComponent("ThanhPhanLanhDao");
        }
        /// <summary>
        /// tổ chức bộ máy quản lý
        /// </summary>
        /// <returns></returns>
        public IActionResult ToChucBoMayQuanLy()
        {
            ViewBag.Imgbm = Imgbm + "/";
            return this.ViewComponent("ToChucBoMayQuanLy");
        }
        /// <summary>
        /// Cơ cấu sở hữu
        /// </summary>
        /// <returns></returns>
        public IActionResult CoCauSoHuu()
        {
            return this.ViewComponent("CoCauSoHuu");
        }

        /// <summary>
        /// Lưu trữ
        /// </summary>
        /// <returns></returns>

        public IActionResult LuuTruTinDoanhNghiep()
        {
            ViewBag.BCTN = FolderLuuTru.BCTN;
            ViewBag.BCQT = FolderLuuTru.BCQT;
            ViewBag.BCTC = FolderLuuTru.BCTC;
            ViewBag.CBTT = FolderLuuTru.CBTT;
            ViewBag.DLQT = FolderLuuTru.DLQT;
            ViewBag.HDQT = FolderLuuTru.HDQT;
            ViewBag.NQDHCD = FolderLuuTru.NQDHCD;
            ViewBag.TLDHCD = FolderLuuTru.TLDHCD;
            ViewBag.BCTNEN = FolderLuuTru.BCTNEN;
            ViewBag.BCQTEN = FolderLuuTru.BCQTEN;
            ViewBag.BCTCEN = FolderLuuTru.BCTCEN;
            ViewBag.CBTTEN = FolderLuuTru.CBTTEN;
            ViewBag.DLQTEN = FolderLuuTru.DLQTEN;
            ViewBag.HDQTEN = FolderLuuTru.HDQTEN;
            ViewBag.NQDHCDEN = FolderLuuTru.NQDHCDEN;
            ViewBag.TLDHCDEN = FolderLuuTru.TLDHCDEN;
            ViewBag.IR = FolderLuuTru.IR;
            ViewBag.TinKhac = FolderLuuTru.TinKhac;
            ViewBag.Protocol = protocol;
            return ViewComponent("LuuTruTinDoanhNghiep");
        }

        public IActionResult LuuTruTinTuSoVaVSD()
        {
            ViewBag.BCTN = FolderLuuTru.BCTN;
            ViewBag.BCQT = FolderLuuTru.BCQT;
            ViewBag.BCTC = FolderLuuTru.BCTC;
            ViewBag.CBTT = FolderLuuTru.CBTT;
            ViewBag.DLQT = FolderLuuTru.DLQT;
            ViewBag.HDQT = FolderLuuTru.HDQT;
            ViewBag.NQDHCD = FolderLuuTru.NQDHCD;
            ViewBag.TLDHCD = FolderLuuTru.TLDHCD;
            ViewBag.BCTNEN = FolderLuuTru.BCTNEN;
            ViewBag.BCQTEN = FolderLuuTru.BCQTEN;
            ViewBag.BCTCEN = FolderLuuTru.BCTCEN;
            ViewBag.CBTTEN = FolderLuuTru.CBTTEN;
            ViewBag.DLQTEN = FolderLuuTru.DLQTEN;
            ViewBag.HDQTEN = FolderLuuTru.HDQTEN;
            ViewBag.NQDHCDEN = FolderLuuTru.NQDHCDEN;
            ViewBag.TLDHCDEN = FolderLuuTru.TLDHCDEN;
            ViewBag.IR = FolderLuuTru.IR;
            ViewBag.TinKhac = FolderLuuTru.TinKhac;
            ViewBag.Protocol = protocol;
            return ViewComponent("LuuTruTinTuSoVaVSD");
        }

        /// <summary>
        /// Kinh doanh - trình độ công nghệ
        /// </summary>
        /// <returns></returns>
        public IActionResult TrinhDoCongNghe()
        {
            ViewBag.CkeditorImgUrl = CkeditorImgUrl;
            var cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            return this.ViewComponent("TrinhDoCongNghe");
        }

        /// <summary>
        /// Kinh doanh - thị trường khách hàng đối thủ
        /// </summary>
        /// <returns></returns>
        public IActionResult ThiTruongKHDT()
        {
            ViewBag.CkeditorImgUrl = CkeditorImgUrl;
            var cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            return this.ViewComponent("ThiTruongKHDT");
        }

        /// <summary>
        /// Kinh doanh - thông tin dự án đầu tư
        /// </summary>
        /// <returns></returns>
        public IActionResult ThongTinDuAnDauTu()
        {
            ViewBag.CkeditorImgUrl = CkeditorImgUrl;
            var cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            ViewBag.Imgbm = Imgbm + "/";
            return this.ViewComponent("ThongTinDuAnDauTu");
        }
        /// <summary>
        /// Kinh doanh - năng lực quản lý
        /// </summary>
        /// <returns></returns>
        public IActionResult NangLucQuanLy()
        {
            ViewBag.CkeditorImgUrl = CkeditorImgUrl;
            var cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            return this.ViewComponent("NangLucQuanLy");
        }
        /// <summary>
        /// Phân tích Swot
        /// </summary>
        /// <returns></returns>
        public IActionResult PhanTichSWOT()
        {
            ViewBag.CkeditorImgUrl = CkeditorImgUrl;
            var cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            return this.ViewComponent("PhanTichSWOT");
        }
        /// <summary>
        /// Vị thế doanh nghiệp
        /// </summary>
        /// <returns></returns>
        public IActionResult ViTheDoanhNghiep()
        {
            ViewBag.CkeditorImgUrl = CkeditorImgUrl;
            var cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            return this.ViewComponent("ViTheDoanhNghiep");
        }
        /// <summary>
        /// Hệ số tài chính
        /// </summary>
        /// <returns></returns>
        public IActionResult HeSoTaiChinh()
        {

            var cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            return this.ViewComponent("HeSoTaiChinh");
        }

        /// <summary>
        /// Cân đối kế toán
        /// </summary>
        /// <returns></returns>
        public IActionResult CanDoiKeToan()
        {

            var cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            ViewBag.Imgbm = Imgbm + "/";
            return this.ViewComponent("CanDoiKeToan");
        }
        /// <summary>
        /// Kế hoạch tài chính
        /// </summary>
        /// <returns></returns>
        public IActionResult KeHoachTaiChinh()
        {

            var cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            ViewBag.Imgbm = Imgbm + "/";
            return this.ViewComponent("KeHoachTaiChinh");
        }

        /// <summary>
        /// kết quả hoạt động kinh doanh
        /// </summary>
        /// <returns></returns>
        public IActionResult KetQuaHoatDongKinhDoanh()
        {

            var cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            ViewBag.Imgbm = Imgbm + "/";
            return this.ViewComponent("KetQuaHoatDongKinhDoanh");
        }

        /// <summary>
        /// Lưu chuyển tiền tệ ICF
        /// </summary>
        /// <returns></returns>
        public IActionResult LuuChuyenTienTeICF()
        {

            var cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            ViewBag.Imgbm = Imgbm + "/";
            return this.ViewComponent("LuuChuyenTienTeICF");
        }

        /// <summary>
        /// Lưu chuyển tiền tệ DCF
        /// </summary>
        /// <returns></returns>
        public IActionResult LuuChuyenTienTeDCF()
        {

            var cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            ViewBag.Imgbm = Imgbm + "/";
            return this.ViewComponent("LuuChuyenTienTeDCF");
        }


        /// <summary>
        /// Lịch sử tăng vố trả cổ tức
        /// </summary>
        /// <returns></returns>
        public IActionResult LichSuTangVonTraCoTuc()
        {
            return this.ViewComponent("LichSuTangVonTraCoTuc");
        }

        /// <summary>
        /// Lịch sử giao dịch
        /// </summary>
        /// <returns></returns>
        public IActionResult LichSuGiaoDich()
        {
            return this.ViewComponent("LichSuGiaoDich");
        }
        /// <summary>
        /// Đồ thị kxy thuật
        /// </summary>
        /// <returns></returns>
        public IActionResult DoThiKyThuat()
        {
            return this.ViewComponent("DoThiKyThuat");
        }


        /// <summary>
        /// Phân tích FPTS
        /// </summary>
        /// <returns></returns>
        public IActionResult PhanTichFPTS()
        {
            var cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            ViewBag.UrlFile = UrlFile + '/';
            return this.ViewComponent("PhanTichFPTS");
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

            return LocalRedirect(returnUrl);
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
