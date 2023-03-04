using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.Entities;
using EzIRCustomerAPI.Services;
using EzIRSpecialist.Authentication;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models;
using EzIRSpecialist.Models.ModelView.QLTKModelView;
using EzIRSpecialist.Models.ViewModel;
using EzIRSpecialist.Models.ViewModel.QLTKViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Controllers.QuanLyTaiKhoan
{
    public class QuanLyDoanhNghiepController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IChuyenVienContext _chuyenVienContext;
        private readonly IDoanhNghiepContext _DoanhNghiepContext;
        private readonly ICommonTypeContext _commonTypeContext;
        private readonly ICompanyContext _companyContext;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailSender _emailSender;
        private readonly string _LinkClientSite;

        private string _username { get; set; }
        private string _roleCode { get; set; }

        public QuanLyDoanhNghiepController(IConfiguration configuration, IChuyenVienContext chuyenVienContext, IDoanhNghiepContext DoanhNghiepContext, ICommonTypeContext commonTypeContext, ICompanyContext companyContext, IAppLogger appLogger, IMemoryCache cache, IStringLocalizer<SharedResource> sharedLocalizer, IHttpContextAccessor httpContextAccessor, IEmailSender emailSender)
        {
            _configuration = configuration;
            _chuyenVienContext = chuyenVienContext;
            _DoanhNghiepContext = DoanhNghiepContext;
            _commonTypeContext = commonTypeContext;
            _companyContext = companyContext;
            _appLogger = appLogger;
            _cache = cache;
            _sharedLocalizer = sharedLocalizer;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
            _username = _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.USERNAME);
            _roleCode = RoleCode.QL_DOANH_NGHIEP;
            _LinkClientSite = _configuration.GetSection("LinkClientSite").Value;
        }


        [EzAuthorize(RoleCode.QL_DOANH_NGHIEP, view: true)]
        public async Task<IActionResult> Index()
        {
            List<CommonType> listStatus = new List<CommonType>();
            List<CommonType> listNienDo = new List<CommonType>();
            List<CommonType> listCompanyType = new List<CommonType>();
            CompanyViewModel companyviewmodel = new CompanyViewModel();
            ViewBag._LinkClientSite = _LinkClientSite + "/";
            ChuyenVienViewModel chuyenVien = new ChuyenVienViewModel
            {
                UserLogin = _username,
                RoleCode = _roleCode
            };
            //var listCustomer = await _DoanhNghiepContext.SelectCustomerByExpert(new DoanhNghiepViewModel());
            var listCompanyRole = await _DoanhNghiepContext.SelectCompanyRole();
            var listMCK = await _companyContext.Select(companyviewmodel);
            var commonType = new CommonTypeViewModel
            {
                ListCategory = "3,12,16",
                UserLogin = _username,
                RoleCode = _roleCode
            };
            var listCommontype = await _commonTypeContext.Select(commonType);

            foreach (var obj in listCommontype.ToList())
            {
                if (obj.Category == ConstCommonTypes.TT_CTY)
                {
                    listStatus.Add(obj);
                }
                if (obj.Category == ConstCommonTypes.NIEN_DO_BCTC)
                {
                    listNienDo.Add(obj);
                }
                if (obj.Category == ConstCommonTypes.NHOM_LOAI_HINH)
                {
                    listCompanyType.Add(obj);
                }
            }
            var listChuyenVien = await _chuyenVienContext.Select(chuyenVien);
            var viewmodel = new QLTKDoanhNghiepViewModel
            {
                listStatus = listStatus,
                listNienDo = listNienDo,
                listCompanyType = listCompanyType,
                listMCK = listMCK.Where(x => listCompanyRole.Any(a => a.CompanyID == x.CompanyID)).ToList(),
                listChuyenVien = listChuyenVien.ToList(),
            };
            return View(viewmodel);
        }

        [HttpGet]
        [Route(LinksRoute.GetDoanhNghiep)]
        [EzAuthorize(RoleCode.QL_DOANH_NGHIEP, view: true)]
        public async Task<IActionResult> ListDoanhNghiep(DoanhNghiepViewModel doanhNghiep, int updclick = 0)
        {
            doanhNghiep.UserLogin = _username;
            doanhNghiep.RoleCode = _roleCode;
            if (!string.IsNullOrEmpty(doanhNghiep.UserLogin) && !string.IsNullOrEmpty(doanhNghiep.RoleCode))
            {
                try
                {
                    var data = await _DoanhNghiepContext.Select(doanhNghiep);
                    
                    var listData = data.ToList();
                    //int k = 0;
                    if (listData.Count > 0)
                    {
                        if (listData.Count == 1 && updclick == 1)
                        {
                            string userCreated = data.ToList().FirstOrDefault().CreateBy;
                            if (userCreated == doanhNghiep.UserLogin
                            || (_httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "ADMIN"
                            || _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "FCF Admin Group"))
                            {
                                
                            }
                            else
                            {
                                return Json(new CResponseMessage { Code = -1, Message = "Tài khoản đang dùng không có quyền sửa thông tin này" });
                            }
                        }
                        for (int i = 0; i < listData.Count(); i++)
                        {
                            if (listData[i].FiscYear != null)
                            {
                                string dateString = listData[i].FiscYear.ToString();
                                string[] dateList = dateString.Split("/");
                                listData[i].FiscYearDay = Convert.ToInt32(dateList[0]);
                                listData[i].FiscYearMonth = Convert.ToInt32(dateList[1]);
                            }

                        }
                        return Json(listData);
                    }
                    return Json(data.ToList());
                }
                catch (Exception e)
                {
                    _appLogger.ErrorLogger.LogError(e);
                }

            }
            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }


        public IActionResult DoanhNghiepViewComponent(int Id)
        {
            return ViewComponent("DoanhNghiepDetail", Id);
        }


        [HttpGet]
        [Route(LinksRoute.GetCustomerByExpert)]
        [EzAuthorize(RoleCode.QL_DOANH_NGHIEP, view: true)]
        public async Task<IActionResult> ListCustomerByExpert(DoanhNghiepViewModel doanhNghiep)
        {
            doanhNghiep.UserLogin = _username;
            doanhNghiep.RoleCode = _roleCode;
            if (!string.IsNullOrEmpty(doanhNghiep.UserLogin) && !string.IsNullOrEmpty(doanhNghiep.RoleCode))
            {
                var data = await _DoanhNghiepContext.SelectCustomerByExpert(doanhNghiep);
                return Json(data.ToList());
            }
            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }

        [HttpGet]
        [Route(LinksRoute.GetAllDoanhnghiep)]
        [EzAuthorize(RoleCode.QL_DOANH_NGHIEP, view: true)]
        public async Task<IActionResult> LoadAllCompany(DoanhNghiepViewModel doanhNghiep)
        {
            doanhNghiep.UserLogin = _username;
            doanhNghiep.RoleCode = _roleCode;
            if (!string.IsNullOrEmpty(doanhNghiep.UserLogin) && !string.IsNullOrEmpty(doanhNghiep.RoleCode))
            {
                var data = await _companyContext.Select(new CompanyViewModel());
                var company = data.Where(x => x.CompanyID == doanhNghiep.CompanyID);
                return Json(company.ToList());
            }
            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }

        [HttpPost]
        [Route(LinksRoute.UpdateDoanhNghiep)]
        [EzAuthorize(RoleCode.QL_DOANH_NGHIEP, view: true, edit: true)]
        public async Task<IActionResult> UpdateDoanhNghiep(DoanhNghiepViewModel doanhNghiep)
        {
            doanhNghiep.UserLogin = _username;
            doanhNghiep.RoleCode = _roleCode;
            doanhNghiep.ListStatus = doanhNghiep.ListStatus.Remove(doanhNghiep.ListStatus.Length - 1, 1);


            if (!string.IsNullOrEmpty(doanhNghiep.UserLogin) && !string.IsNullOrEmpty(doanhNghiep.RoleCode))
            {
                var data = await _DoanhNghiepContext.Select(new DoanhNghiepViewModel { CompanyID = doanhNghiep.CompanyID });
                string userCreated = data.ToList().FirstOrDefault().CreateBy;
                if (userCreated == doanhNghiep.UserLogin
                    || (_httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "ADMIN"
                        || _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "FCF Admin Group"))
                {
                    var cResponse = await _DoanhNghiepContext.Update(doanhNghiep);
                    cResponse.Message = this._sharedLocalizer[cResponse.Message];
                    return Json(cResponse);
                }
                else
                {
                    return Json(new CResponseMessage { Code = -1, Message = "Tài khoản đang dùng không có quyền sửa thông tin này" });
                }

            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }

        [HttpPost]
        [Route(LinksRoute.UpdateRoleDoanhNghiep)]
        [EzAuthorize(RoleCode.QL_DOANH_NGHIEP, view: true, edit: true)]
        public async Task<IActionResult> UpdateRoleDoanhNghiep(CompanyRoleViewModel companyRole)
        {
            companyRole.UserLogin = _username;
            companyRole.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(companyRole.UserLogin) && !string.IsNullOrEmpty(companyRole.RoleCode))
            {
                var cResponse = await _DoanhNghiepContext.UpdateCompanyRole(companyRole);
                cResponse.Message = this._sharedLocalizer[cResponse.Message];
                return Json(cResponse);
            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }

        [HttpPost]
        [Route(LinksRoute.DeleteDoanhNghiep)]
        [EzAuthorize(RoleCode.QL_DOANH_NGHIEP, view: true, delete: true)]
        public async Task<IActionResult> DeleteDoanhNghiep(DoanhNghiepViewModel doanhNghiep)
        {
            doanhNghiep.UserLogin = _username;
            doanhNghiep.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(doanhNghiep.UserLogin) && !string.IsNullOrEmpty(doanhNghiep.RoleCode))
            {

                var data = await _DoanhNghiepContext.Select(new DoanhNghiepViewModel { CompanyID = doanhNghiep.CompanyID });
                string userCreated = data.ToList().FirstOrDefault().CreateBy;
                string stockName = data.ToList().FirstOrDefault().StockName;
                string expert = data.ToList().FirstOrDefault().Expert;
                var dataChuyenVien = await _chuyenVienContext.Select(new ChuyenVienViewModel { Username = expert });
                string email = dataChuyenVien.ToList().FirstOrDefault().Email;
                if (userCreated == doanhNghiep.UserLogin
                    || (_httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "ADMIN"
                        || _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "FCF Admin Group"))
                {
                    var cResponse = await _DoanhNghiepContext.Delete(doanhNghiep);
                    if (cResponse.Code == 0 && expert != null)
                    {
                        var sendEmailResult = await this._emailSender.SendEmailAsync(email,
                                                                                    "Xóa thông tin doanh nghiệp",
                                                                                    $"Đã xóa doanh nghiệp {stockName}",
                                                                                    this._configuration["EmailSettings:Sender"],
                                                                                    this._configuration["EmailSettings:MailServer"],
                                                                                    Convert.ToInt32(this._configuration["EmailSettings:MailPort"]),
                                                                                    this._configuration["EmailSettings:Username"],
                                                                                    this._configuration["EmailSettings:Password"]
                                                                                    );
                    }
                    cResponse.Message = this._sharedLocalizer[cResponse.Message];
                    return Json(cResponse);
                }
                else
                {
                    return Json(new CResponseMessage { Code = -1, Message = "Tài khoản đang dùng không có quyền xóa thông tin này" });
                }

            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }

        [HttpPost]
        [Route(LinksRoute.UpdateCompanyClient)]
        [EzAuthorize(RoleCode.QL_DOANH_NGHIEP, view: true, edit: true)]
        public async Task<IActionResult> UpdateLinkDoanhNghiep(CompanyClientViewModel companyClient)
        {
            companyClient.UserLogin = _username;
            companyClient.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(companyClient.UserLogin) && !string.IsNullOrEmpty(companyClient.RoleCode))
            {
                var cResponse = await _DoanhNghiepContext.UpdateCompanyClient(companyClient);
                
                cResponse.Message = this._sharedLocalizer[cResponse.Message];
                var dataDoanhNghiep = await _DoanhNghiepContext.SelectCustomerByExpert(new DoanhNghiepViewModel { CompanyID = companyClient.CompanyID });
                cResponse.Data = dataDoanhNghiep.FirstOrDefault().StockCode;
                return Json(cResponse);
            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }
    }
}
