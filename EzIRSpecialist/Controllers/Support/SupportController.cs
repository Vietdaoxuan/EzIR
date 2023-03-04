using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Interfaces;
using EzIRSpecialist.Authentication;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models.Support;
using EzIRSpecialist.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Controllers.Support
{
    public class SupportController : Controller
    {
        private readonly IHtmlLocalizer<SharedResource> _localizer;
        private readonly IConfiguration _configuration;
        private readonly ISupportContext _iSupportContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private string _BASE_API_URL;
        private readonly IAppLogger _appLogger;
        private string _username { get; set; }
        private string _roleCode { get; set; }

        public SupportController(IConfiguration configuration, IAppLogger appLogger, IHtmlLocalizer<SharedResource> localizer, ISupportContext iSupportContext , IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _localizer = localizer;
            _iSupportContext = iSupportContext;
            _httpContextAccessor = httpContextAccessor;
            _BASE_API_URL = _configuration.GetSection("ApiUrl").Value;
            _appLogger = appLogger;

            _username = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.USERNAME);
            _roleCode = RoleCode.JOBS_STATUS;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Trạng thái jobs đồng bộ"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        [EzAuthorize(RoleCode.SUPPORT, view: true)]
        public async Task<IActionResult> GetJobsStatus(JobsStatusViewModel jobsStatusViewModel)
        {
            jobsStatusViewModel.UserName = _username;
            jobsStatusViewModel.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(jobsStatusViewModel.UserName) && !string.IsNullOrEmpty(jobsStatusViewModel.RoleCode))
            {
                var data = await _iSupportContext.Select(jobsStatusViewModel);

                return Json(data.ToList());
            }

            var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }


        [HttpPost]
        [EzAuthorize(RoleCode.SUPPORT, view: true, edit:true)]
        public async Task<IActionResult> XuLyDongBo(string Loai, JobsStatusViewModel jobsStatusViewModel)
        {
            jobsStatusViewModel.UserName = _username;
            jobsStatusViewModel.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(jobsStatusViewModel.UserName) && !string.IsNullOrEmpty(jobsStatusViewModel.RoleCode))
            {
                var cResponse = await _iSupportContext.XuLyDongBo(Loai);
                //cResponse.Message = this._sharedLocalizer[cResponse.Message];
                return Json(cResponse);
            }

            var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }
    }
}
