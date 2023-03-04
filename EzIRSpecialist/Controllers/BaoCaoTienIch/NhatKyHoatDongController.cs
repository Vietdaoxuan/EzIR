using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Interfaces;
using EzIRSpecialist.Authentication;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Implementations;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models;
using EzIRSpecialist.Models.BaoCaoTienIch;
using EzIRSpecialist.Models.QuanLyTaiKhoan;
using EzIRSpecialist.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Controllers.BaoCaoTienIch
{
    public class NhatKyHoatDongController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly INhatKyHoatDongContext _nhatKyHoatDongContext;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly ICommon _common;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ICommonTypeContext _commonTypeContext;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ICompanyContext _companyContext;

        private string _username { get; set; }
        private string _roleCode { get; set; }

        public NhatKyHoatDongController(ICompanyContext companyContext, IHttpContextAccessor httpContextAccessor, ICommonTypeContext commonTypeContext,IHostingEnvironment hostingEnvironment, ICommon common, IStringLocalizer<SharedResource> sharedLocalizer, IConfiguration configuration, INhatKyHoatDongContext nhatKyHoatDong, IAppLogger appLogger, IMemoryCache cache)
        {
            _configuration = configuration;
            _nhatKyHoatDongContext = nhatKyHoatDong;
            _appLogger = appLogger;
            _cache = cache;
            _common = common;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _commonTypeContext = commonTypeContext;
            _sharedLocalizer = sharedLocalizer;
            _companyContext = companyContext;

            _username = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.USERNAME);
            _roleCode = RoleCode.NHAT_KY_HOAT_DONG;

        }

        [EzAuthorize(RoleCode.NHAT_KY_HOAT_DONG, view: true)]
        public async Task<IActionResult> Index()
        {
         
            // SelectList Action
            List<CommonType> listAction = new List<CommonType>();
         
            var commonType = new CommonTypeViewModel
            {
                ListCategory = "10"
            };
      
            var listCommontype = await _commonTypeContext.Select(commonType);

            foreach (var obj in listCommontype.ToList())
            {
                if (obj.Category == ConstCommonTypes.NHOM_ACTION)
                {
                    listAction.Add(obj);
                }
            }

            //ListMaCK
            List<Company> listMaCk = new List<Company>();

            var expertcompany = new CompanyViewModel();

            var company = await _companyContext.Select(expertcompany);

            foreach (var obj in company.ToList())
            {
                listMaCk.Add(obj);
            }

            listAction.RemoveAt(0);

            var viewmodel = new NhatKyHoatDongViewModel
            {
                ListAction = listAction,
                ListMaCk = listMaCk
            };

            return View(viewmodel);
        }

        [HttpGet]
        [Route(LinksRoute.GetAction)]
        [EzAuthorize(RoleCode.NHAT_KY_HOAT_DONG, view: true)]
        public async Task<IActionResult> GetNhatKy(NhatKyHoatDongViewModel nhatKyHoatDongViewModel)
        {
            // đoạn này đúng ra phải lấy trong Session vì chưa có nên gán tạm
            nhatKyHoatDongViewModel.UserName = _username;
            nhatKyHoatDongViewModel.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(nhatKyHoatDongViewModel.UserName) && !string.IsNullOrEmpty(nhatKyHoatDongViewModel.RoleCode))
            {
                var data = await _nhatKyHoatDongContext.Select(nhatKyHoatDongViewModel);

                return Json(data.ToList());
            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value};

            return Json(response);
        }

    }
}
