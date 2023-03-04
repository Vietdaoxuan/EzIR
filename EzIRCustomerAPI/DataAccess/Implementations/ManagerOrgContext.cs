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
    public class ManagerOrgContext : IManagerOrgContext 
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;
        private readonly string _connectionStringDBTemp = string.Empty;
        public ManagerOrgContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            _config = config;
            _cBaseDataProvider = cBaseDataProvider;
            _appLogger = appLogger;
            _common = common;
            this._connectionStringDBTemp = _config["ConnectionStringEzSearch"];
        }

        public Task<CResponseMessage> Delete(ManagerOrgViewModel obj)
        {
            throw new System.NotImplementedException();
        }

        public Task<CResponseMessage> Insert(ManagerOrgViewModel obj)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<ManagerOrg>> Select(ManagerOrgViewModel managerOrg)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_AMORGID",
                        OracleDbType = OracleDbType.Int32,
                        Value = managerOrg.AMOrgID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AMORGGROUPID",
                        OracleDbType = OracleDbType.Int32,
                        Value = managerOrg.AMOrgGroupID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "Refcursor",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBTemp, ConstantsSP.SPEZS_IR_MANAGERORG_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZS_IR_MANAGERORG_SEARCH, dbParam, managerOrg.RoleCode, managerOrg.UserLogin));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<ManagerOrg>();

                    var rule = mapper.Map(dataSet.Tables[0]);

                    return Task.FromResult(rule);
                }

                return Task.FromResult(Enumerable.Empty<ManagerOrg>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<ManagerOrg>());
            }
        }

        public Task<CResponseMessage> Update(ManagerOrgViewModel obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
