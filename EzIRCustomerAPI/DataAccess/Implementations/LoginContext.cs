using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using EzIRCustomerAPI.Commons;
using EzIRCustomerAPI.DataAccess.Interfaces;
using EzIRCustomerAPI.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.DataAccess.Implementations
{
    public class LoginContext : ILoginContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public LoginContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;
        }

        public CResponseMessage CheckLogin(LoginModel loginModel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_USERNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = loginModel.Username,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_PASSWORD",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = loginModel.Password,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_IP",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = loginModel.IP,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_BROWSER",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = loginModel.Browser,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_SEED",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = loginModel.Seed,
                    },
                };

                //_cBaseDataProvider.SetUsername(loginModel.Username);
                //_cBaseDataProvider.SetRoleCode("LOGIN");

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_CUSTOMER_LOGIN, dbParam);
                dbParam[1].Value = "";

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_CUSTOMER_LOGIN, dbParam, loginModel.Username, "LOGIN"));

                return cResponseMessage;
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "MSG_TEZIR_CAN_NOT_LOGIN" };

                return cResponseMessage;
            }
        }

        public CResponseMessage CheckSeed(LoginModel loginModel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_USERNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = loginModel.Username,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_SEED",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = loginModel.Seed,
                    },
                };


                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_CUSTOMER_SEED_SEARCH, dbParam);
                dbParam[1].Value = "";

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_CUSTOMER_SEED_SEARCH, dbParam, loginModel.Username, "LOGIN"));

                return cResponseMessage;
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "MSG_TEZIR_CAN_NOT_SEED" };

                return cResponseMessage;
            }
        }
    }
}
