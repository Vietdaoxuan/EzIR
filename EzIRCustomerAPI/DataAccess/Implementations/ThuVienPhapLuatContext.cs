using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using CoreLib.Entities.ModelViews;
using CoreLib.Entities.ViewModels;
using DataServiceLib.Interfaces;
using EzIRCustomerAPI.Commons;
using EzIRCustomerAPI.DataAccess.Interfaces;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.DataAccess.Implementations
{
    public class ThuVienPhapLuatContext: IThuVienPhapLuatContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public ThuVienPhapLuatContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            _config = config;
            _cBaseDataProvider = cBaseDataProvider;
            _appLogger = appLogger;
            _common = common;
        }


        public Task<IEnumerable<ThuVienPhapLuatModelView>> Select(ThuVienPhapLuatViewModel thuvienphapluatviewmodel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_TYPEDOC",
                        OracleDbType = OracleDbType.Int32,
                        Value = thuvienphapluatviewmodel.TypeDoc,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_COMPANY",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = thuvienphapluatviewmodel.Company,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_NO",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = thuvienphapluatviewmodel.No,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_YEAREFF",
                        OracleDbType = OracleDbType.Int32,
                        Value = thuvienphapluatviewmodel.YearDateEff,
                    },new OracleParameter()
                    {
                        ParameterName = "P_YEARPUB",
                        OracleDbType = OracleDbType.Int32,
                        Value = thuvienphapluatviewmodel.YearDatePub,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_TEXTNOTE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = thuvienphapluatviewmodel.TextNote,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_LIBOFLAW_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_LIBOFLAW_SEARCH, dbParam, "", ""));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<ThuVienPhapLuatModelView>();

                    var rule = mapper.Map(dataSet.Tables[0]);

                    return Task.FromResult(rule);
                }

                return Task.FromResult(Enumerable.Empty<ThuVienPhapLuatModelView>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<ThuVienPhapLuatModelView>());
            }
        }
    }
}
