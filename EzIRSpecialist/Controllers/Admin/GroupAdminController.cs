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
using EzIRSpecialist.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace EzIRSpecialist.Controllers.Admin
{
    public class GroupAdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IGroupContext _groupContext;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string _username { get; set; }
        private string _roleCode { get; set; }

        public GroupAdminController(IConfiguration configuration, IGroupContext groupContext, IAppLogger appLogger, IMemoryCache cache, IStringLocalizer<SharedResource> sharedLocalizer, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _groupContext = groupContext;
            _appLogger = appLogger;
            _cache = cache;
            _sharedLocalizer = sharedLocalizer;
            _httpContextAccessor = httpContextAccessor;
            _username = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.USERNAME);
            _roleCode = RoleCode.QUAN_LY_NHOM;
        }

        [EzAuthorize(RoleCode.QUAN_LY_NHOM, view: true)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route(LinksRoute.GET_GROUP)]
        [EzAuthorize(RoleCode.QUAN_LY_NHOM, view: true)]
        public async Task<IActionResult> GetGroup(GroupViewModel groupViewModel)
        {
            groupViewModel.UserName = _username;
            groupViewModel.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(groupViewModel.UserName) && !string.IsNullOrEmpty(groupViewModel.RoleCode))
            {
                var data = await _groupContext.Select(groupViewModel);

                return Json(data.ToList());
            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }

        [HttpPost]
        [Route(LinksRoute.INSERT_GROUP)]
        [EzAuthorize(RoleCode.QUAN_LY_NHOM, view: true, edit: true)]
        public async Task<IActionResult> InsertGroup (GroupViewModel groupViewModel)
        {
            groupViewModel.UserName = _username;
            groupViewModel.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(groupViewModel.UserName) && !string.IsNullOrEmpty(groupViewModel.RoleCode))
            {
                var  cResponse = await _groupContext.Insert(groupViewModel);

                cResponse.Message = _sharedLocalizer[cResponse.Message].Value;
                
                return Json(cResponse);
            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }

        [HttpPost]
        [Route(LinksRoute.UPDATE_GROUP)]
        [EzAuthorize(RoleCode.QUAN_LY_NHOM, view: true, edit: true)]
        public async Task<IActionResult> UpdateGroup(GroupViewModel groupViewModel)
        {
            groupViewModel.UserName = _username;
            groupViewModel.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(groupViewModel.UserName) && !string.IsNullOrEmpty(groupViewModel.RoleCode))
            {
                var cResponse = await _groupContext.Update(groupViewModel);

                cResponse.Message = _sharedLocalizer[cResponse.Message].Value;

                return Json(cResponse);
            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }

        [HttpPost]
        [Route(LinksRoute.DELETE_GROUP)]
        [EzAuthorize(RoleCode.QUAN_LY_NHOM, view: true, delete: true)]
        public async Task<IActionResult> DeleteGroup(GroupViewModel groupViewModel)
        {
            groupViewModel.UserName = _username;
            groupViewModel.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(groupViewModel.UserName) && !string.IsNullOrEmpty(groupViewModel.RoleCode))
            {
                var cResponse = await _groupContext.Delete(groupViewModel);

                cResponse.Message = _sharedLocalizer[cResponse.Message].Value;

                return Json(cResponse);
            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }
    }
}
