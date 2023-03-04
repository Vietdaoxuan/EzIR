using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.Entities;

using EzIRSpecialist.Authentication;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models.Admin;
using EzIRSpecialist.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzIRSpecialist.Controllers.BaoCaoTienIch
{
    public class QuanLyBannerController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IBannerContext _bannerContext;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly ICommon _common;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        string ImgQLBanner = "";
        private string _username { get; set; }
        private string _roleCode { get; set; }

        public QuanLyBannerController(IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, ICommon common, IStringLocalizer<SharedResource> sharedLocalizer, IConfiguration configuration, IBannerContext bannerContext, IAppLogger appLogger, IMemoryCache cache)
        {
            _configuration = configuration;
            _bannerContext = bannerContext;
            _appLogger = appLogger;
            _cache = cache;
            _sharedLocalizer = sharedLocalizer;
            _common = common;
            ImgQLBanner = _configuration.GetSection("ImgUrl").Value;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _username = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.USERNAME);
            _roleCode = RoleCode.QUAN_LY_BANNER;



        }
        [EzAuthorize(RoleCode.QUAN_LY_BANNER, view: true)]
        public IActionResult Index()
        {
            ViewBag.imgbanner = ImgQLBanner;
            return View();
        }

        /// <summary>
        /// Thêm mới banner
        /// </summary>
        /// <param name="bannerviewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(LinksRoute.InsertBanner)]
        [EzAuthorize(RoleCode.QUAN_LY_BANNER, view: true)]
        public async Task<IActionResult> InsertBanner(BannerViewModel bannerviewModel, IFormFile file)
        {
            try
            {
                bannerviewModel.UserName = _username;
                bannerviewModel.RoleCode = _roleCode;

                if (!string.IsNullOrEmpty(bannerviewModel.UserName) || !string.IsNullOrEmpty(bannerviewModel.RoleCode))
                {

                    bannerviewModel.ResponseMessage = AddLogo(file);

                    bannerviewModel.ALogo = (byte[])bannerviewModel.ResponseMessage.Data;



                    if (bannerviewModel.ALogo != null)
                    {
                        var cResponse = await _bannerContext.Insert(bannerviewModel);



                        cResponse.Message = _sharedLocalizer[cResponse.Message].Value;

                        return Json(cResponse);
                    }
                    else
                    {

                        return Json(new CResponseMessage { Code = -5, Message = _sharedLocalizer["ERROR_INSERT_BANNER"].Value });
                    }
                }
                return Json(new CResponseMessage { Code = -5, Message = _sharedLocalizer["ERROR_INSERT_BANNER"].Value });
            }
            catch (Exception e)
            {
                e.ToString();
                var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

                return Json(response);
            }


        }


        public CResponseMessage AddLogo(IFormFile file)
        {
            var cResponseHeader = new CResponseMessage();
            try
            {
                var ms = new MemoryStream();
                file.OpenReadStream().CopyTo(ms);
                byte[] value = ms.ToArray();

                // image = "data:image/gif;base64," + Convert.ToBase64String(Value);
                cResponseHeader.Data = value;
            }
            catch
            {
                cResponseHeader.Message = "Xảy ra lỗi trong quá trình !";
                cResponseHeader.Code = -1;
            }

            return cResponseHeader;
        }

        /// <summary>
        /// Hiển thị thông tin Banner
        /// </summary>
        /// <param name="bannerviewModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(LinksRoute.GetBanner)]
        [EzAuthorize(RoleCode.QUAN_LY_BANNER, view: true)]
        public async Task<IActionResult> BannerList(BannerViewModel bannerviewModel)
        {

            bannerviewModel.UserName = _username;
            bannerviewModel.RoleCode = _roleCode;



            if (!string.IsNullOrEmpty(bannerviewModel.UserName) && !string.IsNullOrEmpty(bannerviewModel.RoleCode))
            {

                var data = await _bannerContext.Select(bannerviewModel);

                foreach (var img in data.ToList())
                {
                    if (img.Alogo != null)
                    {
                        img.imagelogo = "data:image/jpeg;base64," + Convert.ToBase64String(img.Alogo);
                    }

                }

                return Json(data.ToList());
            }

            var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }



        /// <summary>
        /// Sửa Banner
        /// </summary>
        /// <param name="bannerviewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(LinksRoute.UpdateBanner)]
        [EzAuthorize(RoleCode.QUAN_LY_BANNER, view: true, edit: true)]
        public async Task<IActionResult> EditBanner(BannerViewModel bannerviewModel, IFormFile file)
        {
            try
            {
                bannerviewModel.UserName = _username;
                bannerviewModel.RoleCode = _roleCode;

                if (!string.IsNullOrEmpty(bannerviewModel.UserName) && !string.IsNullOrEmpty(bannerviewModel.RoleCode))
                {


                    bannerviewModel.ResponseMessage = AddLogo(file);

                    bannerviewModel.ALogo = (byte[])bannerviewModel.ResponseMessage.Data;

                    var cResponse = await _bannerContext.Update(bannerviewModel);

                    cResponse.Message = _sharedLocalizer[cResponse.Message].Value;


                    return Json(cResponse);

                }
                else
                {
                    return Json(new CResponseMessage { Code = -5, Message = _sharedLocalizer["ERROR_INSERT_BANNER"].Value });
                }
                return Json(new CResponseMessage { Code = -5, Message = _sharedLocalizer["ERROR_INSERT_BANNER"].Value });
            }

            catch (Exception ex)
            {
                ex.ToString();
                var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

                return Json(response);
            }


        }

        [HttpPost]
        [Route(LinksRoute.DeleteBanner)]
        [EzAuthorize(RoleCode.QUAN_LY_BANNER, view: true, delete: true)]
        public async Task<IActionResult> DeleteBanner(BannerViewModel bannerviewModel)
        {

            bannerviewModel.UserName = _username;
            bannerviewModel.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(bannerviewModel.UserName) && !string.IsNullOrEmpty(bannerviewModel.RoleCode))
            {
                var cResponse = await _bannerContext.Delete(bannerviewModel);

                cResponse.Message = this._sharedLocalizer[cResponse.Message].Value;

                return Json(cResponse);
            }

            var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }


    }
}



