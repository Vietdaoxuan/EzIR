using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models.Admin;
using EzIRSpecialist.Models.ModelView;
using EzIRSpecialist.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Implementations
{
    public class SystemManagementContext : ISystemManagementContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public SystemManagementContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;
        }

        public IEnumerable<GroupViewModel> GetListRole()
        {
            throw new NotImplementedException();
        }

        public CResponseMessage GetListRoleByUser(RoleGroupViewModel roleGroupViewModel)
        {
            try
            {
                var errorCode = new OracleParameter()
                {
                    ParameterName = "p_ERRORCODE",
                    OracleDbType = OracleDbType.Int32,
                    Size = 25,
                    Direction = ParameterDirection.Output,
                };

                var message = new OracleParameter()
                {
                    ParameterName = "p_MESSAGE",
                    OracleDbType = OracleDbType.Varchar2,
                    Size = 200,
                    Direction = ParameterDirection.Output,
                };

                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_USERNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = roleGroupViewModel.UserName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AROLETYPE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = roleGroupViewModel.RoleType,
                    },                                       
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                    errorCode,
                    message,
                };                
                
                //_cBaseDataProvider.SetUsername(roleGroupViewModel.UserName);
                //_cBaseDataProvider.SetRoleCode(roleGroupViewModel.RoleCode);

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_ROLE_GET_BY_USER, dbParam);

                if (int.Parse(errorCode.Value.ToString()) != 0 && message.Value != null)
                {
                    return new CResponseMessage  { Code = int.Parse(errorCode.Value.ToString()), Message = message.Value.ToString(), Data = dataSet };
                }

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_ROLE_GET_BY_USER, dbParam, "", roleGroupViewModel.UserName));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<RoleModelView>();

                    var roleGroups = mapper.Map(dataSet.Tables[0]);

                    return new CResponseMessage { Code = int.Parse(errorCode.Value.ToString()), Message = message.Value.ToString(), Data = roleGroups }; ;
                }

                return new CResponseMessage(code: -2, message: "Lỗi sp!");
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return new CResponseMessage(code: -2, message: "Lỗi sp!");
            }
        }

        public IEnumerable<UserModelView> GetListUser(UserViewModel user)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_USERNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = user.Username,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_FULLNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = user.FullName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_EMAIL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = user.Email,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACTIVE",
                        OracleDbType = OracleDbType.Int16,
                        Value = user.Active,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ROLECODE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = user.RoleCode,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_USER_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_USER_SEARCH, dbParam, "", ""));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<UserModelView>();

                    var users = mapper.Map(dataSet.Tables[0]);

                    return users;
                }

                return Enumerable.Empty<UserModelView>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<UserModelView>();
            }
        }
    }
}
