using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models;
using EzIRSpecialist.Models.ModelView.QLTKModelView;
using EzIRSpecialist.Models.ViewModel;
using EzIRSpecialist.Models.ViewModel.QLTKViewModel;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Implementations
{
    public class ChuyenVienContext : IChuyenVienContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public ChuyenVienContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;
        }

        public Task<IEnumerable<ChuyenVienModelView>> Select(ChuyenVienViewModel chuyenVien)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_AUSERNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = chuyenVien.Username,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AREGION",
                        OracleDbType = OracleDbType.Int32,
                        Value = chuyenVien.RegionID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AACTIVE",
                        OracleDbType = OracleDbType.Int32,
                        Value = chuyenVien.Active,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_CHUYENVIEN_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_CHUYENVIEN_SEARCH, dbParam, chuyenVien.RoleCode, chuyenVien.UserLogin));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));
                    var mapper = new DataNamesMapper<ChuyenVienModelView>();

                    var chuyenvien = mapper.Map(dataSet.Tables[0]);
                    return Task.FromResult(chuyenvien);
                }


                return Task.FromResult(Enumerable.Empty<ChuyenVienModelView>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<ChuyenVienModelView>());
            }

        }

        public Task<CResponseMessage> Insert(ChuyenVienViewModel chuyenvien)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_AUSERNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = chuyenvien.Username,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AFULLNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = chuyenvien.FullName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AEMAIL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = chuyenvien.Email,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACC",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = chuyenvien.EmailCc,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APHONE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = chuyenvien.Phone,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANOTE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = chuyenvien.Note,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AREGIONID",
                        OracleDbType = OracleDbType.Int32,
                        Value = chuyenvien.RegionID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AACTIVE",
                        OracleDbType = OracleDbType.Int32,
                        Value = chuyenvien.Active,
                    }
                };

                _cBaseDataProvider.SetUsername(chuyenvien.UserLogin);
                _cBaseDataProvider.SetRoleCode(chuyenvien.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_CHUYENVIEN_INSERT, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_CHUYENVIEN_INSERT, dbParam, chuyenvien.RoleCode, chuyenvien.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);    

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể thêm mới chuyên viên!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<CResponseMessage> Update(ChuyenVienViewModel chuyenvien)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_AUSERNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = chuyenvien.Username,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AFULLNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = chuyenvien.FullName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AEMAIL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = chuyenvien.Email,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACC",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = chuyenvien.EmailCc,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APHONE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = chuyenvien.Phone,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANOTE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = chuyenvien.Note,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AREGIONID",
                        OracleDbType = OracleDbType.Int32,
                        Value = chuyenvien.RegionID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AACTIVE",
                        OracleDbType = OracleDbType.Int32,
                        Value = chuyenvien.Active,
                    },
                    
                };

                _cBaseDataProvider.SetUsername(chuyenvien.UserLogin);
                _cBaseDataProvider.SetRoleCode(chuyenvien.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_CHUYENVIEN_UPDATE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_CHUYENVIEN_UPDATE, dbParam, chuyenvien.RoleCode, chuyenvien.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể update chuyên viên!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<CResponseMessage> Delete(ChuyenVienViewModel chuyenvien)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_AUSERNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = chuyenvien.Username,
                    },
                };

                _cBaseDataProvider.SetUsername(chuyenvien.UserLogin);
                _cBaseDataProvider.SetRoleCode(chuyenvien.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_CHUYENVIEN_DELETE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_CHUYENVIEN_DELETE, dbParam, chuyenvien.RoleCode, chuyenvien.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể xóa chuyên viên!" };

                return Task.FromResult(cResponseMessage);
            }
        }
    }
}
