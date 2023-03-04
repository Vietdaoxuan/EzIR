using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models.ModelView.QLTKModelView;
using EzIRSpecialist.Models.QuanLyTaiKhoan;
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
    public class QLTKDoanhNghiepContext : IQLTKDoanhNghiepContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;


        public QLTKDoanhNghiepContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;
        }

        public Task<CResponseMessage> Delete(QLTKDoanhNghiepViewModel qLTKDoanhNghiep)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_AUSERNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.Username,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACPNYID",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.CompanyID,
                    },
                };

                _cBaseDataProvider.SetUsername(qLTKDoanhNghiep.UserLogin);
                _cBaseDataProvider.SetRoleCode(qLTKDoanhNghiep.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_TKDOANHNGHIEP_DELETE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_TKDOANHNGHIEP_DELETE, dbParam, qLTKDoanhNghiep.RoleCode, qLTKDoanhNghiep.Username));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể xóa tài khoản doanh nghiệp!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<CResponseMessage> Insert(QLTKDoanhNghiepViewModel qLTKDoanhNghiep)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_AUSERNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.Username,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AFULLNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.FullName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APASSWORD",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.Password,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AEMAIL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.Email,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APHONE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.Phone,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANOTE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.Note,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AACTIVE",
                        OracleDbType = OracleDbType.Int32,
                        Value = qLTKDoanhNghiep.Active,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACOMPANYTYPE",
                        OracleDbType = OracleDbType.Int32,
                        Value = qLTKDoanhNghiep.CompanyType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ASTOCKCODE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.StockCode,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.CompanyName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANAMEEN",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.CompanyNameEN,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = qLTKDoanhNghiep.CompanyID,
                    },
                };

                _cBaseDataProvider.SetUsername(qLTKDoanhNghiep.UserLogin);
                _cBaseDataProvider.SetRoleCode(qLTKDoanhNghiep.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_TKDOANHNGHIEP_INSERT, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_TKDOANHNGHIEP_INSERT, dbParam, qLTKDoanhNghiep.RoleCode, qLTKDoanhNghiep.Username));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể thêm mới tài khoản doanh nghiệp!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<IEnumerable<QLTKDoanhNghiepModelView>> Select(QLTKDoanhNghiepViewModel qLTKDoanhNghiep)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_USERNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.Username,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_FULLNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.FullName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_EMAIL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.Email,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACTIVE",
                        OracleDbType = OracleDbType.Int32,
                        Value = qLTKDoanhNghiep.Active,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AEXPERT",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.Expert,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = qLTKDoanhNghiep.CompanyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACOMPANYTYPE",
                        OracleDbType = OracleDbType.Int32,
                        Value = qLTKDoanhNghiep.CompanyType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_CUSTOMER_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_CUSTOMER_SEARCH, dbParam, qLTKDoanhNghiep.RoleCode, qLTKDoanhNghiep.UserLogin));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));
                    var mapper = new DataNamesMapper<QLTKDoanhNghiepModelView>();

                    var doanhnghiep = mapper.Map(dataSet.Tables[0]);
                    return Task.FromResult(doanhnghiep);
                }


                return Task.FromResult(Enumerable.Empty<QLTKDoanhNghiepModelView>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<QLTKDoanhNghiepModelView>());
            }
        }

        public Task<CResponseMessage> Update(QLTKDoanhNghiepViewModel qLTKDoanhNghiep)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_AUSERNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.Username,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APASSWORD",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.Password,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AFULLNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.FullName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AEMAIL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.Email,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APHONE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.Phone,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANOTE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = qLTKDoanhNghiep.Note,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AACTIVE",
                        OracleDbType = OracleDbType.Int32,
                        Value = qLTKDoanhNghiep.Active,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = qLTKDoanhNghiep.CompanyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACOMPANYTYPE",
                        OracleDbType = OracleDbType.Int32,
                        Value = qLTKDoanhNghiep.CompanyType,
                    },
                };

                _cBaseDataProvider.SetUsername(qLTKDoanhNghiep.UserLogin);
                _cBaseDataProvider.SetRoleCode(qLTKDoanhNghiep.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_TKDOANHNGHIEP_UPDATE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_TKDOANHNGHIEP_UPDATE, dbParam, qLTKDoanhNghiep.RoleCode, qLTKDoanhNghiep.Username));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể thêm mới tài khoản doanh nghiệp!" };

                return Task.FromResult(cResponseMessage);
            }
        }
    }
}
