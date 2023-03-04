using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
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
    public class ThuVienPhapLuatContext : IThuVienPhapLuatContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public ThuVienPhapLuatContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;
        }
        public Task<CResponseMessage> Delete(ThuVienPhapLuatViewModel thuVien)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ID",
                        OracleDbType = OracleDbType.Int32,
                        Value = thuVien.ID,
                    },
                    
                };

                _cBaseDataProvider.SetUsername(thuVien.UserLogin);
                _cBaseDataProvider.SetRoleCode(thuVien.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_LIBOFLAW_DELETE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_LIBOFLAW_DELETE, dbParam, thuVien.RoleCode, thuVien.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể xóa văn bản!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<CResponseMessage> Insert(ThuVienPhapLuatViewModel thuVien)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_TYPEDOC",
                        OracleDbType = OracleDbType.Int32,
                        Value = thuVien.TypeDoc,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_COMPANY",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = thuVien.Company,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_NO",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = thuVien.No,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_DATEPUB",
                        OracleDbType = OracleDbType.Date,
                        Value = thuVien.DatePub,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_TEXTNOTE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = thuVien.TextNote,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_DATEEFF",
                        OracleDbType = OracleDbType.Date,
                        Value = thuVien.DateEff,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_FILENAME",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = thuVien.FileName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_PATH",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = thuVien.Path,
                    },
                    
                    
                };

                _cBaseDataProvider.SetUsername(thuVien.UserLogin);
                _cBaseDataProvider.SetRoleCode(thuVien.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_LIBOFLAW_INSERT, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_LIBOFLAW_INSERT, dbParam, thuVien.RoleCode, thuVien.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể thêm văn bản!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<IEnumerable<ThuVienPhapLuatModelView>> SelectIndex(ThuVienPhapLuatViewModel thuVien)
        {

            OracleParameter[] dbParam =
            {
                new OracleParameter()
                {
                    ParameterName = "P_ID",
                    OracleDbType = OracleDbType.Int32,
                    Value = thuVien.ID,
                },
                new OracleParameter()
                {
                    ParameterName = "REFCURSOR",
                    Direction = ParameterDirection.Output,
                    OracleDbType = OracleDbType.RefCursor,
                },
            };


            var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_LIBOFLAW_ID_SEARCH, dbParam);
            if (dataSet.Tables.Count > 0)
            {
                _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));
                var mapper = new DataNamesMapper<ThuVienPhapLuatModelView>();

                var doanhnghiep = mapper.Map(dataSet.Tables[0]);
                return Task.FromResult(doanhnghiep);
            }


            return Task.FromResult(Enumerable.Empty<ThuVienPhapLuatModelView>());
            

        }

        public Task<IEnumerable<ThuVienPhapLuatModelView>> Select(ThuVienPhapLuatViewModel thuVien)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                   
                   new OracleParameter()
                    {
                        ParameterName = "P_TYPEDOC",
                        OracleDbType = OracleDbType.Int32,
                        Value = thuVien.TypeDoc,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_COMPANY",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = thuVien.Company,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_NO",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = thuVien.No,
                    },

                    new OracleParameter()
                    {
                        ParameterName = "P_YEARPUB",
                        OracleDbType = OracleDbType.Int32,
                        Value = thuVien.YearPub,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_YEAREFF",
                        OracleDbType = OracleDbType.Int32,
                        Value = thuVien.YearEff,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_TEXTNOTE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = thuVien.TextNote,
                    },
                   
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                };


                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_LIBOFLAW_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_LIBOFLAW_SEARCH, dbParam, thuVien.RoleCode, thuVien.UserLogin));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));
                    var mapper = new DataNamesMapper<ThuVienPhapLuatModelView>();

                    var doanhnghiep = mapper.Map(dataSet.Tables[0]);
                    return Task.FromResult(doanhnghiep);
                }


                return Task.FromResult(Enumerable.Empty<ThuVienPhapLuatModelView>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<ThuVienPhapLuatModelView>());
            }
        }

        public Task<CResponseMessage> Update(ThuVienPhapLuatViewModel thuVien)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ID",
                        OracleDbType = OracleDbType.Int32,
                        Value = thuVien.ID,
                    },
                   new OracleParameter()
                    {
                        ParameterName = "P_TYPEDOC",
                        OracleDbType = OracleDbType.Int32,
                        Value = thuVien.TypeDoc,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_COMPANY",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = thuVien.Company,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_NO",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = thuVien.No,
                    },

                    
                    new OracleParameter()
                    {
                        ParameterName = "P_TEXTNOTE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = thuVien.TextNote,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_DATEEFF",
                        OracleDbType = OracleDbType.Date,
                        Value = thuVien.DateEff,
                    },new OracleParameter()
                    {
                        ParameterName = "P_DATEPUB",
                        OracleDbType = OracleDbType.Date,
                        Value = thuVien.DatePub,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_FILENAME",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = thuVien.FileName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_PATH",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = thuVien.Path,
                    },
                    
                };

                _cBaseDataProvider.SetUsername(thuVien.UserLogin);
                _cBaseDataProvider.SetRoleCode(thuVien.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_LIBOFLAW_UPDATE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_LIBOFLAW_UPDATE, dbParam, thuVien.RoleCode, thuVien.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể sửa văn bản!" };

                return Task.FromResult(cResponseMessage);
            }
        }
    }
}
