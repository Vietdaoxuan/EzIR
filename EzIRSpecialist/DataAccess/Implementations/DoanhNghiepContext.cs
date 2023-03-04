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
    public class DoanhNghiepContext : IDoanhNghiepContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public DoanhNghiepContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;
        }

        public Task<CResponseMessage> Delete(DoanhNghiepViewModel doanhNghiep)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ACPNYID",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = doanhNghiep.CompanyID,
                    },
                };

                _cBaseDataProvider.SetUsername(doanhNghiep.UserLogin);
                _cBaseDataProvider.SetRoleCode(doanhNghiep.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_COMPANY_DELETE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_COMPANY_DELETE, dbParam, doanhNghiep.RoleCode, doanhNghiep.Username));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể xóa doanh nghiệp!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<CResponseMessage> Insert(DoanhNghiepViewModel obj)
        {
            throw new NotImplementedException();
        }

        public Task<CResponseMessage> InsertCompanyClient(CompanyClientViewModel companyClient)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ACPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = companyClient.CompanyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ASTOCK_CODE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = companyClient.StockCode,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AISDELETE",
                        OracleDbType = OracleDbType.Int32,
                        Value = companyClient.IsDelete,
                    }
                };

                _cBaseDataProvider.SetUsername(companyClient.UserLogin);
                _cBaseDataProvider.SetRoleCode(companyClient.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_COMPANYCLIENT_INSERT, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_COMPANYCLIENT_INSERT, dbParam, companyClient.RoleCode, companyClient.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể update link doanh nghiệp!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<IEnumerable<DoanhNghiepModelView>> Select(DoanhNghiepViewModel qLTKDoanhNghiep)
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
                        ParameterName = "P_NDBC",
                        OracleDbType = OracleDbType.Int32,
                        Value = qLTKDoanhNghiep.NDBC,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_LEVEL",
                        OracleDbType = OracleDbType.Int32,
                        Value = qLTKDoanhNghiep.Level,
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


                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_DOANHNGHIEP_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_DOANHNGHIEP_SEARCH, dbParam, qLTKDoanhNghiep.RoleCode, qLTKDoanhNghiep.UserLogin));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));
                    var mapper = new DataNamesMapper<DoanhNghiepModelView>();

                    var doanhnghiep = mapper.Map(dataSet.Tables[0]);
                    return Task.FromResult(doanhnghiep);
                }


                return Task.FromResult(Enumerable.Empty<DoanhNghiepModelView>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<DoanhNghiepModelView>());
            }
        }

        public Task<IEnumerable<CompanyRole>> SelectCompanyRole()
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                };


                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_COMPANYROLE_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_COMPANYROLE_SEARCH, dbParam, null, null));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));
                    var mapper = new DataNamesMapper<CompanyRole>();

                    var companyRole = mapper.Map(dataSet.Tables[0]);
                    return Task.FromResult(companyRole);
                }


                return Task.FromResult(Enumerable.Empty<CompanyRole>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<CompanyRole>());
            }
        }

        public Task<IEnumerable<DoanhNghiepModelView>> SelectCustomerByExpert(DoanhNghiepViewModel doanhNghiep)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_USERNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = doanhNghiep.Username,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_FULLNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = doanhNghiep.FullName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_EMAIL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = doanhNghiep.Email,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACTIVE",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiep.Active,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AEXPERT",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = doanhNghiep.Expert,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiep.CompanyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACOMPANYTYPE",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiep.CompanyType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_CUSTOMER_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_CUSTOMER_SEARCH, dbParam, doanhNghiep.RoleCode, doanhNghiep.UserLogin));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));
                    var mapper = new DataNamesMapper<DoanhNghiepModelView>();

                    var doanhnghiep = mapper.Map(dataSet.Tables[0]);
                    return Task.FromResult(doanhnghiep);
                }


                return Task.FromResult(Enumerable.Empty<DoanhNghiepModelView>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<DoanhNghiepModelView>());
            }
        }

        public Task<CResponseMessage> Update(DoanhNghiepViewModel doanhNghiep)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ACPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiep.CompanyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AEXPERT",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = doanhNghiep.Expert,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AFISCALYEAR",
                        OracleDbType = OracleDbType.Date,
                        Value = doanhNghiep.FiscalYear,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ALEVEL",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiep.Level,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANOTE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = doanhNghiep.Note,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ALISTSTATUS",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = doanhNghiep.ListStatus,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ASTATUS",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiep.Status,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_NIENDOBCTC",
                        OracleDbType = OracleDbType.Int32,
                        Value = doanhNghiep.NienDoBCTC,
                    }
                };

                _cBaseDataProvider.SetUsername(doanhNghiep.UserLogin);
                _cBaseDataProvider.SetRoleCode(doanhNghiep.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_DOANHNGHIEP_UPDATE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_DOANHNGHIEP_UPDATE, dbParam, doanhNghiep.RoleCode, doanhNghiep.Username));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể thêm mới doanh nghiệp!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<CResponseMessage> UpdateCompanyClient(CompanyClientViewModel companyClient)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ACPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = companyClient.CompanyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AISDELETE",
                        OracleDbType = OracleDbType.Int32,
                        Value = companyClient.IsDelete,
                    }
                };

                _cBaseDataProvider.SetUsername(companyClient.UserLogin);
                _cBaseDataProvider.SetRoleCode(companyClient.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_COMPANYCLIENT_UPDATE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_COMPANYCLIENT_UPDATE, dbParam, companyClient.RoleCode, companyClient.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể update link doanh nghiệp!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<CResponseMessage> UpdateCompanyRole(CompanyRoleViewModel companyRole)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ACPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = companyRole.CompanyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AROLECODE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = companyRole.RoleCompany,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ASTATUS",
                        OracleDbType = OracleDbType.Int32,
                        Value = companyRole.Status,
                    }
                };

                _cBaseDataProvider.SetUsername(companyRole.UserLogin);
                _cBaseDataProvider.SetRoleCode(companyRole.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_COMPANYROLE_UPDATE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_COMPANYROLE_UPDATE, dbParam, companyRole.RoleCode, companyRole.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể update Role doanh nghiệp!" };

                return Task.FromResult(cResponseMessage);
            }
        }
    }
}
