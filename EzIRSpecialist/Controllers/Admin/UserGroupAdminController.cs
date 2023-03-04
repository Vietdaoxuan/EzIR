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
    public class UserGroupAdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserGroupContext _userGroupContext;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string _username { get; set; }
        private string _roleCode { get; set; }

        public UserGroupAdminController(IConfiguration configuration, IUserGroupContext userGroupContext, IAppLogger appLogger, IMemoryCache cache, IStringLocalizer<SharedResource> sharedLocalizer, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _userGroupContext = userGroupContext;
            _appLogger = appLogger;
            _cache = cache;
            _sharedLocalizer = sharedLocalizer;
            _httpContextAccessor = httpContextAccessor;
            _username = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.USERNAME);
            _roleCode = RoleCode.PHAN_NHOM_NGUOI_DUNG;
        }

        [EzAuthorize(RoleCode.PHAN_NHOM_NGUOI_DUNG, view: true)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route(LinksRoute.GET_USER_GROUP)]
        [EzAuthorize(RoleCode.PHAN_NHOM_NGUOI_DUNG, view: true)]
        public async Task<IActionResult> GetUserGroup(UserGroupViewModel userGroupViewModel)
        {            
            userGroupViewModel.UserName = _username;
            userGroupViewModel.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(userGroupViewModel.UserName) && !string.IsNullOrEmpty(userGroupViewModel.RoleCode))
            {
                var data = await _userGroupContext.Select(userGroupViewModel);

                return Json(data.ToList());
            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }

        [HttpPost]
        [Route(LinksRoute.INSERT_USER_GROUP)]
        [EzAuthorize(RoleCode.PHAN_NHOM_NGUOI_DUNG, view: true, edit: true)]
        public async Task<IActionResult> InsertUserGroup(List<UserGroupViewModel> listUserGroup)
        {            
            if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_roleCode))
            {
                var cResponse = new CResponseMessage();

                if (listUserGroup.Count > 0)
                {
                    foreach (var item in listUserGroup)
                    {
                        item.UserName = _username;
                        item.RoleCode = _roleCode;
                        cResponse = await _userGroupContext.Insert(item);
                    }                   
                }

                cResponse.Message = _sharedLocalizer[cResponse.Message].Value;

                return Json(cResponse);
            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }

        [HttpPost]
        [Route(LinksRoute.DELETE_USER_GROUP)]
        [EzAuthorize(RoleCode.PHAN_NHOM_NGUOI_DUNG, view: true, delete: true)]
        public async Task<IActionResult> DeleteUserGroup(List<UserGroupViewModel> listUserGroup)
        {           
            if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_roleCode))
            {
                var cResponse = new CResponseMessage();

                if (listUserGroup.Count > 0)
                {
                    foreach (var item in listUserGroup)
                    {
                        item.UserName = _username;
                        item.RoleCode = _roleCode;
                        cResponse = await _userGroupContext.Delete(item);
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
