using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using EzIRSpecialist.Authentication;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models;
using EzIRSpecialist.Models.BaoCaoTienIch;
using EzIRSpecialist.Models.ModelView;
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EzIRSpecialist.Controllers.BaoCaoTienIch
{
    public class QuanLyRulesController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IRulesContext _rulesContext;
        private readonly IAppLogger _appLogger;
        private readonly IMemoryCache _cache;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly ICommon _common;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommonTypeContext _commonTypeContext;
        private readonly ITemplateContext _templateContext;
        private string _username { get; set; }
        private string _roleCode { get; set; }

        public QuanLyRulesController(ICommonTypeContext commonTypeContext, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, ICommon common, IStringLocalizer<SharedResource> sharedLocalizer, IConfiguration configuration, IRulesContext rulesContext, IAppLogger appLogger, IMemoryCache cache, ITemplateContext templateContext)
        {
            _configuration = configuration;
            _rulesContext = rulesContext;
            _appLogger = appLogger;
            _cache = cache;
            _sharedLocalizer = sharedLocalizer;
            _common = common;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _username = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.USERNAME);
            _commonTypeContext = commonTypeContext;
            _roleCode = RoleCode.QL_RULES;
            _templateContext = templateContext;
        }

        [EzAuthorize(RoleCode.QL_RULES, view: true)]
        public async Task<IActionResult> Index(int? value)
        {

            List<CommonType> listloaidn = new List<CommonType>();
            List<CommonType> listloaitl = new List<CommonType>();
            List<CommonType> listloaitin = new List<CommonType>();
            List<CommonType> listdoituongad = new List<CommonType>();

            var commonType = new CommonTypeViewModel
            {
                //ListCategory = "3,4,101,102,103,104,105,106,107,108,109,110,111"

                ListCategory = "3,4,11,18"
            };

            var listCommontype = await _commonTypeContext.Select(commonType);

            if (value == null)
            {

                foreach (var obj in listCommontype.ToList())
                {
                    switch (obj.Category)
                    {
                        case ConstCommonTypes.NHOM_LOAI_HINH:
                            listloaidn.Add(obj);
                            break;
                        case ConstCommonTypes.NHOM_DOI_TUONG:
                            listdoituongad.Add(obj);
                            break;
                        case ConstCommonTypes.NHOM_TAI_LIEU:
                            listloaitl.Add(obj);
                            break;
                        default:
                            break;

                    }
                }

                var viewmodel = new RulesViewModel
                {
                    ListLoaiTaiLieu = listloaitl,
                    ListLoaiHinhDN = listloaidn,
                    ListTin = listloaitin,
                    ListDoiTuongAD = listdoituongad

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

        public IActionResult RuleViewComponent(int Id)
        {
            return ViewComponent("RuleDetail", Id);
        }

        [EzAuthorize(RoleCode.QL_RULES, view: true)]
        public async Task<IActionResult> GetCommonType()
        {
            try
            {
                var commonType = new CommonTypeViewModel
                {
                    //ListCategory = "3,4,101,102,103,104,105,106,107,108,109,110,111"

                    ListCategory = "3,4,11,16,18"
                };

                var listCommontype = await _commonTypeContext.Select(commonType);

                var viewmodel = new RulesViewModel
                {
                    ListLoaiTaiLieu = listCommontype.Where(a => a.Category == ConstCommonTypes.NHOM_TAI_LIEU).OrderBy(a => a.Ord),
                    ListLoaiHinhDN = listCommontype.Where(a => a.Category == ConstCommonTypes.NHOM_LOAI_HINH).OrderBy(a => a.Ord),
                    ListNienDoBCTC = listCommontype.Where(a => a.Category == ConstCommonTypes.NIEN_DO_BCTC).OrderBy(a => a.Ord),
                    ListDoiTuongAD = listCommontype.Where(a => a.Category == ConstCommonTypes.NHOM_DOI_TUONG).OrderBy(a => a.Ord)
                };

                return Json(viewmodel);
            }
            catch (Exception)
            {
                return Json(new RulesViewModel { ListLoaiTaiLieu = null, ListLoaiHinhDN = null, ListNienDoBCTC = null });
            }

        }

        [EzAuthorize(RoleCode.QL_RULES, view: true)]
        public async Task<IActionResult> GetBieuMauCBTT(string cpnyTypes, int tempType = 0)
        {
            try
            {
                if (cpnyTypes == null) return Json(null);

                var arrStr = cpnyTypes.Split(',');

                List<int> numbers = new List<int>();
                for (int i = 0; i < arrStr.Length; i++)
                {
                    if (string.IsNullOrEmpty(arrStr[i])) break;
                    numbers.Add(int.Parse(arrStr[i]));
                }

                var listBieuMauCBTT = await _templateContext.Select(new TemplateViewModel { });

                if (listBieuMauCBTT == null && listBieuMauCBTT.Count() < 1) return Json(null);

                if (cpnyTypes == "-1")
                {
                    switch (tempType)
                    {
                        case 1:
                            return Json(listBieuMauCBTT[0]);
                        case 2:
                            return Json(listBieuMauCBTT[1]);
                        case 3:
                            return Json(listBieuMauCBTT[2]);
                        default:
                            return Json(null);
                    }
                }

                switch (tempType)
                {
                    case 1:
                        return Json(listBieuMauCBTT[0].Where(a => numbers.Contains(a.CompanyType ?? 0)));
                    case 2:
                        return Json(listBieuMauCBTT[1].Where(a => numbers.Contains(a.CompanyType ?? 0)));
                    case 3:
                        return Json(listBieuMauCBTT[2].Where(a => numbers.Contains(a.CompanyType ?? 0)));
                    default:
                        return Json(null);
                }
            }
            catch (Exception)
            {
                return Json(null);
            }

        }

        /// <summary>
        /// Thêm mới Rules
        /// </summary>
        /// <param name="rulesViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(LinksRoute.InsertRules)]
        [EzAuthorize(RoleCode.QL_RULES, view: true)]
        public async Task<IActionResult> InsertRules(RulesViewModel rulesViewModel)
        {
            try
            {
                rulesViewModel.UserName = _username;
                rulesViewModel.RoleCode = _roleCode;

                if (!string.IsNullOrEmpty(rulesViewModel.UserName) || !string.IsNullOrEmpty(rulesViewModel.RoleCode))
                {
                    // Tài khoản admin có thể tạo mới / tra cứu / sửa / xóa được tất cả rules CBTT.TK chuyên viên thường chỉ tra cứu và xem chi chi tiết rules CBTT nhưng không được quyền sửa / xóa.
                    string CheckAccount = _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE);
                    if (CheckAccount == "ADMIN" || CheckAccount == "FCF Admin Group")
                    {
                        rulesViewModel.LoaiHinhIDs = rulesViewModel.LoaiHinhIDs.Remove(rulesViewModel.LoaiHinhIDs.Length - 1);
                        if (rulesViewModel.StrADate != null) rulesViewModel.ADate = DateTime.ParseExact(rulesViewModel.StrADate, "yyyy-M-d HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        if (rulesViewModel.StrADateExtend != null) rulesViewModel.ADateExtend = DateTime.ParseExact(rulesViewModel.StrADateExtend, "yyyy-M-d HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                        var cResponse = await _rulesContext.Insert(rulesViewModel);

                        cResponse.Message = _sharedLocalizer[cResponse.Message].Value;

                        return Json(cResponse);
                    }
                    else
                    {
                        var responses = new CResponseMessage { Code = -3, Message = "Tài khoản đang dùng không có quyền tạo rules!" };
                        return Json(responses);
                    }
                }

                var response = new CResponseMessage { Code = -3, Message = "Tài khoản đang dùng không có quyền tạo rules!" };

                return Json(response);
            }
            catch (Exception e)
            {
                var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

                return Json(response);
            }
        }

        /// <summary>
        /// Tìm kiếm Rules
        /// </summary>
        /// <param name="rulesContext"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(LinksRoute.GetRules)]
        [EzAuthorize(RoleCode.QL_RULES, view: true)]
        public async Task<IActionResult> RulesList(RulesViewModel rulesViewModel)
        {
            rulesViewModel.UserName = _username;
            rulesViewModel.RoleCode = _roleCode;

            var listBieuMauCBTT = await _templateContext.Select(new TemplateViewModel { });
            var listTemp = new List<TemplateModelView>();

            // khi click vào nút Edit thì sẽ vào đây, lấy bản ghi của rule cần sửa
            if (rulesViewModel.ACompanyType == null)
            {
                var data = await _rulesContext.Select(rulesViewModel);

                return Json(data);
            }

            // Lấy tất cả Biểu mẫu CBTT theo Loại biểu mẫu
            if (rulesViewModel.ACompanyType == -1)
            {
                switch (rulesViewModel.LoaiBieuMau)
                {
                    case 1: // VN
                        listTemp = listBieuMauCBTT[0].ToList();
                        break;
                    case 2: // EN
                        listTemp = listBieuMauCBTT[1].ToList();
                        break;
                    case 3: // Song ngữ
                        listTemp = listBieuMauCBTT[2].ToList();
                        break;
                }
            }
            // Lấy Biểu mẫu CBTT theo Loại biểu mẫu và Loại công ty
            else 
            {                              
                switch (rulesViewModel.LoaiBieuMau)
                {
                    case 1: // VN
                        listTemp = listBieuMauCBTT[0].Where(a => a.CompanyType == rulesViewModel.ACompanyType).ToList(); 
                        break;
                    case 2: // EN
                        listTemp = listBieuMauCBTT[1].Where(a => a.CompanyType == rulesViewModel.ACompanyType).ToList();
                        break;
                    case 3: // Song ngữ
                        listTemp = listBieuMauCBTT[2].Where(a => a.CompanyType == rulesViewModel.ACompanyType).ToList();
                        break;
                }
            }

            // Danh sách id của Biểu mẫu CBTT
            var tempIDs = listTemp.Select(a => a.TemplateID).ToList();

            if (!string.IsNullOrEmpty(rulesViewModel.UserName) && !string.IsNullOrEmpty(rulesViewModel.RoleCode))
            {
                var data = await _rulesContext.Select(rulesViewModel);

                // Lọc rule theo danh sách Biểu mẫu số
                data = data.Where(a => tempIDs.Contains(a.ABieuMauSo));

                return Json(data);
            }

            var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }

        /// <summary>
        /// Sửa Rules
        /// </summary>
        /// <param name="rulesViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(LinksRoute.UpdateRules)]
        [EzAuthorize(RoleCode.QL_RULES, view: true, edit: true)]
        public async Task<IActionResult> EditRules(RulesViewModel rulesViewModel)
        {
            try
            {
                rulesViewModel.UserName = _username;
                rulesViewModel.RoleCode = _roleCode;



                if (!string.IsNullOrEmpty(rulesViewModel.UserName) && !string.IsNullOrEmpty(rulesViewModel.RoleCode))
                {
                    // Tài khoản admin có thể tạo mới / tra cứu / sửa / xóa được tất cả rules CBTT.TK chuyên viên thường chỉ tra cứu và xem chi chi tiết rules CBTT nhưng không được quyền sửa / xóa.
                    string CheckAccount = _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE);
                    if (CheckAccount == "ADMIN" || CheckAccount == "FCF Admin Group")
                    {
                        rulesViewModel.LoaiHinhIDs = rulesViewModel.LoaiHinhIDs.Remove(rulesViewModel.LoaiHinhIDs.Length - 1);
                        if (rulesViewModel.StrADate != null) rulesViewModel.ADate = DateTime.ParseExact(rulesViewModel.StrADate, "yyyy-M-d HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        if (rulesViewModel.StrADateExtend != null) rulesViewModel.ADateExtend = DateTime.ParseExact(rulesViewModel.StrADateExtend, "yyyy-M-d HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                        var cResponse = await _rulesContext.Update(rulesViewModel);
                        cResponse.Message = _sharedLocalizer[cResponse.Message].Value;
                        return Json(cResponse);
                    }
                    else
                    {
                        var responses = new CResponseMessage { Code = -3, Message = "Tài khoản đang dùng không có quyền sửa rules!" };
                        return Json(responses);
                    }
                }

                var response = new CResponseMessage { Code = -3, Message = "Tài khoản đang dùng không có quyền sửa rules!" };

                return Json(response);
            }
            catch (Exception e)
            {
                var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

                return Json(response);
            }
        }


        /// <summary>
        /// Xóa Rules
        /// </summary>
        /// <param name="rulesViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(LinksRoute.DeleteRules)]
        [EzAuthorize(RoleCode.QL_RULES, view: true, delete: true)]
        public async Task<IActionResult> DeleteRules(RulesViewModel rulesViewModel)
        {

            rulesViewModel.UserName = _username;
            rulesViewModel.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(rulesViewModel.UserName) && !string.IsNullOrEmpty(rulesViewModel.RoleCode))
            {
                // Tài khoản admin có thể tạo mới / tra cứu / sửa / xóa được tất cả rules CBTT.TK chuyên viên thường chỉ tra cứu và xem chi chi tiết rules CBTT nhưng không được quyền sửa / xóa.
                string CheckAccount = _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE);
                if (CheckAccount == "ADMIN" || CheckAccount == "FCF Admin Group")
                {
                    var cResponse = await _rulesContext.Delete(rulesViewModel);
                    cResponse.Message = _sharedLocalizer[cResponse.Message].Value;
                    return Json(cResponse);
                }
                else
                {
                    var responses = new CResponseMessage { Code = -3, Message = "Tài khoản đang dùng không có quyền xóa rules!" };
                    return Json(responses);
                }

            }

            var response = new CResponseMessage { Code = -3, Message = "Tài khoản đang dùng không có quyền xóa rules!" };

            return Json(response);
        }

        /// <summary>
        /// Xuất báo cáo Excel
        /// </summary>
        /// <param name="rulesViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [EzAuthorize(RoleCode.QL_RULES, view: true, edit: true)]
        public async Task<IActionResult> ExportExcel(RulesViewModel rulesViewModel)
        {
            rulesViewModel.UserName = _username;
            rulesViewModel.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(rulesViewModel.UserName) && !string.IsNullOrEmpty(rulesViewModel.RoleCode))
            {
                //lấy ra loại biểu mẫu
                var listBieuMauCBTT = await _templateContext.Select(new TemplateViewModel { });
                var listTemp = new List<TemplateModelView>();
                var listTemp1 = new List<TemplateModelView>();
                var listTemp2 = new List<TemplateModelView>();
                //TV
                listTemp = listBieuMauCBTT[0].ToList();
                //TA
                listTemp1 = listBieuMauCBTT[1].ToList();
                //SN
                listTemp2 = listBieuMauCBTT[2].ToList();

                var tempIDs = listTemp.Select(a => a.TemplateID).Distinct().ToList();
                var tempIDs1 = listTemp1.Select(a => a.TemplateID).Distinct().ToList();
                var tempIDs2 = listTemp2.Select(a => a.TemplateID).Distinct().ToList();

                var listdataRules = await _rulesContext.Listrules(rulesViewModel);
                //add loại biểu mẫu vào datatable
                listdataRules.Columns.Add("LOAIBIEUMAU", Type.GetType("System.String"));
                // so sánh
                for (int i = 0; i < listdataRules.Rows.Count; i++)
                {
                    try
                    {
                        string name = listdataRules.Rows[i]["ABIEUMAUSO"].ToString();
                        var BieuMauSo = Convert.ToInt32(name);
                        for (int j = 0; j < tempIDs.Count; j++)
                        {

                            if (BieuMauSo == tempIDs[j])
                            {
                                // add row cho cột
                                listdataRules.Rows[i].SetField("LOAIBIEUMAU", "Tiếng Việt");
                            }

                        }

                        for (int m = 0; m < tempIDs1.Count; m++)
                        {

                            if (BieuMauSo == tempIDs1[m])
                            {
                                listdataRules.Rows[i].SetField("LOAIBIEUMAU", "Tiếng Anh");
                            }

                        }
                        for (int k = 0; k < tempIDs2.Count; k++)
                        {

                            if (BieuMauSo == tempIDs2[k])
                            {

                                listdataRules.Rows[i].SetField("LOAIBIEUMAU", "Song ngữ");
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }


                }

                var fileResultStream = GenerateExcelFileToStream(listdataRules);

                return this.File(fileResultStream, "application/vnd.ms-excel");
            }

            var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }

        // check quyền
        [HttpPost]
        [EzAuthorize(RoleCode.QL_RULES, view: true, edit: true)]
        public async Task<IActionResult> UpdateCheck()
        {
            string CheckAccount = _httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.GROUP_CODE);
            if (CheckAccount == "ADMIN" || CheckAccount == "FCF Admin Group")
            {
                var responses = new CResponseMessage { Code = 0, Message = "" };
                return Json(responses);
            }
            else
            {
                var responses = new CResponseMessage { Code = -1, Message = "Tài khoản đang dùng không có quyền sửa thông tin này" };
                return Json(responses);
            }
        }

        private MemoryStream GenerateExcelFileToStream(DataTable dataTableRulesList)
        {
            IWorkbook workbook = new XSSFWorkbook();

            ISheet excelSheet = workbook.CreateSheet("danhsachrules");
            IRow row = excelSheet.CreateRow(0);
            //int k = 0;
            for (int i = 0; i < dataTableRulesList.Columns.Count; i++)
            {
                if (
                    dataTableRulesList.Columns[i].ColumnName != "ARULEID"
                    && dataTableRulesList.Columns[i].ColumnName != "ADOCTYPE"
                    && dataTableRulesList.Columns[i].ColumnName != "ANIENDOBCTC"
                    && dataTableRulesList.Columns[i].ColumnName != "ABIEUMAUSO"
                    && dataTableRulesList.Columns[i].ColumnName != "STT"
                    && dataTableRulesList.Columns[i].ColumnName != "ACOMPANYTYPE"
                    && dataTableRulesList.Columns[i].ColumnName != "ANEWTYPE"
                    && dataTableRulesList.Columns[i].ColumnName != "ATYPECODE"
                    && dataTableRulesList.Columns[i].ColumnName != "ACATEGORY"
                    && dataTableRulesList.Columns[i].ColumnName != "SXLT"
                    && dataTableRulesList.Columns[i].ColumnName != "ORD0"

                    )
                {
                    if (dataTableRulesList.Columns[i].ColumnName == "AQDCBTT")
                        row.CreateCell(i).SetCellValue(dataTableRulesList.Columns[i].ColumnName.Replace("AQDCBTT", "Quy định CBTT"));
                    if (dataTableRulesList.Columns[i].ColumnName == "LOAIBIEUMAU")
                        row.CreateCell(i).SetCellValue(dataTableRulesList.Columns[i].ColumnName.Replace("LOAIBIEUMAU", "Thông tin Loại biểu mẫu"));

                    if (dataTableRulesList.Columns[i].ColumnName == "ADATE")
                        row.CreateCell(i).SetCellValue(dataTableRulesList.Columns[i].ColumnName.Replace("ADATE", "Ngày tính từ thời điểm (theo Deadline CBTT thường)"));

                    if (dataTableRulesList.Columns[i].ColumnName == "ADATEEXTEND")
                        row.CreateCell(i).SetCellValue(dataTableRulesList.Columns[i].ColumnName.Replace("ADATEEXTEND", "Ngày tính từ thời điểm (theo Deadline CBTT gia hạn)"));

                    if (dataTableRulesList.Columns[i].ColumnName == "ACCPLCBTT")
                        row.CreateCell(i).SetCellValue(dataTableRulesList.Columns[i].ColumnName.Replace("ACCPLCBTT", "Căn cứ pháp lý CBTT"));

                    if (dataTableRulesList.Columns[i].ColumnName == "ATENBIEUMAUSO")
                        row.CreateCell(i).SetCellValue(dataTableRulesList.Columns[i].ColumnName.Replace("ATENBIEUMAUSO", "Biểu mẫu số"));
                    if (dataTableRulesList.Columns[i].ColumnName == "ATENNIENDOBCTC")
                        row.CreateCell(i).SetCellValue(dataTableRulesList.Columns[i].ColumnName.Replace("ATENNIENDOBCTC", "Niên độ BCTC"));
                    if (dataTableRulesList.Columns[i].ColumnName == "ALOAITAILIEU")
                        row.CreateCell(i).SetCellValue(dataTableRulesList.Columns[i].ColumnName.Replace("ALOAITAILIEU", "Loại tài liệu"));
                    if (dataTableRulesList.Columns[i].ColumnName == "DATESEARCH")
                        row.CreateCell(i).SetCellValue(dataTableRulesList.Columns[i].ColumnName.Replace("DATESEARCH", "Thời hạn CBTT"));
                    if (dataTableRulesList.Columns[i].ColumnName == "ALOAIDOANHNGHIEP")
                        row.CreateCell(i).SetCellValue(dataTableRulesList.Columns[i].ColumnName.Replace("ALOAIDOANHNGHIEP", "Loại doanh nghiệp"));
                    if (dataTableRulesList.Columns[i].ColumnName == "ALOAITIN")
                        row.CreateCell(i).SetCellValue(dataTableRulesList.Columns[i].ColumnName.Replace("ALOAITIN", "Loại tin"));
                    if (dataTableRulesList.Columns[i].ColumnName == "ADEADLINE")
                        row.CreateCell(i).SetCellValue(dataTableRulesList.Columns[i].ColumnName.Replace("ADEADLINE", "DeadlineCBTT"));
                    if (dataTableRulesList.Columns[i].ColumnName == "ADEADLINE_GIAHAN")
                        row.CreateCell(i).SetCellValue(dataTableRulesList.Columns[i].ColumnName.Replace("ADEADLINE_GIAHAN", "DeadlineCBTT (gia hạn)"));
                    //k++;

                }

            }

            for (int j = 0; j < dataTableRulesList.Rows.Count; j++)
            {
                int k = j + 1;
                row = excelSheet.CreateRow(j + 1);
                //int t = 0;
                for (int i = 0; i < dataTableRulesList.Columns.Count; i++)
                {
                    if (
                        dataTableRulesList.Columns[i].ColumnName != "ARULEID"
                        && dataTableRulesList.Columns[i].ColumnName != "ADOCTYPE"
                        && dataTableRulesList.Columns[i].ColumnName != "ANIENDOBCTC"
                        && dataTableRulesList.Columns[i].ColumnName != "ABIEUMAUSO"
                        && dataTableRulesList.Columns[i].ColumnName != "STT"
                        && dataTableRulesList.Columns[i].ColumnName != "ACOMPANYTYPE"
                        && dataTableRulesList.Columns[i].ColumnName != "ANEWTYPE"
                        && dataTableRulesList.Columns[i].ColumnName != "ATYPECODE"
                        && dataTableRulesList.Columns[i].ColumnName != "ACATEGORY"
                        && dataTableRulesList.Columns[i].ColumnName != "SXLT"
                        && dataTableRulesList.Columns[i].ColumnName != "ORD0"
                         )
                    {
                        row.CreateCell(i).SetCellValue(dataTableRulesList.Rows[j][dataTableRulesList.Columns[i].ColumnName].ToString());

                        //t++;
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

        /// <summary>
        /// Load loại tài liệu, loại tin
        /// </summary>
        /// <param name="rulesViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(LinksRoute.GetNewsType)]
        [EzAuthorize(RoleCode.QL_RULES, view: true)]
        public async Task<IActionResult> GetRules(RulesViewModel rulesViewModel)
        {

            rulesViewModel.UserName = _username;
            rulesViewModel.RoleCode = _roleCode;

            List<CommonType> listNewTypes = new List<CommonType>();

            if (!string.IsNullOrEmpty(rulesViewModel.UserName) && !string.IsNullOrEmpty(rulesViewModel.RoleCode))
            {

                var commontypes = new CommonTypeViewModel
                {
                    ListCategory = rulesViewModel.ADocType.ToString()
                };

                var listtailieu = await _commonTypeContext.Select(commontypes);


                foreach (var listtl in listtailieu.ToList())
                {
                    if (listtl.TypeCode == rulesViewModel.ADocType.ToString())
                    {
                        listNewTypes.Add(listtl);
                    }
                    return Json(listNewTypes);
                }
            }

            var response = new CResponseMessage { Code = -3, Message = _sharedLocalizer["YouNotHavePermission"].Value };

            return Json(response);
        }
    }

}
