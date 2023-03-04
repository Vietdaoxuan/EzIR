using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using CoreLib.Entities.ModelViews;
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
    public class ToChucBoMayQuanLyContext : IToChucBoMayQuanLyContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;
        private readonly string _connectionStringDBTemp = string.Empty;
        public ToChucBoMayQuanLyContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            _config = config;
            _cBaseDataProvider = cBaseDataProvider;
            _appLogger = appLogger;
            _common = common;
            this._connectionStringDBTemp = _config["ConnectionStringEzSearch"];
        }

        public CResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }

        public CResponseMessage Insert(ToChucBoMayQuanLyViewModel toChucBoMayQuanLyViewModel)
        {
            try
            {
                OracleParameter[] dbParam =
                 {
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = toChucBoMayQuanLyViewModel.CpnyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AORGMODELDESC",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = toChucBoMayQuanLyViewModel.OrgModelDesc,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AORGMODELPATH",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = toChucBoMayQuanLyViewModel.OrgModelPath,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AORGTYPE",
                        OracleDbType = OracleDbType.Int32,
                        Value = toChucBoMayQuanLyViewModel.OrgType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANOTE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = toChucBoMayQuanLyViewModel.OrgModelDescNote.Trim() + ";" + toChucBoMayQuanLyViewModel.OrgModelPath.Trim(),
                        
                    },                    
                };

                _cBaseDataProvider.SetUsername(toChucBoMayQuanLyViewModel.UserLogin);
                _cBaseDataProvider.SetRoleCode(toChucBoMayQuanLyViewModel.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_ORGMODEL_INSERT, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_ORGMODEL_INSERT, dbParam, toChucBoMayQuanLyViewModel.RoleCode, toChucBoMayQuanLyViewModel.UserLogin));

                return cResponseMessage;
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể thêm mới!" };

                return cResponseMessage;
            }
        }
        public Task<IEnumerable<ToChucBoMayQuanLyModelView>> Select(ToChucBoMayQuanLyViewModel toChucBoMayQuanLyViewModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ToChucBoMayQuanLyModelView> SelectEzIR(ToChucBoMayQuanLyViewModel toChucBoMayQuanLyViewModel)
        {           
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ORGMODELID",
                        OracleDbType = OracleDbType.Int32,
                        Value = toChucBoMayQuanLyViewModel.OrgModelID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = toChucBoMayQuanLyViewModel.CpnyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APPROVE",
                        OracleDbType = OracleDbType.Int32,
                        Value = toChucBoMayQuanLyViewModel.APPROVE,
                    },
                    // new OracleParameter()
                    //{
                    //    ParameterName = "P_AORGTYPE",
                    //    OracleDbType = OracleDbType.Int32,
                    //    Value = toChucBoMayQuanLyViewModel.OrgType,
                    //},
                    new OracleParameter()
                    {
                        ParameterName = "Refcursor",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSetEzIR = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_ORGMODEL_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_ORGMODEL_SEARCH, dbParam, toChucBoMayQuanLyViewModel.RoleCode, toChucBoMayQuanLyViewModel.UserLogin));

                if (dataSetEzIR.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSetEzIR));

                    var mapper = new DataNamesMapper<ToChucBoMayQuanLyModelView>();

                    var toChucBoMayQuanLys = mapper.Map(dataSetEzIR.Tables[0]);

                    return toChucBoMayQuanLys;
                }

                return Enumerable.Empty<ToChucBoMayQuanLyModelView>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<ToChucBoMayQuanLyModelView>();
            }
        }

        public IEnumerable<ToChucBoMayQuanLyModelView> SelectEzSearch(ToChucBoMayQuanLyViewModel toChucBoMayQuanLyViewModel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "Porgmodelid",
                        OracleDbType = OracleDbType.Int32,
                        Value = toChucBoMayQuanLyViewModel.OrgModelID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "Pcpnyid",
                        OracleDbType = OracleDbType.Int32,
                        Value = toChucBoMayQuanLyViewModel.CpnyID,
                    },
                    //new OracleParameter()
                    //{
                    //    ParameterName = "Porgtype",
                    //    OracleDbType = OracleDbType.Int32,
                    //    Value = toChucBoMayQuanLyViewModel.OrgType,
                    //},
                    new OracleParameter()
                    {
                        ParameterName = "RefCursor",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSetEzSearch = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBTemp, ConstantsSP.SPEZS_IR_ORGMODEL_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZS_IR_ORGMODEL_SEARCH, dbParam, toChucBoMayQuanLyViewModel.RoleCode, toChucBoMayQuanLyViewModel.UserLogin));

                if (dataSetEzSearch.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSetEzSearch));

                    var mapper = new DataNamesMapper<ToChucBoMayQuanLyModelView>();

                    var toChucBoMayQuanLys = mapper.Map(dataSetEzSearch.Tables[0]);

                    return toChucBoMayQuanLys;
                }

                return Enumerable.Empty<ToChucBoMayQuanLyModelView>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<ToChucBoMayQuanLyModelView>();
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

        public CResponseMessage Update(ToChucBoMayQuanLyViewModel toChucBoMayQuanLyViewModel)
        {
            try
            {
                OracleParameter[] dbParam =
                 {
                    new OracleParameter()
                    {
                        ParameterName = "P_AORGMODELID",
                        OracleDbType = OracleDbType.Int32,
                        Value = toChucBoMayQuanLyViewModel.OrgModelID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = toChucBoMayQuanLyViewModel.CpnyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AORGMODELDESC",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = toChucBoMayQuanLyViewModel.OrgModelDesc,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AORGMODELPATH",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = toChucBoMayQuanLyViewModel.OrgModelPath,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AORGTYPE",
                        OracleDbType = OracleDbType.Int32,
                        Value = toChucBoMayQuanLyViewModel.OrgType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANOTE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = toChucBoMayQuanLyViewModel.OrgModelDescNote.Trim() + ";" +toChucBoMayQuanLyViewModel.OrgModelPath.Trim(),
                    },
                };

                _cBaseDataProvider.SetUsername(toChucBoMayQuanLyViewModel.UserLogin);
                _cBaseDataProvider.SetRoleCode(toChucBoMayQuanLyViewModel.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_ORGMODEL_UPDATE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_ORGMODEL_UPDATE, dbParam, toChucBoMayQuanLyViewModel.RoleCode, toChucBoMayQuanLyViewModel.UserLogin));

                return cResponseMessage;
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể sửa!" };

                return cResponseMessage;
            }
        }
    } 
}
