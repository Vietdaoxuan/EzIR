using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models.BaoCaoTienIch;
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
    public class NhatKyHoatDongContext : INhatKyHoatDongContext
    {

        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public NhatKyHoatDongContext(ICommon common, IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;

        }

        public Task<CResponseMessage> Delete(NhatKyHoatDongViewModel obj)
        {
            throw new NotImplementedException();
        }

        public Task<CResponseMessage> Insert(NhatKyHoatDongViewModel obj)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NhatKyHoatDong>> Select(NhatKyHoatDongViewModel nhatKyHoatDongViewModel)
        {
            try
            {

                OracleParameter[] dbParam =
                {
                   new OracleParameter()
                    {
                        ParameterName = "P_STOCKCODE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = nhatKyHoatDongViewModel.Astockcode,
                    },

                    new OracleParameter()
                    {
                        ParameterName = "P_USERSEARCH",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = nhatKyHoatDongViewModel.Ausername,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACTION",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = nhatKyHoatDongViewModel.ActionC,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_FROMDATE",
                        OracleDbType = OracleDbType.Date,
                        Value = nhatKyHoatDongViewModel.FormDate,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_TODATE",
                        OracleDbType = OracleDbType.Date,
                        Value = nhatKyHoatDongViewModel.ToDate,
                    },

                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                _cBaseDataProvider.SetUsername(nhatKyHoatDongViewModel.UserName);
                _cBaseDataProvider.SetRoleCode(nhatKyHoatDongViewModel.RoleCode);

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_ACTION_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_ACTION_SEARCH, dbParam, nhatKyHoatDongViewModel.RoleCode, nhatKyHoatDongViewModel.UserName));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<NhatKyHoatDong>();

                    var actionhistory = mapper.Map(dataSet.Tables[0]);

                    return Task.FromResult(actionhistory);
                }

                return Task.FromResult(Enumerable.Empty<NhatKyHoatDong>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<NhatKyHoatDong>());
            }
        }

        public Task<CResponseMessage> Update(NhatKyHoatDongViewModel obj)
        {
            throw new NotImplementedException();
        }
    }
}
