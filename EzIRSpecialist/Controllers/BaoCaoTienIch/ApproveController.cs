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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EzIRSpecialist.Controllers.BaoCaoTienIch
{
    public class ApproveController : Controller
    {
        private readonly IApproveContext _approveContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppLogger _appLogger;
        public ApproveController(IApproveContext approveContext, IHttpContextAccessor httpContextAccessor, IAppLogger appLogger)
        {
            _approveContext = approveContext;
            _httpContextAccessor = httpContextAccessor;
            _appLogger = appLogger;
        }


        // GET: ApproveController
        [EzAuthorize(RoleCode.PHE_DUYET_THONG_TIN, view: true)]
        public ActionResult Index()
        {
            var companys = _approveContext.GetListCompanyApprove();

            return View(companys);
        }

        [HttpGet]
        [EzAuthorize(RoleCode.PHE_DUYET_THONG_TIN, view: true)]
        public IActionResult GetApproveInfo(int cpnyID)
        {
            var functions = _approveContext.GetListFunctionApprove(cpnyID);

            return Json(functions);
        }

        [HttpPost]
        [EzAuthorize(RoleCode.PHE_DUYET_THONG_TIN, view: true, edit: true)]
        public IActionResult ApproveInfo(int cpnyID, string detaiLevelID, int approve)
        {
            
            int? authorID = 0;//_httpContextAccessor.HttpContext.Session.GetString(ConstantsSessionName.USERNAME);
            var modifierID = authorID;

            CResponseMessage cResponseMessage = new CResponseMessage { Code = -1, Message = "Phê duyệt không thành công, vui lòng phê duyệt sau 5 phút nữa!" };

            var getChangeInfos = GetChangeInfos(cpnyID, detaiLevelID);

            getChangeInfos = getChangeInfos.Where(a => a.Status == 1 || a.Status == 2).ToList();

            var menuIDs = getChangeInfos.Select(a => a.MenuID).Distinct().ToList();

            if (getChangeInfos != null && getChangeInfos.Count > 0)
            {                
                foreach (var menuId in menuIDs)
                {
                    var item = getChangeInfos.Where(a => a.MenuID == menuId).FirstOrDefault();
                    int? approved = (approve == 0) ? item.Status : approve;
                    _appLogger.InfoLogger.LogInfo("approved: " + approved.ToString());
                    _appLogger.InfoLogger.LogInfo("menuID: " + item.MenuID.ToString());
                    cResponseMessage = SwitchFunctionApprove(cpnyID, detaiLevelID, approved ?? 0, item.MenuID ?? 0, authorID ?? 0, modifierID ?? 0);
                }

                if (cResponseMessage.Code == -1) cResponseMessage.Message = "Phê duyệt không thành công, vui lòng phê duyệt sau 5 phút nữa!";

                return Json(cResponseMessage);
            }

            if (cResponseMessage.Code == -1) cResponseMessage.Message = "Phê duyệt không thành công, vui lòng phê duyệt sau 5 phút nữa!";

            return Json(cResponseMessage);
        }

        private CResponseMessage SwitchFunctionApprove(int cpnyID, string detaiLevelID, int approve, int menuid, int authorID, int modifierID)
        {
            try
            {
                _appLogger.InfoLogger.LogInfo("SwitchFunctionApprove_cpnyID: " + cpnyID.ToString());
                _appLogger.InfoLogger.LogInfo("SwitchFunctionApprove_approved: " + approve.ToString());
                _appLogger.InfoLogger.LogInfo("SwitchFunctionApprove_menuID: " + menuid.ToString());
                _appLogger.InfoLogger.LogInfo("SwitchFunctionApprove_detaiLevelID: " + detaiLevelID.ToString());
                _appLogger.InfoLogger.LogInfo("SwitchFunctionApprove_authorID: " + authorID.ToString());
                _appLogger.InfoLogger.LogInfo("SwitchFunctionApprove_modifierID: " + modifierID.ToString());

                switch (detaiLevelID)
                {
                    // Thông tin chung
                    case "TTC":
                        {
                            CResponseMessage cResponse = new CResponseMessage();

                            if (menuid == 311) cResponse = _approveContext.ApproveInfoCompany(cpnyID, menuid, approve);
                            if (menuid == 672) cResponse = _approveContext.ApproveDevelopprogess(cpnyID, menuid, approve);

                            return cResponse;
                        }
                    // Tầm nhìn chiến lược
                    case "TNCL":
                        return _approveContext.ApproveInfoSheet(cpnyID, menuid, approve, authorID, modifierID);

                    // Tổ chức bộ máy Quản lý
                    case "TCBMQL":
                        return _approveContext.ApproveOrgModel(cpnyID, menuid, approve);

                    // Thành phần lãnh đạo
                    case "TPLD":
                        return _approveContext.ApproveManager(cpnyID, menuid, approve);

                    // Cơ cấu sở hữu
                    case "CCSH":
                        {
                            CResponseMessage cResponse = new CResponseMessage();
                            if (menuid == 677)
                            {
                                cResponse = _approveContext.ApproveSubCompany(cpnyID, menuid, approve);
                                if (cResponse.Code != 0) return cResponse;
                            }
                            else if (menuid == 678)
                            {
                                cResponse = _approveContext.ApproveCompanyHolder(cpnyID, menuid, approve);
                                if (cResponse.Code != 0) return cResponse;
                            }
                            return cResponse;
                        }


                    // Thị trường/Khách hàng/Đối thủ
                    case "TTKHDT":
                        return _approveContext.ApproveInfoSheet(cpnyID, menuid, approve, authorID, modifierID);

                    // Trình độ công nghệ
                    case "TDCN":
                        return _approveContext.ApproveInfoSheet(cpnyID, menuid, approve, authorID, modifierID);

                    // Năng lực quản lý
                    case "NLQL":
                        return _approveContext.ApproveInfoSheet(cpnyID, menuid, approve, authorID, modifierID);

                    // Thông tin dự án đầu tư
                    case "TTDADT":
                        return _approveContext.ApproveInfoSheet(cpnyID, menuid, approve, authorID, modifierID);

                    // Phân tích SWOT
                    case "PTSWOT":
                        return _approveContext.ApproveInfoSheet(cpnyID, menuid, approve, authorID, modifierID);

                    // Vị thế doanh nghiệp
                    case "VTDN":
                        return _approveContext.ApproveInfoSheet(cpnyID, menuid, approve, authorID, modifierID);

                    default:
                        return new CResponseMessage { Code = -1, Message = "Phê duyệt không thành công, vui lòng phê duyệt sau 5 phút nữa!" };
                }
            }
            catch (Exception ex)
            {
                _appLogger.InfoLogger.LogInfo("SwitchFunctionApprove_error: " + ex.ToString());
                return new CResponseMessage { Code = -1, Message = "Xảy ra lỗi trong quá trình phê duyệt!Vui lòng check lại!" };
            }         
        }

        private List<ChangeInfo> GetChangeInfos(int cpnyID, string detailLevalID)
        {
            return _approveContext.GetListChangeInfo(cpnyID).Where(a => a.DetailLevelID == detailLevalID).ToList();
        }
    }
}
