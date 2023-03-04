using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using DataServiceLib.Interfaces;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models;
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
    public class DSTinDangBackDateContext : IDSTinDangBackDateContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public DSTinDangBackDateContext(ICommon common, IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;

        }

        private OracleParameter[] InitDbParam(DSTinDangBackDateViewModel dSTinDangBackDateViewModel)
        {
            return new[]
            {
                new OracleParameter()
                    {
                        ParameterName = "P_STOCKCODE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = dSTinDangBackDateViewModel.AStockCode,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_COMPANYTYPE",
                        OracleDbType = OracleDbType.Int16,
                        Value = dSTinDangBackDateViewModel.ACompanyType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_EXPERT",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = dSTinDangBackDateViewModel.AExpert,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_REGION",
                        OracleDbType = OracleDbType.Int16,
                        Value = dSTinDangBackDateViewModel.ARegion,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_FROMDATE",
                        OracleDbType = OracleDbType.Date,
                        Value = dSTinDangBackDateViewModel.AFromDate,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_TODATE",
                        OracleDbType = OracleDbType.Date,
                        Value = dSTinDangBackDateViewModel.AToDate,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_DOCTYPE",
                        OracleDbType = OracleDbType.Int16,
                        Value = dSTinDangBackDateViewModel.ADocType,
                    },
                      new OracleParameter()
                    {
                        ParameterName = "P_NEWTYPE",
                        OracleDbType = OracleDbType.Int16,
                        Value = dSTinDangBackDateViewModel.ANewType,
                    },

                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
            };
        }

        public Task<IEnumerable<DSTinDangBackDate>> ListTinDangBackDate(DSTinDangBackDateViewModel dSTinDangBackDateViewModel)
        {

            try
            {
                var dbParam = this.InitDbParam(dSTinDangBackDateViewModel);

                _cBaseDataProvider.SetUsername(dSTinDangBackDateViewModel.UserName);
                _cBaseDataProvider.SetRoleCode(dSTinDangBackDateViewModel.RoleCode);

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_LISTNEWS_BACKDATE_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_LISTNEWS_BACKDATE_SEARCH, dbParam, dSTinDangBackDateViewModel.UserName, dSTinDangBackDateViewModel.RoleCode));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<DSTinDangBackDate>();

                    var listnew = mapper.Map(dataSet.Tables[0]);

                    return Task.FromResult(listnew);
                }

                return Task.FromResult(Enumerable.Empty<DSTinDangBackDate>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<DSTinDangBackDate>());
            }

            throw new NotImplementedException();
        }

        public Task<DataTable> ListNewsBackDateExcel(DSTinDangBackDateViewModel dSTinDangBackDateViewModel)
        {
            var dbParam = this.InitDbParam(dSTinDangBackDateViewModel);

            _cBaseDataProvider.SetUsername(dSTinDangBackDateViewModel.UserName);
            _cBaseDataProvider.SetRoleCode(dSTinDangBackDateViewModel.RoleCode);

            var datatblistnews = _cBaseDataProvider.GetDataTableFromSP(ConstantsSP.SPEZIR_LISTNEWS_BACKDATE_SEARCH, dbParam);

            return Task<DataTable>.FromResult(datatblistnews);
        }
    }
}
