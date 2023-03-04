using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Entities.ViewModels;
using EzIRCustomerAPI.Commons;
using EzIRCustomerAPI.DataAccess.Interfaces;
using EzIRCustomerAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.Controllers
{
    [ApiController]
    public class ListManagerAPIController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILoginContext _loginContext;
        private readonly IChangeInfoContext _changeInfoContext;
        private readonly IAppLogger _appLogger;
        private readonly IEmailSender _emailSender;
        private readonly IManagerContext _managerContext;
        private readonly IKnowLedgeLevelContext _knowLedgeLevelContext;
        private readonly IManagerOrgContext _managerOrgContext;
        public ListManagerAPIController(IConfiguration configuration, ILoginContext loginContext, IAppLogger appLogger, IManagerContext managerContext, IEmailSender emailSender, IChangeInfoContext changeInfoContext, IKnowLedgeLevelContext knowLedgeLevelContext, IManagerOrgContext managerOrgContext)
        {
            _managerOrgContext = managerOrgContext;
            _managerContext = managerContext;
            _configuration = configuration;
            _loginContext = loginContext;
            _appLogger = appLogger;
            _emailSender = emailSender;
            _changeInfoContext = changeInfoContext;
            _knowLedgeLevelContext = knowLedgeLevelContext;
        }

        [HttpGet]
        [Route(ApiRoute.Get_Chucvu)]
        public async Task<IActionResult> GetListChucvu(string ListManager)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion
                var managerOrg = new ManagerOrgViewModel
                {
                    ListManager = ListManager
                };


                var listManager = await _managerOrgContext.Select(managerOrg);
                return Ok(new CResponseMessage { Code = 200, Message = "Ok", Data = listManager });
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = 401, Message = "Unauthorized" });
            }

        }

        [HttpGet]
        [Route(ApiRoute.Get_KnowLedgeLevel)]
        public async Task<IActionResult> GetListKnowLedgeLevel(string ListKnowLedgeLevel)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion
                var knowLedgeLevel = new KnowLedgeLevelViewModel
                {
                    KnowLedgeLevels = ListKnowLedgeLevel
                };


                var listKnowLedgeLevel = await _knowLedgeLevelContext.Select(knowLedgeLevel);
                return Ok(new CResponseMessage { Code = 200, Message = "Ok", Data = listKnowLedgeLevel });
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = 401, Message = "Unauthorized" });
            }

        }

        [HttpGet]
        [Route(ApiRoute.Get_Manager)]
        public IActionResult GetManager([FromHeader] ManagerViewModel managerViewModel)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion
                if (managerViewModel.MNGERID == 0 || managerViewModel.MNGERID == null)
                {
                    var ManagersEzIR = (List<Manager>)_managerContext.SelectEzIR(managerViewModel);
                    var ManagersEzSearch = (List<Manager>)_managerContext.SelectEzSearch(managerViewModel);
                    var data = new List<Manager>();
                    if (managerViewModel.MNGID == 0 || managerViewModel.MNGID == null)
                    {
                        if (ManagersEzIR.Count > 0)
                        {
                            if (ManagersEzSearch.Count == 0) return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = ManagersEzIR });
                            //if(ManagersEzIR.Count > ManagersEzSearch.Count) return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = ManagersEzIR });
                            // so sánh data giữa 2 db 
                            foreach (var manager in ManagersEzIR)
                            {
                                if(manager.MNGERID == 0)
                                {
                                    ManagersEzSearch.Add(manager);
                                }else
                                {
                                    var index = ManagersEzSearch.FindIndex(a => a.MNGERID == manager.MNGERID && a.CPNYID == manager.CPNYID);
                                    if (index >= 0)
                                    {
                                        ManagersEzSearch.RemoveAt(index);
                                        ManagersEzSearch.Add(manager);
                                    }
                                }
                                
                                
                            }
                            ;

                            //foreach (var item in ManagersEzSearch)
                            //{
                            //    if (item.MNGERID == 0)
                            //    {
                            //        data.Add(item);
                            //    }
                            //}
                            //var data1 = ManagersEzSearch.Where(a => a.MNGERID != 0).ToList(); //.OrderBy(d => d.ORD)
                            //foreach (var item in data1)
                            //{
                            //    data.Add(item);
                            //}
                            data = ManagersEzSearch.OrderByDescending(x => x.ORD.HasValue).ThenBy(x=>x.ORD).ToList();
                            return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = data });
                        }
                         data = ManagersEzSearch.OrderByDescending(x => x.ORD.HasValue).ThenBy(x=>x.ORD).ToList(); //.OrderBy(d => d.ORD)
                        return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = data });
                    }
                    else
                    {
                        return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = ManagersEzIR.OrderByDescending(x => x.ORD.HasValue).ThenBy(x=>x.ORD).ToList() });

                    }                

                }
                else
                {
                    var ManagersEzIR = (List<Manager>)_managerContext.SelectEzIR(managerViewModel);
                    var ManagersEzSearch = (List<Manager>)_managerContext.SelectEzSearch(managerViewModel);
                    var data = new List<Manager>();
                    if (ManagersEzIR.Count > 0)
                        {
                            if (ManagersEzSearch.Count == 0) return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = ManagersEzIR });
                            // so sánh data giữa 2 db 
                            ManagersEzIR.ForEach(manager => {
                                // tìm và xóa bỏ data ở db EzSearch rồi add lại data ở EzIR vào --- (trường hợp update)
                                var index = ManagersEzSearch.FindIndex(a => a.MNGERID == manager.MNGERID && a.CPNYID == manager.CPNYID);
                                if (index >= 0)
                                {
                                    ManagersEzSearch.RemoveAt(index);
                                    ManagersEzSearch.Add(manager);
                                }
                                else
                                {
                                    ManagersEzSearch.Add(manager);
                                }

                                //// nếu aid == 0 || aid == null => add data vào luôn --- (trường hợp insert)
                                //if (manager.MNGERID == 0 || manager.MNGERID == null)
                                //{
                                //    ManagersEzSearch.Add(manager);
                                //}
                            });

                            data = ManagersEzSearch.OrderByDescending(x => x.ORD.HasValue).ThenBy(x=>x.ORD).ToList(); 
                        
                            return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = data });
                        }
                    data = ManagersEzSearch.OrderByDescending(x => x.ORD.HasValue).ThenBy(x=>x.ORD).ToList();  //.OrderBy(d => d.ORD)
                    return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = data });

                }

            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        [HttpPost]
        [Route(ApiRoute.Save_Manager)]
        public async Task<IActionResult> SaveManager([FromBody] ManagerViewModel managerViewModel, [FromHeader] string roleCode, [FromHeader] string username)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion
                if (managerViewModel == null)
                    return Ok(new CResponseMessage { Code = -1, Message = "InvalidInputData" });
                CResponseMessage cResponseMessage = new CResponseMessage { Code = -1, Message = "UnknowError" };
                var manager = managerViewModel;
                {
                    if (manager == null || manager.CPNYID == null)
                        return Ok(new CResponseMessage { Code = -1, Message = "InvalidInputData" });



                    if (manager.ID == null || manager.ID == 0)
                    {
                        cResponseMessage = await _managerContext.Insert(managerViewModel);
                        // insert các thông tin thay đổi vào bảng ChangeInfo
                        if (cResponseMessage.Code == 0)
                        {                         
                              
                            foreach (var prop in manager.GetType().GetProperties())
                            {
                                var title = prop.Name;
                                var value = prop.GetValue(manager);
                                if (value != null && (title == "MORGID" || title == "MNGERNAME" || title == "KNOWLEDGELEVELID" || title == "ISLEGALREP" || title == "DATEOFBIRTHVN" || title == "NATIONALITY" || title == "KNOWLEDGESPECIALLEVEL" || title == "KNOWLEDGESPECIALLEVELEN" || title == "CVPATH"))
                                {
                                    if (title == "MORGID")
                                    {
                                        value = getValuebyMorgID(manager.MORGID);
                                    }
                                    if(title == "KNOWLEDGELEVELID")
                                    {
                                        value = manager.KNOWLEDGELEVELID;
                                    }
                                    if(title == "ISLEGALREP")
                                    {
                                        value = manager.ISLEGALREP;
                                    }
                                    if (title == "ORD")
                                    {
                                        value = manager.ORD;
                                    }
                                    _changeInfoContext.Insert(new ChangeInfo
                                    {
                                        CpnyID = manager.CPNYID,
                                        MenuID = ConstMenuID.THANH_PHAN_LANH_DAO,
                                        Key = title,
                                        Value = value.ToString(),
                                        Status =  1,
                                        KeyFunction = $"Thành phần lãnh đạo - {title}",
                                        Function = "Thành phần lãnh đạo",
                                        LevelID = "TQ",
                                        DetailLevelID = "TPLD"
                                    });
                                }

                            }

                        }

                        if (cResponseMessage.Code == 0)
                        {
                            await this._emailSender.SendEmailAsync(
                                                                managerViewModel.EmailSettings.Mail,
                                                                managerViewModel.EmailSettings.Subject,
                                                                managerViewModel.EmailSettings.Message,
                                                                this._configuration["EmailSettings:Sender"],
                                                                this._configuration["EmailSettings:MailServer"],
                                                                Convert.ToInt32(this._configuration["EmailSettings:MailPort"]),
                                                                this._configuration["EmailSettings:Username"],
                                                                this._configuration["EmailSettings:Password"]
                                                               );
                            _appLogger.InfoLogger.LogInfo($"Mail: {managerViewModel.EmailSettings.Mail} - Subject: {managerViewModel.EmailSettings.Subject} - Status: {cResponseMessage.Message}");
                        }
                    }
                    else
                    {
                        return Ok(cResponseMessage);
                    }
                    return Ok(cResponseMessage);
                }
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        private string getValuebyMorgID(string morgids)
        {

            char[] separators = new char[] { ',' };

            string[] subs = morgids.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            List<string> listmanager = new List<string>();

            foreach (var morgid in subs)
            {
                var managerorg = "";
                {
                    if (morgid == ConstManagerOrg.CHU_TICH_HĐQT)
                    {
                        managerorg = "Chủ tịch HĐQT";
                    }
                    else if (morgid == ConstManagerOrg.PHO_CHU_TICH_HĐQT)
                    {
                        managerorg = "Phó Chủ tịch HĐQT";
                    }
                    else if (morgid == ConstManagerOrg.THANH_VIEN_HĐQT)
                    {
                        managerorg = "Thành viên HĐQT";
                    }
                    else if (morgid == ConstManagerOrg.NGUOI_DUOC_UY_QUYEN_CONG_BO_THONG_TIN)
                    {
                        managerorg = "Người được ủy quyền công bố thông tin";
                    }
                    else if (morgid == ConstManagerOrg.TONG_GIAM_DOC)
                    {
                        managerorg = "Tổng Giám đốc";
                    }
                    else if (morgid == ConstManagerOrg.PHO_TONG_GIAM_DOC)
                    {
                        managerorg = "Phó Tổng giám đốc";
                    }
                    else if (morgid == ConstManagerOrg.GIAM_DỐC_DIEU_HANH)
                    {
                        managerorg = "Giám đốc điều hành";
                    }
                    else if (morgid == ConstManagerOrg.KE_TOAN_TRUONG)
                    {
                        managerorg = "Kế toán trưởng";
                    }
                    else if (morgid == ConstManagerOrg.GIAM_DOC_TAI_CHINH)
                    {
                        managerorg = "Giám đốc Tài chính";
                    }
                    else if (morgid == ConstManagerOrg.TRUONG_BAN_KS)
                    {
                        managerorg = "Trưởng ban KS";
                    }
                    else if (morgid == ConstManagerOrg.THANH_VIEN_BAN_KS)
                    {
                        managerorg = "Thành viên ban KS";
                    }
                    else if (morgid == ConstManagerOrg.GIAM_DOC)
                    {
                        managerorg = "Giám đốc";
                    }
                    else if (morgid == ConstManagerOrg.PHO_GIAM_DOC)
                    {
                        managerorg = "Phó Giám đốc";
                    }
                    else if (morgid == ConstManagerOrg.TRUONG_PHONG)
                    {
                        managerorg = "Trưởng phòng";
                    }
                    else if (morgid == ConstManagerOrg.THANH_VIEN_HĐQT_DOC_LAP)
                    {
                        managerorg = "Thành viên HĐQT độc lập";
                    }
                    else if (morgid == ConstManagerOrg.CHU_TICH_UY_BAN_KIEM_TOAN)
                    {
                        managerorg = "Chủ tịch ủy ban kiểm toán";
                    }
                    else if (morgid == ConstManagerOrg.THANH_VIEN_UY_BAN_KIEM_TOAN)
                    {
                        managerorg = "Thành viên ủy ban kiểm toán";
                    }
                    else if (morgid == ConstManagerOrg.NGUOI_PHU_TRACH_QUAN_TRI_CONG_TY)
                    {
                        managerorg = "Người phụ trách quản trị công ty";
                    }


                };
                listmanager.Add(managerorg);

            }
            var json = JsonConvert.SerializeObject(listmanager);
            json.Replace(@"\", " ");
            return json;



        }

        /*private string getValuebyIsLegalRep(int? isLegalRep)
        {
            var IsLegalRep = "";
            if(isLegalRep == 1)
            {
                IsLegalRep = "True";
            }
            else if(isLegalRep == 0)
            {
                IsLegalRep = "False";
            }
            else
            {
                IsLegalRep = null;
            }
            return IsLegalRep;
        }*/

       /* private string getValuebyKnowLedgeLevelId(int? KnowLedgeLevelId)
        {
            var knowLedgeLevel = "";
                {
                    if (KnowLedgeLevelId == ConstKnowLedgeLevelId.TREN_DH)
                    {
                    knowLedgeLevel = "Trên ĐH";
                    }
                    else if (KnowLedgeLevelId == ConstKnowLedgeLevelId.DAI_HOC)
                    {
                    knowLedgeLevel = "Đại Học";
                    }
                    else if (KnowLedgeLevelId == ConstKnowLedgeLevelId.TRUNG_CAP)
                    {
                    knowLedgeLevel = "Trung cấp";
                    }
                    else if (KnowLedgeLevelId == ConstKnowLedgeLevelId.CONG_NHAN)
                    {
                    knowLedgeLevel = "Công nhân";
                    }
                    else if (KnowLedgeLevelId == ConstKnowLedgeLevelId.LD_PHO_THONG)
                    {
                    knowLedgeLevel = "LĐ phổ thông";
                    }
                    else if (KnowLedgeLevelId == ConstKnowLedgeLevelId.TIEN_SY)
                    {
                    knowLedgeLevel = "Tiến sỹ";
                    }
                    else if (KnowLedgeLevelId == ConstKnowLedgeLevelId.THAC_SY)
                    {
                    knowLedgeLevel = "Thạc sỹ";
                    }
                    else if (KnowLedgeLevelId == ConstKnowLedgeLevelId.CU_NHAN)
                    {
                    knowLedgeLevel = "Cử nhân";
                    }
                    else if (KnowLedgeLevelId == ConstKnowLedgeLevelId.HET_PHO_THONG)
                    {
                    knowLedgeLevel = "12/12";
                    }
                    else if (KnowLedgeLevelId == ConstKnowLedgeLevelId.HET_TRUNG_HOC)
                    {
                    knowLedgeLevel = "10/10";
                    }
                    else if (KnowLedgeLevelId == ConstKnowLedgeLevelId.CAO_DANG)
                    {
                    knowLedgeLevel = "Cao đẳng";
                    }
                    else if (KnowLedgeLevelId == ConstKnowLedgeLevelId.TRUNG_HOC)
                    {
                    knowLedgeLevel = "Trung học";
                    }
                    else if (KnowLedgeLevelId == ConstKnowLedgeLevelId.CHUA_HET_PHO_THONG)
                    {
                    knowLedgeLevel = "11/12";
                    }
                    else if (KnowLedgeLevelId == ConstKnowLedgeLevelId.KY_SU)
                    {
                    knowLedgeLevel = "Kỹ Sư";
                    }
                    else
                    {
                    knowLedgeLevel = null;
                    }

                return knowLedgeLevel;

                }

        }*/

        private string getTitleByMenuID(int? menuid)
        {
            switch (menuid)
            {
                case ConstMenuID.THANH_PHAN_LANH_DAO:
                    {
                        return "Thành phần lãnh đạo";
                    }
                default:
                    return "";
            }
        }

        [HttpPost]
        [Route(ApiRoute.Update_Manage)]
        public async Task<IActionResult> UpdateManager([FromBody] ManagerViewModel managerViewModel, [FromHeader] string roleCode, [FromHeader] string username)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion
                if (managerViewModel == null)
                    return Ok(new CResponseMessage { Code = -1, Message = "InvalidInputData" });
                CResponseMessage cResponseMessage = new CResponseMessage { Code = -1, Message = "UnknowError" };
                var manager = managerViewModel;
                {
                    if (manager == null || manager.CPNYID == null)
                        return Ok(new CResponseMessage { Code = -1, Message = "InvalidInputData" });



                    if (manager.ID == null || manager.ID == 0)
                    {
                        cResponseMessage = await _managerContext.Update(managerViewModel);
                        // insert các thông tin thay đổi vào bảng ChangeInfo
                        if (cResponseMessage.Code == 0)
                        {

                            foreach (var prop in manager.GetType().GetProperties())
                            {
                                var title = prop.Name;
                                var value = prop.GetValue(manager);
                                if (value != null && (title == "MORGID" || title == "MNGERNAME" || title == "KNOWLEDGELEVELID" || title == "ISLEGALREP" || title == "DATEOFBIRTHVN" || title == "NATIONALITY" || title == "KNOWLEDGESPECIALLEVEL" || title == "KNOWLEDGESPECIALLEVELEN" || title == "CVPATH" || title == "ORD"))
                                {
                                    if (title == "MORGID")
                                    {
                                        value = getValuebyMorgID(manager.MORGID);
                                    }
                                    if (title == "KNOWLEDGELEVELID")
                                    {
                                        value = manager.KNOWLEDGELEVELID;
                                    }
                                    if (title == "ISLEGALREP")
                                    {
                                        value = manager.ISLEGALREP;
                                    }
                                    if (title == "ORD")
                                    {
                                        value = manager.ORD;
                                    }
                                    _changeInfoContext.Insert(new ChangeInfo
                                    {
                                        CpnyID = manager.CPNYID,
                                        MenuID = ConstMenuID.THANH_PHAN_LANH_DAO,
                                        Key = title,
                                        Value = value.ToString(),
                                        Status = 2,
                                        KeyFunction = $"Thành phần lãnh đạo - {title}",
                                        Function = "Thành phần lãnh đạo",
                                        LevelID = "TQ",
                                        DetailLevelID = "TPLD"
                                    });
                                }

                            }

                        }

                        if (cResponseMessage.Code == 0)
                        {
                            await this._emailSender.SendEmailAsync(
                                                                managerViewModel.EmailSettings.Mail,
                                                                managerViewModel.EmailSettings.Subject,
                                                                managerViewModel.EmailSettings.Message,
                                                                this._configuration["EmailSettings:Sender"],
                                                                this._configuration["EmailSettings:MailServer"],
                                                                Convert.ToInt32(this._configuration["EmailSettings:MailPort"]),
                                                                this._configuration["EmailSettings:Username"],
                                                                this._configuration["EmailSettings:Password"]
                                                               );
                            _appLogger.InfoLogger.LogInfo($"Mail: {managerViewModel.EmailSettings.Mail} - Subject: {managerViewModel.EmailSettings.Subject} - Status: {cResponseMessage.Message}");
                        }
                    }
                    else
                    {
                        return Ok(cResponseMessage);
                    }
                    return Ok(cResponseMessage);
                }
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

    }
}
