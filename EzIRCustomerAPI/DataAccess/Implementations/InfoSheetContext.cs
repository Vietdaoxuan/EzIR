using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using EzIRCustomerAPI.Commons;
using EzIRCustomerAPI.DataAccess.Interfaces;
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
    public class InfoSheetContext : IInfoSheetContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly ICBaseDataProvider _cBaseDataProvider2;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;
        private readonly string _connectionStringDBEzSearch = string.Empty;

        public InfoSheetContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, ICBaseDataProvider cBaseDataProvider2, IAppLogger appLogger, ICommon common)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._cBaseDataProvider2 = cBaseDataProvider2;
            this._appLogger = appLogger;
            this._common = common;
            this._connectionStringDBEzSearch = _config["ConnectionStringEzSearch"];
        }

        public IEnumerable<InfoSheet> SelectEzIR(InfoSheetViewModel infoSheet)
        {            
            try
            {            
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ID",
                        OracleDbType = OracleDbType.Int64,
                        Value = infoSheet.ID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int64,
                        Value = infoSheet.CpnyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_HASCONTENT",
                        OracleDbType = OracleDbType.Int64,
                        Value = infoSheet.HasContent,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_MENUID",
                        OracleDbType = OracleDbType.Int64,
                        Value = infoSheet.MenuID,
                    },                    
                    new OracleParameter()
                    {
                        ParameterName = "P_POSTDATE",
                        OracleDbType = OracleDbType.Date,
                        Value = infoSheet.PostDate,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_LANGUAGE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = infoSheet.Language,
                    },                   
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                };

                var dataSetEzIR = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_INFOSHEET_SEARCH, dbParam);
                
                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_INFOSHEET_SEARCH, dbParam, infoSheet.RoleCode, infoSheet.Username));
                
                if (dataSetEzIR.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSetEzIR));

                    var mapper = new DataNamesMapper<InfoSheet>();

                    var infoSheets = mapper.Map(dataSetEzIR.Tables[0]).ToList();

                    return infoSheets;
                }
                
                return Enumerable.Empty<InfoSheet>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<InfoSheet>();
            }
        }

        public IEnumerable<InfoSheet> SelectEzSearch(InfoSheetViewModel infoSheet)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ID",
                        OracleDbType = OracleDbType.Int64,
                        Value = infoSheet.ID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int64,
                        Value = infoSheet.CpnyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_HASCONTENT",
                        OracleDbType = OracleDbType.Int64,
                        Value = infoSheet.HasContent,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_MENUID",
                        OracleDbType = OracleDbType.Int64,
                        Value = infoSheet.MenuID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_POSTDATE",
                        OracleDbType = OracleDbType.Date,
                        Value = infoSheet.PostDate,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_LANGUAGE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = infoSheet.Language,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                };

                var dataSetEzSearch = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBEzSearch, ConstantsSP.SPEZS_IR_INFOSHEET_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZS_IR_INFOSHEET_SEARCH, dbParam, infoSheet.RoleCode, infoSheet.Username));

                if (dataSetEzSearch.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSetEzSearch));

                    var mapper = new DataNamesMapper<InfoSheet>();

                    var infoSheets = mapper.Map(dataSetEzSearch.Tables[0]).ToList();

                    return infoSheets;
                }

                return Enumerable.Empty<InfoSheet>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<InfoSheet>();
            }
        }

        public CResponseMessage Insert(InfoSheetViewModel infoSheet)
        {
            try
            {
                OracleParameter[] dbParam =
                {                    
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int64,
                        Value = infoSheet.CpnyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_HASCONTENT",
                        OracleDbType = OracleDbType.Int64,
                        Value = infoSheet.HasContent,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_MENUID",
                        OracleDbType = OracleDbType.Int64,
                        Value = infoSheet.MenuID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_CONTENT",
                        OracleDbType = OracleDbType.Clob,
                        Value = infoSheet.Content,  
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_LANGUAGE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = infoSheet.Language,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_TITLE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = infoSheet.Title,
                    },
                };

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_INFOSHEET_INSERT, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_INFOSHEET_INSERT, dbParam, infoSheet.RoleCode, infoSheet.Username));

                return cResponseMessage;
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return new CResponseMessage { Code = -3, Message = "MSG_TEZIR_AN_ERROR_OCCURRED" };
            }
        }

        public CResponseMessage Update(InfoSheetViewModel infoSheet)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ID",
                        OracleDbType = OracleDbType.Int64,
                        Value = infoSheet.ID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int64,
                        Value = infoSheet.CpnyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_HASCONTENT",
                        OracleDbType = OracleDbType.Int64,
                        Value = infoSheet.HasContent,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_MENUID",
                        OracleDbType = OracleDbType.Int64,
                        Value = infoSheet.MenuID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_CONTENT",
                        OracleDbType = OracleDbType.Clob,
                        Value = infoSheet.Content,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_LANGUAGE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = infoSheet.Language,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_TITLE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = infoSheet.Title,
                    },
                };

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_INFOSHEET_UPDATE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_INFOSHEET_UPDATE, dbParam, infoSheet.RoleCode, infoSheet.Username));

                return cResponseMessage;
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return new CResponseMessage { Code = -3, Message = "MSG_TEZIR_AN_ERROR_OCCURRED" };
            }
        }

        public CResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void SetRoleCode(string roleCode)
        {
            this._cBaseDataProvider.SetRoleCode(roleCode);
        }

        public void SetUsername(string username)
        {
            this._cBaseDataProvider.SetUsername(username);
        }        
    }
}
