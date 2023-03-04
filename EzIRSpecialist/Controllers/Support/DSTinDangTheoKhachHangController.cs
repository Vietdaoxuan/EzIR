using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.ViewModel;
using EzIRSpecialist.Authentication;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models;
using EzIRSpecialist.Models.QuanLyTaiKhoan;
using EzIRSpecialist.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommonTypeViewModel = EzIRSpecialist.Models.ViewModel.CommonTypeViewModel;

namespace EzIRSpecialist.Controllers.Support
{
    public class DSTinDangTheoKhachHangController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IDSTinDangTheoKhachHangContext _dstindangtheokhContext;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly ICommon _common;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommonTypeContext _commonTypeContext;
        private readonly ICompanyContext _companyContext;

        private string _username { get; set; }
        private string _roleCode { get; set; }
        public DSTinDangTheoKhachHangController(ICompanyContext companyContext, ICommonTypeContext commonTypeContext, IDSTinDangTheoKhachHangContext dstindangtheokhContext, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, ICommon common, IStringLocalizer<SharedResource> sharedLocalizer, IConfiguration configuration, IRulesContext rulesContext, IAppLogger appLogger, IMemoryCache cache)
        {
            _configuration = configuration;
            _dstindangtheokhContext = dstindangtheokhContext;
            _appLogger = appLogger;
            _cache = cache;
            _sharedLocalizer = sharedLocalizer;
            _common = common;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _username = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.USERNAME);
            _commonTypeContext = commonTypeContext;
            _companyContext = companyContext;
            _roleCode = RoleCode.QL_RULES;
        }

        [EzAuthorize(RoleCode.DS_TIN_DANG_THEO_KHACH_HANG, view: true)]
        public async  Task<IActionResult> Index()
        {
            
            List<CommonType> listloaidn = new List<CommonType>();
            List<CommonType> listloaitl = new List<CommonType>();   

            var commonType = new CommonTypeViewModel
            {
                ListCategory = "3,4"
            };

            var listCommontype = await _commonTypeContext.Select(commonType);

            foreach (var obj in listCommontype.ToList())
            {
                switch (obj.Category)
                {
                    case ConstCommonTypes.NHOM_LOAI_HINH:
                        listloaidn.Add(obj);
                        break;
                    case ConstCommonTypes.NHOM_TAI_LIEU:
                        listloaitl.Add(obj);
                        break;
                    default:
                        break;
                }
            }
                //ListMaCK
            List<Company> listMaCk = new List<Company>();

            CompanyViewModel companyViewModel = new CompanyViewModel();

            var company = await _companyContext.Select(companyViewModel);

            foreach (var obj in company.ToList())
            {
                listMaCk.Add(obj);
            }

            var viewmodel = new DSTinDangTheoKhachHangViewModel
            {
                ListCompanyType = listloaidn,
                ListDocType = listloaitl,
                ListMaCk = listMaCk
            };

            return View(viewmodel);
        }

        /// <summary>
        /// Tìm kiếm danh sách tin đăng theo khách hàng
        /// </summary>
        /// <param name="dsTinDangTheoKhachHangViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(LinksRoute.GetListNews)]
        [EzAuthorize(RoleCode.DS_TIN_DANG_THEO_KHACH_HANG, view: true,special:true)]
        public async Task<IActionResult> ListNews(DSTinDangTheoKhachHangViewModel dsTinDangTheoKhachHangViewModel)
        {
            dsTinDangTheoKhachHangViewModel.UserName = _username;
            dsTinDangTheoKhachHangViewModel.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(dsTinDangTheoKhachHangViewModel.UserName) && !string.IsNullOrEmpty(dsTinDangTheoKhachHangViewModel.RoleCode))
            {
                var data = await _dstindangtheokhContext.ListTinDang(dsTinDangTheoKhachHangViewModel);

                return Json(data);
            }

            var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }

        /// <summary>
        /// Xuất báo cáo Excel danh sách tin đăng theo khách hàng
        /// </summary>
        /// <param name="dsTinDangTheoKhachHangViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [EzAuthorize(RoleCode.QL_RULES, view: true)]
        public async Task<IActionResult> ExportExcel(DSTinDangTheoKhachHangViewModel dsTinDangTheoKhachHangViewModel)
        {
            dsTinDangTheoKhachHangViewModel.UserName = _username;
            dsTinDangTheoKhachHangViewModel.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(dsTinDangTheoKhachHangViewModel.UserName) && !string.IsNullOrEmpty(dsTinDangTheoKhachHangViewModel.RoleCode))
            {
                var listdataRules = await _dstindangtheokhContext.ListTinDangExcel(dsTinDangTheoKhachHangViewModel);

                var fileResultStream = GenerateExcelFileToStream(listdataRules);

                return this.File(fileResultStream, "application/vnd.ms-excel");
            }

            var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }

        private MemoryStream GenerateExcelFileToStream(DataTable dataListNews)
        {
            IWorkbook workbook = new XSSFWorkbook();

            ISheet excelSheet = workbook.CreateSheet("DanhSachTinDangTheoKhachHang");
            IRow row = excelSheet.CreateRow(0);
            int k = 0;
            for (int i = 0; i < dataListNews.Columns.Count; i++)
            {
                if (dataListNews.Columns[i].ColumnName != "AREGIONID")  
                {
                    row.CreateCell(k).SetCellValue(dataListNews.Columns[i].ColumnName);
                    k++;
                }

            }

            for (int j = 0; j < dataListNews.Rows.Count; j++)
            {

                row = excelSheet.CreateRow(j + 1);
                int t = 0;
                for (int i = 0; i < dataListNews.Columns.Count; i++)
                {
                    if (dataListNews.Columns[i].ColumnName != "AREGIONID")
                    {
                        row.CreateCell(t).SetCellValue(dataListNews.Rows[j][dataListNews.Columns[i].ColumnName].ToString());
                        t++;
                    }

                }
            }

            MemoryStream tempStream = new MemoryStream();
            workbook.Write(tempStream);

            // Sau khi ghi vào thì stream bị đóng bởi thư viện nên phải chuyển sang stream khác
            var memoryStream = new MemoryStream();
            var bytes = tempStream.ToArray();
            memoryStream.Write(tempStream.ToArray(), 0, bytes.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

    }
}
