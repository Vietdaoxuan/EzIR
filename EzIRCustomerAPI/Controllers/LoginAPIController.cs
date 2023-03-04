using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CommonLib.Interfaces.Logger;
using CoreLib.Commons;
using CoreLib.Configs;
using CoreLib.Entities;
using EzIRCustomerAPI.DataAccess.Interfaces;
using EzIRCustomerAPI.Models;
using EzIRCustomerAPI.Models.ViewModels;
using EzIRCustomerAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EzIRCustomerAPI.Controllers
{
    [ApiController]
    public class LoginAPIController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILoginContext _loginContext;
        private readonly ICustomerContext _customerContext;
        private readonly IAppLogger _appLogger;
        //private readonly IMemoryCache _cache;

        public LoginAPIController(IConfiguration configuration, ILoginContext loginContext, ICustomerContext customerContext, IAppLogger appLogger)
        {
            _configuration = configuration;
            _loginContext = loginContext;
            _customerContext = customerContext;
            _appLogger = appLogger;
        }

        [HttpPost]
        [Route(ApiRoute.GET_TOKEN)]
        [AllowAnonymous]
        public IActionResult GetToken([FromBody] LoginModel loginModel)
        {
            string errorMessage = "UnknowError";
            int errorCode = -1;

            if (!Validate(loginModel, out errorCode, out errorMessage))
            {
                return Ok(new CResponseMessage(errorCode, errorMessage));
            }

            loginModel.Password = PasswordGenerator.EncodePassword(loginModel.Password);

            var response = _loginContext.CheckLogin(loginModel);

            if (response.Code == 0)
            {
                var customers = _customerContext.Select(new CustomerViewModel { Username = loginModel.Username, RoleCode = "LOGIN" });

                if (customers.Count() > 0)
                {
                    var user = customers.First();

                    var userInfo = new UserInfo
                    {
                        Username = user.Username,
                        CpnyID = user.CpnyID,
                        Email = user.Email,
                        FullName = user.FullName,
                        Phone = user.Phone,
                        StockCode = user.StockCode,
                        StockName = user.StockName,
                        StockNameEn = user.StockNameEn,
                        CompanyType = user.CompanyType,
                        CompanyTypeName = user.CompanyTypeName,
                        CompanyTypeNameEN = user.CompanyTypeNameEN,
                        EmailSpecialist = user.EmailSpecialist,
                        EmailSpecialistCC = user.EmailSpecialistCC,
                        ExpertName = user.ExpertName,
                        ExpertPhone = user.ExpertPhone
                    };

                    var tokenString = TokenManager.GenerateToken(userInfo, -1, _appLogger);                    

                    return Ok(new CResponseMessage(0, "GET_TOKEN_SUCCESSUL")
                    {
                        Data = new
                        {
                            Token = tokenString,
                            UserName = customers.First().Username,
                            customers.First().FullName,
                            customers.First().Email,                            
                            customers.First().Phone,
                            customers.First().CpnyID,
                            customers.First().StockCode,
                            customers.First().StockName,
                            customers.First().StockNameEn,
                            customers.First().CompanyType,
                            customers.First().CompanyTypeName,
                            customers.First().CompanyTypeNameEN,
                            customers.First().EmailSpecialist,
                            customers.First().EmailSpecialistCC,
                            customers.First().ExpertName,
                            customers.First().ExpertPhone,
                        },
                    });
                }
            }

            return Ok(response);
        }


        [HttpPost]
        [Route(ApiRoute.GET_SEED)]
        [AllowAnonymous]
        public IActionResult GetSeed([FromBody] LoginModel loginModel)
        {
            var response = _loginContext.CheckSeed(loginModel);

            return Ok(response);
        }

        private bool Validate(LoginModel loginModel, out int errorCode, out string errorMess)
        {
            errorCode = 0;
            errorMess = null;
            try
            {
                if (string.IsNullOrEmpty(loginModel.Username))
                {
                    errorMess = "UsernameIsNotToEmpty";
                    errorCode = -1;
                    return false;
                }

                if (!CheckUtils.ContainsUnicodeCharacter(loginModel.Username))
                {
                    errorMess = "IncorrectUsernameOrPassword.";
                    errorCode = -1;
                    return false;
                }
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                throw;
            }
            return true;
        }
    }
}
