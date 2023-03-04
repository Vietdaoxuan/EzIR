using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Entities.ViewModels;
using EzIRSpecialist.Authentication;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models.ViewModel;
using CoreLib.Entities.ModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using CoreLib.Commons;
using Microsoft.AspNetCore.Hosting;
using System.Net;

namespace EzIRSpecialist.Controllers.BaoCaoTienIch
{
    public class ViewApproveController : Controller
    {
        private readonly IHtmlLocalizer<SharedResource> _sharedLocalizer;
        private readonly IConfiguration _configuration;
        private readonly ICompanyContext _iCompanyContext;
        private readonly IThongTinPheDuyetContext _iThongTinPheDuyetContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private List<IEnumerable<CompanyEzSearchTemp>> _companyEzSearch;
        private string _BASE_API_URL;
        private readonly IAppLogger _appLogger;
        private readonly IHostingEnvironment _hostingEnvironment;
        string imgUrl = "";
        string imgComapyUrl = "";
        string imgLogo = "";
        private string _username { get; set; }
        private string _roleCode { get; set; }

        public ViewApproveController(IConfiguration configuration, IAppLogger appLogger, IHtmlLocalizer<SharedResource> sharedLocalizer, ICompanyContext iCompanyContext, IThongTinPheDuyetContext iThongTinPheDuyetContext, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment)
        {

            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _sharedLocalizer = sharedLocalizer;
            _iCompanyContext = iCompanyContext;
            _iThongTinPheDuyetContext = iThongTinPheDuyetContext;
            _httpContextAccessor = httpContextAccessor;
            _BASE_API_URL = _configuration.GetSection("ApiUrl").Value;
            _appLogger = appLogger;
             imgUrl = _configuration.GetSection("CkeditorImgUrl").Value; 
             imgComapyUrl = _configuration.GetSection("Upload").Value;
             imgLogo = _configuration.GetSection("LogoApprove").Value;
            _username = _httpContextAccessor.HttpContext.Session.GetString(SessionTypes.USERNAME);
            _roleCode = RoleCode.THONG_TIN_PHE_DUYET;

            _companyEzSearch = _iCompanyContext.ListCommontype();

        }

        [EzAuthorize(RoleCode.THONG_TIN_PHE_DUYET, view: true)]
        public IActionResult Index()
        {

            //2021-09-04 17:21:08 OanhNTT so sánh data giữa 2 db EzIR và EzSearch
            var listEzIR = (List<CompanyEzSearchTemp>)_iCompanyContext.ListCompanyEdit();
            //var listEzSearch = (List<CompanyEzSearchTemp>)_iCompanyContext.ListCompanyEditSearch();

            var list = _companyEzSearch;
            var lisTT = list[0];

            //if (listEzIR.ToList().Count > 0)
            //{
            //    if (listEzSearch.ToList().Count == 0)
            //        listEzSearch = listEzIR;

            //    // so sánh data giữa 2 db 
            //    listEzIR.ToList().ForEach(listcompany => {

            //        // tìm và xóa bỏ data ở db EzSearch rồi add lại data ở EzIR vào --- (trường hợp update)
            //        var index = listEzSearch.ToList().FindIndex(a => a.ACpnyID == listcompany.ACpnyID);
            //        if (index >= 0)
            //        {
            //            listEzSearch.RemoveAt(index);

            //            listEzSearch.Add(listcompany);
            //        }
            //        else
            //        {
            //            listEzSearch.Add(listcompany);
            //        }
            //    });

            //}

            var viewmodel = new ThongTinPheDuyetViewModel
            {
                listMCKEzSearch = listEzIR.ToList(),
                listTT = lisTT.ToList()

            };

            return View(viewmodel);
        }

        //2021-09-04 17:21:08 OanhNTT so sánh data giữa 2 db EzIR và EzSearch
        public IActionResult DanhSachThongTinChiTiet(string alevelID, double? cpnyID)
        {
            var listCommonType = (List<CompanyEzSearchTemp>)_iCompanyContext.ListCommontype()[1];
            //var listCommonTypeSearch = (List<CompanyEzSearchTemp>)_iCompanyContext.ListCommontypeSearch()[1];


            //if (listCommonType.ToList().Count > 0)
            //{
            //    if (listCommonTypeSearch.ToList().Count == 0)
            //        return Json(listCommonType.Where(a => a.ALEVELID == alevelID && a.ACpnyID == cpnyID));

            //    //// so sánh data giữa 2 db

            //    listCommonType.ForEach(listcommon => {

            //        if (listcommon.ACpnyID == cpnyID)
            //        {
            //            if (listCommonTypeSearch.Where(a => a.ACpnyID == cpnyID && a.ATypename == listcommon.ATypename).Count() > 0)
            //            {
            //                var index = listCommonTypeSearch.ToList().FindIndex(a => a.ACpnyID == cpnyID && a.ATypename == listcommon.ATypename);
            //                listCommonTypeSearch.RemoveAt(index);
            //            }
            //            listCommonTypeSearch.Add(listcommon);

            //        }
            //    });
            //}

            //danh sách thông tin chi tiết theo ALEVELID 
            var listTTCT = listCommonType.Where(a => a.ALEVELID == alevelID && a.ACpnyID == cpnyID);
            return Json(listTTCT);

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="swot"></param>
        /// <returns></returns>
        [HttpGet]
        [EzAuthorize(RoleCode.THONG_TIN_PHE_DUYET, view: true)]
        public IActionResult GetSwot(ThongTinPheDuyetViewModel thongTin)
        {
            thongTin.Username = _username;
            thongTin.RoleCode = _roleCode;

            if (!string.IsNullOrEmpty(thongTin.Username) && !string.IsNullOrEmpty(thongTin.RoleCode))
            {
                var data = CompareData(thongTin).Where(a => (a.MenuID == ConstMenuID.SWOT_DIEM_MANH || a.MenuID == ConstMenuID.SWOT_DIEM_YEU || a.MenuID == ConstMenuID.SWOT_CO_HOI || a.MenuID == ConstMenuID.SWOT_THACH_THUC) && (a.Approve == 1 || a.Approve == 2));

                //var data = _iThongTinPheDuyetContext.SelectSearch(thongTin).Where(a => (a.MenuID == ConstMenuID.SWOT_DIEM_MANH || a.MenuID == ConstMenuID.SWOT_DIEM_YEU || a.MenuID == ConstMenuID.SWOT_CO_HOI || a.MenuID == ConstMenuID.SWOT_THACH_THUC) && (a.Approve == 1 || a.Approve == 2));
                return PartialView("_Swot", data.ToList());
            }
            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["loi"].Value };
            return Json(response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trình độ công nghệ"></param>
        /// <returns></returns>
        [HttpGet]
        [EzAuthorize(RoleCode.THONG_TIN_PHE_DUYET, view: true)]
        public IActionResult GetTrinhDoCongNghe(ThongTinPheDuyetViewModel thongTin)
        {
            thongTin.Username = _username;
            thongTin.RoleCode = _roleCode;
            if (!string.IsNullOrEmpty(thongTin.Username) && !string.IsNullOrEmpty(thongTin.RoleCode))
            {
                thongTin.MenuID = ConstMenuID.TRINH_DO_CONG_NGHE;
                var data = CompareData(thongTin).Where(a => a.Approve == 1 || a.Approve == 2);
                return PartialView("_TrinhDoCongNghe", data.ToList());
            }
            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["loi"].Value };
            return Json(response);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Năng lực quản lý"></param>
        /// <returns></returns>
        [HttpGet]
        [EzAuthorize(RoleCode.THONG_TIN_PHE_DUYET, view: true)]
        public IActionResult GetNangLucQuanLy(ThongTinPheDuyetViewModel thongTin)
        {
            thongTin.Username = _username;
            thongTin.RoleCode = _roleCode;
            if (!string.IsNullOrEmpty(thongTin.Username) && !string.IsNullOrEmpty(thongTin.RoleCode))
            {
                var data = CompareData(thongTin).Where(a => (a.MenuID == ConstMenuID.NANG_LUC_QUAN_LY_QUAN_LY_CHAT_LUONG || a.MenuID == ConstMenuID.NANG_LUC_QUAN_LY_QUAN_TRI_TAI_CHINH || a.MenuID == ConstMenuID.NANG_LUC_QUAN_LY_QUAN_TRI_NHAN_SU || a.MenuID == ConstMenuID.NANG_LUC_QUAN_LY_CAC_HE_THONG_QUAN_LY_KHAC) && (a.Approve == 1 || a.Approve == 2));
                return PartialView("_NangLucQuanLy", data.ToList());
            }
            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["loi"].Value };
            return Json(response);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tầm nhìn chiến lược"></param>
        /// <returns></returns>
        [HttpGet]
        [EzAuthorize(RoleCode.THONG_TIN_PHE_DUYET, view: true)]
        public IActionResult GetTamNhinChienLuoc(ThongTinPheDuyetViewModel thongTin)
        {
            ViewBag.imgurl = imgUrl;
            thongTin.Username = _username;
            thongTin.RoleCode = _roleCode;
            if (!string.IsNullOrEmpty(thongTin.Username) && !string.IsNullOrEmpty(thongTin.RoleCode))
            {
                var data = CompareData(thongTin).Where(a => (a.MenuID == ConstMenuID.TAM_NHIN || a.MenuID == ConstMenuID.CHIEN_LUOC) && (a.Approve == 1 || a.Approve == 2)).Distinct().ToList();
                return PartialView("_TamNhinChienLuoc", data.ToList());
            }
            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["loi"].Value };
            return Json(response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thông tin chung"></param>
        /// <returns></returns>
        [HttpGet]
        [EzAuthorize(RoleCode.THONG_TIN_PHE_DUYET, view: true)]
        public IActionResult GetThongTinChung(ThongTinPheDuyetViewModel thongTin)
        {
            ViewBag.imgLogo = imgLogo;
            // thông tin công ty
            thongTin.Status = 2;
            var responseCompanies = CompareDataCompany(thongTin).ToList();
            // cán mốc lịch sử
            //var data = CompareDataDevelopProgress(thongTin).Where(a => a.Approve == 1 || a.Approve == 2);
            var data = CompareDataDevelopProgress(thongTin);
            data = data.OrderByDescending(x => x.EventOrder.HasValue).ThenBy(x=>x.EventOrder);
            var obj = new ThongTinChungViewModel
            {
                listTTC = responseCompanies,
                listCMLS = data.ToList(),
            };
            return PartialView("_ThongTinChung", obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thông tin dự án đầu tư"></param>
        /// <returns></returns>
        [HttpGet]
        [EzAuthorize(RoleCode.THONG_TIN_PHE_DUYET, view: true)]
        public IActionResult GetThongTinDuAnDT(ThongTinPheDuyetViewModel thongTin)
        {
            thongTin.Username = _username;
            thongTin.RoleCode = _roleCode;
            if (!string.IsNullOrEmpty(thongTin.Username) && !string.IsNullOrEmpty(thongTin.RoleCode))
            {
                thongTin.MenuID = ConstMenuID.THONG_TIN_DU_AN;
                var data = CompareData(thongTin).Where(a => a.Approve == 1 || a.Approve == 2);
                return PartialView("_ThongTinDuANDT", data.ToList());
            }
            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["loi"].Value };
            return Json(response);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Vị thế doanh nghiệp"></param>
        /// <returns></returns>
        [HttpGet]
        [EzAuthorize(RoleCode.THONG_TIN_PHE_DUYET, view: true)]
        public IActionResult GetViTheDoanhNghiep(ThongTinPheDuyetViewModel thongTin)
        {
            thongTin.Username = _username;
            thongTin.RoleCode = _roleCode;
            if (!string.IsNullOrEmpty(thongTin.Username) && !string.IsNullOrEmpty(thongTin.RoleCode))
            {
                var data = CompareData(thongTin).Where(a => (a.MenuID == ConstMenuID.VI_THE_DOANH_NGHIEP_VI_THE_CHUNG || a.MenuID == ConstMenuID.VI_THE_DOANH_NGHIEP_VI_THE_TRONG_TUNG_LINH_VUC_KINH_DOANH) && (a.Approve == 1 || a.Approve == 2));
                return PartialView("_ViTheDoanhNghiep", data.ToList());
            }
            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["loi"].Value };
            return Json(response);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thị trường khách hàng đối thủ"></param>
        /// <returns></returns>
        [HttpGet]
        [EzAuthorize(RoleCode.THONG_TIN_PHE_DUYET, view: true)]
        public IActionResult GetKhachHangDoiThu(ThongTinPheDuyetViewModel thongTin)
        {
            ViewBag.imgurl = imgUrl;
            thongTin.Username = _username;
            thongTin.RoleCode = _roleCode;
            if (!string.IsNullOrEmpty(thongTin.Username) && !string.IsNullOrEmpty(thongTin.RoleCode))
            {
                var data = CompareData(thongTin).Where(a => (a.MenuID == ConstMenuID.NGUYEN_LIEU_DAU_VAO_NCC || a.MenuID == ConstMenuID.CO_CAU_DOANH_THU || a.MenuID == ConstMenuID.SAN_PHAM_THAY_THE || a.MenuID == ConstMenuID.DOI_THU_CANH_TRANH || a.MenuID == ConstMenuID.THI_TRUONG_KHACH_HANG || a.MenuID == ConstMenuID.THI_TRUONG_KHAC_HANG_DOI_THU_KHAC) && (a.Approve == 1 || a.Approve == 2));
                return PartialView("_GetKhachHangDoiThu", data.ToList());
            }
            var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["loi"].Value };
            return Json(response);

        }
        /// <summary>
        /// tổ chức bộ máy quản lý
        /// </summary>
        /// <param name="thongTin"></param>
        /// <returns></returns>
        [HttpGet]
        [EzAuthorize(RoleCode.THONG_TIN_PHE_DUYET, view: true)]
        public IActionResult GetToChucBoMayQuanLy(ThongTinPheDuyetViewModel thongTin)
        {
            ViewBag.imgLogo = imgLogo;
            var data = CompareDataBMQL(thongTin).Where(a => a.APPROVE == 1 || a.APPROVE == 2);
            return PartialView("_ToChucBoMayQuanLy", data.ToList());
        }

        [HttpGet]
        [EzAuthorize(RoleCode.THONG_TIN_PHE_DUYET, view: true)]
        public IActionResult GetThanhPhanLanhDao(ThongTinPheDuyetViewModel thongTin)
        {
            //var data = _iThongTinPheDuyetContext.SelectTPLDEzIR(thongTin).Where(a => a.APPROVE == 1 || a.APPROVE ==2);
            var data = CompareDataTPLD(thongTin);
            return PartialView("_ThanhPhanLanhDao", data.ToList());
        }


        [HttpGet]
        [EzAuthorize(RoleCode.THONG_TIN_PHE_DUYET, view: true)]
        public IActionResult GetCoCauSoHuu(ThongTinPheDuyetViewModel thongTin)
        {
            var data = _iThongTinPheDuyetContext.SelectCCSHEzSearch(thongTin);
            // công ty con
            data[0] = data[0].OrderByDescending(a => a.aorder.HasValue).ThenBy(b => b.aorder).ToList();
     

            //công ty liên kết   
            data[1] = data[1].OrderByDescending(a => a.aorder.HasValue).ThenBy(b => b.aorder).ToList();


            //cổ đông lớn              
            data[2] = data[2].OrderByDescending(a => a.aorder.HasValue).ThenBy(b => b.aorder).ToList();

            return PartialView("_CoCauSoHuu", data.ToList());
        }



        /// <summary>
        /// Compare dữ liệu thông tin phê duyệt giữa 2 db EzIR và EzSearch
        /// 
        /// sử dụng cho:
        ///     GetSwot
        ///     GetTrinhDoCongNghe
        ///     GetNangLucQuanLy
        ///     GetTamNhinChienLuoc
        ///     GetThongTinDuAnDT
        ///     GetViTheDoanhNghiep
        ///     GetKhachHangDoiThu
        /// </summary>
        public IEnumerable<InfoSheet> CompareData(ThongTinPheDuyetViewModel thongtin)
        {
            var listTTPD = (List<InfoSheet>)_iThongTinPheDuyetContext.SelectEzIR(thongtin);
            var listTTPDSearch = (List<InfoSheet>)_iThongTinPheDuyetContext.SelectEzSearch(thongtin);

            if (listTTPD.Count > 0)
            {
                if (listTTPDSearch.Count == 0)
                    return listTTPD;

                // so sánh data giữa 2 db 
                listTTPD.ForEach(listTTPD =>
                {

                    if (listTTPD.CpnyID == thongtin.CpnyID)
                    {
                        if (listTTPDSearch.Where(a => a.CpnyID == thongtin.CpnyID && a.MenuID == listTTPD.MenuID).Count() > 0)
                        {
                            var index = listTTPDSearch.FindIndex(a => a.CpnyID == thongtin.CpnyID && a.MenuID == listTTPD.MenuID &&(a.Language==listTTPD.Language   ));

                            if (index >= 0)
                            {
                                listTTPDSearch.RemoveAt(index);
                            }

                        }
                        listTTPDSearch.Add(listTTPD);

                    }

                });

            }
            return listTTPDSearch;
        }

        /// <summary>
        /// Compare dữ liệu thông tin phê duyệt giữa 2 db EzIR và EzSearch
        /// </summary>
        public IEnumerable<CompanyEzSearchTemp> CompareDataCompany(ThongTinPheDuyetViewModel thongtin)
        {

            var listEzIR = (List<CompanyEzSearchTemp>)_iThongTinPheDuyetContext.ListCompanyEzIR(thongtin);
            var listEzSearch = (List<CompanyEzSearchTemp>)_iThongTinPheDuyetContext.ListCompanyEzSearch(thongtin);

            if (listEzIR.Count > 0)
            {
                if (listEzSearch.Count == 0)
                    return listEzIR;

                //Edit by QuangKS 13:40 08/09/2022 gán ngược lại ngành nghề
                if (listEzSearch.Count > 0)
                {
                    listEzIR[0].minisnamevn = listEzSearch[0].minisnamevn;
                    listEzIR[0].minisnameen = listEzSearch[0].minisnameen;
                }                 

                // so sánh data giữa 2 db 
                listEzIR.ForEach(listTTPD =>
                {
                    var index = listEzSearch.FindIndex(a => a.ACpnyID == thongtin.CpnyID);
                    listEzSearch.RemoveAt(index);
                    listEzSearch.Add(listTTPD);

                });

            }
            return listEzSearch;
        }

        public IEnumerable<DevelopProgress> CompareDataDevelopProgress(ThongTinPheDuyetViewModel thongtin)
        {
            var listEzIR = (List<DevelopProgress>)_iThongTinPheDuyetContext.ListDevelopProgressEzIR(thongtin);
            var listEzSearch = (List<DevelopProgress>)_iThongTinPheDuyetContext.ListDevelopProgressEzSearch(thongtin);

            if (listEzIR.Count > 0)
            {
                if (listEzSearch.Count == 0)
                    return listEzIR;

                // so sánh data giữa 2 db 
                listEzIR.ForEach(list =>
                {

                    var index = listEzSearch.FindIndex(a => a.CpnyID == list.CpnyID && a.EventID == list.EventID);

                    if (index >= 0)
                    {
                        listEzSearch.RemoveAt(index);
                        listEzSearch.Add(list);
                    }

                    if (list.EventID == 0 || list.EventID == null || index<0) {
                        listEzSearch.Add(list);
                    }
                });

            }
            return listEzSearch;
        }

        public IEnumerable<ToChucBoMayQuanLyModelView> CompareDataBMQL(ThongTinPheDuyetViewModel thongtin)
        {
            var listEzIR = (List<ToChucBoMayQuanLyModelView>)_iThongTinPheDuyetContext.SelectBMQLEzIR(thongtin);
            var listEzSearch = (List<ToChucBoMayQuanLyModelView>)_iThongTinPheDuyetContext.SelectBMQLEzSearch(thongtin);

            if (listEzIR.Count > 0)
            {
                if (listEzSearch.Count == 0)
                    return listEzIR;

                // so sánh data giữa 2 db 
                listEzIR.ForEach(list =>
                {
                    if (listEzSearch.Where(a => a.CpnyID == thongtin.CpnyID && a.OrgModelDesc == list.OrgModelDesc).Count() > 0)
                    {
                        var index = listEzSearch.FindIndex(a => a.CpnyID == thongtin.CpnyID && a.OrgModelDesc == list.OrgModelDesc);
                        listEzSearch.RemoveAt(index);
                    }
                    listEzSearch.Add(list);

                });

            }
            return listEzSearch;
        }

        public IEnumerable<Manager> CompareDataTPLD(ThongTinPheDuyetViewModel thongtin)
        {
            var listEzIR = (List<Manager>)_iThongTinPheDuyetContext.SelectTPLDEzIR(thongtin);
            var listEzSearch = (List<Manager>)_iThongTinPheDuyetContext.SelectTPLDEzSearch(thongtin);

            if (listEzIR.Count > 0)
            {
                if (listEzSearch.Count == 0)
                    return listEzIR;

                // so sánh data giữa 2 db 
                listEzIR.ForEach(list =>
                {
                    if (listEzSearch.Where(a => a.CPNYID == thongtin.CpnyID && a.MNGERID == list.MNGERID).Count() > 0)
                    {
                        var index = listEzSearch.FindIndex(a => a.CPNYID == thongtin.CpnyID && a.MNGERID == list.MNGERID);
                        listEzSearch.RemoveAt(index);
                    }
                    listEzSearch.Add(list);

                });

            }
            return listEzIR;
        }

        [HttpPost]
        public async Task<IActionResult> DownloadFile(ManagerViewModel managerViewModel)
        {
            try
            {
                managerViewModel.UserLogin = HttpContext.Session.GetString(SessionTypes.USERNAME);
                managerViewModel.CPNYID = HttpContext.Session.GetInt32(SessionTypes.CPNY_ID);
                managerViewModel.RoleCode = RoleCode.THANH_PHAN_LANH_DAO;

                string remoteUri = this._configuration["Upload"];
                string Url = managerViewModel.CVPATH;
                string myStringWebResource = null;

                myStringWebResource = remoteUri + Url;

                var filePath = myStringWebResource;

                if (!string.IsNullOrEmpty(managerViewModel.UserLogin) && !string.IsNullOrEmpty(managerViewModel.RoleCode))
                {
                    var myClient = new WebClient();
                    byte[] bytes = myClient.DownloadData(myStringWebResource);
                    var type = Path.GetExtension(myStringWebResource).Replace(".", "");
                    //var ms = new MemoryStream(CheckUtils.ReadFile(filePath));
                    //string FileExtension = Path.GetFileName(filePath).Substring(Path.GetFileName(filePath).LastIndexOf('.') + 1).ToLower();

                    /*string FileExtension = Path.GetFileName(managerViewModel.CVPATH).Substring(Path.GetFileName(managerViewModel.CVPATH).LastIndexOf('.') + 1).ToLower();*/
                    if (type == "doc" || type == "docx")
                    {
                        return File(bytes, "application/msword");
                    }
                    if (type == "pdf")
                    {
                        return File(bytes, "application/pdf");
                    }
                    if (type == "rar")
                    {
                        return File(bytes, "application/x-rar-compressed");
                    }
                }

                var response = new CResponseMessage { Code = -1, Message = _sharedLocalizer["YouNotHavePermission"].Value };
                return Json(response);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Json(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }

        }
    }
}
