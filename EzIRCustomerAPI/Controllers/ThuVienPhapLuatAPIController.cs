using System;
using System.Linq;
using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.Entities;
using CoreLib.Entities.ViewModels;
using CoreLib.Interfaces;
using EzIRCustomerAPI.Commons;
using EzIRCustomerAPI.DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EzIRCustomerAPI.Controllers
{
    [ApiController]
    public class ThuVienPhapLuatAPIController : ControllerBase
    {
        private readonly IConfiguration _configuration;   
        private readonly IThuVienPhapLuatContext _thuvienphapluatcontext;
        private readonly ICommon _common;
        private readonly IErrorHandler _errorHandler;
        private readonly IAppLogger _appLogger;
        public ThuVienPhapLuatAPIController(
            IThuVienPhapLuatContext ithuvienphapluatcontext,
            ICommon common,
            IConfiguration configuration,
            IErrorHandler errorHandler,
            IAppLogger appLogger)        
        {
            this._common = common;
            this._configuration = configuration;
            this._errorHandler = errorHandler;
            this._thuvienphapluatcontext = ithuvienphapluatcontext;
            this._appLogger = appLogger;
        }

        [HttpGet]
        [Route(ApiRoute.GetThuVienPhapLuat)]
        public IActionResult GetThongTinThuVienPhapLuat([FromHeader] ThuVienPhapLuatViewModel thuvienphapluatviewmodel)
        {
            try
            {
                #region valid token
                var header = Request.Headers;
                var token = header["Authorization"].ToString() ?? null;
                var result = CheckValidToken.Check(token, _appLogger);
                if (result.Code != 200) return Ok(result);
                #endregion

                var thuvienphapluat_info = _thuvienphapluatcontext.Select(thuvienphapluatviewmodel);

                return Ok(new CResponseMessage { Code = 0, Message = "Ok", Data = thuvienphapluat_info.Result });
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);
                return Ok(new CResponseMessage { Code = -3, Message = "InternalServerError" });
            }
        }
    }
}
