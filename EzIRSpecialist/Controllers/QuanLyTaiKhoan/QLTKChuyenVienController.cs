using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.Entities;
using EzIRSpecialist.Authentication;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models;
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

namespace EzIRSpecialist.Controllers
{
    public class QLTKChuyenVienController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IChuyenVienContext _chuyenVienContext;
        private readonly ICommonTypeContext _commonTypeContext;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string _username { get; set; }
        private string _roleCode { get; set; }
        public QLTKChuyenVienController(IConfiguration configuration, IChuyenVienContext chuyenVienContext, ICommonTypeContext commonTypeContext, IAppLogger appLogger, IMemoryCache cache, IStringLocalizer<SharedResource> sharedLocalizer, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _chuyenVienContext = chuyenVienContext;
            _commonTypeContext = commonTypeContext;
            _appLogger = appLogger;
            _cache = cache;
            _sharedLocalizer = sharedLocalizer;
            _httpContextAccessor = httpContextAccessor;
            _username = _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.USERNAME);
            _roleCode = RoleCode.ACCOUNT_CHUYENVIEN;
        }

        [EzAuthorize(RoleCode.ACCOUNT_CHUYENVIEN, view: true)]
        public async Task<IActionResult> Index()
        {
            List<CommonType> listStatus = new List<CommonType>();
            List<CommonType> listRegion = new List<CommonType>();
            var commonType = new CommonTypeViewModel
            {
                ListCategory = "1,2"
            };

            var listCommontype = await _commonTypeContext.Select(commonType);
            foreach (var obj in listCommontype.ToList())
            {
                if (obj.Category == ConstCommonTypes.NHOM_TRANGTHAI)
                {
                    listStatus.Add(obj);
                }
                if (obj.Category == ConstCommonTypes.NHOM_VUNG_MIEN)
                {
                    listRegion.Add(obj);
                }
            }

            var viewmodel = new ChuyenVienViewModel
            {
                listStatus = listStatus,
                listRegion = listRegion,

            };
            return View(viewmodel);
        }

        [HttpGet]
        [Route(LinksRoute.GetChuyenVien)]
        [EzAuthorize(RoleCode.ACCOUNT_CHUYENVIEN, view: true)]
        public async Task<IActionResult> ListChuyenVien(ChuyenVienViewModel chuyenVien)
        {
            chuyenVien.UserLogin = _username;
            chuyenVien.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(chuyenVien.UserLogin) && !string.IsNullOrEmpty(chuyenVien.RoleCode))
            {

                var data = await _chuyenVienContext.Select(chuyenVien);
                return Json(data.ToList());
            }
            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);

        }

        [HttpGet]
        [Route(LinksRoute.GetChuyenVienUpdate)]
        [EzAuthorize(RoleCode.ACCOUNT_CHUYENVIEN, view: true)]
        public async Task<IActionResult> ListChuyenVienUpdate(ChuyenVienViewModel chuyenVien)
        {
            chuyenVien.UserLogin = _username;
            chuyenVien.RoleCode = _roleCode;

            //Chỉ TK admin/FCF Admin Group hoặc tk của người dùng đó mới sửa đc. TK chuyên viên thường đang login không phải tk chuyên viên muốn sửa thì không sửa đc.
            var data = await _chuyenVienContext.Select(new ChuyenVienViewModel { Username = chuyenVien.Username });
            string Username = data.ToList().FirstOrDefault().Username;
            if (Username == chuyenVien.UserLogin
                 || (_httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "ADMIN"
                     || _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "FCF Admin Group"))
            {
                return Json(data.ToList());
            }
            else
            {
                return Json(new CResponseMessage { Code = -1, Message = "Tài khoản đang dùng không có quyền sửa thông tin của tài khoản này!" });
            }



        }

        [HttpPost]
        [Route(LinksRoute.InsertChuyenVien)]
        [EzAuthorize(RoleCode.ACCOUNT_CHUYENVIEN, view: true, edit: true)]
        public async Task<IActionResult> InsertChuyenVien(ChuyenVienViewModel chuyenVien)
        {
            chuyenVien.UserLogin = _username;
            chuyenVien.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(chuyenVien.UserLogin) && !string.IsNullOrEmpty(chuyenVien.RoleCode))
            {
                //Chỉ TK admin/FCF Admin Group mới thêm đc TK mới. TK chuyên viên thường không thêm được tài khoản.
                if ((_httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "ADMIN"
                    || _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "FCF Admin Group"))
                {
                    if (chuyenVien.Active == null)
                    {
                        chuyenVien.Active = 0;
                    }
                    var cResponse = await _chuyenVienContext.Insert(chuyenVien);
                    cResponse.Message = this._sharedLocalizer[cResponse.Message];
                    return Json(cResponse);
                }
                else
                {
                    return Json(new CResponseMessage { Code = -1, Message = "Tài khoản đang dùng không có quyền tạo tài khoản Chuyên viên mới!" });
                }
              
            }

            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
            return Json(response);
        }

        [HttpPost]
        [Route(LinksRoute.UpdateChuyenVien)]
        [EzAuthorize(RoleCode.ACCOUNT_CHUYENVIEN, view: true, edit: true)]
        public async Task<IActionResult> UpdateChuyenVien(ChuyenVienViewModel chuyenVien)
        {
            chuyenVien.UserLogin = _username;
            chuyenVien.RoleCode = _roleCode;

            //Chỉ TK admin/FCF Admin Group hoặc tk của người dùng đó mới sửa đc. TK chuyên viên thường đang login không phải tk chuyên viên muốn sửa thì không sửa đc.
            var data = await _chuyenVienContext.Select(new ChuyenVienViewModel { Username = chuyenVien.Username });
            string Username = data.ToList().FirstOrDefault().Username;
            if (Username == chuyenVien.UserLogin
                 || (_httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "ADMIN"
                     || _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "FCF Admin Group"))
            {
                if (chuyenVien.Active == null)
                {
                    chuyenVien.Active = 0;
                }
                var cResponse = await _chuyenVienContext.Update(chuyenVien);
                cResponse.Message = this._sharedLocalizer[cResponse.Message];
                return Json(cResponse);
            }

            var response = new CResponseMessage { Code = -1, Message = "Tài khoản đang dùng không có quyền sửa thông tin của tài khoản này!"};
            return Json(response);


        }

        [HttpPost]
        [Route(LinksRoute.DeleteChuyenVien)]
        [EzAuthorize(RoleCode.ACCOUNT_CHUYENVIEN, view: true, delete: true)]
        public async Task<IActionResult> DeleteChuyenVien(ChuyenVienViewModel chuyenVien)
        {
            chuyenVien.UserLogin = _username;
            chuyenVien.RoleCode = _roleCode;

            //Chỉ TK admin/FCF Admin Group mới xóa đc tk khác. TK chuyên viên thường không xóa được tài khoản.
            if ((_httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "ADMIN"
                     || _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE) == "FCF Admin Group"))
            {
                var cResponse = await _chuyenVienContext.Delete(chuyenVien);
                cResponse.Message = this._sharedLocalizer[cResponse.Message];
                return Json(cResponse);
            }
            else
            {
                return Json(new CResponseMessage { Code = -1, Message = "Tài khoản đang dùng không có quyền xóa tài khoản Chuyên viên!" });
            }
        }
    }
}
