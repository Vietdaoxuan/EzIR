using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using CoreLib.Entities.ViewModels;
using DataServiceLib.Interfaces;
using EzIRCustomerAPI.Commons;
using EzIRCustomerAPI.DataAccess.Interfaces;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.DataAccess.Implementations
{
    public class ManagerContext : IManagerContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;
        private readonly string _connectionStringDBTemp = string.Empty;
        public ManagerContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            _config = config;
            _cBaseDataProvider = cBaseDataProvider;
            _appLogger = appLogger;
            _common = common;
            this._connectionStringDBTemp = _config["ConnectionStringEzSearch"];
        }

        public Task<CResponseMessage> Delete(ManagerViewModel obj)
        {
            throw new NotImplementedException();
        }

        public Task<CResponseMessage> Insert(ManagerViewModel manager)
        {
            try
            {
                OracleParameter[] dbParam =
                 {
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = manager.CPNYID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACORGID",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = manager.CORGID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AMNGERNAME",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = manager.MNGERNAME,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANATIONALITY",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = manager.NATIONALITY,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AKNOWLEDGELEVELID",
                        OracleDbType = OracleDbType.Int32,
                        Value = manager.KNOWLEDGELEVELID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AISLEGALREP",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = manager.ISLEGALREP,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADATEOFBIRTHVN",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = manager.DATEOFBIRTHVN,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AKNOWLEDGESPECIALLEVEL",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = manager.KNOWLEDGESPECIALLEVEL,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AKNOWLEDGESPECIALLEVELEN",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = manager.KNOWLEDGESPECIALLEVELEN,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACVPATH",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = manager.CVPATH,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AORD",
                        OracleDbType = OracleDbType.Int32,
                        Value = manager.ORD,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANOTE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = manager.NOTE,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_MORGID",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = manager.MORGID,
                    },
                };

                _cBaseDataProvider.SetUsername(manager.UserLogin);
                _cBaseDataProvider.SetRoleCode(manager.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_MANAGER_INSERT, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_MANAGER_INSERT, dbParam, manager.RoleCode, manager.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể thêm mới!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<IEnumerable<Manager>> Select(ManagerViewModel manager)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Manager> SelectEzIR(ManagerViewModel manager)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_AMNGID",
                        OracleDbType = OracleDbType.Int32,
                        Value = manager.MNGID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AMNGERID",
                        OracleDbType = OracleDbType.Int32,
                        Value = manager.MNGERID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = manager.CPNYID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APPROVE",
                        OracleDbType = OracleDbType.Int32,
                        Value = manager.APPROVE,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "Refcursor",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSetEzIR = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBTemp, ConstantsSP.SPEZS_IR_MANAGER_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZS_IR_MANAGER_SEARCH, dbParam, manager.RoleCode, manager.UserLogin));

                if (dataSetEzIR.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSetEzIR));

                    var mapper = new DataNamesMapper<Manager>();

                    var managers = mapper.Map(dataSetEzIR.Tables[0]);

                    return managers;
                }

                return Enumerable.Empty<Manager>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<Manager>();
            }
        }

        public IEnumerable<Manager> SelectEzSearch(ManagerViewModel manager)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = manager.CPNYID,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_AMNGERID",
                        OracleDbType = OracleDbType.Int32,
                        Value = manager.MNGERID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "Refcursor",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSetEzSearch = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBTemp, ConstantsSP.SPEZS_IR_MANAGER_CP_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZS_IR_MANAGER_CP_SEARCH, dbParam, manager.RoleCode, manager.UserLogin));

                if (dataSetEzSearch.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSetEzSearch));

                    var mapper = new DataNamesMapper<Manager>();

                    var managers = mapper.Map(dataSetEzSearch.Tables[0]);

                    return managers;
                }

                return Enumerable.Empty<Manager>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<Manager>();
            }
        }

        public void SetRoleCode(string roleCode)
        {
            this._cBaseDataProvider.SetRoleCode(roleCode);
        }

        public void SetUsername(string username)
        {
            this._cBaseDataProvider.SetUsername(username);
        }

        public Task<CResponseMessage> Update(ManagerViewModel manager)
        {
            try
            {
                OracleParameter[] dbParam =
                 {
                    new OracleParameter()
                    {
                        ParameterName = "P_AMNGERID",
                        OracleDbType = OracleDbType.Int32,
                        Value = manager.MNGERID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = manager.CPNYID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACORGID",
                        OracleDbType = OracleDbType.Int32,
                        Value = manager.CORGID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AMNGERNAME",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = manager.MNGERNAME,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANATIONALITY",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = manager.NATIONALITY,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AKNOWLEDGELEVELID",
                        OracleDbType = OracleDbType.Int32,
                        Value = manager.KNOWLEDGELEVELID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AISLEGALREP",
                        OracleDbType = OracleDbType.Int32,
                        Value = manager.ISLEGALREP,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADATEOFBIRTHVN",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = manager.DATEOFBIRTHVN,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AKNOWLEDGESPECIALLEVEL",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = manager.KNOWLEDGESPECIALLEVEL,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AKNOWLEDGESPECIALLEVELEN",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = manager.KNOWLEDGESPECIALLEVELEN,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACVPATH",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = manager.CVPATH,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AORD",
                        OracleDbType = OracleDbType.Int32,
                        Value = manager.ORD,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANOTE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = manager.NOTE,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_MORGID",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = manager.MORGID,
                    },
                };

                _cBaseDataProvider.SetUsername(manager.UserLogin);
                _cBaseDataProvider.SetRoleCode(manager.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_MANAGER_UPDATE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_MANAGER_UPDATE, dbParam, manager.RoleCode, manager.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể sửa!" };

                return Task.FromResult(cResponseMessage);
            }
        }
    }
}
