using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Commons;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Interfaces;
using EzIRCustomerAPI.DataAccess.Interfaces;
using EzIRCustomerAPI.Models.ModelViews;
using EzIRCustomerAPI.Models.ViewModels;
using EzIRCustomerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgotPasswordAPIController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICustomerContext _customerContext;
        private readonly ICommon _common;
        private readonly IErrorHandler _errorHandler;
        private readonly IEmailSender _emailSender;
        private readonly IAppLogger _appLogger;
        public ForgotPasswordAPIController(
            ICustomerContext customerContext,
            IEmailSender emailSender,
            ICommon common,
            IConfiguration configuration,
            IErrorHandler errorHandler)
        {
            this._emailSender = emailSender;
            this._common = common;
            this._configuration = configuration;
            this._errorHandler = errorHandler;
            this._customerContext = customerContext;
        }
        [HttpPost(ApiRoute.Forgot_Password)]
        public async Task<CResponseMessage> ForgetPassword(ForgotPasswordViewModel forgotPasswordModel, [FromHeader] string roleCode, [FromHeader] string username)
        {
            var newPassword = new Random().Next(100000, 999999).ToString();
            forgotPasswordModel.NewPassword = PasswordGenerator.EncodePassword(newPassword);
            this._customerContext.SetRoleCode(roleCode);
            this._customerContext.SetUsername(username);          
            var result =this._customerContext.ForgotPassword(forgotPasswordModel);
            if (result.Code != 0)
            {
                result.Message = "MSG_TEZIR_CUSTOMER_INFO_INCORRECT";
                return result;
            }
            // gửi email thông báo
            try
            {
                var emailContent = await System.IO.File.ReadAllTextAsync(this._configuration["ResetPasswordFrontOnlineFilePath"]);
                emailContent = emailContent.Replace("<!UserName>", forgotPasswordModel.Username);
                emailContent = emailContent.Replace("<!NewPassword>", newPassword);
                emailContent = emailContent.Replace("<!LinkReset1>", this._configuration["LinkResetPassword"]);
                emailContent = emailContent.Replace("<!LinkReset2>", this._configuration["LinkResetPassword"]);
                var sendEmailResult = await this._emailSender.SendEmailAsync(forgotPasswordModel.Email, "Thông tin tài khoản doanh nghiệp", emailContent, this._configuration["EmailSettings:Sender"], this._configuration["EmailSettings:MailServer"],
                  Convert.ToInt32(this._configuration["EmailSettings:MailPort"]), this._configuration["EmailSettings:Username"], this._configuration["EmailSettings:Password"]);
                return sendEmailResult.Code != 0
                    ? result
                    : new CResponseMessage { Code = 0, Message = "Gui_email_thanh_cong"};
            }
            catch (Exception e)
            {
                _errorHandler.WriteToFile(e);
                return new CResponseMessage { Code = -1, Message = "Gui_email_that_bai" };
            }
        }
    }

}
