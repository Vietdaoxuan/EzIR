using CommonLib.Interfaces.Logger;
using CoreLib.Commons;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Interfaces;
using CoreLib.Services;
using CoreLib.SharedKernel;
using EzIRCustomerAPI.Services;
using EzIRSpecialist.Authentication;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models;
using EzIRSpecialist.Models.QuanLyTaiKhoan;
using EzIRSpecialist.Models.ViewModel;
using EzIRSpecialist.Models.ViewModel.QLTKViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EzIRSpecialist.Controllers.QuanLyTaiKhoan
{
    public class QLTKDoanhNghiepController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IQLTKDoanhNghiepContext _qltkDoanhNghiepContext;
        private readonly ICommonTypeContext _commonTypeContext;
        private readonly ICompanyContext _companyContext;
        private readonly IChuyenVienContext _chuyenVienContext;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailSender _emailSender;
        private readonly IErrorHandler _errorHandler;

        private string _username { get; set; }
        private string _roleCode { get; set; }

        public QLTKDoanhNghiepController(IConfiguration configuration, IQLTKDoanhNghiepContext qltkDoanhNghiepContext, ICommonTypeContext commonTypeContext, ICompanyContext companyContext, IChuyenVienContext chuyenVienContext, IAppLogger appLogger, IMemoryCache cache, IStringLocalizer<SharedResource> sharedLocalizer, IHttpContextAccessor httpContextAccessor, IEmailSender emailSender, IErrorHandler errorHandler)
        {
            _configuration = configuration;
            _qltkDoanhNghiepContext = qltkDoanhNghiepContext;
            _commonTypeContext = commonTypeContext;
            _companyContext = companyContext;
            _chuyenVienContext = chuyenVienContext;
            _appLogger = appLogger;
            _cache = cache;
            _sharedLocalizer = sharedLocalizer;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
            _errorHandler = errorHandler;
            _username = _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.USERNAME);
            _roleCode = RoleCode.ACCOUNT_DOANH_NGHIEP;
        }

        [EzAuthorize(RoleCode.ACCOUNT_DOANH_NGHIEP, view: true)]
        public async Task<IActionResult> Index()
        {
            List<CommonType> listCompanyType = new List<CommonType>();
            List<CommonType> listStatus = new List<CommonType>();
            CompanyViewModel companyviewmodel = new CompanyViewModel();

            var commonType = new CommonTypeViewModel {
                ListCategory = "2,3"
            };

            var listCommontype = await _commonTypeContext.Select(commonType);
            var listMCK = await _companyContext.Select(companyviewmodel);
            var listMCKEzsearch = _companyContext.ListCompanyEzSearchTest();
            
            foreach (var obj in listCommontype.ToList())
            {
                if (obj.Category == ConstCommonTypes.NHOM_TRANGTHAI)
                {
                    listStatus.Add(obj);
                }
                if (obj.Category == ConstCommonTypes.NHOM_LOAI_HINH)
                {
                    listCompanyType.Add(obj);
                }
            }

            List<CompanyEzSearchTemp> ListCom = new List<CompanyEzSearchTemp>();
            ListCom = listMCKEzsearch.ToList();

            //foreach ( var obj in listMCK.ToList())
            //    ListCom.RemoveAll(x => x.ACpnyID == obj.CompanyID);   

            var viewmodel = new QLTKDoanhNghiepViewModel
            {
                listStatus = listStatus,
                listCompanyType = listCompanyType,
                listMCK = listMCK.ToList(),
                listMCKEzSearch = ListCom
            };
            return View(viewmodel);
        }

        [HttpGet]
        [Route(LinksRoute.GetMCKTKDoanhNghiep)]
        [EzAuthorize(RoleCode.ACCOUNT_DOANH_NGHIEP, view: true)]
        public IActionResult ListMCKDoanhNghiep(QLTKDoanhNghiepViewModel doanhNghiep)
        {
            doanhNghiep.UserLogin = _username;
            doanhNghiep.RoleCode = _roleCode;
            if (!string.IsNullOrEmpty(doanhNghiep.UserLogin) && !string.IsNullOrEmpty(doanhNghiep.RoleCode))
            {
                var data = _companyContext.ListCompanyEzSearchTest();
                //var listMCK = data.Where(x => x.IsExist == 1);
                //foreach (var name in listMCK)
                //{
                //    string[] arrName = name.NameVN.Split("-");
                //    foreach (var i in arrName)
                //    {
                //        if (name.AStockCode != i)
                //        {
                //            name.NameVN = name.AStockCode + " - " + i;
                //        }
                //    }
                //}
                return Json(data.ToList());
            }
            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }

        [HttpGet]
        [Route(LinksRoute.GetTKDoanhNghiep)]
        [EzAuthorize(RoleCode.ACCOUNT_DOANH_NGHIEP, view: true)]
        public async Task<IActionResult> ListDoanhNghiep(QLTKDoanhNghiepViewModel doanhNghiep)
        {
            doanhNghiep.UserLogin = _username;
            doanhNghiep.RoleCode = _roleCode;
            if (!string.IsNullOrEmpty(doanhNghiep.UserLogin) && !string.IsNullOrEmpty(doanhNghiep.RoleCode))
            {
                var data = await _qltkDoanhNghiepContext.Select(doanhNghiep);
                foreach(var item in data)
                {
                    doanhNghiep.StockCode = item.StockCode;
                }
                return Json(data.ToList());
            }
            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }


        [HttpGet]
        [Route(LinksRoute.GetDoanhNghiepUpdate)]
        [EzAuthorize(RoleCode.ACCOUNT_DOANH_NGHIEP, view: true)]
        public async Task<IActionResult> ListDoanhNghiepUpdate(QLTKDoanhNghiepViewModel doanhNghiep)
        {
            doanhNghiep.UserLogin = _username;
            doanhNghiep.RoleCode = _roleCode;

            var data = await _qltkDoanhNghiepContext.Select(doanhNghiep);
            string userCreated = data.ToList().FirstOrDefault().CreateBy;
            if (userCreated == doanhNghiep.UserLogin
                 || (_httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "ADMIN"
                     || _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "FCF Admin Group"))
            {

                return Json(data.ToList());
            }
            else
            {
                return Json(new CResponseMessage { Code = -1, Message = "Tài khoản đang dùng không có quyền sửa thông tin này" });
            }
        }

        [HttpPost]
        [Route(LinksRoute.InsertTKDoanhNghiep)]
        [EzAuthorize(RoleCode.ACCOUNT_DOANH_NGHIEP, view: true, edit: true)]
        public async Task<IActionResult> InsertTKDoanhNghiep(QLTKDoanhNghiepViewModel doanhNghiep)
        {
            doanhNghiep.UserLogin = _username;
            doanhNghiep.RoleCode = _roleCode;
            var password = PasswordGenerator.GetRandomPassword();
            doanhNghiep.Password = PasswordGenerator.EncodePassword(password);
       
            if (doanhNghiep.CompanyID != null)
            {
                // sp cũ thiếu trường NameEN --> sửa lại lấy theo sp SPEZS_IR_COMPANY_LIST
                var listAMCK = _companyContext.ListCompanyEzSearchTemp(doanhNghiep.CompanyID);
                doanhNghiep.StockCode = listAMCK.FirstOrDefault().AStockCode;
                doanhNghiep.CompanyName = listAMCK.FirstOrDefault().NameVN;
                doanhNghiep.CompanyNameEN = listAMCK.FirstOrDefault().NameEN;
            }    
            

            if (!string.IsNullOrEmpty(doanhNghiep.UserLogin) && !string.IsNullOrEmpty(doanhNghiep.RoleCode))
            {
                if (doanhNghiep.Active == null)
                {
                    doanhNghiep.Active = 0;
                }
                var cResponse = await _qltkDoanhNghiepContext.Insert(doanhNghiep);
                cResponse.Message = this._sharedLocalizer[cResponse.Message];
                return Json(cResponse);
            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }

        [HttpPost]
        [Route(LinksRoute.SendMailTKDoanhNghiep)]
        [EzAuthorize(RoleCode.ACCOUNT_DOANH_NGHIEP, view: true, edit: true)]
        public async Task<IActionResult> SendMail(QLTKDoanhNghiepViewModel doanhNghiep)
        {
            doanhNghiep.UserLogin = _username;
            doanhNghiep.RoleCode = _roleCode;
            var password = PasswordGenerator.GetRandomPassword();
            doanhNghiep.Password = PasswordGenerator.EncodePassword(password);
            var allData = await _qltkDoanhNghiepContext.Select(doanhNghiep);
            var oldData = allData.Where(x => x.CompanyID == doanhNghiep.CompanyID).FirstOrDefault();
            doanhNghiep.CompanyType = oldData.CompanyType;
            doanhNghiep.FullName = oldData.FullName;
            doanhNghiep.Email = oldData.Email;
            doanhNghiep.Phone = oldData.Phone;
            doanhNghiep.Note = oldData.Note;
            doanhNghiep.Active = oldData.Active;
            if (!string.IsNullOrEmpty(doanhNghiep.UserLogin) && !string.IsNullOrEmpty(doanhNghiep.RoleCode))
            {
                
                var cResponse = await _qltkDoanhNghiepContext.Update(doanhNghiep);
                //cResponse.Message = this._sharedLocalizer[cResponse.Message];
                if (cResponse.Code == 0 && doanhNghiep.Email != null)
                {
                    try
                    {
                        var dataDoanhNghiep = await _qltkDoanhNghiepContext.Select(new QLTKDoanhNghiepViewModel { CompanyID = doanhNghiep.CompanyID });
                        var dataChuyenVien = await _chuyenVienContext.Select(new ChuyenVienViewModel { Username = _username });
                        var emailContent = await System.IO.File.ReadAllTextAsync(this._configuration["ResetPasswordCompany"]);

                        emailContent = emailContent.Replace("<!UserName>", dataDoanhNghiep.FirstOrDefault().Username);
                        emailContent = emailContent.Replace("<!NewPassword>", password);
                        emailContent = emailContent.Replace("<!LinkReset1>", this._configuration["LinkResetPassword"]);
                        emailContent = emailContent.Replace("<!LinkReset2>", this._configuration["LinkResetPassword"]);
                        if (!string.IsNullOrEmpty(dataDoanhNghiep.FirstOrDefault().Email) && !string.IsNullOrEmpty(dataChuyenVien.FirstOrDefault().EmailCc))
                        {
                            var sendEmailResult = await this._emailSender.SendEmailAsync(dataDoanhNghiep.FirstOrDefault().Email,
                                                                                    dataChuyenVien.FirstOrDefault().EmailCc,
                                                                                    "Thông tin tài khoản doanh nghiệp",
                                                                                    emailContent,
                                                                                    this._configuration["EmailSettings:Sender"],
                                                                                    this._configuration["EmailSettings:MailServer"],
                                                                                    Convert.ToInt32(this._configuration["EmailSettings:MailPort"]),
                                                                                    this._configuration["EmailSettings:Username"],
                                                                                    this._configuration["EmailSettings:Password"]
                                                                                    );
                            return Json(new CResponseMessage { Code = 0, Message = _sharedLocalizer[sendEmailResult.Message].Value});
                        }
                        else
                        {
                            return Json(new CResponseMessage { Code = 0, Message = "Lỗi không thể gửi email" });
                        }

                    }
                    catch (Exception e)
                    {

                        this._errorHandler.WriteToFile(e);
                        return Json(new CResponseMessage(-1, "Lỗi không thể gửi email"));
                    }
                }
                else
                {
                    cResponse.Message = this._sharedLocalizer[cResponse.Message];
                    return Json(cResponse);
                }

            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }

        [HttpPost]
        [Route(LinksRoute.UpdateTKDoanhNghiep)]
        [EzAuthorize(RoleCode.ACCOUNT_DOANH_NGHIEP, view: true, edit: true)]
        public async Task<IActionResult> UpdateTKDoanhNghiep(QLTKDoanhNghiepViewModel doanhNghiep)
        {
            doanhNghiep.UserLogin = _username;
            doanhNghiep.RoleCode = _roleCode;

            var data = await _qltkDoanhNghiepContext.Select(new QLTKDoanhNghiepViewModel { CompanyID = doanhNghiep.CompanyID });
            string userCreated = data.ToList().FirstOrDefault().CreateBy;
            if (userCreated == doanhNghiep.UserLogin
                || (_httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "ADMIN"
                    || _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "FCF Admin Group"))
            {
                if (doanhNghiep.Active == null)
                {
                    doanhNghiep.Active = 0;
                }
                var cResponse = await _qltkDoanhNghiepContext.Update(doanhNghiep);
                cResponse.Message = this._sharedLocalizer[cResponse.Message];
                return Json(cResponse);
            }

            var response = new CResponseMessage { Code = -1, Message = "Tài khoản đang dùng không có quyền sửa thông tin này" };
            return Json(response);
        }

        [HttpPost]
        [Route(LinksRoute.DeleteTKDoanhNghiep)]
        [EzAuthorize(RoleCode.ACCOUNT_DOANH_NGHIEP, view: true, delete: true)]
        public async Task<IActionResult> DeleteTKDoanhNghiep(QLTKDoanhNghiepViewModel doanhNghiep)
        {
            doanhNghiep.UserLogin = _username;
            doanhNghiep.RoleCode = _roleCode;

            var data = await _qltkDoanhNghiepContext.Select(new QLTKDoanhNghiepViewModel { CompanyID = doanhNghiep.CompanyID });
            string userCreated = data.ToList().FirstOrDefault().CreateBy;
            if (userCreated == doanhNghiep.UserLogin
                || (_httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "ADMIN"
                    || _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "FCF Admin Group"))
            {
                var cResponse = await _qltkDoanhNghiepContext.Delete(doanhNghiep);
                cResponse.Message = this._sharedLocalizer[cResponse.Message];
                return Json(cResponse);
            }
            else
            {
                return Json(new CResponseMessage { Code = -1, Message = "Tài khoản đang dùng không có quyền xóa thông tin này." });
            }
        }

    }
}
