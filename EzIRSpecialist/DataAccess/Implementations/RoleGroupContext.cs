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
    public class RoleGroupContext : IRoleGroupContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public RoleGroupContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;
        }

        public Task<IEnumerable<RoleGroupModelView>> Select(RoleGroupViewModel roleGroupViewModel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_AGROUPCODE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = roleGroupViewModel.GroupCode,
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
                };

                //_cBaseDataProvider.SetUsername(roleGroupViewModel.UserName);
                //_cBaseDataProvider.SetRoleCode(roleGroupViewModel.RoleCode);

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_ROLEGROUP_GET_ALL, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_ROLEGROUP_GET_ALL, dbParam, roleGroupViewModel.RoleCode, roleGroupViewModel.UserName));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<RoleGroupModelView>();

                    var roleGroups = mapper.Map(dataSet.Tables[0]);
                    
                    return Task.FromResult(roleGroups);
                }

                return Task.FromResult(Enumerable.Empty<RoleGroupModelView>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<RoleGroupModelView>());
            }
        }       

        public Task<CResponseMessage> Insert(RoleGroupViewModel roleGroupViewModel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_AGROUPCODE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = roleGroupViewModel.GroupCode,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AROLECODE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = roleGroupViewModel.ARoleCode,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADELETE",
                        OracleDbType = OracleDbType.Int16,
                        Value = roleGroupViewModel.Delete,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AVIEW",
                        OracleDbType = OracleDbType.Int16,
                        Value = roleGroupViewModel.View,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AEDIT",
                        OracleDbType = OracleDbType.Int16,
                        Value = roleGroupViewModel.Edit,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ASPECIAL",
                        OracleDbType = OracleDbType.Int16,
                        Value = roleGroupViewModel.Special,
                    },
                };

                _cBaseDataProvider.SetUsername(roleGroupViewModel.UserName);
                _cBaseDataProvider.SetRoleCode(roleGroupViewModel.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_ROLEGROUP_INSERT, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_ROLEGROUP_INSERT, dbParam, roleGroupViewModel.RoleCode, roleGroupViewModel.UserName));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể cập nhật quyền cho nhóm!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<CResponseMessage> Update(RoleGroupViewModel obj)
        {
            throw new NotImplementedException();
        }
        public Task<CResponseMessage> Delete(RoleGroupViewModel obj)
        {
            throw new NotImplementedException();
        }
    }
}
