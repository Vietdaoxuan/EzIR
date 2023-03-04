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
using System.Globalization;

namespace EzIRCustomerAPI.Controllers
{
    [ApiController]
    public class CongBoThongTinAPIController : ControllerBase
    {
        private readonly ICongBoThongTinContext _congBoThongTinContext;
        private readonly IConfiguration _configuration;
        private readonly ILoginContext _loginContext;
        private readonly IAppLogger _appLogger;
        private readonly IEmailSender _emailSender;

        public CongBoThongTinAPIController(IConfiguration configuration, ILoginContext loginContext, ICongBoThongTinContext congBoThongTinContext, IAppLogger appLogger, ICommonTypeContext commonTypeContext, IEmailSender emailSender)
        {
            _congBoThongTinContext = congBoThongTinContext;
            _configuration = configuration;
            _loginContext = loginContext;
            _appLogger = appLogger;
            _emailSender = emailSender;
        }

        [HttpGet]
        [Route(ApiRoute.Get_CongBoThongTin)]
        public IActionResult GetCongBoThongTin([FromHeader] CongBoThongTinViewModel congBoThongTinViewModel)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion

                var tinCongBos = _congBoThongTinContext.Select(congBoThongTinViewModel);

                return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = tinCongBos.Result });
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        [HttpPost]
        [Route(ApiRoute.Insert_TinCongBo)]
        public async Task<IActionResult> InsertCongBoThongTin([FromBody] CongBoThongTinViewModel congBoThongTinViewModels, [FromHeader] string roleCode, [FromHeader] string username)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion
                var cResponse = await _congBoThongTinContext.Insert(congBoThongTinViewModels);
                if (cResponse.Code == 0)
                {
                    //congBoThongTinViewModels.emailSettings.Message = congBoThongTinViewModels.StockCode + ": " + congBoThongTinViewModels.Title + " " + "(" + DateTime.Parse(congBoThongTinViewModels.DatePub.ToString()).ToString("dd/MM/yyyy HH:mm") + ")";
                    var url = this._configuration["UrlFileCommon"] + congBoThongTinViewModels.Url;
                    var emailContent = await System.IO.File.ReadAllTextAsync(this._configuration["FilePath"]);
                    emailContent = emailContent.Replace("<!StockCode>", congBoThongTinViewModels.StockCode);
                    emailContent = emailContent.Replace("<!Title>", congBoThongTinViewModels.Title);
                    emailContent = emailContent.Replace("<!Date>", DateTime.Parse(congBoThongTinViewModels.DatePub.ToString()).ToString("dd/MM/yyyy HH:mm"));
                    emailContent = emailContent.Replace("<!LinkURL>", url);
                    var response = await this._emailSender.SendEmailAsync(
                                                                congBoThongTinViewModels.emailSettings.Mail,
                                                                congBoThongTinViewModels.emailSettings.Cc,
                                                              congBoThongTinViewModels.emailSettings.Subject,
                                                                emailContent,
                                                                this._configuration["EmailSettings:Sender"],
                                                                this._configuration["EmailSettings:MailServer"],
                                                                Convert.ToInt32(this._configuration["EmailSettings:MailPort"]),
                                                                this._configuration["EmailSettings:Username"],
                                                                this._configuration["EmailSettings:Password"]
                                                               );
                    _appLogger.InfoLogger.LogInfo($"Mail: {congBoThongTinViewModels.emailSettings.Mail} - Subject: {congBoThongTinViewModels.emailSettings.Subject} - Status: {response.Message}");
                }
                return Ok(cResponse);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

        [HttpPost]
        [Route(ApiRoute.Update_TinCongBo)]
        public async Task<IActionResult> UpdateCongBoThongTin([FromBody] CongBoThongTinViewModel congBoThongTinViewModels, [FromHeader] string roleCode, [FromHeader] string username)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion
                var cResponseMessage = await _congBoThongTinContext.Update(congBoThongTinViewModels);
                return Ok(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
        [HttpPost]
        [Route(ApiRoute.Delete_TinCongBo)]
        public async Task<IActionResult> DeleteCongBoThongTin([FromBody] CongBoThongTinViewModel congBoThongTinViewModels, [FromHeader] string roleCode, [FromHeader] string username)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion
                var cResponseMessage = await _congBoThongTinContext.Delete(congBoThongTinViewModels);
                if (cResponseMessage.Code == 0)
                {
                    //congBoThongTinViewModels.emailSettings.Message = congBoThongTinViewModels.StockCode + ": " + congBoThongTinViewModels.Title + " " + "(" + DateTime.Parse(congBoThongTinViewModels.DatePub.ToString()).ToString("dd/MM/yyyy HH:mm") + ")";
                    var url = this._configuration["UrlFileCommon"] + congBoThongTinViewModels.Url;
                    var emailContent = await System.IO.File.ReadAllTextAsync(this._configuration["FilePathDelete"]);
                    emailContent = emailContent.Replace("<!StockCode>", congBoThongTinViewModels.StockCode);
                    emailContent = emailContent.Replace("<!Title>", congBoThongTinViewModels.Title);
                    emailContent = emailContent.Replace("<!Date>", DateTime.Now.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture));
                    emailContent = emailContent.Replace("<!LinkURL>", url);
                    var response = await this._emailSender.SendEmailAsync(
                                                                congBoThongTinViewModels.emailSettings.Mail,
                                                                congBoThongTinViewModels.emailSettings.Cc,
                                                              congBoThongTinViewModels.emailSettings.Subject,
                                                                emailContent,
                                                                this._configuration["EmailSettings:Sender"],
                                                                this._configuration["EmailSettings:MailServer"],
                                                                Convert.ToInt32(this._configuration["EmailSettings:MailPort"]),
                                                                this._configuration["EmailSettings:Username"],
                                                                this._configuration["EmailSettings:Password"]
                                                               );
                    _appLogger.InfoLogger.LogInfo($"Mail: {congBoThongTinViewModels.emailSettings.Mail} - Subject: {congBoThongTinViewModels.emailSettings.Subject} - Status: {response.Message}");
                }
                return Ok(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }

    }
}