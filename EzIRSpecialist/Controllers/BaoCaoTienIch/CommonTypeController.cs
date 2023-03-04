using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.Entities;
using EzIRSpecialist.Authentication;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models;
using EzIRSpecialist.Models.ViewModel;
//using EzIRSpecialist.Models.ViewModel.CommonTypeViewModel;
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
    public class CommonTypeController : Controller
    {

        private readonly ICommonTypeContext _commonTypeContext;
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string _username { get; set; }
        private string _roleCode { get; set; }


        public CommonTypeController(IConfiguration configuration, ICommonTypeContext commonTypeContext, IAppLogger appLogger, IMemoryCache cache, IStringLocalizer<SharedResource> sharedLocalizer, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _commonTypeContext = commonTypeContext;
            _appLogger = appLogger;
            _cache = cache;
            _sharedLocalizer = sharedLocalizer;
            _httpContextAccessor = httpContextAccessor;
            _username = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.USERNAME);
            _roleCode = RoleCode.COMMONTYPE;
        }

        [EzAuthorize(RoleCode.COMMONTYPE, view: true)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route(LinksRoute.GetCommonType)]
        [EzAuthorize(RoleCode.COMMONTYPE, view: true)]
        public async Task<IActionResult> ListCommonType(CommonTypeViewModel commonType)
        {
            commonType.UserLogin = _username;
            commonType.RoleCode = _roleCode;
            if (!string.IsNullOrEmpty(commonType.UserLogin) && !string.IsNullOrEmpty(commonType.RoleCode))
            {
                var data = await _commonTypeContext.SelectById(commonType);
                return Json(data.ToList());
            }
            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);

        }

        [HttpPost]
        [Route(LinksRoute.InsertCommonType)]
        [EzAuthorize(RoleCode.COMMONTYPE, view: true)]
        public async Task<IActionResult> InsertCommonType(CommonTypeViewModel commonType)
        {
            commonType.UserLogin = _username;
            commonType.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(commonType.UserLogin) && !string.IsNullOrEmpty(commonType.RoleCode))
            {
                var cResponse = await _commonTypeContext.Insert(commonType);
                cResponse.Message = this._sharedLocalizer[cResponse.Message];
                return Json(cResponse);
            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);


        }
        [HttpPost]
        [Route(LinksRoute.UpdateCommonType)]
        [EzAuthorize(RoleCode.COMMONTYPE, view: true)]
        public async Task<IActionResult> UpdateCommonType(CommonTypeViewModel commonType)
        {
            commonType.UserLogin = _username;
            commonType.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(commonType.UserLogin) && !string.IsNullOrEmpty(commonType.RoleCode))
            {
                var cResponse = await _commonTypeContext.Update(commonType);
                cResponse.Message = this._sharedLocalizer[cResponse.Message];
                return Json(cResponse);
            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);


        }

        [HttpPost]
        [Route(LinksRoute.DeleteCommonType)]
        [EzAuthorize(RoleCode.COMMONTYPE, view: true)]
        public async Task<IActionResult> DeleteCommonType(CommonTypeViewModel commonType)
        {
            commonType.UserLogin = _username;
            commonType.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(commonType.UserLogin) && !string.IsNullOrEmpty(commonType.RoleCode))
            {
                var cResponse = await _commonTypeContext.Delete(commonType);
                cResponse.Message = this._sharedLocalizer[cResponse.Message];
                return Json(cResponse);
            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);


        }
    }
}
