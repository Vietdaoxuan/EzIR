using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using EzIRCustomerAPI.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models.Support;
using EzIRSpecialist.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Implementations
{
    public class SupportContext : ISupportContext
    {
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;
        private readonly string _connectionStringDBTemp = string.Empty;

        public SupportContext(IStringLocalizer<SharedResource> stringLocalizer, IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            this._stringLocalizer = stringLocalizer;
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;
            this._connectionStringDBTemp = _config[""];
        }

        public Task<CResponseMessage> XuLyDongBo(string LoaiDongBo)
        {
            try
            {
                CResponseMessage cResponseMessages = new CResponseMessage();

                cResponseMessages.Code = 1;
                cResponseMessages.Message = "error";

                //_cBaseDataProvider.SetUsername(JobsStatusViewModel.UserLogin);
                //_cBaseDataProvider.SetRoleCode(JobsStatusViewModel.RoleCode);

                //_appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_CHUYENVIEN_INSERT, dbParam, chuyenvien.RoleCode, chuyenvien.UserLogin));

                OracleParameter[] dbParam =
                    {
                        new OracleParameter()
                        {
                            ParameterName = "PName",
                            OracleDbType = OracleDbType.NVarchar2,
                            Value = LoaiDongBo,
                        }
                    };

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(_connectionStringDBTemp, ConstantsSP.SPEZSTEMP_CALL_SYNC, dbParam);
                return Task.FromResult(cResponseMessage);
            }
            catch(Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -2, Message = "Lỗi hệ thống" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<IEnumerable<JobsStatus>> Select(JobsStatusViewModel jobsStatusViewModel)
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
                    }
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBTemp, Commons.ConstantsSP.SPEZSTEMP_JOBS_STATUS, dbParam);

                var c = dataSet.Tables[0].Rows;

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZSTEMP_JOBS_STATUS, dbParam, jobsStatusViewModel.RoleCode, jobsStatusViewModel.UserName));

                if (dataSet.Tables.Count > 0)
                {

                    var mapper = new DataNamesMapper<JobsStatus>();

                    var actionhistory = mapper.Map(dataSet.Tables[0]);

                    return Task.FromResult(actionhistory);
                }

                return Task.FromResult(Enumerable.Empty<JobsStatus>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<JobsStatus>());
            }
        }
    }
}
