using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Commons;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Interfaces;
using EzIRCustomerAPI.DataAccess.Interfaces;
using EzIRCustomerAPI.Models;
using EzIRCustomerAPI.Models.ViewModels;
using EzIRCustomerAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.Controllers
{
    [ApiController]
    public class ChangePasswordAPIController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICustomerContext _customerContext;
        private readonly ICommon _common;
        private readonly IErrorHandler _errorHandler;
        private readonly IEmailSender _emailSender;
        private readonly IAppLogger _appLogger;
        public ChangePasswordAPIController(
            ICustomerContext customerContext,
            ICommon common,
            IConfiguration configuration,
            IErrorHandler errorHandler,
            IEmailSender emailSender
            )
        {
            this._common = common;
            this._configuration = configuration;
            this._errorHandler = errorHandler;
            this._customerContext = customerContext;
            this._emailSender = emailSender;
        }

        [HttpPost]
        [Route(ApiRoute.Change_Password)]
        public async Task<CResponseMessage> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            var newPassword = changePasswordViewModel.NewPassword;
            // mã hóa password
            changePasswordViewModel.OldPassword = PasswordGenerator.EncodePassword(changePasswordViewModel.OldPassword);
            changePasswordViewModel.NewPassword = PasswordGenerator.EncodePassword(newPassword);
            changePasswordViewModel.ReEnterPassword = PasswordGenerator.EncodePassword(changePasswordViewModel.ReEnterPassword);
            var result = this._customerContext.ChangePassword(changePasswordViewModel);
            // QuangKS edit không gửi email khi thay đổi Pass 2021-12-01
            if (result.Code != 0)
                return new CResponseMessage { Code = -1, Message = "MSG_TEZIR_CUSTOMER_APASSWORD_INCORRECT" };            
            else
                return new CResponseMessage(0, "changePassWord_DOI_MAT_KHAU_THANH_CONG");
        }
    }
}
