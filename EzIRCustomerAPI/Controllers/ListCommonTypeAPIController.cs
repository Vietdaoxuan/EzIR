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
using CoreLib.ViewModel;

namespace EzIRCustomerAPI.Controllers
{
    [ApiController]
    public class ListCommonTypeAPIController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILoginContext _loginContext;
        private readonly ICommonTypeContext _commonTypeContext;
        private readonly IAppLogger _appLogger;

        public ListCommonTypeAPIController(IConfiguration configuration, ILoginContext loginContext, IAppLogger appLogger, ICommonTypeContext commonTypeContext)
        {
            _configuration = configuration;
            _loginContext = loginContext;
            _commonTypeContext = commonTypeContext;
            _appLogger = appLogger;
        }

        [HttpGet]
        [Route(ApiRoute.CommonCategory)]
        public async Task<IActionResult> GetListCommonType(string ListCategory)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion
                var commonType = new CommonTypeViewModel
                {
                    ListCategory = ListCategory
                };


                var listCommontype = await _commonTypeContext.Select(commonType);
                return Ok(new CResponseMessage { Code = 200, Message = "Ok", Data = listCommontype });
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = 401, Message = "Unauthorized" });
            }
        }
    }
}
