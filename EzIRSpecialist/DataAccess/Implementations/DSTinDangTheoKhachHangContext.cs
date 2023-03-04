using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using DataServiceLib.Interfaces;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models;
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
    public class DSTinDangTheoKhachHangContext : IDSTinDangTheoKhachHangContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public DSTinDangTheoKhachHangContext(ICommon common, IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;

        }

        private OracleParameter[] InitDbParam(DSTinDangTheoKhachHangViewModel dSTinDangTheoKhachHangViewModel)
        {
            return new[]
            {
                new OracleParameter()
                    {
                        ParameterName = "P_STOCKCODE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = dSTinDangTheoKhachHangViewModel.AStockCode,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_COMPANYTYPE",
                        OracleDbType = OracleDbType.Int16,
                        Value = dSTinDangTheoKhachHangViewModel.ACompanyType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_EXPERT",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = dSTinDangTheoKhachHangViewModel.AExpert,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_REGION",
                        OracleDbType = OracleDbType.Int16,
                        Value = dSTinDangTheoKhachHangViewModel.ARegion,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_FROMDATE",
                        OracleDbType = OracleDbType.Date,
                        Value = dSTinDangTheoKhachHangViewModel.AFromDate,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_TODATE",
                        OracleDbType = OracleDbType.Date,
                        Value = dSTinDangTheoKhachHangViewModel.AToDate,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_DOCTYPE",
                        OracleDbType = OracleDbType.Int16,
                        Value = dSTinDangTheoKhachHangViewModel.ADocType,
                    },
                      new OracleParameter()
                    {
                        ParameterName = "P_STATUS",
                        OracleDbType = OracleDbType.Int16,
                        Value = dSTinDangTheoKhachHangViewModel.AStatus,
                    },

                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
            };
        }

        public Task<IEnumerable<DSTinDangTheoKhachHang>> ListTinDang(DSTinDangTheoKhachHangViewModel dSTinDangTheoKhachHangViewModel)
        {
            try
            {
                var dbParam = this.InitDbParam(dSTinDangTheoKhachHangViewModel);

                _cBaseDataProvider.SetUsername(dSTinDangTheoKhachHangViewModel.UserName);
                _cBaseDataProvider.SetRoleCode(dSTinDangTheoKhachHangViewModel.RoleCode);

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_LISTNEWS_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_LISTNEWS_SEARCH, dbParam, dSTinDangTheoKhachHangViewModel.UserName, dSTinDangTheoKhachHangViewModel.RoleCode));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<DSTinDangTheoKhachHang>();

                    var listnew = mapper.Map(dataSet.Tables[0]);

                    return Task.FromResult(listnew);
                }

                return Task.FromResult(Enumerable.Empty<DSTinDangTheoKhachHang>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<DSTinDangTheoKhachHang>());
            }
        }

        public Task<DataTable> ListTinDangExcel(DSTinDangTheoKhachHangViewModel dSTinDangTheoKhachHangViewModel)
        {
            var dbParam = this.InitDbParam(dSTinDangTheoKhachHangViewModel);

            _cBaseDataProvider.SetUsername(dSTinDangTheoKhachHangViewModel.UserName);
            _cBaseDataProvider.SetRoleCode(dSTinDangTheoKhachHangViewModel.RoleCode);

            var datatblistnews = _cBaseDataProvider.GetDataTableFromSP(ConstantsSP.SPEZIR_LISTNEWS_SEARCH, dbParam);

            return Task<DataTable>.FromResult(datatblistnews);
        }
    }
}
