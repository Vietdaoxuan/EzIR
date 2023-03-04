using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Implementations;
using DataServiceLib.Interfaces;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models.Admin;
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
    public class BannerContext : IBannerContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public BannerContext(ICommon common, IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;

        }

        private OracleParameter[] InitDbParam(BannerViewModel bannerViewModel)
        {
            return new[]
            {
                 new OracleParameter()
                    {
                        ParameterName = "P_ABannerID",
                        OracleDbType = OracleDbType.Int16,
                        Value = bannerViewModel.AbannerID,
                    },

                    new OracleParameter()
                    {
                        ParameterName = "P_AFILENAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = bannerViewModel.AfileName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ATITLE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = bannerViewModel.Atitle,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AURL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = bannerViewModel.Aurl,
                    },
                 
                   new OracleParameter()
                    {
                        ParameterName = "P_ALOGO",
                        OracleDbType = OracleDbType.Blob,
                        Value = bannerViewModel.ALogo,
                    }

            };
        }

        public Task<CResponseMessage> Delete(BannerViewModel bannerViewModel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ABannerID",
                        OracleDbType = OracleDbType.Int16,
                        Value = bannerViewModel.AbannerID,
                    },
                };

                _cBaseDataProvider.SetUsername(bannerViewModel.UserName);
                _cBaseDataProvider.SetRoleCode(bannerViewModel.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_BANNER_DELETE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_BANNER_DELETE, dbParam, bannerViewModel.RoleCode, bannerViewModel.UserName));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể xóa nhóm!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<CResponseMessage> Insert(BannerViewModel bannerViewModel)
        {
            try
            {
                var dbParam = this.InitDbParam(bannerViewModel);

                _cBaseDataProvider.SetUsername(bannerViewModel.UserName);
                _cBaseDataProvider.SetRoleCode(bannerViewModel.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_BANNER_INSERT, dbParam);

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -1, Message = "Lỗi không thể thêm Banner!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<IEnumerable<Banner>> Select(BannerViewModel bannerViewModel)
        {
            try
            {

                OracleParameter[] dbParam =
                {

                    new OracleParameter()
                    {
                        ParameterName = "P_AFILENAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = bannerViewModel.AfileName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ATITLE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = bannerViewModel.Atitle,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AURL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = bannerViewModel.Aurl,
                    },
                  
                    
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                _cBaseDataProvider.SetUsername(bannerViewModel.UserName);
                _cBaseDataProvider.SetRoleCode(bannerViewModel.RoleCode);

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_BANNER_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_BANNER_SEARCH, dbParam, bannerViewModel.RoleCode, bannerViewModel.UserName));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<Banner>();

                    var banner = mapper.Map(dataSet.Tables[0]);

                    return Task.FromResult(banner);
                }

                return Task.FromResult(Enumerable.Empty<Banner>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<Banner>());
            }
        }

        public Task<CResponseMessage> Update(BannerViewModel bannerViewModel)
        {
            try
            {
                var dbParam = this.InitDbParam(bannerViewModel);

                _cBaseDataProvider.SetUsername(bannerViewModel.UserName);

                _cBaseDataProvider.SetRoleCode(bannerViewModel.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_BANNER_UPDATE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_BANNER_UPDATE, dbParam, bannerViewModel.RoleCode, bannerViewModel.UserName));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -1, Message = "Lỗi không thể sửa banner!" };

                return Task.FromResult(cResponseMessage);
            }
        }
    }
}
