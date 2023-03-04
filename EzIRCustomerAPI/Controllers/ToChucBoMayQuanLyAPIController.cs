using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Commons;
using CoreLib.Configs;
using CoreLib.Entities;
using EzIRCustomerAPI.Commons;
using EzIRCustomerAPI.DataAccess.Interfaces;
using EzIRCustomerAPI.Models;
using EzIRCustomerAPI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using CoreLib.Entities.ViewModels;
using EzIRCustomerAPI.Services;
using CoreLib.Entities.ModelViews;

namespace EzIRCustomerAPI.Controllers
{
    [ApiController]
    public class ToChucBoMayQuanLyAPIController : ControllerBase
    {
        private readonly IToChucBoMayQuanLyContext _toChucBoMayQuanLyContext;
        private readonly IConfiguration _configuration;
        private readonly ILoginContext _loginContext;
        private readonly IChangeInfoContext _changeInfoContext;
        private readonly IAppLogger _appLogger;
        private readonly IEmailSender _emailSender;

        public ToChucBoMayQuanLyAPIController(IConfiguration configuration, ILoginContext loginContext, IToChucBoMayQuanLyContext toChucBoMayQuanLyContext, IAppLogger appLogger, IEmailSender emailSender, IChangeInfoContext changeInfoContext)
        {
            _toChucBoMayQuanLyContext = toChucBoMayQuanLyContext;
            _configuration = configuration;
            _loginContext = loginContext;
            _emailSender = emailSender;
            _appLogger = appLogger;
            _changeInfoContext = changeInfoContext;
        }
        [HttpGet]
        [Route(ApiRoute.Get_ToChucBoMayQuanLy)]
        public IActionResult GetToChucBoMayQuanLy([FromHeader] ToChucBoMayQuanLyViewModel toChucBoMayQuanLyViewModel)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion

                var tochucbomaysEzIR = (List<ToChucBoMayQuanLyModelView>)_toChucBoMayQuanLyContext.SelectEzIR(toChucBoMayQuanLyViewModel);
                var tochucbomaysEzSearch = (List<ToChucBoMayQuanLyModelView>)_toChucBoMayQuanLyContext.SelectEzSearch(toChucBoMayQuanLyViewModel);

                if (tochucbomaysEzIR.Count > 0)
                {
                    if (tochucbomaysEzSearch.Count == 0) return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = tochucbomaysEzIR });
                    // so sánh data giữa 2 db 
                    tochucbomaysEzIR.ForEach(tochucbomay => {
                        // tìm và xóa bỏ data ở db EzSearch rồi add lại data ở EzIR vào --- (trường hợp update)
                        var index = tochucbomaysEzSearch.FindIndex(a => a.OrgModelID == tochucbomay.OrgModelID  && a.CpnyID == tochucbomay.CpnyID);
                        if (index >= 0)
                        {
                            tochucbomaysEzSearch.RemoveAt(index);
                            tochucbomaysEzSearch.Add(tochucbomay);
                        }
                           
                        // nếu aid == 0 || aid == null => add data vào luôn --- (trường hợp insert)
                        if (tochucbomay.OrgModelID == 0 || tochucbomay.OrgModelID == null) tochucbomaysEzSearch.Add(tochucbomay);
                    });

                    return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = tochucbomaysEzSearch });
                }

                return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = tochucbomaysEzSearch });
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        [HttpPost]
        [Route(ApiRoute.Insert_ToChucBoMayQuanLy)]
        public async Task<IActionResult> InsertToChucBoMayQuanLy([FromBody] ToChucBoMayQuanLyViewModel toChucBoMayQuanLyViewModel, [FromHeader] string roleCode, [FromHeader] string username)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion
                if (toChucBoMayQuanLyViewModel == null)
                    return Ok(new CResponseMessage { Code = -1, Message = "InvalidInputData" });
                CResponseMessage cResponseMessage = new CResponseMessage { Code = -1, Message = "UnknowError" };
                var toChucBoMayQuanLy = toChucBoMayQuanLyViewModel;
                {
                    if (toChucBoMayQuanLy == null || toChucBoMayQuanLy.CpnyID == null)
                        return Ok(new CResponseMessage { Code = -1, Message = "InvalidInputData" });

                    //_toChucBoMayQuanLyContext.SetRoleCode(roleCode);
                    //_toChucBoMayQuanLyContext.SetUsername(username);


                    if (toChucBoMayQuanLy.ID == null || toChucBoMayQuanLy.ID == 0)
                    {
                        cResponseMessage = _toChucBoMayQuanLyContext.Insert(toChucBoMayQuanLyViewModel);
                        if (toChucBoMayQuanLyViewModel.ChangeInfos.Count > 0) _changeInfoContext.Insert(toChucBoMayQuanLyViewModel.ChangeInfos[0]);
                        if (cResponseMessage.Code == 0)
                        {
                            await this._emailSender.SendEmailAsync(
                                                                toChucBoMayQuanLyViewModel.EmailSettings.Mail,
                                                                toChucBoMayQuanLyViewModel.EmailSettings.Subject,
                                                                toChucBoMayQuanLyViewModel.EmailSettings.Message,
                                                                this._configuration["EmailSettings:Sender"],
                                                                this._configuration["EmailSettings:MailServer"],
                                                                Convert.ToInt32(this._configuration["EmailSettings:MailPort"]),
                                                                this._configuration["EmailSettings:Username"],
                                                                this._configuration["EmailSettings:Password"]
                                                               );
                            _appLogger.InfoLogger.LogInfo($"Mail: {toChucBoMayQuanLyViewModel.EmailSettings.Mail} - Subject: {toChucBoMayQuanLyViewModel.EmailSettings.Subject} - Status: {cResponseMessage.Message}");
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
        [HttpPost]
        [Route(ApiRoute.Update_ToChucBoMayQuanLy)]
        public async Task<IActionResult> UpdateToChucBoMayQuanLy([FromBody] ToChucBoMayQuanLyViewModel toChucBoMayQuanLyViewModel, [FromHeader] string roleCode, [FromHeader] string username)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion
                if (toChucBoMayQuanLyViewModel == null)
                    return Ok(new CResponseMessage { Code = -1, Message = "InvalidInputData" });
                CResponseMessage cResponseMessage = new CResponseMessage { Code = -1, Message = "UnknowError" };
                var toChucBoMayQuanLy = toChucBoMayQuanLyViewModel;
                {
                    if (toChucBoMayQuanLy == null || toChucBoMayQuanLy.CpnyID == null)
                        return Ok(new CResponseMessage { Code = -1, Message = "InvalidInputData" });

                    //_toChucBoMayQuanLyContext.SetRoleCode(roleCode);
                    //_toChucBoMayQuanLyContext.SetUsername(username);


                    if (toChucBoMayQuanLy.ID == null || toChucBoMayQuanLy.ID == 0)
                    {
                        cResponseMessage = _toChucBoMayQuanLyContext.Update(toChucBoMayQuanLyViewModel);
                        if (toChucBoMayQuanLyViewModel.ChangeInfos.Count > 0) _changeInfoContext.Insert(toChucBoMayQuanLyViewModel.ChangeInfos[0]);
                        if (cResponseMessage.Code == 0)
                        {
                            await this._emailSender.SendEmailAsync(
                                                                toChucBoMayQuanLyViewModel.EmailSettings.Mail,
                                                                toChucBoMayQuanLyViewModel.EmailSettings.Subject,
                                                                toChucBoMayQuanLyViewModel.EmailSettings.Message,
                                                                this._configuration["EmailSettings:Sender"],
                                                                this._configuration["EmailSettings:MailServer"],
                                                                Convert.ToInt32(this._configuration["EmailSettings:MailPort"]),
                                                                this._configuration["EmailSettings:Username"],
                                                                this._configuration["EmailSettings:Password"]
                                                               );
                            _appLogger.InfoLogger.LogInfo($"Mail: {toChucBoMayQuanLyViewModel.EmailSettings.Mail} - Subject: {toChucBoMayQuanLyViewModel.EmailSettings.Subject} - Status: {cResponseMessage.Message}");
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
