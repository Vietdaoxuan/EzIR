using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using CoreLib.Entities.ViewModels;
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
    public class CongBoThongTinContext : ICongBoThongTinContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public CongBoThongTinContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            _config = config;
            _cBaseDataProvider = cBaseDataProvider;
            _appLogger = appLogger;
            _common = common;
        }
        public Task<CResponseMessage> Delete(CongBoThongTinViewModel congBoThongTinviewmodel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ANewID",
                        OracleDbType = OracleDbType.Int32,
                        Value = congBoThongTinviewmodel.NewID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_Ausertype",
                        OracleDbType = OracleDbType.Int32,
                        Value = 2,
                    },
                };

                _cBaseDataProvider.SetUsername(congBoThongTinviewmodel.UserLogin);
                _cBaseDataProvider.SetRoleCode(congBoThongTinviewmodel.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_NEWS_DELETE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_NEWS_DELETE, dbParam, congBoThongTinviewmodel.RoleCode, congBoThongTinviewmodel.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể xóa tin mới!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task <CResponseMessage> Insert(CongBoThongTinViewModel congBoThongTinviewmodel)
        {
            try
            {
                OracleParameter[] dbParam =
                 {
                    new OracleParameter()
                    {
                        ParameterName = "P_ACpnyID",
                        OracleDbType = OracleDbType.Int32,
                        Value = congBoThongTinviewmodel.CompanyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ASID",
                        OracleDbType = OracleDbType.Int32,
                        Value = congBoThongTinviewmodel.Sid,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADATEPUB",
                        OracleDbType = OracleDbType.Date,
                        Value = congBoThongTinviewmodel.DatePub,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADocType",
                        OracleDbType = OracleDbType.Int32,
                        Value = congBoThongTinviewmodel.DocType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANewType",
                        OracleDbType = OracleDbType.Int32,
                        Value = congBoThongTinviewmodel.NewType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ATitle",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = congBoThongTinviewmodel.Title,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_Ayear",
                        OracleDbType = OracleDbType.Int32,
                        Value = congBoThongTinviewmodel.Year,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APath",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = congBoThongTinviewmodel.Path,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AURL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = congBoThongTinviewmodel.Url,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ALanguage",
                        OracleDbType = OracleDbType.Int32,
                        Value = congBoThongTinviewmodel.Language,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AFileName",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = congBoThongTinviewmodel.FileName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "p_Ausertype",
                        OracleDbType = OracleDbType.Int32,
                        Value = 2,
                    }
                };

                _cBaseDataProvider.SetUsername(congBoThongTinviewmodel.UserLogin);
                _cBaseDataProvider.SetRoleCode(congBoThongTinviewmodel.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_NEWS_INSERT, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_NEWS_INSERT, dbParam, congBoThongTinviewmodel.RoleCode, congBoThongTinviewmodel.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể thêm tin mới!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<IEnumerable<CongBoThongTin>> Select(CongBoThongTinViewModel congBoThongTinviewmodel)
        {
            try
            {
                OracleParameter[] dbParam = 
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ANewID",
                        OracleDbType = OracleDbType.Int32,
                        Value = congBoThongTinviewmodel.NewID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADocType",
                        OracleDbType = OracleDbType.Int32,
                        Value = congBoThongTinviewmodel.DocType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANewType",
                        OracleDbType = OracleDbType.Int32,
                        Value = congBoThongTinviewmodel.NewType,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_FROMDATE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = congBoThongTinviewmodel.FromDate,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_TODATE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = congBoThongTinviewmodel.ToDate,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACreateBy",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = congBoThongTinviewmodel.CreateBy,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACpnyID",
                        OracleDbType = OracleDbType.Int32,
                        Value = congBoThongTinviewmodel.CompanyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_Username",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = null, //QuangKS
                    },
                    new OracleParameter()
                    {
                        ParameterName = "Refcursor",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_NEWS_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_NEWS_SEARCH, dbParam, congBoThongTinviewmodel.RoleCode, congBoThongTinviewmodel.UserLogin));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<CongBoThongTin>();

                    var rule = mapper.Map(dataSet.Tables[0]);

                    return Task.FromResult(rule);
                }

                return Task.FromResult(Enumerable.Empty<CongBoThongTin>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<CongBoThongTin>());
            }
        }

        public Task<CResponseMessage> Update(CongBoThongTinViewModel congBoThongTinviewmodel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ANewID",
                        OracleDbType = OracleDbType.Int32,
                        Value = congBoThongTinviewmodel.NewID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACpnyID",
                        OracleDbType = OracleDbType.Int32,
                        Value = congBoThongTinviewmodel.CompanyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADatePub",
                        OracleDbType = OracleDbType.Date,
                        Value = congBoThongTinviewmodel.DatePub,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADocType",
                        OracleDbType = OracleDbType.Int32,
                        Value = congBoThongTinviewmodel.DocType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANewType",
                        OracleDbType = OracleDbType.Int32,
                        Value = congBoThongTinviewmodel.NewType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ATitle",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = congBoThongTinviewmodel.Title,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ALanguage",
                        OracleDbType = OracleDbType.Int32,
                        Value = congBoThongTinviewmodel.Language,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_Ayear",
                        OracleDbType = OracleDbType.Int32,
                        Value = congBoThongTinviewmodel.Year,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APath",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = congBoThongTinviewmodel.Path,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AURL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = congBoThongTinviewmodel.Url,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AFileName",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = congBoThongTinviewmodel.FileName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AIsBackDate",
                        OracleDbType = OracleDbType.Int32,
                        Value = congBoThongTinviewmodel.IsBackDate,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "p_Ausertype",
                        OracleDbType = OracleDbType.Int32,
                        Value = 1,
                    }
                };

                _cBaseDataProvider.SetUsername(congBoThongTinviewmodel.UserLogin);
                _cBaseDataProvider.SetRoleCode(congBoThongTinviewmodel.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_NEWS_UPDATE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_NEWS_UPDATE, dbParam, congBoThongTinviewmodel.RoleCode, congBoThongTinviewmodel.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể sửa tin mới!" };

                return Task.FromResult(cResponseMessage);
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
    }
}
