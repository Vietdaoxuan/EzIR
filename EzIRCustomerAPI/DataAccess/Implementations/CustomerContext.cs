using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using EzIRCustomerAPI.Commons;
using EzIRCustomerAPI.DataAccess.Interfaces;
using EzIRCustomerAPI.Models;
using EzIRCustomerAPI.Models.ModelViews;
using EzIRCustomerAPI.Models.ViewModels;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.DataAccess.Implementations
{
    public class CustomerContext : ICustomerContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public CustomerContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;
        }

        public IEnumerable<CustomerModelView> Select(CustomerViewModel customer)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_USERNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = customer.Username,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_FULLNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = customer.FullName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_EMAIL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = customer.Email,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACTIVE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = customer.Active,
                    },                   
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },

                };

                //_cBaseDataProvider.SetRoleCode(customer.RoleCode);
                //_cBaseDataProvider.SetUsername(customer.Username);

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_CUSTOMER_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_CUSTOMER_SEARCH, dbParam, customer.RoleCode, customer.Username));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<CustomerModelView>();

                    var customers = mapper.Map(dataSet.Tables[0]);

                    return customers;
                }

                return Enumerable.Empty<CustomerModelView>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<CustomerModelView>();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="changepassword"></param>
        /// <returns></returns>

        public CResponseMessage ChangePassword(ChangePasswordViewModel changePasswordViewModel)

        {
            var dbParam = new[]
             {
                    new OracleParameter()
                    {
                        ParameterName = "P_AUSERNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = changePasswordViewModel.Username,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_OLDAPASSWORD",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = changePasswordViewModel.OldPassword,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_NEWAPASSWORD",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = changePasswordViewModel.NewPassword,
                    },
               new OracleParameter()
                    {
                        ParameterName = "P_ACPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = changePasswordViewModel.Cpnyid,
                    },
            };
            _cBaseDataProvider.SetRoleCode(changePasswordViewModel.RoleCode);
            return this._cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_CUSTOMER_CHANGE_PASSWORD, dbParam);

        }
        public void SetRoleCode(string roleCode)
        {
            this._cBaseDataProvider.SetRoleCode(roleCode);
        }

        public void SetUsername(string username)
        {
            this._cBaseDataProvider.SetUsername(username);
        }
        public CResponseMessage ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)

        {
            var dbParam = new[]
                       {
                    new OracleParameter()
                    {
                        ParameterName = "P_AUSERNAME",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = forgotPasswordViewModel.Username,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_EMAIL",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = forgotPasswordViewModel.Email,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_NEWAPASSWORD",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = forgotPasswordViewModel.NewPassword,
                    }
            };
          


            return this._cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_CUSTOMER_FORGOT_PASSWORD, dbParam);

        }


    }
}
