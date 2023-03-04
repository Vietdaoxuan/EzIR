using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Commons;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Interfaces;
using EzIRCustomerAPI.Commons;
using EzIRCustomerAPI.DataAccess.Interfaces;
using EzIRCustomerAPI.Models;
using EzIRCustomerAPI.Models.ViewModels;
using EzIRCustomerAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.Controllers
{
    [ApiController]
    public class CoCauSoHuuAPIController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICoCauSoHuuContext _coCauSoHuuContext;
        private readonly ICommon _common;
        private readonly IErrorHandler _errorHandler;
        private readonly IAppLogger _appLogger;
        private readonly IChangeInfoContext _changeInfoContext;
        private readonly IEmailSender _emailSender;
        public CoCauSoHuuAPIController(
            IAppLogger appLogger,
            IEmailSender emailSender,
            ICoCauSoHuuContext coCauSoHuuContext,
            ICommon common,
            IConfiguration configuration,
            IErrorHandler errorHandler, IChangeInfoContext changeInfoContext)
        {
            this._common = common;
            this._configuration = configuration;
            this._errorHandler = errorHandler;
            this._coCauSoHuuContext = coCauSoHuuContext;
            this._changeInfoContext = changeInfoContext;
            this._emailSender = emailSender;
            this._appLogger = appLogger;
        }

        /// <summary>
        /// Lấy thông tin công ty con, công ty liên kết, cổ đông lớn
        /// </summary>
        /// <param name="coCauSoHuuViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoute.GetCongTy)]
        public IActionResult GetCongty([FromHeader] CoCauSoHuuViewModel coCauSoHuuViewModel)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion

                var coCauSoHuus = _coCauSoHuuContext.Select(coCauSoHuuViewModel);

                var list = new List<List<SubCompany>>();

                // công ty con
                var data = coCauSoHuus.Result[0].OrderByDescending(d => d.aapprove).ThenByDescending(a => a.aorder.HasValue).ThenBy(b => b.aorder).ToList();               
                list.Add(data);

                //công ty liên kết   
                var data2 = coCauSoHuus.Result[1].OrderByDescending(d => d.aapprove).ThenByDescending(a => a.aorder.HasValue).ThenBy(b => b.aorder).ToList();              
                list.Add(data2);

                //cổ đông lớn              
                var data3 = coCauSoHuus.Result[2].OrderByDescending(d => d.aapprove).ThenByDescending(a => a.aorder.HasValue).ThenBy(b => b.aorder).ToList();               
                list.Add(data3);

                return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = list });
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        /// <summary>
        /// Get thông tin ngành
        /// </summary>
        /// <param name="coCauSoHuuViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoute.Get_Ministry)]
        public IActionResult GetMinistry()
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion

                var ministry = _coCauSoHuuContext.Select_Misnitry();

                return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = ministry });
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coCauSoHuuViewModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoute.Get_SubCompanyType)]
        public IActionResult GetSubCompanyType([FromHeader] CoCauSoHuuViewModel coCauSoHuuViewModel)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion

                var subCompaniesType = _coCauSoHuuContext.Select_SubCompanyType(coCauSoHuuViewModel);

                return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = subCompaniesType });
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        [HttpPost]
        [Route(ApiRoute.Insert_Company_ShareHolder)]
        public async Task<IActionResult> InsertCCSH(CoCauSoHuu coCauSoHuu)
        {
            #region valid token
            var header = Request.Headers;
            var token = header["Authorization"].ToString() ?? null;
            var result = CheckValidToken.Check(token, _appLogger);
            if (result.Code != 200) return Ok(result);
            #endregion
            var cResponseMessage = new CResponseMessage();

            try
            {
                
                if (coCauSoHuu.ListSubCompany != null || coCauSoHuu.ListSubCompany.Count > 0)
                {
                    int isInsertUpdate = 1; // biến kiểm tra insert hay update
                    foreach (var subcompany in coCauSoHuu.ListSubCompany)
                    {
                        //isInsertUpdate = subcompany.aapprove == 2 ? subcompany.asubcompanyid : 0;

                        //Insert thông tin công ty con, công ty liên kết
                        cResponseMessage = _coCauSoHuuContext.InsertSubcompany(subcompany);

                        //Insert các thông tin thay đổi vào bảng ChangeInfo

                        if (cResponseMessage.Code == 0)
                        {
                            var arr = subcompany.anote.Split(';');

                            foreach (var item in arr)
                            {
                                if (string.IsNullOrEmpty(item)) break;
                                _changeInfoContext.Insert(new ChangeInfo
                                {
                                    CpnyID = subcompany.acompanyid,
                                    Key = item,
                                    Value = CheckUtils.GetPropValue(subcompany, item)?.ToString(),
                                    Status = subcompany.aapprove,
                                    KeyFunction = $"Công ty con, liên kết - {getTitleByPropertyName(item)}",
                                    MenuID = ConstMenuID.CONG_TY_CON_LIEN_KET,
                                    Function = "Cơ cấu sở hữu",
                                    LevelID = "TQ",
                                    DetailLevelID = "CCSH"
                                }//
                                );
                            }
                        }
                    }

                    if (coCauSoHuu.ListShareHolder != null && coCauSoHuu.ListShareHolder.Count > 0)
                    {
                        //int isInsertUpdateSH = 1; // biến kiểm tra insert hay update
                        foreach (var shareholder in coCauSoHuu.ListShareHolder)
                        {
                            //isInsertUpdateSH = shareholder.aapprove == 2 ? shareholder.asherid : 0;

                            //Insert thông tin vào bảng cổ đông lớn
                            cResponseMessage = _coCauSoHuuContext.InsertShareHolder(shareholder);

                            //Insert các thông tin thay đổi vào bảng ChangeInfo
                            if (cResponseMessage.Code == 0)
                            {

                                var arr = shareholder.anote.Split(';');

                                foreach (var item in arr)
                                {
                                    if (string.IsNullOrEmpty(item)) break;
                                    _changeInfoContext.Insert(new ChangeInfo
                                    {
                                        CpnyID = shareholder.acpnyid,
                                        Key = item,
                                        Value = (CheckUtils.GetPropValue(shareholder, item)??"").ToString(),
                                        Status = shareholder.aapprove ,
                                        KeyFunction = $"Cổ đông lớn - {getTitleByPropertyName(item)}",
                                        MenuID = ConstMenuID.CO_CAU_CO_DONG,
                                        Function = "Cơ cấu sở hữu",
                                        LevelID = "TQ",
                                        DetailLevelID = "CCSH"
                                    });


                                }
                            }
                        }

                    }

                }
                if (cResponseMessage.Code == 0)
                {
                    var response = await this._emailSender.SendEmailAsync(
                                                                coCauSoHuu.emailSettings.Mail,
                                                                coCauSoHuu.emailSettings.Subject,
                                                                coCauSoHuu.emailSettings.Message,
                                                                this._configuration["EmailSettings:Sender"],
                                                                this._configuration["EmailSettings:MailServer"],
                                                                Convert.ToInt32(this._configuration["EmailSettings:MailPort"]),
                                                                this._configuration["EmailSettings:Username"],
                                                                this._configuration["EmailSettings:Password"]
                                                               );
                    _appLogger.InfoLogger.LogInfo($"Mail: {coCauSoHuu.emailSettings.Mail} - Subject: {coCauSoHuu.emailSettings.Subject} - Status: {response.Message}");

                    return Ok(new CResponseMessage { Code = 0, Message = "Cập nhập thành công" });
                }

                else
                {
                    return Ok(new CResponseMessage { Code = -1, Message = "Cập nhập không thành công" });
                }

            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        private string getTitleByPropertyName(string propertyName)
        {
            switch (propertyName)
            {
                case ConstPropertiesName.TEN_CONG_TY_CON:
                    {
                        return "Tên";
                    }
                case ConstPropertiesName.DIA_CHI_CONG_TY_CON:
                    {
                        return "Địa chỉ";
                    }
                case ConstPropertiesName.THUOC_NGANH:
                    {
                        return "Thuộc ngành";
                    }
                case ConstPropertiesName.LOAI_CONG_TY_CON:
                    {
                        return "Loại hình công ty con";
                    }
                case ConstPropertiesName.NAM_GIU:
                    {
                        return "Nắm giữ";
                    }
                case ConstPropertiesName.TEN_CO_DONG:
                    {
                        return "Cổ đông";
                    }
                case ConstPropertiesName.CO_PHAN:
                    {
                        return "Cổ phần";
                    }
                case ConstPropertiesName.TY_LE:
                    {
                        return "Tỷ lệ";
                    }
                default:
                    return "";
            }
        }
    }
}
