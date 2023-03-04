using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
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
    public class ChangeInfoContext : IChangeInfoContext
    {

        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;
        //private readonly string _connectionStringDBTemp = string.Empty;

        public ChangeInfoContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;            
        }

        public CResponseMessage Insert(ChangeInfo changeInfo)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int64,
                        Value = changeInfo.CpnyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_KEY",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = changeInfo.Key,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_VALUE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = changeInfo.Value,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_STATUS",
                        OracleDbType = OracleDbType.Int64,
                        Value = changeInfo.Status,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_KEYFUNCTION",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = changeInfo.KeyFunction,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_MENUID",
                        OracleDbType = OracleDbType.Int64,
                        Value = changeInfo.MenuID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_FUNCTION",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = changeInfo.Function,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_LEVELID",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = changeInfo.LevelID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_DETAILLEVELID",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = changeInfo.DetailLevelID,
                    },
                };

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_CHANGEINFO_INSERT, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_CHANGEINFO_INSERT, dbParam, changeInfo.RoleCode, changeInfo.Username));

                return cResponseMessage;
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return new CResponseMessage { Code = -3, Message = "MSG_TEZIR_AN_ERROR_OCCURRED" };
            }
        }

        //public IEnumerable<ChangeInfo> Select(ChangeInfo changeInfo)
        //{
        //    try
        //    {
        //        OracleParameter[] dbParam =
        //        {
        //            new OracleParameter()
        //            {
        //                ParameterName = "P_CPNYID",
        //                OracleDbType = OracleDbType.Int64,
        //                Value = changeInfo.CpnyID,
        //            },                    
        //            new OracleParameter()
        //            {
        //                ParameterName = "REFCURSOR",
        //                Direction = ParameterDirection.Output,
        //                OracleDbType = OracleDbType.RefCursor,
        //            },
        //        };

        //        var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBTemp, ConstantsSP.SPEZSTEMP_CHANGEINFO_SEARCH, dbParam);

        //        _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZSTEMP_CHANGEINFO_SEARCH, dbParam, changeInfo.RoleCode, changeInfo.Username));

        //        if (dataSet.Tables.Count > 0)
        //        {
        //            _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

        //            var mapper = new DataNamesMapper<ChangeInfo>();

        //            var infoSheets = mapper.Map(dataSet.Tables[0]);

        //            return infoSheets;
        //        }

        //        return Enumerable.Empty<ChangeInfo>();
        //    }
        //    catch (Exception ex)
        //    {
        //        _appLogger.ErrorLogger.LogError(ex);

        //        return Enumerable.Empty<ChangeInfo>();
        //    }
        //}

        public CResponseMessage Update(ChangeInfo changeInfo)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_AID",
                        OracleDbType = OracleDbType.Int64,
                        Value = changeInfo.ID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_STATUS",
                        OracleDbType = OracleDbType.Int64,
                        Value = changeInfo.Status,
                    },                    
                };

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_CHANGEINFO_UPDATE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_CHANGEINFO_UPDATE, dbParam, "", ""));

                return cResponseMessage;
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return new CResponseMessage { Code = -3, Message = "MSG_TEZIR_AN_ERROR_OCCURRED" };
            }
        }
    }
}
