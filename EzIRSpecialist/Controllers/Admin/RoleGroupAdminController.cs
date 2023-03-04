using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.Entities;
using EzIRSpecialist.Authentication;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models.Admin;
using EzIRSpecialist.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace EzIRSpecialist.Controllers.Admin
{
    public class RoleGroupAdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IRoleGroupContext _roleGroupContext;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string _username { get; set; }
        private string _roleCode { get; set; }

        public RoleGroupAdminController(IConfiguration configuration, IRoleGroupContext roleGroupContext, IAppLogger appLogger, IMemoryCache cache, IStringLocalizer<SharedResource> sharedLocalizer, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _roleGroupContext = roleGroupContext;
            _appLogger = appLogger;
            _cache = cache;
            _sharedLocalizer = sharedLocalizer;
            _httpContextAccessor = httpContextAccessor;
            _username = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.USERNAME);
            _roleCode = RoleCode.PHAN_QUYEN_CHO_NHOM;
        }

        [EzAuthorize(RoleCode.PHAN_QUYEN_CHO_NHOM, view: true)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route(LinksRoute.GET_ROLE_GROUP)]
        [EzAuthorize(RoleCode.PHAN_QUYEN_CHO_NHOM, view: true)]
        public async Task<IActionResult> GetRoleGroup(RoleGroupViewModel roleGroupViewModel)
        {            
            roleGroupViewModel.UserName = _username;
            roleGroupViewModel.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(roleGroupViewModel.UserName) && !string.IsNullOrEmpty(roleGroupViewModel.RoleCode))
            {
                var data = await _roleGroupContext.Select(roleGroupViewModel);

                return Json(data.ToList());
            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }

        [HttpPost]
        [Route(LinksRoute.INSERT_ROLE_GROUP)]
        [EzAuthorize(RoleCode.PHAN_QUYEN_CHO_NHOM, view: true, edit: true)]
        public async Task<IActionResult> InsertroleGroup(List<RoleGroupViewModel> listRoleGroup)
        {            
            if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_roleCode))
            {
                var cResponse = new CResponseMessage();

                if (listRoleGroup.Count > 0)
                {
                    foreach (var item in listRoleGroup)
                    {
                        item.UserName = _username;
                        item.RoleCode = _roleCode;
                        cResponse = await _roleGroupContext.Insert(item);
                    }
                }

                cResponse.Message = _sharedLocalizer[cResponse.Message].Value;

                return Json(cResponse);
            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }
    }
}
