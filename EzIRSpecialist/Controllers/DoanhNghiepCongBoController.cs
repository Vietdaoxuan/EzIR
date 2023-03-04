using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Commons;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Interfaces;
using EzIRSpecialist.Authentication;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models;
using EzIRSpecialist.Models.ModelView.QuanLyDoanhNghiepCongBo;
using EzIRSpecialist.Models.ViewModel;
using EzIRSpecialist.Models.ViewModel.QLTKViewModel;
using EzIRSpecialist.Models.ViewModel.QuanLyDoanhNghiepCongBo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace EzIRSpecialist.Controllers.QuanLyDoanhNghiepCong_Bo
{
    public class DoanhNghiepCongBoController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IDoanhNghiepCongBoContext _DoanhNghiepCongBoContext;
        private readonly IDoanhNghiepContext _DoanhNghiepContext;
        private readonly ICommonTypeContext _commonTypeContext;
        private readonly ICompanyContext _companyContext;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IHttpService _httpService;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ICommon _common;
        string _UrlFileCommon;

        private string _username { get; set; }
        private string _roleCode { get; set; }

        public DoanhNghiepCongBoController(IConfiguration configuration, IDoanhNghiepCongBoContext DoanhNghiepCongBoContext, IDoanhNghiepContext DoanhNghiepContext, ICommonTypeContext commonTypeContext, ICompanyContext companyContext, IAppLogger appLogger, IMemoryCache cache, IHttpService httpService, IStringLocalizer<SharedResource> sharedLocalizer, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, ICommon common)
        {
            _configuration = configuration;
            _DoanhNghiepCongBoContext = DoanhNghiepCongBoContext;
            _DoanhNghiepContext = DoanhNghiepContext;
            _commonTypeContext = commonTypeContext;
            _companyContext = companyContext;
            _appLogger = appLogger;
            _cache = cache;
            _httpService = httpService;
            _sharedLocalizer = sharedLocalizer;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _common = common;
            _username = _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.USERNAME);
            _roleCode = RoleCode.QL_DOANH_NGHIEP_CONG_BO;
            _UrlFileCommon = _configuration.GetSection("UrlFileCommon").Value;
        }

        [EzAuthorize(RoleCode.QL_DOANH_NGHIEP_CONG_BO, view: true)]
        public async Task<IActionResult> Index(int? value)
        {
            List<CommonType> listCompanyType = new List<CommonType>();
            List<CommonType> listloaitl = new List<CommonType>();
            List<CommonType> listloaitin = new List<CommonType>();
            List<CommonType> listRegion = new List<CommonType>();
            var listMCK = await _companyContext.Select(new CompanyViewModel());
            ViewBag._UrlFileCommon = _UrlFileCommon + "/";
            var commonType = new CommonTypeViewModel
            {
                ListCategory = "1,3,4,11"
            };
            var listCommontype = await _commonTypeContext.Select(commonType);

            if (value == null)
            {
                foreach (var obj in listCommontype.ToList())
                {
                    if (obj.Category == ConstCommonTypes.NHOM_VUNG_MIEN)
                    {
                        listRegion.Add(obj);
                    }
                    if (obj.Category == ConstCommonTypes.NHOM_LOAI_HINH)
                    {
                        listCompanyType.Add(obj);
                    }
                    // QuangKS edit chỉ lấy ra danh sách tài liệu có ord < 12 loại trừ 2 tài liêu mới -- Chú ý việc sắp xếp lại ord trong tương lai
                    // Thêm lại Báo cáo phân tích doanh nghiệp (11)
                    if (obj.Category == ConstCommonTypes.NHOM_TAI_LIEU && obj.Ord < 12)
                    {
                        listloaitl.Add(obj);
                    }
                }

                var viewmodel = new DoanhNghiepCongBoViewModel
                {
                    listloaitl = listloaitl,
                    listCompanyType = listCompanyType,
                    listMCK = listMCK.ToList(),
                    listloaitin = listloaitin,
                    listRegion = listRegion
                };

                return View(viewmodel);
            }
            else
            {
                foreach (var listin in listCommontype.ToList())
                {
                    if (listin.ParentID == value)

                        listloaitin.Add(listin);

                }

                return Json(listloaitin);
            }
        }

        [HttpGet]
        [Route(LinksRoute.GetCongBoDoanhNghiep)]
        [EzAuthorize(RoleCode.QL_DOANH_NGHIEP_CONG_BO, view: true)]
        public async Task<IActionResult> ListDoanhNghiepCongBo(DoanhNghiepCongBoViewModel doanhNghiep, int updclick = 0)
        {
            doanhNghiep.UserLogin = _username;
            doanhNghiep.RoleCode = _roleCode;
            if (!string.IsNullOrEmpty(doanhNghiep.UserLogin) && !string.IsNullOrEmpty(doanhNghiep.RoleCode))
            {

                TimeSpan start = new TimeSpan(0, 00, 01);
                TimeSpan end = new TimeSpan(11, 59, 59);
                if (doanhNghiep.FromDate != null)
                {
                    string datetime = doanhNghiep.FromDate.Value.ToShortDateString() + " " + start + " SA";
                    doanhNghiep.FromDate = DateTime.ParseExact(datetime, "dd/MM/yyyy hh:mm:ss tt", null);
                }
                if (doanhNghiep.ToDate != null)
                {
                    string datetime = doanhNghiep.ToDate.Value.ToShortDateString() + " " + end + " CH";
                    doanhNghiep.ToDate = DateTime.ParseExact(datetime, "dd/MM/yyyy hh:mm:ss tt", null);
                }
                var data = await _DoanhNghiepCongBoContext.Select(doanhNghiep);
                if (data.Count() == 1 && updclick == 1)
                {
                    //Lấy ttin chuyên viên phụ trách doanh nghiệp
                    var dataDoanhNghiep = await _DoanhNghiepContext.Select(new DoanhNghiepViewModel { CompanyID = doanhNghiep.CompanyID });
                    string expert = dataDoanhNghiep.ToList().FirstOrDefault().Expert;

                    // lấy ttin người đăng tin và loại tài khoản đăng tin
                    string userPost = data.ToList().FirstOrDefault().UserPost;
                    string loaiTK = data.ToList().FirstOrDefault().AccountType;


                    //Tài khoản chuyên viên chỉ sửa/ xóa được thông tin do chính TK chuyên viên đó hoặc do TK doanh nghiệp được chuyên viên đó phụ trách công bố (nhưng không sửa được trường “ngày đăng tin”)
                    if (expert == doanhNghiep.UserLogin || (userPost == doanhNghiep.UserLogin && loaiTK == "Chuyên viên")
                        || (_httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "ADMIN"
                        || _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "FCF Admin Group"))
                    {

                    }
                    else
                    {
                        return Json(new CResponseMessage { Code = -1, Message = "Tài khoản đang dùng không có quyền sửa thông tin này" });
                    }
                }
                return Json(data.ToList());
            }
            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }

        [HttpPost]
        [Route(LinksRoute.InsertCongBoDoanhNghiep)]
        [EzAuthorize(RoleCode.QL_DOANH_NGHIEP_CONG_BO, view: true, edit: true)]
        public async Task<IActionResult> InsertNews(DoanhNghiepCongBoViewModel doanhNghiep, IFormFile file)
        {

            try
            {
                doanhNghiep.UserLogin = _username;
                doanhNghiep.RoleCode = _roleCode;
                String sDate = DateTime.Now.ToString();
                DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

                String dy = datevalue.Day.ToString();
                String mn = datevalue.Month.ToString();
                String yy = datevalue.Year.ToString();

                if (Convert.ToInt32(dy) <= 9) dy = "0" + dy;
                if (Convert.ToInt32(mn) <= 9) mn = "0" + mn;


                string s = DateTime.Now.ToString("hh:mm:ss tt");

                string datetime = doanhNghiep.DatePub.Value.ToShortDateString() + " " + s;

                doanhNghiep.DatePub = DateTime.ParseExact(datetime, "dd/MM/yyyy hh:mm:ss tt", null);



                if (!string.IsNullOrEmpty(doanhNghiep.UserLogin) && !string.IsNullOrEmpty(doanhNghiep.RoleCode))
                {
                    //Lấy ttin chuyên viên phụ trách doanh nghiệp
                    var dataDoanhNghiep = await _DoanhNghiepContext.Select(new DoanhNghiepViewModel { CompanyID = doanhNghiep.CompanyID });
                    string expert = dataDoanhNghiep.ToList().FirstOrDefault().Expert;

                    // lấy ttin người đăng tin và loại tài khoản đăng tin
                    //var data = await _DoanhNghiepCongBoContext.Select(new DoanhNghiepCongBoViewModel { NewID = doanhNghiep.NewID });
                    //string userPost = data.ToList().FirstOrDefault().UserPost;
                    //string loaiTK = data.ToList().FirstOrDefault().AccountType;


                    //Tài khoản chuyên viên chỉ thêm được thông tin do TK doanh nghiệp được chuyên viên đó phụ trách 
                    if (expert == doanhNghiep.UserLogin || (_httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "ADMIN"
                        || _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "FCF Admin Group"))
                    {
                        if (file == null)
                        {
                            return Json(new CResponseMessage { Code = -1, Message = "Chưa chọn File" });
                        }

                        string[] validFileType = { "doc", "docx", "rar", "pdf", "zip" };
                        string FileExtension = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();


                        //File không vượt quá 30MB
                        if (file.Length > 30 * 1024 * 1024)
                        {
                            return Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["FileIsOver30MB"].Value });
                        }

                        //kiểm tra định dạng file và dung lương file
                        if (validFileType.Contains(FileExtension) && file.Length < 30 * 1024 * 1024)
                        {
                            //var result = await this._common.SaveBieuMau(file, this._configuration["NewsPath"], this._hostingEnvironment.ContentRootPath, this._configuration);
                            var result = await this._common.CopyFile(file, this._configuration["NewsPath"], this._hostingEnvironment, this._configuration);
                            if (result.Message == "UPLOAD_FILE_SUCCESS")
                            {
                                //Gán FileName, Path, Url
                                doanhNghiep.FileName = Path.GetFileName(result.Data.ToString());
                                doanhNghiep.Path = _configuration["PathFile"] + result.Data.ToString();
                                doanhNghiep.Url = _configuration["UrlSaveFile"] + result.Data.ToString();
                                //this._configuration["NewsPath"] + "/" + yy + "/" + mn + "/" + dy + "/" + doanhNghiep.FileName;


                                //Nếu loại tài liệu là bản tin IR hoặc báo cáo phân tích doanh nghiệp thì chỉ insert vào db EzIR
                                //QuangKS edit DocType = 111 => 11 2021-11-23
                                if (doanhNghiep.DocType == 10 || doanhNghiep.DocType == 11)
                                {
                                    var cResponse = await _DoanhNghiepCongBoContext.Insert(doanhNghiep);
                                    cResponse.Message = this._sharedLocalizer[cResponse.Message];
                                    return Json(cResponse);
                                }


                                //Lấy ra mã chứng khoán
                                var company = await this._companyContext.Select(new CompanyViewModel());
                                var stockCode = company.ToList().Where(x => x.CompanyID == doanhNghiep.CompanyID).FirstOrDefault().StockCode;

                                //Gán thông tin News bên CBTT
                                NewsWebsiteAPI newsWebsiteAPI = new NewsWebsiteAPI()
                                {
                                    DatePub = DateTime.Parse(doanhNghiep.DatePub.ToString()).ToString("MM/dd/yyyy HH:mm"),
                                    DocTypeID = doanhNghiep.DocType ?? default(int),
                                    Title = doanhNghiep.Title,
                                    Language = "VN",
                                    ReportYear = doanhNghiep.Year.ToString(),
                                    URL = doanhNghiep.Url,
                                    NewsID = doanhNghiep.NewID ?? default(int),
                                    UserName = doanhNghiep.UserLogin,
                                    Stock_Code = stockCode,
                                };
                                //Call API CBTT insert db CBTT
                                var cResponseMessage = await _httpService.CallAPIWebsite(newsWebsiteAPI, this._configuration["API_CBTT"], LinksRoute.API_INSERT_NEW);
                                if (cResponseMessage.Code == 0 && cResponseMessage.Data.NewsID != -1)
                                {
                                    doanhNghiep.Sid = cResponseMessage.Data.NewsID;
                                    var cResponse = await _DoanhNghiepCongBoContext.Insert(doanhNghiep);

                                    //Nếu insert news vào db EzIR lỗi thì xóa dữ liệu vừa insert bên CBTT
                                    if (cResponse.Code != 0)
                                    {
                                        var cResponseMessageDelete = await _httpService.CallAPIWebsite(newsWebsiteAPI, this._configuration["API_CBTT"], LinksRoute.API_DELETE_NEW);
                                    }

                                    cResponse.Message = this._sharedLocalizer[cResponse.Message];
                                    return Json(cResponse);
                                }
                                else
                                {
                                    return Json(new CResponseMessage { Code = -1, Message = "Lỗi khi thêm tin mới!" });
                                }

                            }
                            else
                            {
                                return Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer[result.Message].Value });
                            }
                        }
                        else
                        {
                            return Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["FileIsNotAccepted"].Value });
                        }
                    }
                    else
                    {
                        return Json(new CResponseMessage { Code = -1, Message = " Tài khoản đang dùng không có quyền đăng tin cho MCK này!" });
                    }




                }

                var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new CResponseMessage { Code = -1, Message = "Lỗi hệ thống" });
            }

        }

        [HttpPost]
        [Route(LinksRoute.UpdateCongBoDoanhNghiep)]
        [EzAuthorize(RoleCode.QL_DOANH_NGHIEP_CONG_BO, view: true, edit: true)]
        public async Task<IActionResult> UpdateNews(DoanhNghiepCongBoViewModel doanhNghiep, IFormFile file)
        {
            try
            {
                doanhNghiep.UserLogin = _username;
                doanhNghiep.RoleCode = _roleCode;

                String sDate = DateTime.Now.ToString();
                DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));

                String dy = datevalue.Day.ToString();
                String mn = datevalue.Month.ToString();
                String yy = datevalue.Year.ToString();

                clsWebsiteRequest clsWebsiteRequest;

                if (Convert.ToInt32(dy) <= 9) dy = "0" + dy;
                if (Convert.ToInt32(mn) <= 9) mn = "0" + mn;


                string s = DateTime.Now.ToString("hh:mm:ss tt");

                string datetime = doanhNghiep.DatePub.Value.ToShortDateString() + " " + s;

                doanhNghiep.DatePub = DateTime.ParseExact(datetime, "dd/MM/yyyy hh:mm:ss tt", null);

                if (!string.IsNullOrEmpty(doanhNghiep.UserLogin) && !string.IsNullOrEmpty(doanhNghiep.RoleCode))
                {

                    //Lấy ttin chuyên viên phụ trách doanh nghiệp
                    var dataDoanhNghiep = await _DoanhNghiepContext.Select(new DoanhNghiepViewModel { CompanyID = doanhNghiep.CompanyID });
                    string expert = dataDoanhNghiep.ToList().FirstOrDefault().Expert;

                    // lấy ttin người đăng tin và loại tài khoản đăng tin
                    var data = await _DoanhNghiepCongBoContext.Select(new DoanhNghiepCongBoViewModel { NewID = doanhNghiep.NewID });
                    string userPost = data.ToList().FirstOrDefault().UserPost;
                    string loaiTK = data.ToList().FirstOrDefault().AccountType;


                    //Tài khoản chuyên viên chỉ sửa/ xóa được thông tin do chính TK chuyên viên đó hoặc do TK doanh nghiệp được chuyên viên đó phụ trách công bố (nhưng không sửa được trường “ngày đăng tin”)
                    if (expert == doanhNghiep.UserLogin || (userPost == doanhNghiep.UserLogin && loaiTK == "Chuyên viên")
                    || (_httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "ADMIN"
                        || _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "FCF Admin Group"))
                    {
                        string[] validFileType = { "doc", "docx", "rar", "pdf", "zip" };


                        //Lấy ra mã chứng khoán
                        var company = await this._companyContext.Select(new CompanyViewModel());
                        var stockCode = company.ToList().Where(x => x.CompanyID == doanhNghiep.CompanyID).FirstOrDefault().StockCode;

                        DateTime? dateCreate = data.Where(x => x.NewID == doanhNghiep.NewID).FirstOrDefault().DateCreate;
                        dateCreate = DateTime.Parse(dateCreate.Value.ToShortDateString());

                        //Gán thông tin News bên CBTT
                        NewsWebsiteAPI newsWebsiteAPI = new NewsWebsiteAPI()
                        {
                            
                            DatePub = DateTime.Parse(doanhNghiep.DatePub.ToString()).ToString("MM/dd/yyyy HH:mm"),
                            DocTypeID = doanhNghiep.DocType ?? default(int),
                            Title = doanhNghiep.Title,
                            Language = "VN",
                            ReportYear = doanhNghiep.Year.ToString(),
                            //URL = doanhNghiep.Url,
                            NewsID = doanhNghiep.Sid ?? default(int),
                            UserName = doanhNghiep.UserLogin,
                            Stock_Code = stockCode,
                        };



                        if (file != null)
                        {
                            string FileExtension = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();

                            //File không vượt quá 30MB
                            if (file.Length > 30 * 1024 * 1024)
                            {
                                return Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["FileIsOver30MB"].Value });
                            }

                            //Kiểm tra định dạng File
                            if (validFileType.Contains(FileExtension))
                            {
                                //var result = await this._common.SaveBieuMau(file, this._configuration["NewsPath"], this._hostingEnvironment.ContentRootPath, this._configuration);
                                var result = await this._common.CopyFile(file, this._configuration["NewsPath"], this._hostingEnvironment, this._configuration);
                                if (result.Message == "UPLOAD_FILE_SUCCESS")
                                {

                                    //Gán FileName, Path, Url
                                    doanhNghiep.FileName = Path.GetFileName(result.Data.ToString());
                                    doanhNghiep.Path = this._configuration["PathFile"] + result.Data.ToString();
                                    doanhNghiep.Url = this._configuration["UrlSaveFile"] + result.Data.ToString();
                                    //this._configuration["NewsPath"] + "/" + yy + "/" + mn + "/" + dy + "/" + doanhNghiep.FileName;
                                    newsWebsiteAPI.URL = doanhNghiep.Url;
                                    //Nếu UserLogin là admin thì cho sửa datepub
                                    if ((_httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "ADMIN" || _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "FCF Admin Group") && doanhNghiep.DatePub != dateCreate)
                                    {
                                        doanhNghiep.IsBackDate = 1;
                                    }

                                    if (doanhNghiep.DocType == 10 || doanhNghiep.DocType == 11)
                                    {
                                        var cResponse = await _DoanhNghiepCongBoContext.Update(doanhNghiep);
                                        cResponse.Message = this._sharedLocalizer[cResponse.Message];
                                        return Json(cResponse);
                                    }
                                    //Call API CBTT update db CBTT
                                    clsWebsiteRequest = await _httpService.CallAPIWebsite(newsWebsiteAPI, this._configuration["API_CBTT"], LinksRoute.API_UPDATE_NEW);
                                }
                                else
                                {
                                    return Json(result);
                                }
                            }
                            else
                            {
                                return Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["FileIsNotAccepted"].Value });
                            }


                        }
                        else
                        {
                            doanhNghiep.FileName = doanhNghiep.OldFileName;
                            doanhNghiep.Path = doanhNghiep.OldPath;
                            doanhNghiep.Url = doanhNghiep.OldUrl;
                            newsWebsiteAPI.URL = doanhNghiep.Url;
                            //Nếu UserLogin là admin thì cho sửa datepub
                            if ((_httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "ADMIN" 
                                || _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "FCF Admin Group") 
                                && doanhNghiep.DatePub != dateCreate)
                            {
                                doanhNghiep.IsBackDate = 1;
                            }
                            if (doanhNghiep.DocType == 10 || doanhNghiep.DocType == 11)
                            {
                                var cResponse = await _DoanhNghiepCongBoContext.Update(doanhNghiep);
                                cResponse.Message = this._sharedLocalizer[cResponse.Message];
                                return Json(cResponse);
                            }

                            //Call API CBTT update db CBTT
                            clsWebsiteRequest = await _httpService.CallAPIWebsite(newsWebsiteAPI, this._configuration["API_CBTT"], LinksRoute.API_UPDATE_NEW);
                        }



                        if (clsWebsiteRequest.Code == 0 && clsWebsiteRequest.Data.NewsID != -1)
                        {
                            var cResponse = await _DoanhNghiepCongBoContext.Update(doanhNghiep);

                            //Nếu updte news trong EzIR lỗi thì update dữ liệu cũ vào db CBTT
                            if (cResponse.Code != 0)
                            {
                                var oldNewsData = await _DoanhNghiepCongBoContext.Select(new DoanhNghiepCongBoViewModel { NewID = doanhNghiep.NewID });
                                var listOldData = oldNewsData.FirstOrDefault();

                                //gán dữ liệu cũ vào newsWebsiteAPI
                                newsWebsiteAPI.DatePub = DateTime.Parse(listOldData.DatePublic.ToString()).ToString("dd/MM/yyyy");
                                newsWebsiteAPI.DocTypeID = listOldData.DocType ?? default(int);
                                newsWebsiteAPI.Title = listOldData.Title;
                                newsWebsiteAPI.Language = "VN";
                                newsWebsiteAPI.ReportYear = listOldData.Year.ToString();
                                newsWebsiteAPI.URL = listOldData.Url;
                                newsWebsiteAPI.NewsID = listOldData.SID ?? default(int);
                                newsWebsiteAPI.UserName = doanhNghiep.UserLogin;
                                newsWebsiteAPI.Stock_Code = listOldData.StockCode;

                                //Call API CBTT delete db CBTT
                                var cResponseMessageDelete = await _httpService.CallAPIWebsite(newsWebsiteAPI, this._configuration["API_CBTT"], LinksRoute.API_UPDATE_NEW);
                            }
                            cResponse.Message = this._sharedLocalizer[cResponse.Message];
                            return Json(cResponse);
                        }
                        else
                        {
                            return Json(new CResponseMessage { Code = -1, Message = "Lỗi khi update tin mới!" });
                        }
                    }
                    else
                    {
                        return Json(new CResponseMessage { Code = -1, Message = " Tài khoản đang dùng không có quyền sửa thông tin này!" });
                    }




                }

                var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
                return Json(response);
            }
            catch (Exception)
            {
                return Json(new CResponseMessage { Code = -1, Message = "Lỗi hệ thống" });
            }

        }

        [HttpPost]
        [Route(LinksRoute.DeleteCongBoDoanhNghiep)]
        [EzAuthorize(RoleCode.QL_DOANH_NGHIEP_CONG_BO, view: true, edit: true)]
        public async Task<IActionResult> DeleteNews(DoanhNghiepCongBoViewModel doanhNghiep)
        {
            doanhNghiep.UserLogin = _username;
            doanhNghiep.RoleCode = _roleCode;

            NewsWebsiteAPI newsWebsiteAPI = new NewsWebsiteAPI()
            {
                NewsID = doanhNghiep.Sid ?? default(int),
                UserName = doanhNghiep.UserLogin,
            };

            if (!string.IsNullOrEmpty(doanhNghiep.UserLogin) && !string.IsNullOrEmpty(doanhNghiep.RoleCode))
            {
                //Lấy ttin chuyên viên phụ trách doanh nghiệp
                var dataDoanhNghiep = await _DoanhNghiepContext.Select(new DoanhNghiepViewModel { CompanyID = doanhNghiep.CompanyID });
                string expert = dataDoanhNghiep.ToList().FirstOrDefault().Expert;

                // lấy ttin người đăng tin và loại tài khoản đăng tin
                var data = await _DoanhNghiepCongBoContext.Select(new DoanhNghiepCongBoViewModel { NewID = doanhNghiep.NewID });
                string userPost = data.ToList().FirstOrDefault().UserPost;
                string loaiTK = data.ToList().FirstOrDefault().AccountType;

                //Tài khoản chuyên viên chỉ sửa/ xóa được thông tin do chính TK chuyên viên đó hoặc do TK doanh nghiệp được chuyên viên đó phụ trách công bố (nhưng không sửa được trường “ngày đăng tin”)
                if (expert == doanhNghiep.UserLogin || (userPost == doanhNghiep.UserLogin && loaiTK == "Chuyên viên")
                    || (_httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "ADMIN"
                        || _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "FCF Admin Group"))
                {
                    if (data.FirstOrDefault().DocType == 10 || data.FirstOrDefault().DocType == 11)
                    {
                        var cResponse = await _DoanhNghiepCongBoContext.Delete(doanhNghiep);
                        cResponse.Message = this._sharedLocalizer[cResponse.Message];
                        return Json(cResponse);
                    }

                    //Call API CBTT delete db CBTT
                    var cResponseMessageDelete = await _httpService.CallAPIWebsite(newsWebsiteAPI, this._configuration["API_CBTT"], LinksRoute.API_DELETE_NEW);

                    if (cResponseMessageDelete.Code == 0)
                    {
                        var cResponse = await _DoanhNghiepCongBoContext.Delete(doanhNghiep);
                        cResponse.Message = this._sharedLocalizer[cResponse.Message];
                        return Json(cResponse);
                    }
                }
                else
                {
                    return Json(new CResponseMessage { Code = -1, Message = "Tài khoản đang dùng không có quyền xóa thông tin này." });
                }



            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }


        //[HttpPost]
        //[EzAuthorize(RoleCode.QL_DOANH_NGHIEP_CONG_BO, view: true, edit: true)]
        //public async Task<IActionResult> DownloadFile(DoanhNghiepCongBoViewModel doanhNghiep)
        //{
        //    doanhNghiep.UserLogin = _username;
        //    doanhNghiep.RoleCode = _roleCode;

        //    //string filePath = Path.Combine(this._hostingEnvironment.ContentRootPath, doanhNghiep.Url.Replace(@"/", @"\"));
        //    if (!string.IsNullOrEmpty(doanhNghiep.UserLogin) && !string.IsNullOrEmpty(doanhNghiep.RoleCode))
        //    {
        //        /*if (!System.IO.File.Exists(filePath))
        //        {
        //            return Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["FileIsNotExist"].Value });
        //        }
        //        var data = await _DoanhNghiepCongBoContext.Select(doanhNghiep);
        //        var ms = new MemoryStream(CheckUtils.ReadFile(filePath));*/
        //        //var fileDownload = CheckFile.ReadFile(template.Path);

        //        //string FileExtension = Path.GetFileName(filePath).Substring(Path.GetFileName(filePath).LastIndexOf('.') + 1).ToLower();
        //        string remoteUri = _configuration["UrlFileCommon"];
        //        string Filename = doanhNghiep.Path;
        //        string myStringWebResource = null;

        //        myStringWebResource = remoteUri + Filename;
        //        _appLogger.InfoLogger.LogInfo(myStringWebResource);

        //        var filePath = myStringWebResource;
        //        var myClient = new WebClient();
        //        try
        //        {
        //            byte[] bytes = myClient.DownloadData(myStringWebResource);

        //            var type = Path.GetExtension(myStringWebResource).Replace(".", "");

        //            if (type == "doc" || type == "docx")
        //            {
        //                return File(bytes, "application/msword");
        //            }
        //            if (type == "pdf")
        //            {
        //                return File(bytes, "application/pdf");
        //            }
        //            if (type == "rar")
        //            {
        //                return File(bytes, "application/x-rar-compressed");
        //            }
        //            if (type == "zip")
        //            {
        //                return File(bytes, "application/x-zip-compressed");
        //            }
        //        }
        //        catch(Exception ex)
        //        {
        //            _appLogger.ErrorLogger.LogError(ex);
        //            return this.Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["File không tồn tại"].Value });
        //        }
                
        //    }

        //    var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
        //    return Json(response);
        //}


        [HttpPost]
        [EzAuthorize(RoleCode.QL_DOANH_NGHIEP_CONG_BO, view: true)]
        public async Task<IActionResult> ExportExcel(DoanhNghiepCongBoViewModel doanhNghiep, string ListHeader)
        {
            doanhNghiep.UserLogin = _username;
            doanhNghiep.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(doanhNghiep.UserLogin) && !string.IsNullOrEmpty(doanhNghiep.RoleCode))
            {
                ListHeader = ListHeader.Substring(1, ListHeader.Length - 2); ;
                string[] arrHeader = ListHeader.Split(',');
                var listDNCongBo = await _DoanhNghiepCongBoContext.ListDoanhNghiepCongBo(doanhNghiep);

                var fileResultStream = GenerateExcelFileToStream(listDNCongBo, arrHeader);

                return this.File(fileResultStream, "application/vnd.ms-excel");
            }

            var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }

        private MemoryStream GenerateExcelFileToStream(DataTable DT, string[] arrHeader)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet("listDNCongBo");
            IRow row = excelSheet.CreateRow(0);
            for (int i = 0; i < DT.Columns.Count; i++)
            {
                DT.Columns[i].ColumnName = DT.Columns[i].ColumnName.Replace(DT.Columns[i].ColumnName, arrHeader[i]);
                row.CreateCell(i).SetCellValue(DT.Columns[i].ColumnName);
            }

            for (int j = 0; j < DT.Rows.Count; j++)
            {
                int k = j + 1;
                row = excelSheet.CreateRow(k);
                for (int i = 0; i < DT.Columns.Count; i++)
                {
                    row.CreateCell(i).SetCellValue(DT.Rows[j][DT.Columns[i].ColumnName].ToString());
                }
            }

            MemoryStream tempStream = new MemoryStream();
            workbook.Write(tempStream);
            // Sau khi ghi vào thì stream bị đóng bởi thư viện nên phải chuyển sang stream khác
            var memoryStream = new MemoryStream();
            var bytes = tempStream.ToArray();
            memoryStream.Write(tempStream.ToArray(), 0, bytes.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
    }
}
