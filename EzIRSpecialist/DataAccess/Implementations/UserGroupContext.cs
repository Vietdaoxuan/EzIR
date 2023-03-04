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
    public class UserGroupContext : IUserGroupContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public UserGroupContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;
        }

        public Task<IEnumerable<UserGroupModelView>> Select(UserGroupViewModel userGroupViewModel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_GROUPCODE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = userGroupViewModel.GroupCode,
                    },                    
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR2",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                };

                _cBaseDataProvider.SetUsername(userGroupViewModel.UserName);
                _cBaseDataProvider.SetRoleCode(userGroupViewModel.RoleCode);

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_USERGROUP_GET_ALL, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_USERGROUP_GET_ALL, dbParam, userGroupViewModel.RoleCode, userGroupViewModel.UserName));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<UserGroupModelView>();

                    var userInGroup = mapper.Map(dataSet.Tables[0]);

                    var userNotInGroup = mapper.Map(dataSet.Tables[1]);

                    List<UserGroupModelView> listUserGroup = new List<UserGroupModelView>();

                    listUserGroup.AddRange(userInGroup);
                    listUserGroup.AddRange(userNotInGroup);

                    return Task.FromResult(listUserGroup.AsEnumerable());
                }

                return Task.FromResult(Enumerable.Empty<UserGroupModelView>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<UserGroupModelView>());
            }
        }

        public Task<CResponseMessage> Insert(UserGroupViewModel userGroupViewModel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_GROUPCODE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = userGroupViewModel.GroupCode,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AUSERNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = userGroupViewModel.AUsername,
                    },
                };

                _cBaseDataProvider.SetUsername(userGroupViewModel.UserName);
                _cBaseDataProvider.SetRoleCode(userGroupViewModel.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_USERGROUP_INSERT, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_USERGROUP_INSERT, dbParam, userGroupViewModel.RoleCode, userGroupViewModel.UserName));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể thêm người dùng vào nhóm!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<CResponseMessage> Update(UserGroupViewModel t3)
        {
            throw new NotImplementedException();
        }

        public Task<CResponseMessage> Delete(UserGroupViewModel userGroupViewModel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_GROUPCODE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = userGroupViewModel.GroupCode,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AUSERNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = userGroupViewModel.AUsername,
                    },
                };

                _cBaseDataProvider.SetUsername(userGroupViewModel.UserName);
                _cBaseDataProvider.SetRoleCode(userGroupViewModel.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_USERGROUP_DELETE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_USERGROUP_DELETE, dbParam, userGroupViewModel.RoleCode, userGroupViewModel.UserName));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể thêm người dùng vào nhóm!" };

                return Task.FromResult(cResponseMessage);
            }
        }
    }
}
