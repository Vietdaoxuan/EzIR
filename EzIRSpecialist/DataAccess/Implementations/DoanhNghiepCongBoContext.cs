using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models.ModelView.QuanLyDoanhNghiepCongBo;
using EzIRSpecialist.Models.ViewModel.QuanLyDoanhNghiepCongBo;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Implementations
{
    public class DoanhNghiepCongBoContext : IDoanhNghiepCongBoContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public DoanhNghiepCongBoContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;
        }
        public Task<CResponseMessage> Delete(DoanhNghiepCongBoViewModel doanhNghiepCongBo)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ANewID",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.NewID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACpnyID",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.CompanyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_Ausertype",
                        OracleDbType = OracleDbType.Int32,
                        Value = 1,
                    },
                };

                _cBaseDataProvider.SetUsername(doanhNghiepCongBo.UserLogin);
                _cBaseDataProvider.SetRoleCode(doanhNghiepCongBo.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_NEWS_DELETE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_NEWS_DELETE, dbParam, doanhNghiepCongBo.RoleCode, doanhNghiepCongBo.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể xóa tin mới!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<CResponseMessage> Insert(DoanhNghiepCongBoViewModel doanhNghiepCongBo)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ACpnyID",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.CompanyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ASID",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.Sid,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADATEPUB",
                        OracleDbType = OracleDbType.Date,
                        Value = doanhNghiepCongBo.DatePub,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADocType",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.DocType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANewType",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.NewType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ATitle",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = doanhNghiepCongBo.Title,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_Ayear",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.Year,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APath",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = doanhNghiepCongBo.Path,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AURL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = doanhNghiepCongBo.Url,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ALanguage",
                        OracleDbType = OracleDbType.Int32,
                        Value = 1,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AFileName",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = doanhNghiepCongBo.FileName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "p_Ausertype",
                        OracleDbType = OracleDbType.Int32,
                        Value = 1,
                    }
                };

                _cBaseDataProvider.SetUsername(doanhNghiepCongBo.UserLogin);
                _cBaseDataProvider.SetRoleCode(doanhNghiepCongBo.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_NEWS_INSERT, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_NEWS_INSERT, dbParam, doanhNghiepCongBo.RoleCode, doanhNghiepCongBo.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể thêm tin mới!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<DataTable> ListDoanhNghiepCongBo(DoanhNghiepCongBoViewModel doanhNghiepCongBo)
        {

            OracleParameter[] dbParam =
            {
                new OracleParameter()
                {
                    ParameterName = "P_LISTNEWID",
                    OracleDbType = OracleDbType.NVarchar2,
                    Value = doanhNghiepCongBo.ListNewID,
                },
                new OracleParameter()
                {
                    ParameterName = "REFCURSOR",
                    Direction = ParameterDirection.Output,
                    OracleDbType = OracleDbType.RefCursor,
                },
            };


            var dataTable = _cBaseDataProvider.GetDataTableFromSP(ConstantsSP.SPEZIR_NEWS_EXPORTEXCEL, dbParam);

            return Task.FromResult(dataTable);

        }

        public Task<IEnumerable<DoanhNghiepCongBoModelView>> Select(DoanhNghiepCongBoViewModel doanhNghiepCongBo)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_STOCKCODE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = doanhNghiepCongBo.StockCode,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACOMPANYTYPE",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.CompanyType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANEWID",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.NewID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANEWTYPE",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.NewType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADOCTYPE",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.DocType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AUSERTYPE",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.UserType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AREGIONID",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.RegionID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AEXPERT",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = doanhNghiepCongBo.Expert,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.CompanyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_FROMDATE",
                        OracleDbType = OracleDbType.Date,
                        Value = doanhNghiepCongBo.FromDate,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_TODATE",
                        OracleDbType = OracleDbType.Date,
                        Value = doanhNghiepCongBo.ToDate,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ISPUBLIC",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.IsPublic,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_USERPOST",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = doanhNghiepCongBo.UserPost,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                };


                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_DOANHNGHIEPCONGBO_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_DOANHNGHIEPCONGBO_SEARCH, dbParam, doanhNghiepCongBo.RoleCode, doanhNghiepCongBo.UserLogin));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));
                    var mapper = new DataNamesMapper<DoanhNghiepCongBoModelView>();

                    var doanhnghiep = mapper.Map(dataSet.Tables[0]);
                    return Task.FromResult(doanhnghiep);
                }


                return Task.FromResult(Enumerable.Empty<DoanhNghiepCongBoModelView>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<DoanhNghiepCongBoModelView>());
            }
        }

        public Task<CResponseMessage> Update(DoanhNghiepCongBoViewModel doanhNghiepCongBo)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ANewID",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.NewID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACpnyID",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.CompanyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADatePub",
                        OracleDbType = OracleDbType.Date,
                        Value = doanhNghiepCongBo.DatePub,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADocType",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.DocType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANewType",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.NewType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ALanguage",
                        OracleDbType = OracleDbType.Int32,
                        Value = 1,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ATitle",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = doanhNghiepCongBo.Title,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_Ayear",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.Year,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APath",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = doanhNghiepCongBo.Path,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AURL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = doanhNghiepCongBo.Url,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AFileName",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = doanhNghiepCongBo.FileName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AIsBackDate",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiepCongBo.IsBackDate,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "p_Ausertype",
                        OracleDbType = OracleDbType.Int32,
                        Value = 1,
                    }
                };

                _cBaseDataProvider.SetUsername(doanhNghiepCongBo.UserLogin);
                _cBaseDataProvider.SetRoleCode(doanhNghiepCongBo.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_NEWS_UPDATE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_NEWS_UPDATE, dbParam, doanhNghiepCongBo.RoleCode, doanhNghiepCongBo.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể sửa tin mới!" };

                return Task.FromResult(cResponseMessage);
            }
        }
    }
}
