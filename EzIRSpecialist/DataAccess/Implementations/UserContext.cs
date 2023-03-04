using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
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
    public class UserContext : IUserContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public UserContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;
        }

        public Task<IEnumerable<User>> Select(UserViewModel user)
        {
            try
            {                
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_USERNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = user.Username,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_FULLNAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = user.FullName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_EMAIL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = user.Email,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACTIVE",
                        OracleDbType = OracleDbType.Int16,
                        Value = user.Active,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ROLECODE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = user.RoleCode,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_USER_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_USER_SEARCH, dbParam, "", ""));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<User>();

                    var users = mapper.Map(dataSet.Tables[0]);

                    return Task.FromResult(users);
                }

                return Task.FromResult(Enumerable.Empty<User>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<User>());
            }

        }

        public Task<CResponseMessage> Insert(UserViewModel user)
        {
            throw new NotImplementedException();
        }

        public Task<CResponseMessage> Update(UserViewModel user)
        {
            throw new NotImplementedException();
        }

        public Task<CResponseMessage> Delete(UserViewModel user)
        {
            throw new NotImplementedException();
        }
    }
}
