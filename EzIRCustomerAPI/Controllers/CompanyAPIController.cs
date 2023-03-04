using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CommonLib.Interfaces.Logger;
using CoreLib.Commons;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Entities.ModelViews;
using CoreLib.Entities.ViewModels;
using EzIRCustomerAPI.Commons;
using EzIRCustomerAPI.DataAccess.Interfaces;
using EzIRCustomerAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EzIRCustomerAPI.Controllers
{
    [ApiController]
    public class CompanyAPIController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICompanyContext _companyContext;
        private readonly IChangeInfoContext _changeInfoContext;
        private readonly IAppLogger _appLogger;
        private readonly IEmailSender _emailSender;

        public CompanyAPIController(IConfiguration configuration, ICompanyContext companyContext, IAppLogger appLogger, IEmailSender emailSender, IChangeInfoContext changeInfoContext)
        {
            _configuration = configuration;
            _companyContext = companyContext;
            _appLogger = appLogger;
            _emailSender = emailSender;
            _changeInfoContext = changeInfoContext;
        }

        /// <summary>
        /// API lấy thông các công ty trong DB Temp
        /// </summary>
        /// <param name="cpnyID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoute.GET_COMPANY)]
        public IActionResult GetCompanies(int? cpnyID, int? appove)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion

                List<CompanyEzSearchTemp> companiesEzIR = _companyContext.ListCompanyEzIR(cpnyID, appove).ToList();
                List<CompanyEzSearchTemp> companiesEzSearch = _companyContext.ListCompanyEzSearch(cpnyID).ToList();

                if (companiesEzIR.Count() > 0) 
                {
                    // Lấy nghề nghiệp từ ezsearch thêm vào ezir
                    companiesEzIR.FirstOrDefault().minisnameen = companiesEzSearch.FirstOrDefault().minisnameen;
                    companiesEzIR.FirstOrDefault().minisnamevn = companiesEzSearch.FirstOrDefault().minisnamevn;

                
                    var companyType = (List<CompanyType>)_companyContext.ListCompanyType();

                    companiesEzIR.FirstOrDefault().CompanyTypeDesc = companyType.Find(a => a.AType == companiesEzIR.FirstOrDefault().ACpnyType).ADesc;

                    return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = companiesEzIR }); 
                }

                return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = companiesEzSearch });
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        /// <summary>
        /// API lấy role công ty trong DB 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoute.SPEZIR_ROLE_GET_BY_COMPANY)]
        public IActionResult GetRoleCompany(int? cpnyid)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion

                var rolecpny = (List<RoleCompanyModelView>)_companyContext.GetRoleCompany(cpnyid);

                return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = rolecpny });
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        /// <summary>
        /// API lấy thông tin các mốc lịch sử trong DB Temp
        /// </summary>
        /// <param name="cpnyID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoute.GET_DEVELOP_PROGRESS)]
        public IActionResult GetDevelopProgress(int? cpnyID, int? status, string language)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion

                var developProgressesEzIR = (List<DevelopProgress>)_companyContext.ListDevelopProgressEzIR(cpnyID, status, language);
                var developProgressesEzSearch = (List<DevelopProgress>)_companyContext.ListDevelopProgressEzSearch(cpnyID, language);

                if (developProgressesEzIR.Count() > 0)
                {
                    if (developProgressesEzSearch.Count() == 0) return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = developProgressesEzIR });

                    developProgressesEzIR.ForEach(progress =>
                    {
                        // tìm và xóa bỏ data ở db EzSearch rồi add lại data ở EzIR vào --- (trường hợp update)
                        var index = developProgressesEzSearch.FindIndex(a => a.CpnyID == progress.CpnyID && a.EventID == progress.EventID);

                        if (index >= 0)
                        {
                            developProgressesEzSearch.RemoveAt(index);
                            developProgressesEzSearch.Add(progress);
                        }
                        
                        // nếu aid == 0 || aid == null => add data vào luôn --- (trường hợp insert)
                        if (progress.EventID == 0 || progress.EventID == null || index <0) developProgressesEzSearch.Add(progress);
                    });
                }

                return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = developProgressesEzSearch });
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        [HttpPost]
        [Route(ApiRoute.INSERT_COMPANY_DEVELOP_PROGRESS)]
        public async Task<IActionResult> InsertCompanyDevelopProgress(CompanyInfo companyInfo)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion

                CResponseMessage cResponseMessage = new CResponseMessage { Code = -1, Message = "InvalidInputData" };

                if (companyInfo == null) return Ok(cResponseMessage);

                //thông tin công ty
                 if (companyInfo.Company.ACpnyID != null)
                {
                    var companiesEzSearch = _companyContext.ListCompanyEzSearch((int)companyInfo.Company.ACpnyID);

                    if (companiesEzSearch.Count() <= 0 || companiesEzSearch == null) return Ok(cResponseMessage);

                    ChangeInfoCompany(companiesEzSearch.FirstOrDefault(), companyInfo.Company);

                    cResponseMessage = _companyContext.UpdateCompanyInfomation(companiesEzSearch.FirstOrDefault());

                    // insert các thông tin thay đổi vào bảng ChangeInfo
                    if (cResponseMessage.Code == 0)
                    {
                        var company = companyInfo.Company;
                        var arr = company.ANote.Split(';');

                        foreach (var item in arr)
                        {
                            if (string.IsNullOrEmpty(item)) break;
                            _changeInfoContext.Insert(new ChangeInfo
                            {
                                CpnyID = (int)company.ACpnyID
                                                                       ,
                                Key = item
                                                                       ,
                                //thêm điều kiện cho phép nhập thông tin thay đổi là null 
                                /*Value = CheckUtils.GetPropValue(company, item) == null ? null : CheckUtils.GetPropValue(company, item)?.ToString()*/
                                Value =  CheckUtils.GetPropValue(company, item)?.ToString()
                                                                       ,
                                Status = 2
                                                                       ,
                                KeyFunction = $"Thông tin chung - {getTitleByPropertyName(item)}"
                                                                       ,
                                MenuID = ConstMenuID.THONG_TIN_CHUNG
                                                                       ,
                                Function = "Thông tin chung"
                                                                       ,
                                LevelID = "TQ"
                                                                       ,
                                DetailLevelID = "TTC"
                            }
                            );
                        }
                    }
                }

                // Các mốc lịch sử
                if (companyInfo.DevelopProgresses != null && companyInfo.DevelopProgresses.Count > 0)
                {
                    var developProgressesEzSearch = (List<DevelopProgress>)_companyContext.ListDevelopProgressEzSearch(companyInfo.DevelopProgresses.FirstOrDefault().CpnyID, "VN");

                    int? i = 0;
                    foreach (var developProgress in companyInfo.DevelopProgresses)
                    {
                        DevelopProgress develop = new DevelopProgress();

                        if (developProgress.Approve == 2)
                        {
                            develop = developProgressesEzSearch.Where(a => a.EventID == developProgress.EventID).FirstOrDefault();
                            i = develop.EventID+1;
                        }
                        else
                        {
                            developProgress.EventID = i; i = i+1;
                        }

                        if (string.IsNullOrEmpty(developProgress.Language) || developProgress.Language != "VN")
                        {
                            developProgress.EventDescEN = developProgress.EventDesc;
                            developProgress.EventDateEN = developProgress.EventDate;
                            developProgress.EventDesc = develop.EventDesc;
                            developProgress.EventDate = develop.EventDate;
                        } 
                        else
                        {
                            developProgress.EventDescEN = develop.EventDescEN ?? "";
                            developProgress.EventDateEN = develop.EventDateEN ?? "";                            
                        }

                        cResponseMessage = _companyContext.InsertDevelopProgress(developProgress);

                        // insert các thông tin thay đổi vào bảng ChangeInfo
                        if (cResponseMessage.Code == 0)
                        {
                            var arr = developProgress.Note.Split(';');

                            foreach (var item in arr)
                            {
                                if (string.IsNullOrEmpty(item)) break;
                                _changeInfoContext.Insert(new ChangeInfo
                                {
                                    CpnyID = developProgress.CpnyID,
                                    Key = item,
                                    Value = CheckUtils.GetPropValue(developProgress, item)?.ToString(),
                                    Status = developProgress.EventID == 0 ? 1 : 2,
                                    KeyFunction = $"Các mốc lịch sử - {getTitleByPropertyName(item)}",
                                    MenuID = ConstMenuID.CAC_MOC_LICH_SU,
                                    Function = "Thông tin chung",
                                    LevelID = "TQ",
                                    DetailLevelID = "TTC"
                                }
                                );
                            }
                        }
                    }


                }

                if (cResponseMessage.Code == 0)
                {
                    cResponseMessage.Message = "MSG_TEZIR_INSERT_SUCCESSFUL";

                    var response = await this._emailSender.SendEmailAsync(
                                                                companyInfo.EmailSettings.Mail,
                                                                companyInfo.EmailSettings.Subject,
                                                                companyInfo.EmailSettings.Message,
                                                                this._configuration["EmailSettings:Sender"],
                                                                this._configuration["EmailSettings:MailServer"],
                                                                Convert.ToInt32(this._configuration["EmailSettings:MailPort"]),
                                                                this._configuration["EmailSettings:Username"],
                                                                this._configuration["EmailSettings:Password"]
                                                               );
                    _appLogger.InfoLogger.LogInfo($"Mail: {companyInfo.EmailSettings.Mail} - Subject: {companyInfo.EmailSettings.Subject} - Status: {response.Message}");
                }

                return Ok(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        // gán lại các trường bị thay đổi giá trị
        private void ChangeInfoCompany(CompanyEzSearchTemp companyReal, CompanyEzSearchTemp companyChanged)
        {
            if (string.IsNullOrEmpty(companyChanged.Language) || companyChanged.Language == "VN")
            {
                companyReal.AHeadOffice = companyChanged.AHeadOffice ?? null; //companyReal.AHeadOffice
            }
            else
            {
                companyReal.AHeadOfficeEN = companyChanged.AHeadOffice ?? null; //companyReal.AHeadOfficeEN
            }
            //kiểm tra nếu giá trị thay đổi khác null thì gán giá trị thay đổi vào dữ liệu gốc, còn bằng null thì gán rỗng vào giá trị gốc
            //companyReal.APhone = companyChanged.APhone ?? companyReal.APhone;
            companyReal.APhone = companyChanged.APhone ?? null;
            companyReal.AFax = companyChanged.AFax ?? null;
            companyReal.AEmail = companyChanged.AEmail ?? null;
            companyReal.AWebsite = companyChanged.AWebsite ?? null;
            companyReal.ATaxCode = companyChanged.ATaxCode ?? null;
            companyReal.AReg_Biz_No = companyChanged.AReg_Biz_No ?? null;
            companyReal.ALogoPath = companyChanged.ALogoPath ?? companyReal.ALogoPath;
            companyReal.ALogoImage = companyChanged.ALogoImage ?? companyReal.ALogoImage;
            companyReal.ANote = companyChanged.ANote;
            
        }

        private string getTitleByPropertyName(string propertyName)
        {
            switch (propertyName)
            {
                case ConstPropertiesName.TRU_SO_CHINH:
                    {
                        return "Trụ sở chính";
                    }
                case ConstPropertiesName.DIEN_THOAI:
                    {
                        return "Điện thoại";
                    }
                case ConstPropertiesName.FAX:
                    {
                        return "Fax";
                    }
                case ConstPropertiesName.EMAIL:
                    {
                        return "Email";
                    }
                case ConstPropertiesName.WEBSITE:
                    {
                        return "Website";
                    }
                case ConstPropertiesName.MST:
                    {
                        return "MST";
                    }
                case ConstPropertiesName.DANG_KY_KINH_DOANH:
                    {
                        return "Đăng ký kinh doanh";
                    }
                case ConstPropertiesName.MOC_THOI_GIAN:
                    {
                        return "Mốc thời gian";
                    }
                case ConstPropertiesName.SU_KIEN_TIEU_BIEU:
                    {
                        return "Sự kiện tiêu biểu";
                    }
                default:
                    return "";
            }
        }
    }
}
