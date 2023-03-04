using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Commons;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Entities.ViewModels;
using CoreLib.Services;
using CoreLib.SharedKernel;
using EzIRFront.DataAccess;

//using EzIRFront.Commons;
using EzIRFront.Models;
using EzIRFront.Models.ModelViews;
using EzIRFront.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace EzIRFront.Controllers
{
    public class ThongTinDoanhNghiepController : Controller
    {
        private readonly IConfiguration _config;
        //private readonly ILogger<HomeController> _logger;
        private IMemoryCache _cache;
        private IAppLogger _appLogger;
        private static readonly HttpClient client = new HttpClient();
        private HttpClient client_KLGD;
        string baseUri = "";
        private readonly ISerilogProvider _serilogProvider;
        //private readonly IValidateApi _validateApi;
        private static WebApiConfigs _WebApiConfig = new WebApiConfigs();
        string tintucUri = "";
        string tinTucLink = "";
        string tinTucDNLink = "";
        string Imgbm = "";
        string fileLink = "";
        string ImgbmLogo = "";
        string CkeditorImgUrl = "";
        string Gia = "";
        string priceUri = "";
        string UrlFile = "";
        private readonly IHtmlLocalizer<SharedResource> _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommon _common;
        private readonly IExportExcel _excel;
        private string _api_KLGDTT;
        private string _api_BETA;
        string language = "vn";
        string cookieValue = "vn";
        string protocol = "";

        CultureInfo provider = CultureInfo.InvariantCulture;

        public ThongTinDoanhNghiepController(IHttpContextAccessor httpContextAccessor, IMemoryCache cache, IConfiguration config, IAppLogger appLogger, ISerilogProvider serilogProvider, IHtmlLocalizer<SharedResource> localizer, ICommon common, IExportExcel excel)

        {
            _cache = cache;
            _config = config;
            _appLogger = appLogger;
            this._serilogProvider = serilogProvider;
            config.GetSection("API").Bind(_WebApiConfig);
            baseUri = _WebApiConfig.BASE_URL;
            tintucUri = _WebApiConfig.API_TinTuc;
            priceUri = _WebApiConfig.API_PRICE;
            _api_KLGDTT = _WebApiConfig.API_KLGDTT;
            _api_BETA = _WebApiConfig.API_BETA;
            UrlFile = _config.GetSection("UrlFile").Value;
            _localizer = localizer;
            tinTucDNLink = "&pageSize=8&selectedPage=1&cbtt=1&from=01-01-1970&to=01-01-3000&newsType=1";
            tinTucLink = "&pageSize=8&selectedPage=1&cbtt=0&from=01-01-1970&to=01-01-3000&newsType=1";
            Imgbm = _config.GetSection("UrlImg").Value;
            ImgbmLogo = _config.GetSection("UrlImgLogo").Value;
            fileLink = _config.GetSection("UrlTPLD").Value;
            protocol = _config.GetSection("ProtocolStatus").Value;
            CkeditorImgUrl = _config.GetSection("CkeditorImgUrl").Value;
            Gia = "&fromDate=2020-1-1&toDate=2021-1-1";
            _httpContextAccessor = httpContextAccessor;
            _common = common;
            _excel = excel;
        }

        public IActionResult Index()
        {

            language = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            TempData["tintucLanguage"] = language;
            ViewBag.Protocol = protocol;
            ViewBag.CookiesVaule = cookieValue;
            return View();
        }
        ////api/companyinfo/listcompany
        /// <summary>
        /// Lấy thông tin mã chứng khoán, công ty
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<IActionResult> GetStockCodeThongTinDN()
        {

            try
            {
                //   string language = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
                language = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
                if (language != null)
                {
                    language = language.Replace("en-US", "EN");
                }

                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_StockCode + "?Language=" + language}";

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


        /// <summary>
        /// Load lại thông tin trag EzIR thông tin doanh nghiệp theo mã chứng khoán được search
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SelectStockCode(ThongTinViewModel thongTinViewModel)
        {
            cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            if (cookieValue.Contains("?culture=en-US"))
                cookieValue = "?culture=en-US";
            else
                cookieValue = "";
            return Redirect("/ThongTinDoanhNghiep/" + thongTinViewModel.astocK_CODE + cookieValue);


        }

        public IActionResult ThongTinChung()
        {
            cookieValue = ViewBag.CookiesVaule;
            ViewBag.Protocol = protocol;
            //   cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            ViewBag.Imgbm = Imgbm + "/";
            ViewBag.ImgbmLogo = ImgbmLogo + "/";
            ViewBag.CkeditorImgUrl = CkeditorImgUrl;
            return this.ViewComponent("Thongtinchung");
        }


        public IActionResult Chart()
        {
            //   cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            //ViewBag.Language = cookieValue;
            return View();
        }

        /// <summary>
        /// Lấy khối lượng giao dịch thỏa thuận
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetKhoiLuongGiaoDichThoaThuan()
        {
            try
            {

                //var fromDate = DateTime.Now.ToString("yyyy-MM-dd");
                //var toDate = DateTime.Now.ToString("yyyy-MM-dd");


                //var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listpricehistory + "?fromDate=" + fromDate + "&toDate=" + toDate + "&stockCode=" + stock_code }";

                //var response = await HttpRequestFactory.Get(requestUri);

                //this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                ////cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                //var Data_Banner = response.Content.ReadAsStringAsync().Result;

                //var jObj = JsonConvert.DeserializeObject(Data_Banner) as JObject;

                //var jArr = jObj.GetValue("table");

                //var table = jArr.ToObject<List<ThongTinViewModel>>();

                //return Json(table);

                string klgdtt = _api_KLGDTT;
                HttpResponseMessage res = await client.GetAsync(klgdtt);
                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var jObj = JsonConvert.DeserializeObject(data) as JObject;
                    var dataKLGDTT = jObj.ToString();
                    return Ok(dataKLGDTT);
                }
                return Ok();
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        /// <summary>
        /// Lấy Beta
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CalBetaStock(string stock_code, DateTime fromDate, DateTime toDate)
        {
            string beta = _api_BETA + "?fDate=" + fromDate.ToString("MM-dd-yyyy") + "&tDate=" + toDate.ToString("MM-dd-yyyy") + "&stockCode=" + stock_code;
            HttpResponseMessage res = await client.GetAsync(beta); 
            if (res.IsSuccessStatusCode)
            {
                var data = await res.Content.ReadAsStringAsync();
                var jObj = JsonConvert.DeserializeObject(data) as JObject;
                var dataBeta = jObj.ToString();
                return Ok(dataBeta);
            }
            return Ok();
        }

        /// <summary>
        /// tin tức
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<IActionResult> GetTTC(string stock_code)
        {

            try
            {
                //cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
                cookieValue = (string)TempData["tintucLanguage"];

                if (cookieValue == "vn" || cookieValue == null)
                {
                    var requestUri = $"{tintucUri + WebApiConfigs.Link_WebApi_TinTucVN + "&code=" + stock_code + tinTucLink}";
                    clsData tinTuc = new clsData();
                    var data = await CallNewsAPI.API_GetNew(requestUri);
                    tinTuc.Table1 = data.Data.Table1;

                    return Json(tinTuc.Table1);
                }
                else
                {
                    var requestUri = $"{tintucUri + WebApiConfigs.Link_WebApi_TinTucEN + "&code=" + stock_code + tinTucLink}";
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
        // tin doanh nghiệp công bố
        [HttpGet]
        public async Task<IActionResult> GetTinDoanhNghiepCongBo(string stock_code)
        {
            try
            {
                //cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
                // var stock_code = Request.Cookies[CookieTypes.STOCK_CODE];
                cookieValue = (string)TempData["tintucLanguage"];

                if (cookieValue == "vn" || cookieValue == null)
                {
                    var requestUri = $"{tintucUri + WebApiConfigs.Link_WebApi_TinTucVN + "&code=" + stock_code + tinTucDNLink}";
                    clsData tinTuc = new clsData();
                    var data = await CallNewsAPI.API_GetNew(requestUri);
                    tinTuc.Table1 = data.Data.Table1;

                    return Json(tinTuc.Table1);
                }
                else
                {
                    var requestUri = $"{tintucUri + WebApiConfigs.Link_WebApi_TinTucEN + "&code=" + stock_code + tinTucDNLink}";
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
        /// <summary>
        /// Lấy thông tin mốc lịch sử
        /// </summary>
        /// <param name="stock_code"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCacMocLichSu(string stock_code)
        {
            try
            {
                //stockCode = Request.Cookies[CookieTypes.STOCK_CODE];

                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_CMLS + "?stockCode=" + stock_code}";

                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Banner = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Banner) as JObject;

                var jArr = jObj.GetValue("table");

                var table = jArr.ToObject<List<ThongTinViewModel>>();

                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        /// <summary>
        /// Láy thông tin công ty
        /// </summary>
        /// <param name="stock_Code"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ThongTin(string stock_Code)
        {

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
        /// Lấy th
        /// </summary>
        /// <param name="stock_Code"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public async Task<IActionResult> ThongTinCompanyViEn(string stock_Code, string culture)
        {
            if(culture == "en-US")
            {
                culture = culture.Replace("en-US", "EN");
            }

            var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_StockCode + "?StockCode=" + stock_Code + "&Language=" + culture}";
            var response = await HttpRequestFactory.Get(requestUri);
            this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống
            var Data_Company = response.Content.ReadAsStringAsync().Result;
            var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;
            var jArr = jObj.GetValue("table");
            var table = jArr.ToObject<List<CompanyService>>();
            return Json(table);
        }

        /// <summary>
        ///  tầm nhìn chiến lược
        /// </summary>
        /// <returns></returns>

        public async Task<IActionResult> TamNhinChienLuoc()
        {
            ViewBag.Imgbm = Imgbm;
            return this.ViewComponent("TamNhinChienLuoc");
        }
        [HttpGet]
        public async Task<IActionResult> GetTamNhin(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listinfosheet + "?MenuID=" + ConstMenuID.TAM_NHIN + "&CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinViewModel>>();
                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        [HttpGet]

        public async Task<IActionResult> GetChienLuoc(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listinfosheet + "?MenuID=" + ConstMenuID.CHIEN_LUOC + "&CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");

                var table = jArr.ToObject<List<ThongTinViewModel>>();

                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        /// <summary>
        /// // tổ chức bộ máy quản lý
        /// </summary>
        /// <returns></returns>

        public IActionResult ToChucBoMayQuanLy()
        {
            ViewBag.Imgbm = Imgbm + "/";


            return this.ViewComponent("ToChucBoMayQuanLy");
        }

        [HttpGet]
        public async Task<IActionResult> GetTCBMQL(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_TCBMQL + "?CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");

                var table = jArr.ToObject<List<ThongTinViewModel>>();

                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }

        }
        /// <summary>
        /// thành phần lãnh đạo
        /// </summary>
        /// <returns></returns>


        public IActionResult ThanhPhanLanhDao()
        {
            ViewBag.Imgbm = fileLink + "/";
            return this.ViewComponent("ThanhPhanLanhDao");
        }

        [HttpGet]
        public async Task<IActionResult> GetTPLD(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_TPLD + "?CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");

                var table = jArr.ToObject<List<ThongTinViewModel>>();

                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        /// <summary>
        /// view component cơ cấu sở hữu
        /// </summary>
        /// <returns></returns>

        public IActionResult CoCauSoHuu()
        {
            return this.ViewComponent("CoCauSoHuu");
        }
        [HttpGet]
        public async Task<IActionResult> GetDataCoCauSoHuu(string cpnyID)
        {

            try
            {

                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_TQ_CCSH + "?cpnyID=" + cpnyID}";

                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống


                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var jArr1 = jObj.GetValue("table1");
                var jArr2 = jObj.GetValue("table2");

                List<CoCauSoHuuModelViews> coCauSoHuus = new List<CoCauSoHuuModelViews>();

                var tab = jArr.ToObject<List<CoCauSoHuu>>();
                var tab1 = jArr1.ToObject<List<CoCauSoHuu>>();
                var tab2 = jArr2.ToObject<List<CoCauSoHuu>>();

                coCauSoHuus.Add(new CoCauSoHuuModelViews { table = tab, table1 = tab1, table2 = tab2 });

                return Json(coCauSoHuus);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }

        }

        /// <summary>
        /// lưu trữ
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

            return this.ViewComponent("LuuTruTinDoanhNghiep");
        }

        [HttpGet]
        public async Task<IActionResult> GetLuuTruTinDoanhNghiep(string stock_code, string cpnyID)
        {

            try
            {
                
                cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
                if (cookieValue == "vn" || cookieValue == null)
                {
                    //tin CBTT
                    //var requestUri = $"{tintucUri + WebApiConfigs.Link_WebApi_TinTucVN + "&code=" + stock_code + "&pageSize=" + 20000 + "&selectedPage=" + 1 + "&newsType=" + 2}";
                    var requestUri = $"{tintucUri + WebApiConfigs.Link_WebApi_TinTucVN + "&code=" + stock_code + "&pageSize=" + 20000 + "&selectedPage=" + 1 + "&cbtt=" + 1 + "&newsType=" + 2}";
                    List<TinTucViewModel> tinTuc = new List<TinTucViewModel>();
                    var data = await CallNewsAPI.API_GetNew(requestUri);
                    tinTuc = data.Data.Table1;
                    foreach (var item in tinTuc)
                    {
                        if (item.FiscYear == null)
                        {
                            //item.FiscYear = Convert.ToInt32(DateTime.Parse(item.DatePub.ToString()).ToString("yyyy"));
                            item.FiscYear = Convert.ToInt32(DateTime.ParseExact(item.DatePub, "dd/MM/yyyy HH:mm", provider).ToString("yyyy"));
                        }
                    }
                    //Tin khác
                    var requestUri2 = $"{baseUri + WebApiConfigs.Link_WebApi_CBTT + "?aCpnyID=" + cpnyID + "&aDocType=" + 9}";

                    var response2 = await HttpRequestFactory.Get(requestUri2);

                    this._serilogProvider.Logger.Debug($"Status:" + response2.StatusCode.ToString());  // Ghi log hệ thống

                    var Data_Company2 = response2.Content.ReadAsStringAsync().Result;

                    var jObj2 = JsonConvert.DeserializeObject(Data_Company2) as JObject;

                    var jArr2 = jObj2.GetValue("table");

                    var table2 = jArr2.ToObject<List<PhanTichFPTSViewModel>>();

                    foreach (var re in table2)
                    {
                        TinTucViewModel tinTucViewModel = new TinTucViewModel();
                        tinTucViewModel.DatePub = DateTime.Parse(re.adatepub.ToString()).ToString("dd/MM/yyyy HH:mm");
                        tinTucViewModel.Title = re.atitle;
                        tinTucViewModel.FolderLuutru = FolderLuuTru.TinKhac;
                        tinTucViewModel.FiscYear = re.ayear;
                        tinTucViewModel.URL = UrlFile + "/" + re.aurl;
                        tinTucViewModel.language = re.alanguage;
                        tinTuc.Add(tinTucViewModel);
                    }

                    //tin IR
                    var requestUri1 = $"{baseUri + WebApiConfigs.Link_WebApi_CBTT + "?aCpnyID=" + cpnyID + "&aDocType=" + FolderLuuTru.IR}";

                    var response = await HttpRequestFactory.Get(requestUri1);

                    this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                    var Data_Company = response.Content.ReadAsStringAsync().Result;

                    var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                    var jArr = jObj.GetValue("table");

                    var table = jArr.ToObject<List<PhanTichFPTSViewModel>>();

                    foreach (var re in table)
                    {
                        TinTucViewModel tinTucViewModel = new TinTucViewModel();
                        tinTucViewModel.DatePub = DateTime.Parse(re.adatepub.ToString()).ToString("dd/MM/yyyy HH:mm");
                        tinTucViewModel.Title = re.atitle;
                        tinTucViewModel.FolderLuutru = re.adoctype; // ban tin ir
                        tinTucViewModel.FiscYear = re.ayear;
                        tinTucViewModel.URL = UrlFile + "/" + re.aurl;
                        tinTucViewModel.language = re.alanguage;
                        tinTuc.Add(tinTucViewModel);
                    }
                    var listLuutru = tinTuc.OrderByDescending(d => Convert.ToDateTime(d.DatePub)).ToList();
                    return Json(listLuutru);
                }
                else
                {
                    //tin CBTT
                    var requestUri = $"{tintucUri + WebApiConfigs.Link_WebApi_TinTucEN + "&code=" + stock_code + "&pageSize=" + 20000 + "&selectedPage=" + 1 + "&cbtt=" + 1 + "&newsType=" + 2}";
                    List<TinTucViewModel> tinTuc = new List<TinTucViewModel>();
                    var data = await CallNewsAPI.API_GetNew(requestUri);
                    tinTuc = data.Data.Table1;
                    foreach (var item in tinTuc)
                    {
                        if (item.FiscYear == null)
                        {
                            //item.FiscYear = Convert.ToInt32(DateTime.Parse(item.DatePub.ToString()).ToString("yyyy"));
                            item.FiscYear = Convert.ToInt32(DateTime.ParseExact(item.DatePub, "dd/MM/yyyy HH:mm", provider).ToString("yyyy"));
                        }

                    }
                    //Tin khác
                    var requestUri2 = $"{baseUri + WebApiConfigs.Link_WebApi_CBTT + "?aCpnyID=" + cpnyID + "&aDocType=" + 9}";

                    var response2 = await HttpRequestFactory.Get(requestUri2);

                    this._serilogProvider.Logger.Debug($"Status:" + response2.StatusCode.ToString());  // Ghi log hệ thống

                    var Data_Company2 = response2.Content.ReadAsStringAsync().Result;

                    var jObj2 = JsonConvert.DeserializeObject(Data_Company2) as JObject;

                    var jArr2 = jObj2.GetValue("table");

                    var table2 = jArr2.ToObject<List<PhanTichFPTSViewModel>>();

                    foreach (var re in table2)
                    {
                        TinTucViewModel tinTucViewModel = new TinTucViewModel();
                        tinTucViewModel.DatePub = DateTime.Parse(re.adatepub.ToString()).ToString("dd/MM/yyyy HH:mm");
                        tinTucViewModel.Title = re.atitle;
                        tinTucViewModel.FolderLuutru = FolderLuuTru.TinKhac;
                        tinTucViewModel.FiscYear = re.ayear;
                        tinTucViewModel.URL = UrlFile + "/" + re.aurl;
                        tinTucViewModel.language = re.alanguage;
                        tinTuc.Add(tinTucViewModel);
                    }
                    // Tin IR
                    var requestUri1 = $"{baseUri + WebApiConfigs.Link_WebApi_CBTT + "?aCpnyID=" + cpnyID + "&aDocType=" + FolderLuuTru.IR}";

                    var response = await HttpRequestFactory.Get(requestUri1);

                    this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                    var Data_Company = response.Content.ReadAsStringAsync().Result;

                    var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                    var jArr = jObj.GetValue("table");

                    var table = jArr.ToObject<List<PhanTichFPTSViewModel>>();

                    foreach (var re in table)
                    {
                        TinTucViewModel tinTucViewModel = new TinTucViewModel();
                        tinTucViewModel.DatePub = DateTime.Parse(re.adatepub.ToString()).ToString("dd/MM/yyyy HH:mm");
                        tinTucViewModel.Title = re.atitle;
                        tinTucViewModel.FolderLuutru = re.adoctype; // ban tin ir
                        tinTucViewModel.FiscYear = re.ayear;
                        tinTucViewModel.URL = UrlFile + "/" + re.aurl;
                        tinTucViewModel.language = re.alanguage;
                        tinTuc.Add(tinTucViewModel);
                    }

                    var listLuutru = tinTuc.OrderByDescending(d => DateTime.ParseExact(d.DatePub, "dd/MM/yyyy HH:mm", provider)).ToList();
                    return Json(listLuutru);
                }
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
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

            return this.ViewComponent("LuuTruTinTuSoVaVSD");
        }

        [HttpGet]
        public async Task<IActionResult> GetLuuTruTinTuSoVaVSD(string stock_code, string cpnyID)
        {
            try
            {
                cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
                if (cookieValue == "vn" || cookieValue == null)
                {
                    //tin CBTT
                    var requestUri = $"{tintucUri + WebApiConfigs.Link_WebApi_TinTucVN + "&code=" + stock_code + "&pageSize=" + 20000 + "&selectedPage=" + 1 + "&cbtt=" + 0 + "&newsType=" + 2}";
                    List<TinTucViewModel> tinTuc = new List<TinTucViewModel>();
                    var data = await CallNewsAPI.API_GetNew(requestUri);
                    tinTuc = data.Data.Table1;
                    foreach (var item in tinTuc)
                    {
                        if (item.FiscYear == null)
                        {
                            item.FiscYear = Convert.ToInt32(DateTime.Parse(item.DatePub.ToString()).ToString("yyyy"));
                        }
                    }
                    var listLuutru = tinTuc.OrderByDescending(d => Convert.ToDateTime(d.DatePub)).ToList();
                    return Json(listLuutru);
                }
                else
                {
                    //tin CBTT
                    var requestUri = $"{tintucUri + WebApiConfigs.Link_WebApi_TinTucEN + "&code=" + stock_code + "&pageSize=" + 20000 + "&selectedPage=" + 1 + "&cbtt=" + 0 + "&newsType=" + 2}";
                    List<TinTucViewModel> tinTuc = new List<TinTucViewModel>();
                    var data = await CallNewsAPI.API_GetNew(requestUri);
                    tinTuc = data.Data.Table1;
                    foreach (var item in tinTuc)
                    {
                        if (item.FiscYear == null)
                        {
                            item.FiscYear = Convert.ToInt32(DateTime.ParseExact(item.DatePub, "dd/MM/yyyy HH:mm", provider).ToString("yyyy"));
                        }

                    }
                    var listLuutru = tinTuc.OrderByDescending(d => DateTime.ParseExact(d.DatePub, "dd/MM/yyyy HH:mm", provider)).ToList();
                    return Json(listLuutru);
                }
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        /// <summary>
        /// kinh doanh
        /// </summary>
        /// <returns></returns>
        /// 


        /*public IActionResult Index()
        {

            return View();
        }*/
        // Trình độ công nghệ

        public IActionResult TrinhDoCongNghe()
        {
            ViewBag.CkeditorImgUrl = CkeditorImgUrl;
            return this.ViewComponent("TrinhDoCongNghe");
        }
        [HttpGet]
        public async Task<IActionResult> GetTrinhDoCongNghe(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listinfosheet + "?MenuID=" + ConstMenuID.TRINH_DO_CONG_NGHE + "&CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinViewModel>>();
                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        //Thị trường khách hàng đối thủ

        public IActionResult ThiTruongKHDT()
        {
            ViewBag.CkeditorImgUrl = CkeditorImgUrl;
            return this.ViewComponent("ThiTruongKHDT");
        }
        [HttpGet]
        public async Task<IActionResult> GetThiTruongKHDT(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listinfosheet + "?MenuID=" + ConstMenuID.CO_CAU_DOANH_THU + "&CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinViewModel>>();
                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetNguyenLieuDauVao(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listinfosheet + "?MenuID=" + ConstMenuID.NGUYEN_LIEU_DAU_VAO_NCC + "&CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinViewModel>>();
                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetSanPham(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listinfosheet + "?MenuID=" + ConstMenuID.SAN_PHAM_THAY_THE + "&CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinViewModel>>();
                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetThiTruong(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listinfosheet + "?MenuID=" + ConstMenuID.THI_TRUONG_KHACH_HANG + "&CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinViewModel>>();
                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetDoiThu(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listinfosheet + "?MenuID=" + ConstMenuID.DOI_THU_CANH_TRANH + "&CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinViewModel>>();
                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetKhac(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listinfosheet + "?MenuID=" + ConstMenuID.THI_TRUONG_KHAC_HANG_DOI_THU_KHAC + "&CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinViewModel>>();
                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        // Thông tin dự án đầu tư

        public IActionResult ThongTinDuAnDauTu()
        {
            ViewBag.CkeditorImgUrl = CkeditorImgUrl;
            return this.ViewComponent("ThongTinDuAnDauTu");
        }
        [HttpGet]
        public async Task<IActionResult> GetThongTinDuAnDauTu(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listinfosheet + "?MenuID=" + ConstMenuID.THONG_TIN_DU_AN + "&CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinViewModel>>();
                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        //năng lực quản lý

        public IActionResult NangLucQuanLy()
        {
            ViewBag.CkeditorImgUrl = CkeditorImgUrl;
            return this.ViewComponent("NangLucQuanLy");
        }
        [HttpGet]
        public async Task<IActionResult> GetQuanLyChatLuong(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listinfosheet + "?MenuID=" + ConstMenuID.NANG_LUC_QUAN_LY_QUAN_LY_CHAT_LUONG + "&CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinViewModel>>();
                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetQuanLyTaiChinh(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listinfosheet + "?MenuID=" + ConstMenuID.NANG_LUC_QUAN_LY_QUAN_TRI_TAI_CHINH + "&CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinViewModel>>();
                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetQuanLyNhanSu(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listinfosheet + "?MenuID=" + ConstMenuID.NANG_LUC_QUAN_LY_QUAN_TRI_NHAN_SU + "&CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinViewModel>>();
                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetQuanLyKhac(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listinfosheet + "?MenuID=" + ConstMenuID.NANG_LUC_QUAN_LY_CAC_HE_THONG_QUAN_LY_KHAC + "&CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinViewModel>>();
                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        //phân tích swot

        public IActionResult PhanTichSWOT()
        {
            ViewBag.CkeditorImgUrl = CkeditorImgUrl;
            return this.ViewComponent("PhanTichSWOT");
        }
        [HttpGet]
        public async Task<IActionResult> GetDiemManh(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listinfosheet + "?MenuID=" + ConstMenuID.SWOT_DIEM_MANH + "&CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinViewModel>>();
                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetDiemYeu(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listinfosheet + "?MenuID=" + ConstMenuID.SWOT_DIEM_YEU + "&CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinViewModel>>();
                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetThachThuc(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listinfosheet + "?MenuID=" + ConstMenuID.SWOT_THACH_THUC + "&CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinViewModel>>();
                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCoHoi(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listinfosheet + "?MenuID=" + ConstMenuID.SWOT_CO_HOI + "&CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinViewModel>>();
                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        //vị thế doanh nghiệp

        public IActionResult ViTheDoanhNghiep()
        {
            ViewBag.CkeditorImgUrl = CkeditorImgUrl;
            return this.ViewComponent("ViTheDoanhNghiep");
        }
        [HttpGet]
        public async Task<IActionResult> GetViThe(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listinfosheet + "?MenuID=" + ConstMenuID.VI_THE_DOANH_NGHIEP_VI_THE_TRONG_TUNG_LINH_VUC_KINH_DOANH + "&CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinViewModel>>();
                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetViTheChung(string cpnyID)
        {
            try
            {
                //cpnyID = Request.Cookies[CookieTypes.CPNY_ID];
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listinfosheet + "?MenuID=" + ConstMenuID.VI_THE_DOANH_NGHIEP_VI_THE_CHUNG + "&CpnyID=" + cpnyID }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinViewModel>>();
                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        /// <summary>
        /// đọc giá liên tực từ gateway 
        /// </summary>
        /// <param name="stockCode"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> PriceRealTime(string stockCode)
        {
            LivePrice result = new LivePrice();

            string priceGatewayUri = $"{priceUri + stockCode}";

            try
            {
                HttpResponseMessage res = await client.GetAsync(priceGatewayUri);


                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();

                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LivePrice>>(data.ToString()); // dữ liệu json trả ra có dấu "[ ]" dạng list nên phải để List mới lấy được.
                    result = obj[0];
                    return Ok(result);

                }

                return Ok(result);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetThongTinChung(string stockCode)
        {
            try
            {
                //stockCode = Request.Cookies[CookieTypes.STOCK_CODE];
                stockCode = stockCode.Replace("?", "");
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_TTC + "?stockCode=" + stockCode }";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");

                var table = jArr.ToObject<List<ThongTinViewModel>>();

                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        public async Task<IActionResult> GetThongTinGiaoDich(string stockCode)
        {
            try
            {
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_TTGD + "?stock_code=" + stockCode}";

                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");

                var table = jArr.ToObject<List<ThongTinViewModel>>();

                return Json(table);

            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        /// <summary>
        /// Cổ phiếu
        /// Lịch sử giao dịch
        /// </summary>
        /// <returns></returns>
        public IActionResult LichSuGiaoDich()
        {
            return this.ViewComponent("LichSuGiaoDich");
        }
        //tìm kiếm lịch sử giao dịch
        [HttpPost]
        public async Task<IActionResult> SearchLichSuGiaoDich(string stock_code, string fromDate, string toDate)
        {
            try
            {

                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_Listpricehistory + "?stockCode=" + stock_code + "&fromDate=" + fromDate + "&toDate=" + toDate}";
                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinLichSuGiaoDichViewModel>>();
                return Json(table);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }


        public IActionResult LichSuTangVonTraCoTuc()
        {
            return this.ViewComponent("LichSuTangVonTraCoTuc");
        }

        /// <summary>
        /// Lịch sử trả cổ tức
        /// </summary>
        /// <param name="stock_code"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetLichSuTraCoTuc(string CpnyID, string Language)
        {
            try
            {

                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_LS_TRACOTUC + "?CpnyID=" + CpnyID + "&Language=" + Language}";

                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");
                var table = jArr.ToObject<List<ThongTinLichSuGiaoDichViewModel>>();
                return Json(table);

            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        /// <summary>
        /// Lịch sử tăng vốn
        /// </summary>
        /// <param name="stock_code"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetLichSuTangVon(string cpnyid, string language)
        {
            try
            {
                //Sp check điều kiện Language = 'VN'
                if (language == "")
                {
                    language = "VN";
                }


                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_LS_TANGVON + "?CpnyID=" + cpnyid + "&Language=" + language}";

                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");

                var table = jArr.ToObject<List<ThongTinLichSuGiaoDichViewModel>>();

                return Json(table);

            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        public IActionResult DoThiKyThuat()
        {
            return this.ViewComponent("DoThiKyThuat");
        }
        //Phân tích của FPTS

        public IActionResult PhanTichFPTS()
        {
            cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            ViewBag.UrlFile = UrlFile + '/';
            return this.ViewComponent("PhanTichFPTS");
        }
        [HttpGet]
        public async Task<IActionResult> GetPhanTichFPTS(string cpnyID)
        {
            try
            {
                var requestUri = $"{baseUri + WebApiConfigs.Link_WebApi_CBTT + "?aCpnyID=" + cpnyID + "&aDocType=" + 11}";

                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                var Data_Company = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_Company) as JObject;

                var jArr = jObj.GetValue("table");

                var table = jArr.ToObject<List<PhanTichFPTSViewModel>>();

                return Json(table);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        //#Region tài chinh
        //Hệ số tài chính
        public IActionResult HeSoTaiChinh()
        {

            cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            return this.ViewComponent("HeSoTaiChinh");
        }

        [HttpGet]
        public async Task<IActionResult> GetHSTC(string stock_code, int? year = 2020, string dateType = "Q", int? period = 3, int datatype = 1)
        {
            try
            {
                var language = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());

                var requestUri = baseUri + WebApiConfigs.Link_WebApi_TCHSTC + "?Year=" + year + "&DateType=" + dateType + "&Period=" + period + "&StockCode=" + stock_code + "&Language=" + language.ToUpper() + "&DataType=" + datatype;

                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_HSTC = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_HSTC) as JObject;

                var jArr = jObj.GetValue("table");
                var jArr1 = jObj.GetValue("table1");

                List<HSTCModelView> hstc = new List<HSTCModelView>();

                var tab = jArr.ToObject<List<HSTCViewModel>>();
                var tab1 = jArr1.ToObject<List<HSTCViewModel>>();
                hstc.Add(new HSTCModelView { table = tab, table1 = tab1 });
                return Json(hstc);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExcelHSTC(string stock_code, int? year = 2020, string dateType = "Q", int? period = 3, int datatype = 1, int numofPeriod = 5, string unit = "d")
        {
            try
            {
                var language = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());

                var requestUri = baseUri + WebApiConfigs.Link_WebApi_TCHSTC + "?Year=" + year + "&DateType=" + dateType + "&Period=" + period + "&StockCode=" + stock_code + "&Language=" + language.ToUpper() + "&DataType=" + datatype;

                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());
                var Data_HSTC = response.Content.ReadAsStringAsync().Result;
                var jObj = JsonConvert.DeserializeObject(Data_HSTC) as JObject;

                return File(_excel.HSTCExcel(jObj, numofPeriod, unit).ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"FTPSecurities_{stock_code}_He-so-tai-chinh.xlsx");
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        

        public IActionResult CanDoiKeToan()
        {

            cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            return this.ViewComponent("CanDoiKeToan");
        }

        [HttpGet]
        public async Task<IActionResult> GetCDKT(string stock_code, int? year = 2020, string dateType = "Q", int? period = 9, int datatype = 1)
        {
            try
            {
                var language = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
                /*if (language != null)
                {
                    language = language.Substring(language.Length - 2, 2);
                }

                if (language == "US")
                {
                    language = "EN";
                }*/

                var requestUri = baseUri + WebApiConfigs.Link_WebApi_TCCDKT + "?Year=" + year + "&DateType=" + dateType + "&Period=" + period + "&StockCode=" + stock_code + "&Language=" + language.ToUpper() + "&DataType=" + datatype;

                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                var Data_CDKT = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_CDKT) as JObject;

                var jArr = jObj.GetValue("table");
                var jArr1 = jObj.GetValue("table1");

                var root = (JContainer)jArr1;
                var list = root.DescendantsAndSelf().OfType<JProperty>().Where(p => p.Name == "alev").Select(p => Convert.ToInt32(p.Value.Value<string>()));
                int maxlevel = list.ToArray().Max();

                List<CDKTModelView> cdkt = new List<CDKTModelView>();

                var tab = jArr.ToObject<List<CDKTViewModel>>();
                var tab1 = jArr1.ToObject<List<CDKTViewModel>>();
                cdkt.Add(new CDKTModelView { table = tab, table1 = tab1, MaxLevel = maxlevel });
                return Json(cdkt);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExcelCDKT(string stock_code, int? year = 2020, string dateType = "Q", int? period = 9, int datatype = 1, int numofPeriod = 5, string unit = "d")
        {
            try
            {
                var language = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());

                var requestUri = baseUri + WebApiConfigs.Link_WebApi_TCCDKT + "?Year=" + year + "&DateType=" + dateType + "&Period=" + period + "&StockCode=" + stock_code + "&Language=" + language.ToUpper() + "&DataType=" + datatype;

                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                var Data_CDKT = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_CDKT) as JObject;

                return File(_excel.CDKTExcel(jObj, numofPeriod, unit).ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"FTPSecurities_{stock_code}_Can-doi-ke-toan.xlsx");
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        public IActionResult KeHoachTaiChinh()
        {

            cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            return this.ViewComponent("KeHoachTaiChinh");
        }

        [HttpGet]
        public async Task<IActionResult> GetKHTC(string stock_code, int? year = 2020, string type = null)
        {
            try
            {
                var language = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());

                var requestUri = baseUri + WebApiConfigs.Link_WebApi_TCKHTC + "?Year=" + year + "&StockCode=" + stock_code;

                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                var Data_HSTC = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_HSTC) as JObject;

                var jArr = jObj.GetValue("table");

                //Type typeList = typeof(List<>);

                //object list;
                List<KHTCModelView> khtc = new List<KHTCModelView>();

                if (jArr != null)
                {
                    switch (type)
                    {
                        //cty thường & chứng khoán
                        case "0":
                        case "2":
                            var tabSecurity = jArr.ToObject<List<KHTCSecuritiesViewModel>>();
                            khtc.Add(new KHTCModelView { table = tabSecurity.Cast<dynamic>().ToList() });
                            break;
                        //ngân hàng
                        case "1":
                            var tabBank = jArr.ToObject<List<KHTCBankViewModel>>();
                            khtc.Add(new KHTCModelView { table = tabBank.Cast<dynamic>().ToList() });
                            break;
                        //bảo hiểm
                        case "3":
                        case "5":
                            var tabInsurance = jArr.ToObject<List<KHTCInsuranceViewModel>>();
                            khtc.Add(new KHTCModelView { table = tabInsurance.Cast<dynamic>().ToList() });
                            break;
                    }
                }

                //var tab = jArr.ToObject<List<KHTCSecuritiesViewModel>>();

                return Json(khtc);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExcelKHTC(string stock_code, int? year = 2020, string type = null, string unit = "d")
        {
            try
            {
                var language = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());

                var requestUri = baseUri + WebApiConfigs.Link_WebApi_TCKHTC + "?Year=" + year + "&StockCode=" + stock_code;

                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                var Data_HSTC = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_HSTC) as JObject;

                return File(_excel.KHTCExcel(jObj, type, unit).ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"FTPSecurities_{stock_code}_Ke-hoach-tai-chinh.xlsx");
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        //#Endregion

        //#region Kết quả hoạt động kinh doanh

        public IActionResult KetQuaHoatDongKinhDoanh()
        {

            cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            return this.ViewComponent("KetQuaHoatDongKinhDoanh");
        }

        [HttpGet]
        public async Task<IActionResult> GetKQHDKD(string stock_code, string language, int? year = 2019, string dateType = "Q", int? period = 9, int datatype = 1)
        {
            try
            {
                

                var requestUri = baseUri + WebApiConfigs.Link_WebApi_KQKD + "?Year=" + year + "&DateType=" + dateType + "&Period=" + period + "&StockCode=" + stock_code + "&Language=" + language + "&DataType=" + datatype;

                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_KQKD = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_KQKD) as JObject;

                var jArr = jObj.GetValue("table");
                var jArr1 = jObj.GetValue("table1");

                List<KQKDModelView> kqkd = new List<KQKDModelView>();

                var tab = jArr.ToObject<List<KQKDViewModel>>();
                var tab1 = jArr1.ToObject<List<KQKDViewModel>>();
                kqkd.Add(new KQKDModelView { table = tab, table1 = tab1 });
                return Json(kqkd);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExcelKQHDKD(string stock_code,  int? year = 2019, string dateType = "Q", int? period = 9, int datatype = 1,int numofPeriod = 5,string unit = "d")
        {
            try
            {
                var language = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());

                language = language.ToUpper();

                var requestUri = baseUri + WebApiConfigs.Link_WebApi_KQKD + "?Year=" + year + "&DateType=" + dateType + "&Period=" + period + "&StockCode=" + stock_code + "&Language=" + language + "&DataType=" + datatype;

                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_KQKD = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_KQKD) as JObject;

                return File(_excel.CommonExcel(jObj, numofPeriod, unit, "KQHDKD", _localizer["KQHDKD"].Value).ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"FTPSecurities_{stock_code}_Ket-qua-hoat-dong-kinh-doanh.xlsx");
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        //#Endregion

        //#region Lưu chuyển tiền tệ ICF
        public IActionResult LuuChuyenTienTeICF()
        {

            cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            return this.ViewComponent("LuuChuyenTienTeICF");
        }

        [HttpGet]
        public async Task<IActionResult> GetLuuChuyenTienTeICF(string stock_code, string language, int? year = 2019, string dateType = "Y", int? period = 12, int datatype = 1)
        {
            try
            {
                var requestUri = baseUri + WebApiConfigs.Link_WebApi_LuuChuyenICF + "?Year=" + year + "&DateType=" + dateType + "&Period=" + period + "&StockCode=" + stock_code + "&Language=" + language + "&DataType=" + datatype;

                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_ICF = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_ICF) as JObject;

                var jArr = jObj.GetValue("table");
                var jArr1 = jObj.GetValue("table1");

                List<LuuChuyenTienTeICFModelView> ICF = new List<LuuChuyenTienTeICFModelView>();

                var tab = jArr.ToObject<List<LuuChuyenTienTeICFViewModel>>();
                var tab1 = jArr1.ToObject<List<LuuChuyenTienTeICFViewModel>>();
                ICF.Add(new LuuChuyenTienTeICFModelView { table = tab, table1 = tab1 });
                return Json(ICF);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExcelLuuChuyenTienTeICF(string stock_code,  int? year = 2019, string dateType = "Y", int? period = 12, int datatype = 1,int numofPeriod = 5,string unit = "d")
        {
            try
            {
                var language = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());

                var requestUri = baseUri + WebApiConfigs.Link_WebApi_LuuChuyenICF + "?Year=" + year + "&DateType=" + dateType + "&Period=" + period + "&StockCode=" + stock_code + "&Language=" + language + "&DataType=" + datatype;

                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                var Data_ICF = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_ICF) as JObject;

                return File(_excel.CommonExcel(jObj,numofPeriod,unit,"ICF", _localizer["ICF"].Value).ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"FTPSecurities_{stock_code}_Luu-chuyen-tien-te-ICF.xlsx");
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        //#Endregion

        //#region Lưu chuyển tiền tệ DCF
        public IActionResult LuuChuyenTienTeDCF()
        {

            cookieValue = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
            ViewBag.Language = cookieValue;
            return this.ViewComponent("LuuChuyenTienTeDCF");
        }

        [HttpGet]
        public async Task<IActionResult> GetLuuChuyenTienTeDCF(string stock_code, string language, int? year = 2019, string dateType = "Y", int? period = 12, int datatype = 1)
        {
            try
            {
                var requestUri = baseUri + WebApiConfigs.Link_WebApi_LuuChuyenDCF + "?Year=" + year + "&DateType=" + dateType + "&Period=" + period + "&StockCode=" + stock_code + "&Language=" + language + "&DataType=" + datatype;

                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                //cResponseMessage = JsonConvert.DeserializeObject<CResponseMessage>(response.Content.ReadAsStringAsync().Result);

                var Data_DCF = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_DCF) as JObject;

                var jArr = jObj.GetValue("table");
                var jArr1 = jObj.GetValue("table1");

                List<LuuChuyenTienTeDCFModelView> DCF = new List<LuuChuyenTienTeDCFModelView>();

                var tab = jArr.ToObject<List<LuuChuyenTienTeDCFViewModel>>();
                var tab1 = jArr1.ToObject<List<LuuChuyenTienTeDCFViewModel>>();
                DCF.Add(new LuuChuyenTienTeDCFModelView { table = tab, table1 = tab1 });
                return Json(DCF);
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExcelLuuChuyenTienTeDCF(string stock_code,  int? year = 2019, string dateType = "Y", int? period = 12, int datatype = 1, int numofPeriod = 5, string unit = "d")
        {
            try
            {
                var language = CommonLib.CommonUtil.ReturnLanguge(_httpContextAccessor.HttpContext.Request.QueryString.ToString());

                var requestUri = baseUri + WebApiConfigs.Link_WebApi_LuuChuyenDCF + "?Year=" + year + "&DateType=" + dateType + "&Period=" + period + "&StockCode=" + stock_code + "&Language=" + language + "&DataType=" + datatype;

                var response = await HttpRequestFactory.Get(requestUri);

                this._serilogProvider.Logger.Debug($"Status:" + response.StatusCode.ToString());  // Ghi log hệ thống

                var Data_DCF = response.Content.ReadAsStringAsync().Result;

                var jObj = JsonConvert.DeserializeObject(Data_DCF) as JObject;

                return File(_excel.CommonExcel(jObj, numofPeriod, unit,"DCF", _localizer["DCF"].Value).ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"FTPSecurities_{stock_code}_Luu-chuyen-tien-te-DCF.xlsx");
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        //#Endregion



        /// <summary>
        /// Language
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {

            //Response.Cookies.Append(
            //    CookieRequestCultureProvider.DefaultCookieName,
            //    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            //    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), SameSite = SameSiteMode.Lax }
            //);
            if (returnUrl.Contains("?culture=en-US"))
                returnUrl = returnUrl.Replace("?culture=en-US", "");
            else
                returnUrl = returnUrl + "?culture=en-US";



            return Redirect(returnUrl);
        }


    }
}
