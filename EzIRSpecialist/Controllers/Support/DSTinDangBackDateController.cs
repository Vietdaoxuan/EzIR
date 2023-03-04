using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.Entities;

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

namespace EzIRSpecialist.Controllers.Support
{
    public class DSTinDangBackDateController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IDSTinDangBackDateContext _dSTinDangBackDateContext;
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

        public DSTinDangBackDateController(ICompanyContext companyContext, ICommonTypeContext commonTypeContext, IDSTinDangBackDateContext dstindangbackdateContext, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, ICommon common, IStringLocalizer<SharedResource> sharedLocalizer, IConfiguration configuration, IRulesContext rulesContext, IAppLogger appLogger, IMemoryCache cache)
        {
            _configuration = configuration;
            _dSTinDangBackDateContext = dstindangbackdateContext;
            _appLogger = appLogger;
            _cache = cache;
            _sharedLocalizer = sharedLocalizer;
            _common = common;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _username = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.USERNAME);
            _commonTypeContext = commonTypeContext;
            _companyContext = companyContext;
            _roleCode = RoleCode.DS_TIN_DANG_BACKDATE;
        }
        [EzAuthorize(RoleCode.DS_TIN_DANG_BACKDATE, view: true)]
        public async Task<IActionResult> Index(int? value)
        {
            List<CommonType> listCompanyType = new List<CommonType>();
            List<CommonType> listloaitl = new List<CommonType>();
            List<CommonType> listloaitin = new List<CommonType>();
            List<CommonType> listRegion = new List<CommonType>();
            var listMCK = await _companyContext.Select(new CompanyViewModel());
            var commonType = new CommonTypeViewModel
            {
                ListCategory = "1,3,4,11"
            };
            var listCommontype = await _commonTypeContext.Select(commonType);

            if (value == null)
            {
                foreach (var obj in listCommontype.ToList())
                {
                    if (obj.Category == ConstCommonTypes.NHOM_VUNG_MIEN)
                    {
                        listRegion.Add(obj);
                    }
                    if (obj.Category == ConstCommonTypes.NHOM_LOAI_HINH)
                    {
                        listCompanyType.Add(obj);
                    }
                    if (obj.Category == ConstCommonTypes.NHOM_TAI_LIEU)
                    {
                        listloaitl.Add(obj);
                    }
                    //if (obj.Category == ConstCommonTypes.NHOM_LOAI_TIN)
                    //{
                    //    listloaitin.Add(obj);
                    //}

                }

                var viewmodel = new DSTinDangBackDateViewModel
                {
                    ListDocType = listloaitl,
                    ListCompanyType = listCompanyType,
                    ListMaCk = listMCK.ToList(),
                    ListLoaiTin = listloaitin,
                    ListRegion = listRegion
                };

                return View(viewmodel);
            }
            else
            {
                foreach (var listin in listCommontype.ToList())
                {
                    if (listin.ParentID == value)

                        listloaitin.Add(listin);

                }

                return Json(listloaitin);
            }
        }

        /// <summary>
        /// Tìm kiếm tin Back Date
        /// </summary>
        /// <param name="dSTinDangBackDateViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(LinksRoute.GetListNewsBackDate)]
        [EzAuthorize(RoleCode.DS_TIN_DANG_BACKDATE, view: true)]
        public async Task<IActionResult> ListNewsBackDate(DSTinDangBackDateViewModel dSTinDangBackDateViewModel)
        {
            dSTinDangBackDateViewModel.UserName = _username;
            dSTinDangBackDateViewModel.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(dSTinDangBackDateViewModel.UserName) && !string.IsNullOrEmpty(dSTinDangBackDateViewModel.RoleCode))
            {
                var data = await _dSTinDangBackDateContext.ListTinDangBackDate(dSTinDangBackDateViewModel);

                return Json(data);
            }

            var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }

        /// <summary>
        /// Xuất báo cáo tin backdate
        /// </summary>
        /// <param name="dsTinDangTheoKhachHangViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [EzAuthorize(RoleCode.DS_TIN_DANG_BACKDATE, view: true, special: true)]
        public async Task<IActionResult> ExportExcel(DSTinDangBackDateViewModel dSTinDangBackDateViewModel)
        {
            dSTinDangBackDateViewModel.UserName = _username;
            dSTinDangBackDateViewModel.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(dSTinDangBackDateViewModel.UserName) && !string.IsNullOrEmpty(dSTinDangBackDateViewModel.RoleCode))
            {
                var listdataRules = await _dSTinDangBackDateContext.ListNewsBackDateExcel(dSTinDangBackDateViewModel);

                var fileResultStream = GenerateExcelFileToStream(listdataRules);

                return this.File(fileResultStream, "application/vnd.ms-excel");
            }

            var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }

        private MemoryStream GenerateExcelFileToStream(DataTable dataListNewsBackDate)
        {
            IWorkbook workbook = new XSSFWorkbook();

            ISheet excelSheet = workbook.CreateSheet("DanhSachTinBackDate");
            IRow row = excelSheet.CreateRow(0);
            //int k = 0;

            for (int i = 0; i < dataListNewsBackDate.Columns.Count; i++)
            {
                if (dataListNewsBackDate.Columns[i].ColumnName != "AREGIONID")
                {
                    //row.CreateCell(i).SetCellValue(dataListNewsBackDate.Columns[i].ColumnName);

                    if (dataListNewsBackDate.Columns[i].ColumnName == "ASTOCKCODE")
                        row.CreateCell(i).SetCellValue(dataListNewsBackDate.Columns[i].ColumnName.Replace("ASTOCKCODE", "Mã chứng khoán"));
                    if (dataListNewsBackDate.Columns[i].ColumnName == "ANAME")
                        row.CreateCell(i).SetCellValue(dataListNewsBackDate.Columns[i].ColumnName.Replace("ANAME", "Tên công ty"));
                    if (dataListNewsBackDate.Columns[i].ColumnName == "AVUNGMIEN")
                        row.CreateCell(i).SetCellValue(dataListNewsBackDate.Columns[i].ColumnName.Replace("AVUNGMIEN", "Vùng miền"));
                    if (dataListNewsBackDate.Columns[i].ColumnName == "ASANGIAODICH")
                        row.CreateCell(i).SetCellValue(dataListNewsBackDate.Columns[i].ColumnName.Replace("ASANGIAODICH", "Sàn giao dịch"));
                    if (dataListNewsBackDate.Columns[i].ColumnName == "ALOAITAILIEU")
                        row.CreateCell(i).SetCellValue(dataListNewsBackDate.Columns[i].ColumnName.Replace("ALOAITAILIEU", "Loại tài liệu"));
                    if (dataListNewsBackDate.Columns[i].ColumnName == "ALOAITIN")
                        row.CreateCell(i).SetCellValue(dataListNewsBackDate.Columns[i].ColumnName.Replace("ALOAITIN", "Loại tin"));
                    if (dataListNewsBackDate.Columns[i].ColumnName == "ATITLE")
                        row.CreateCell(i).SetCellValue(dataListNewsBackDate.Columns[i].ColumnName.Replace("ATITLE", "Tiêu đề"));
                    if (dataListNewsBackDate.Columns[i].ColumnName == "AYEAR")
                        row.CreateCell(i).SetCellValue(dataListNewsBackDate.Columns[i].ColumnName.Replace("AYEAR", "Năm"));
                    if (dataListNewsBackDate.Columns[i].ColumnName == "ANGAYDANGTIN")
                        row.CreateCell(i).SetCellValue(dataListNewsBackDate.Columns[i].ColumnName.Replace("ANGAYDANGTIN", "Ngày đăng tin"));
                    if (dataListNewsBackDate.Columns[i].ColumnName == "ACREATEBY")
                        row.CreateCell(i).SetCellValue(dataListNewsBackDate.Columns[i].ColumnName.Replace("ACREATEBY", "Người tải lên"));
                    if (dataListNewsBackDate.Columns[i].ColumnName == "AEXPERT")
                        row.CreateCell(i).SetCellValue(dataListNewsBackDate.Columns[i].ColumnName.Replace("AEXPERT", "Người phụ trách"));
                    if (dataListNewsBackDate.Columns[i].ColumnName == "AEXPERT")
                        row.CreateCell(i).SetCellValue(dataListNewsBackDate.Columns[i].ColumnName.Replace("AEXPERT", "Người phụ trách"));
                    if (dataListNewsBackDate.Columns[i].ColumnName == "ANGAYDANGTINBANDAU")
                        row.CreateCell(i).SetCellValue(dataListNewsBackDate.Columns[i].ColumnName.Replace("ANGAYDANGTINBANDAU", "Ngày đăng tin ban đầu"));

                    //k++;
                }

            }

            for (int j = 0; j < dataListNewsBackDate.Rows.Count; j++)
            {
                //int k = j + 1;
                row = excelSheet.CreateRow(j + 1);


                for (int i = 0; i < dataListNewsBackDate.Columns.Count; i++)
                {
                    if (dataListNewsBackDate.Columns[i].ColumnName != "AREGIONID")
                    {
                        row.CreateCell(i).SetCellValue(dataListNewsBackDate.Rows[j][dataListNewsBackDate.Columns[i].ColumnName].ToString().Replace("12:00:00 SA", ""));


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
