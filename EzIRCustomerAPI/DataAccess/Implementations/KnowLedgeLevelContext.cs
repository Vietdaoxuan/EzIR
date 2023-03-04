using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
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
    public class KnowLedgeLevelContext : IKnowLedgeLevelContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;
        private readonly string _connectionStringDBTemp = string.Empty;
        public KnowLedgeLevelContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            _config = config;
            _cBaseDataProvider = cBaseDataProvider;
            _appLogger = appLogger;
            _common = common;
            this._connectionStringDBTemp = _config["ConnectionStringEzSearch"];
        }

        public Task<CResponseMessage> Delete(KnowLedgeLevelViewModel obj)
        {
            throw new NotImplementedException();
        }

        public Task<CResponseMessage> Insert(KnowLedgeLevelViewModel obj)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<KnowLedgeLevel>> Select(KnowLedgeLevelViewModel knowLedgeLevelViewModel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    
                    new OracleParameter()
                    {
                        ParameterName = "Refcursor",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBTemp, ConstantsSP.SPEZS_IR_KNOWLEDGELEVEL_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZS_IR_KNOWLEDGELEVEL_SEARCH, dbParam, knowLedgeLevelViewModel.RoleCode, knowLedgeLevelViewModel.UserLogin));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<KnowLedgeLevel>();

                    var rule = mapper.Map(dataSet.Tables[0]);

                    return Task.FromResult(rule);
                }

                return Task.FromResult(Enumerable.Empty<KnowLedgeLevel>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<KnowLedgeLevel>());
            }
        }

        public Task<CResponseMessage> Update(KnowLedgeLevelViewModel obj)
        {
            throw new NotImplementedException();
        }
    }
}
