using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using EzIRSpecialist.Authentication;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models.Admin;
using EzIRSpecialist.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace EzIRSpecialist.Controllers.Admin
{
    //[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public class UserAdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserContext _userContext;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string _username { get; set; }
        private string _roleCode { get; set; }

        public UserAdminController(IConfiguration configuration, IUserContext userContext, IAppLogger appLogger, IMemoryCache cache, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _userContext = userContext;
            _appLogger = appLogger;
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _username = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.USERNAME);
            _roleCode = RoleCode.ACCOUNT_CHUYENVIEN;
        }

        [EzAuthorize(RoleCode.ACCOUNT_CHUYENVIEN, view: true)]
        public IActionResult Index()
        {            
            return View();
        }

        [HttpGet]
        [Route(LinksRoute.GET_USER)]
        [EzAuthorize(RoleCode.PHAN_QUYEN_CHO_NHOM, view: true)]
        public async Task<IActionResult> GetData(UserViewModel user)
        {            
            var data = await _userContext.Select(user);

            return Json(data.ToList());
        }
    }
}
