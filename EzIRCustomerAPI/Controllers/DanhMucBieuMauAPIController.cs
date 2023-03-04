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


namespace EzIRCustomerAPI.Controllers
{
    [ApiController]
    public class DanhMucBieuMauAPIController : ControllerBase
    {
        private readonly ITemplateContext _templateContext;
        private readonly IConfiguration _configuration;
        private readonly ILoginContext _loginContext;
        private readonly IAppLogger _appLogger;

        public DanhMucBieuMauAPIController(IConfiguration configuration, ILoginContext loginContext, ITemplateContext templateContext, IAppLogger appLogger, ICommonTypeContext commonTypeContext)
        {
            _templateContext = templateContext;
            _configuration = configuration;
            _loginContext = loginContext;
            _appLogger = appLogger;
        }


        [HttpGet]
        [Route(ApiRoute.Get_DanhMucBieuMau)]
        public IActionResult GetBieuMau([FromHeader] TemplateViewModel templateViewModel)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion

                var bieuMaus = _templateContext.Select(templateViewModel);

                return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = bieuMaus.Result});
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
    }
}
