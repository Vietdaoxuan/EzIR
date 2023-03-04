using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Commons;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Interfaces;
using CoreLib.SharedKernel;
using EzIRCustomerAPI.Services;
using EzIRSpecialist.Authentication;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models;
using EzIRSpecialist.Models.BaoCaoTienIch;
using EzIRSpecialist.Models.ModelView.QLTKModelView;
using EzIRSpecialist.Models.QuanLyTaiKhoan;
using EzIRSpecialist.Models.ViewModel;
using EzIRSpecialist.Models.ViewModel.QLTKViewModel;
using EzIRSpecialist.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzIRSpecialist.Controllers.BaoCaoTienIch
{
    public class NhacCongBoThongTinController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly INhacCongBoThongTinContext _nhacCongBoThongTinContext;
        private readonly IChuyenVienContext _chuyenVienContext;

        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly ICommon _common;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommonTypeContext _commonTypeContext;
        private readonly ICompanyContext _companyContext;
        private readonly IDoanhNghiepContext _doanhNghiepContext;
        private readonly IEmailSender _emailSender;
        private readonly IErrorHandler _errorHandler;
        private string _username { get; set; }
        private string _roleCode { get; set; }

        public IConfiguration Configuration { get; }

        private string _templateURL;


        public NhacCongBoThongTinController(IErrorHandler errorHandler, IEmailSender emailSender, IDoanhNghiepContext doanhNghiepContext, ICompanyContext companyContext, ICommonTypeContext commonTypeContext, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, ICommon common, IStringLocalizer<SharedResource> sharedLocalizer, IConfiguration configuration, INhacCongBoThongTinContext nhacCongBoThongTinContext, IChuyenVienContext chuyenVienContext, IAppLogger appLogger, IMemoryCache cache)
        {
            Configuration = configuration;
            ConfigProvider.Configuration = configuration;
            
            _configuration = configuration;
            _nhacCongBoThongTinContext = nhacCongBoThongTinContext;
            _chuyenVienContext = chuyenVienContext;
            _appLogger = appLogger;
            _cache = cache;
            _sharedLocalizer = sharedLocalizer;
            _common = common;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _commonTypeContext = commonTypeContext;
            _companyContext = companyContext;
            _doanhNghiepContext = doanhNghiepContext;
            _emailSender = emailSender;
            _errorHandler = errorHandler;
            _templateURL = _configuration.GetSection("FileSendMailTemplateUrl").Value;

            _username = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.USERNAME);
            _roleCode = RoleCode.NHAC_CBTT;
        }

        [EzAuthorize(RoleCode.NHAC_CBTT, view: true)]
        public async Task<IActionResult> Index()
        {
            ViewBag.TemplateUrl = _templateURL;
            //List Loại Doanh Nghiệp

            List<CommonType> listLoaiDN = new List<CommonType>();
            List<CommonType> listNienDoBCTCN = new List<CommonType>();
            List<CommonType> listDoiTuongAD = new List<CommonType>();

            var commonType = new CommonTypeViewModel
            {
                ListCategory = "3,4,11,16,18"
            };

            var listCommontype = await _commonTypeContext.Select(commonType);

            foreach (var obj in listCommontype.ToList())
            {
                if (obj.Category == ConstCommonTypes.NHOM_LOAI_HINH)
                {
                    listLoaiDN.Add(obj);
                }
                if (obj.Category == ConstCommonTypes.NIEN_DO_BCTC)
                {
                    listNienDoBCTCN.Add(obj);
                }
                if (obj.Category == ConstCommonTypes.NHOM_DOI_TUONG)
                {
                    listDoiTuongAD.Add(obj);
                }
            }

            //list chuyên viên                        
            var listCV = await _chuyenVienContext.Select(new ChuyenVienViewModel() { });
            
            var viewmodel = new NhacCongBoThongTinViewModel
            {
                ListLoaiDN = listLoaiDN,                
                ListChuyenVien = listCV.ToList(),
                ListNienDoBCTC = listNienDoBCTCN.OrderBy(a => a.Ord),
                ListDoiTuongAD = listDoiTuongAD
            };

            return View(viewmodel);
        }

        [EzAuthorize(RoleCode.QL_RULES, view: true)]
        public async Task<IActionResult> GetCommonType()
        {
            try
            {
                var commonType = new CommonTypeViewModel
                {
                    //ListCategory = "3,4,101,102,103,104,105,106,107,108,109,110,111"

                    ListCategory = "3,4,11,16,18"
                };

                var listCommontype = await _commonTypeContext.Select(commonType);

                var viewmodel = new NhacCongBoThongTinViewModel
                {
                    ListLoaiHinhDN = listCommontype.Where(a => a.Category == ConstCommonTypes.NHOM_LOAI_HINH).OrderBy(a => a.Ord),
                    ListNienDoBCTC = listCommontype.Where(a => a.Category == ConstCommonTypes.NIEN_DO_BCTC).OrderBy(a => a.Ord),
                    ListDoiTuongAD = listCommontype.Where(a => a.Category == ConstCommonTypes.NHOM_DOI_TUONG).OrderBy(a => a.Ord)

                };

                return Json(viewmodel);
            }
            catch (Exception)
            {
                return Json(new RulesViewModel { ListLoaiTaiLieu = null, ListLoaiHinhDN = null, ListNienDoBCTC = null , ListDoiTuongAD = null});
            }

        }


        [HttpGet]
        [EzAuthorize(RoleCode.NHAC_CBTT, view: true)]
        public async Task<IActionResult> GetMCK(NhacCongBoThongTinViewModel nhacCongBoThongTinViewModel)
        {
            nhacCongBoThongTinViewModel.UserName = _username;
            nhacCongBoThongTinViewModel.RoleCode = _roleCode;

            List<NhacCongBoThongTin> listchangecpnytype = new List<NhacCongBoThongTin>();

            var listcpny = await _nhacCongBoThongTinContext.GetTypeCompany(nhacCongBoThongTinViewModel);

            foreach (var obj in listcpny.ToList())
            {
                    listchangecpnytype.Add(obj);
            }

            return Json(listchangecpnytype);
        }

        [HttpGet]
        [EzAuthorize(RoleCode.NHAC_CBTT, view: true)]
        public async Task<IActionResult> RemindCBTTSearch(NhacCongBoThongTinViewModel nhacCongBoThongTinViewModel)
        {

            nhacCongBoThongTinViewModel.UserName = _username;
            nhacCongBoThongTinViewModel.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(nhacCongBoThongTinViewModel.UserName) && !string.IsNullOrEmpty(nhacCongBoThongTinViewModel.RoleCode))
            {
                if (!string.IsNullOrEmpty(nhacCongBoThongTinViewModel.AObject))
                {
                    nhacCongBoThongTinViewModel.AObject = nhacCongBoThongTinViewModel.AObject.Remove(nhacCongBoThongTinViewModel.AObject.Length - 1);
                }
                var data = await _nhacCongBoThongTinContext.Select(nhacCongBoThongTinViewModel);

                return Json(data.ToList());
            }

            var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }

        [HttpPost]
        [EzAuthorize(RoleCode.NHAC_CBTT, view: true)]
        public async Task<IActionResult> SendEmail(string stockcode, List<int> listcbtt, string subject, string loaidn, IFormFile emailFile, string loaidnID)
        {
            subject = "Nhắc công bố thông tin";
            if (emailFile == null)
            {
                return this.Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["SENDMAIL_MSG_NO_FILE_CHOSEN"].Value });

            }
            var acceptFormats = new[] { ".html", ".htm", ".xhtml" };

            var file = string.Empty;

            try
            {
                var isValidFormat = false;

                foreach (var acceptFormat in acceptFormats)
                {
                    isValidFormat = emailFile.FileName.EndsWith(acceptFormat) || isValidFormat;
                }

                if (!isValidFormat)
                {
                    return this.Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["SendMail_MSG_FILE_KHONG_DUNG_DINH_DANG"].Value });
                }

                file = await emailFile.ReadAsStringAsync();
            }
            catch (Exception)
            {

                return this.Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["SendMail_MSG_KHONG_THE_DOC_FILE"].Value });
            }


            //SendMail

            var nhacCBTTviewmodel = new NhacCongBoThongTinViewModel
            {

                TypeID = loaidnID,
                AStockCode = stockcode,
                ALoaiDoanhNghiep = null,
                UserName = _username,
                RoleCode = _roleCode
            };

            if (!string.IsNullOrEmpty(nhacCBTTviewmodel.UserName) && !string.IsNullOrEmpty(nhacCBTTviewmodel.RoleCode))
            {
                try
                {

                    var companyinfo = await _nhacCongBoThongTinContext.Select(nhacCBTTviewmodel);

                    int k = 1;

                    string table = string.Empty;

                    for (int i = 0; i < listcbtt.Count; i++)
                    {
                        string table_mau = "<tr><td>{!stt}</td><td>{!loaitin}</td><td>{!thoidiemcongbo}</td><td>{!endtime}</td><td>{!bieumau}</td><td>{!ccphaply}</td></tr>";

                        var  listCBTT = listcbtt[i];

                        var data = _nhacCongBoThongTinContext.ListCompanyID(listCBTT,nhacCBTTviewmodel);

                        table_mau = table_mau.Replace("{!stt}", k.ToString());
                        table_mau = table_mau.Replace("{!loaitin}", data.FirstOrDefault().LoaiTin);
                        table_mau = table_mau.Replace("{!thoidiemcongbo}", data.FirstOrDefault().AqdCBTT);
                        table_mau = table_mau.Replace("{!endtime}", data.FirstOrDefault().AEndTime.ToString().Replace("12:00:00 SA", ""));
                        table_mau = table_mau.Replace("{!bieumau}", data.FirstOrDefault().ATitle);
                        table_mau = table_mau.Replace("{!ccphaply}", data.FirstOrDefault().accplcbtt);
                        k++;
                        table += table_mau;
                    }

                    file = file.Replace("{!noidungbang}", table);

                    var datetime = DateTime.Now;
                    var day = datetime.Day.ToString();
                    var month = datetime.Month.ToString();
                    var year = datetime.Year.ToString();
                    file = file.Replace("{!day}", day);
                    file = file.Replace("{!month}", month);
                    file = file.Replace("{!year}", year);
                    file = file.Replace("{!tensan}", loaidn);
                    file = file.Replace("{!tencongty}", stockcode);

                    string width = "40%";
                    string logopath = "<img alt=\"\" src = \"" + this._configuration["logourl"] + "\" style=\"margin-top:10px;width:" + width + "\" />";
                    file = file.Replace("{!logoFPTS}", logopath);

                    
                    HtmlToPdf converter = new HtmlToPdf();

                    PdfDocument pdfDocument = new PdfDocument();

                    converter.Options.MarginBottom = 20;
                    converter.Options.DisplayHeader = true;
                    converter.Header.Height = 30;
                    converter.Header.DisplayOnFirstPage = false;

                    PdfDocument doc = converter.ConvertHtmlString(file);

                    pdfDocument.Append(doc);

                    MemoryStream stream = new MemoryStream();

                    pdfDocument.Save(stream);
 
                    var sendmailResult = await this._emailSender.SendEmailAttachAsync(                                                                              
                                                                                  companyinfo.FirstOrDefault().AEmail,
                                                                                  companyinfo.FirstOrDefault().Acc,
                                                                                  subject,
                                                                                  file,
                                                                                  this._configuration["EmailSettings:Sender"],
                                                                                  this._configuration["EmailSettings:MailServer"],
                                                                                  Convert.ToInt32(this._configuration["EmailSettings:MailPort"]),
                                                                                  this._configuration["EmailSettings:Username"],
                                                                                  this._configuration["EmailSettings:Password"],
                                                                                  stream.ToArray());

                    return this.Json(new CResponseMessage { Code = 0, Message = _sharedLocalizer["SendMail_MSG_SUCCESS"].Value });
                }
                catch (Exception ex)
                {

                    return this.Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["SendMail_MSG_KHONG_THE_GUI_EMAIL"].Value });
                }


            }

            var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);


        }


        [HttpPost]
        [EzAuthorize(RoleCode.NHAC_CBTT, view: true)]
        public async Task<IActionResult> SendEmailAll(List<string> liststockcode, List<int> listcbtt, string subject, string loaidn, IFormFile emailFile, string loaidnID)
        {
            subject = "Nhắc công bố thông tin";
            if (emailFile == null)
            {
                return this.Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["SENDMAIL_MSG_NO_FILE_CHOSEN"].Value });

            }
            var acceptFormats = new[] { ".html", ".htm", ".xhtml" };

            var file = string.Empty;

            try
            {
                var isValidFormat = false;

                foreach (var acceptFormat in acceptFormats)
                {
                    isValidFormat = emailFile.FileName.EndsWith(acceptFormat) || isValidFormat;
                }

                if (!isValidFormat)
                {
                    return this.Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["SendMail_MSG_FILE_KHONG_DUNG_DINH_DANG"].Value });
                }

                
            }
            catch (Exception)
            {

                return this.Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["SendMail_MSG_KHONG_THE_DOC_FILE"].Value });
            }


            //SendMail

            
            if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_roleCode))
            {
                try
                {
                    foreach (var stockCode in liststockcode)
                    {
                        file = await emailFile.ReadAsStringAsync();

                        var nhacCBTTviewmodel = new NhacCongBoThongTinViewModel
                        {
                            TypeID = loaidnID,
                            AStockCode = stockCode,
                            ALoaiDoanhNghiep = null,
                            UserName = _username,
                            RoleCode = _roleCode
                        };


                        var companyinfo = await _nhacCongBoThongTinContext.Select(nhacCBTTviewmodel);

                        int k = 1;

                        string table = string.Empty;

                        //List<NhacCongBoThongTin> listrule = new List<NhacCongBoThongTin>();
                        //foreach(var ruleitem in companyinfo)
                        //{
                        //    listrule.Add(ruleitem);
                        //}


                        for (int i = 0; i < listcbtt.Count; i++)
                        {
                            string table_mau = "<tr><td>{!stt}</td><td>{!loaitin}</td><td>{!thoidiemcongbo}</td><td>{!endtime}</td><td>{!bieumau}</td><td>{!ccphaply}</td></tr>";

                            var listCBTT = listcbtt[i];

                            var data = _nhacCongBoThongTinContext.ListCompanyID(listCBTT, nhacCBTTviewmodel);

                            table_mau = table_mau.Replace("{!stt}", k.ToString());
                            table_mau = table_mau.Replace("{!loaitin}", data.FirstOrDefault().LoaiTin);
                            table_mau = table_mau.Replace("{!thoidiemcongbo}", data.FirstOrDefault().AqdCBTT);
                            table_mau = table_mau.Replace("{!endtime}", data.FirstOrDefault().AEndTime.ToString().Replace("12:00:00 SA", ""));
                            table_mau = table_mau.Replace("{!bieumau}", data.FirstOrDefault().ATitle);
                            table_mau = table_mau.Replace("{!ccphaply}", data.FirstOrDefault().accplcbtt);
                            k++;
                            table += table_mau;
                        }

                        file = file.Replace("{!noidungbang}", table);

                        var datetime = DateTime.Now;
                        var day = datetime.Day.ToString();
                        var month = datetime.Month.ToString();
                        var year = datetime.Year.ToString();
                        file = file.Replace("{!day}", day);
                        file = file.Replace("{!month}", month);
                        file = file.Replace("{!year}", year);
                        file = file.Replace("{!tensan}", loaidn);
                        file = file.Replace("{!tencongty}", stockCode);

                        string width = "40%";
                        string logopath = "<img alt=\"\" src = \"" + this._configuration["logourl"] + "\" style=\"margin-top:10px;width:" + width + "\" />";
                        file = file.Replace("{!logoFPTS}", logopath);


                        HtmlToPdf converter = new HtmlToPdf();

                        PdfDocument pdfDocument = new PdfDocument();

                        converter.Options.MarginBottom = 20;
                        converter.Options.DisplayHeader = true;
                        converter.Header.Height = 30;
                        converter.Header.DisplayOnFirstPage = false;

                        PdfDocument doc = converter.ConvertHtmlString(file);

                        pdfDocument.Append(doc);

                        MemoryStream stream = new MemoryStream();

                        pdfDocument.Save(stream);

                        var sendmailResult = await this._emailSender.SendEmailAttachAsync(
                                                                                        companyinfo.FirstOrDefault().AEmail,
                                                                                        companyinfo.FirstOrDefault().Acc,
                                                                                        subject,
                                                                                        file,
                                                                                        this._configuration["EmailSettings:Sender"],
                                                                                        this._configuration["EmailSettings:MailServer"],
                                                                                        Convert.ToInt32(this._configuration["EmailSettings:MailPort"]),
                                                                                        this._configuration["EmailSettings:Username"],
                                                                                        this._configuration["EmailSettings:Password"],
                                                                                        stream.ToArray());



                    }

                    return this.Json(new CResponseMessage { Code = 0, Message = _sharedLocalizer["SendMail_MSG_SUCCESS"].Value });
                }
                catch (Exception ex)
                {

                    return this.Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["SendMail_MSG_KHONG_THE_GUI_EMAIL"].Value });
                }


            }

            var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);


        }



        [HttpPost]
        [EzAuthorize(RoleCode.NHAC_CBTT, view: true)]
        public async Task<IActionResult> Print(string stockcode, List<int> listcbtt, IFormFile emailFile, string loaidn)
         {
            if (emailFile == null)
            {
                return this.Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["SENDMAIL_MSG_NO_FILE_CHOSEN"].Value });

            }

            var acceptFormats = new[] { ".html", ".htm", ".xhtml" };

            var file = string.Empty;

            try
            {
                var isValidFormat = false;

                foreach (var acceptFormat in acceptFormats)
                {
                    isValidFormat = emailFile.FileName.EndsWith(acceptFormat) || isValidFormat;
                }

                if (!isValidFormat)
                {
                    return this.Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["SendMail_MSG_FILE_KHONG_DUNG_DINH_DANG"].Value });
                }

                file = await emailFile.ReadAsStringAsync();
            }
            catch (Exception)
            {

                return this.Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["SendMail_MSG_KHONG_THE_DOC_FILE"].Value });
            }

            var nhacCBTTviewmodel = new NhacCongBoThongTinViewModel
            {
                AStockCode = stockcode,
                ALoaiDoanhNghiep = null,
                UserName = _username,
                RoleCode = _roleCode
            };


            if (!string.IsNullOrEmpty(nhacCBTTviewmodel.UserName) && !string.IsNullOrEmpty(nhacCBTTviewmodel.RoleCode))
            {
                try
                {

                    int k = 1;

                    string table = string.Empty;
                    
                    for (int i = 0; i < listcbtt.Count; i++) 
                    {
                        string table_mau = "<tr><td>{!stt}</td><td>{!loaitin}</td><td>{!thoidiemcongbo}</td><td>{!endtime}</td><td>{!bieumau}</td><td>{!ccphaply}</td></tr>";
                        
                        var listCBTT = listcbtt[i];
                        var data = _nhacCongBoThongTinContext.ListCompanyID(listCBTT,nhacCBTTviewmodel);

                        table_mau = table_mau.Replace("{!stt}", k.ToString());
                        table_mau = table_mau.Replace("{!loaitin}", data.FirstOrDefault().LoaiTin);
                        table_mau = table_mau.Replace("{!thoidiemcongbo}", data.FirstOrDefault().AqdCBTT);
                        table_mau = table_mau.Replace("{!endtime}", data.FirstOrDefault().AEndTime.ToString().Replace("12:00:00 SA",""));
                        table_mau = table_mau.Replace("{!bieumau}", data.FirstOrDefault().ATitle);
                        table_mau = table_mau.Replace("{!ccphaply}", data.FirstOrDefault().accplcbtt);
                        k++;
                        table += table_mau;
                    }

                    file = file.Replace("{!noidungbang}", table);

                    var datetime = DateTime.Now;
                    var day = datetime.Day.ToString();
                    var month = datetime.Month.ToString();
                    var year = datetime.Year.ToString();

                    file = file.Replace("{!day}", day);
                    file = file.Replace("{!month}", month);
                    file = file.Replace("{!year}", year);
                    file = file.Replace("{!tensan}", loaidn);
                    file = file.Replace("{!tencongty}", stockcode);
                    string width = "80%";
                    string logopath = "<img alt=\"\" src = \"" + this._configuration["logourl"] + "\" style=\"margin-top:10px;width:" + width + "\" />";
                    file = file.Replace("{!logoFPTS}", logopath);
                    HtmlToPdf converter = new HtmlToPdf();
                    PdfDocument pdfDocument = new PdfDocument();
                    
                    converter.Options.MarginBottom = 20;
                    converter.Options.DisplayHeader = true;
                    converter.Header.Height = 30;
                    converter.Header.DisplayOnFirstPage = false;

                    PdfDocument doc = converter.ConvertHtmlString(file);

                    pdfDocument.Append(doc);
                    MemoryStream stream = new MemoryStream();
                    pdfDocument.Save(stream);
                    return File(stream.ToArray(), "application/pdf");


                }
                catch (Exception ex)
                {
                    return this.Json(new CResponseMessage { Code = -1, Message = _sharedLocalizer["SendMail_MSG_IN_MAU_NHAC_CBTT_ERROR"].Value });
                }
            }
            var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);

        }

        //public async Task<IActionResult> GetCpnyType(int value)
        //{
        //    List<NhacCongBoThongTin> listchangecpnytype = new List<NhacCongBoThongTin>();

        //    var listcpny = await _nhacCongBoThongTinContext.GetTypeCompany(value);

        //    foreach (var obj in listcpny.ToList())
        //    {
        //        if(obj.acompanytype == value)
        //        {
        //            listchangecpnytype.Add(obj);

        //        }
               
        //    }
        //    return Json(listchangecpnytype);
        //}


        public async Task<IActionResult> FileImportTemplate()
        {
            try
            {
                var response = await HttpRequestFactory.Get(this._configuration["FileSendMailTemplateUrl"]);

                response.EnsureSuccessStatusCode();

                var fileContent = await response.Content.ReadAsStreamAsync();

                return this.File(fileContent, "application/octet-stream", "templateEmail.html");
            }
            catch
            {
                return this.Json(new CResponseMessage(-1, this._sharedLocalizer["template_MSG_FILE_NOT_FOUND"]));
            }
        }

    }
}
