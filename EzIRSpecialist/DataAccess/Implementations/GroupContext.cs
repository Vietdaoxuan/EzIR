using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models.Admin;
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
    public class GroupContext : IGroupContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public GroupContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;
        }        

        public Task<IEnumerable<Group>> Select(GroupViewModel groupViewModel)
        {            
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_GROUPCODE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = groupViewModel.GroupCode,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_GROUPNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = groupViewModel.GroupName,
                    },                    
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                };

                _cBaseDataProvider.SetUsername(groupViewModel.UserName);
                _cBaseDataProvider.SetRoleCode(groupViewModel.RoleCode);

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_GROUP_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_GROUP_SEARCH, dbParam, groupViewModel.RoleCode, groupViewModel.UserName));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<Group>();

                    var groups = mapper.Map(dataSet.Tables[0]);

                    return Task.FromResult(groups);
                }

                return Task.FromResult(Enumerable.Empty<Group>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<Group>());
            }
        }

        public Task<CResponseMessage> Insert(GroupViewModel groupViewModel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_GROUPCODE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = groupViewModel.GroupCode,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_GROUPNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = groupViewModel.GroupName,
                    },                                        
                };

                _cBaseDataProvider.SetUsername(groupViewModel.UserName);
                _cBaseDataProvider.SetRoleCode(groupViewModel.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_GROUP_INSERT, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_GROUP_INSERT, dbParam, groupViewModel.RoleCode, groupViewModel.UserName));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể thêm nhóm!" };

                return Task.FromResult(cResponseMessage);
            }            
        }

        public Task<CResponseMessage> Update(GroupViewModel groupViewModel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_GROUPCODE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = groupViewModel.GroupCode,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_GROUPNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = groupViewModel.GroupName,
                    },
                };

                _cBaseDataProvider.SetUsername(groupViewModel.UserName);
                _cBaseDataProvider.SetRoleCode(groupViewModel.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_GROUP_UPDATE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_GROUP_UPDATE, dbParam, groupViewModel.RoleCode, groupViewModel.UserName));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể cập nhật nhóm!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<CResponseMessage> Delete(GroupViewModel groupViewModel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_GROUPCODE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = groupViewModel.GroupCode,
                    },
                };

                _cBaseDataProvider.SetUsername(groupViewModel.UserName);
                _cBaseDataProvider.SetRoleCode(groupViewModel.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_GROUP_DELETE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_GROUP_DELETE, dbParam, groupViewModel.RoleCode, groupViewModel.UserName));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể xóa nhóm!" };

                return Task.FromResult(cResponseMessage);
            }
        }        
    }
}
