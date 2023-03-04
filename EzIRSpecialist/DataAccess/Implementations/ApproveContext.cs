using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using DataServiceLib.Implementations;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Implementations
{
    public class ApproveContext : IApproveContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;
        private readonly string _connectionStringEzSearch;
        private readonly string _connectionStringSQLEntDataInfo2;



        public ApproveContext(ICommon common, IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;
            this._connectionStringEzSearch = config["ConnectionStringEzSearch"].ToString();
            this._connectionStringSQLEntDataInfo2 = config["SQLConnectionStringEntDataInfo2"].ToString();
        }

        public CResponseMessage ApproveInfoCompany(int cpnyID, int menuID, int approve)
        {
            var listParam = new[]
            {
                new SqlParameter()
                {
                    ParameterName = "CpnyID",
                    Direction = ParameterDirection.Input,
                    Value = cpnyID,
                },
                new SqlParameter()
                {
                    ParameterName = "MenuID",
                    Direction = ParameterDirection.Input,
                    Value = menuID,
                },
                new SqlParameter()
                {
                    ParameterName = "Approve",
                    Direction = ParameterDirection.Input,
                    Value = approve,
                }
            };

            CDataProvider dataProvider = new CDataProvider();
            var dataSet = dataProvider.GetResponseFromExecutedSp(ConstantsSP.EZIR_SP_APPROVE_COMPANY, listParam, this._connectionStringSQLEntDataInfo2);

            _appLogger.SqlLogger.LogSql(dataProvider.SPParametersToString(ConstantsSP.EZIR_SP_APPROVE_COMPANY, listParam, "", ""));

            return dataSet;
        }

        public CResponseMessage ApproveDevelopprogess(int cpnyID, int menuID, int approve)
            {

            var listParam = new[]
                {
                new SqlParameter()
                {
                    ParameterName = "CpnyID",
                    Direction = ParameterDirection.Input,
                    Value = cpnyID,
                },
                new SqlParameter()
                {
                    ParameterName = "MenuID",
                    Direction = ParameterDirection.Input,
                    Value = menuID,
                },
                new SqlParameter()
                {
                    ParameterName = "Approve",
                    Direction = ParameterDirection.Input,
                    Value = approve,
                }
            };

            CDataProvider dataProvider = new CDataProvider();
            var dataSet = dataProvider.GetResponseFromExecutedSp(ConstantsSP.EZIR_SP_APPROVE_DEVELOPPROGRESS, listParam, this._connectionStringSQLEntDataInfo2);

            _appLogger.SqlLogger.LogSql(dataProvider.SPParametersToString(ConstantsSP.EZIR_SP_APPROVE_DEVELOPPROGRESS, listParam, "", ""));

            return dataSet;

        }

        public CResponseMessage ApproveInfoSheet(int cpnyID, int menuID, int approve, int authorID, int modifierID)
        {
            var listParam = new[]
                {
                new SqlParameter()
                {
                    ParameterName = "CpnyID",
                    Direction = ParameterDirection.Input,
                    Value = cpnyID,
                },
                new SqlParameter()
                {
                    ParameterName = "MenuID",
                    Direction = ParameterDirection.Input,
                    Value = menuID,
                },
                new SqlParameter()
                {
                    ParameterName = "Approve",
                    Direction = ParameterDirection.Input,
                    Value = approve,
                },
                new SqlParameter()
                {
                    ParameterName = "AuthorID",
                    Direction = ParameterDirection.Input,
                    Value = authorID,
                },
                new SqlParameter()
                {
                    ParameterName = "ModifierID",
                    Direction = ParameterDirection.Input,
                    Value = modifierID,
                }
            };

            CDataProvider dataProvider = new CDataProvider();
            _appLogger.SqlLogger.LogSql(dataProvider.SPParametersToString(ConstantsSP.EZIR_SP_APPROVE_INFOSHEET, listParam, "", ""));
            var dataSet = dataProvider.GetResponseFromExecutedSp(ConstantsSP.EZIR_SP_APPROVE_INFOSHEET, listParam, this._connectionStringSQLEntDataInfo2);

            _appLogger.SqlLogger.LogSql("EZIR_SP_APPROVE_INFOSHEET - DONE");

            return dataSet;
        }

        public CResponseMessage ApproveManager(int cpnyID, int menuID, int approve)
        {
            var listParam = new[]
                {
                new SqlParameter()
                {
                    ParameterName = "CpnyID",
                    Direction = ParameterDirection.Input,
                    Value = cpnyID,
                },
                new SqlParameter()
                {
                    ParameterName = "MenuID",
                    Direction = ParameterDirection.Input,
                    Value = menuID,
                },
                new SqlParameter()
                {
                    ParameterName = "Approve",
                    Direction = ParameterDirection.Input,
                    Value = approve,
                }
            };

            CDataProvider dataProvider = new CDataProvider();
            var dataSet = dataProvider.GetResponseFromExecutedSp(ConstantsSP.EZIR_SP_APPROVE_MANAGER, listParam, this._connectionStringSQLEntDataInfo2);

            _appLogger.SqlLogger.LogSql(dataProvider.SPParametersToString(ConstantsSP.EZIR_SP_APPROVE_MANAGER, listParam, "", ""));

            return dataSet;
        }

        public CResponseMessage ApproveOrgModel(int cpnyID, int menuID, int approve)
        {
            var listParam = new[]
                {
                new SqlParameter()
                {
                    ParameterName = "CpnyID",
                    Direction = ParameterDirection.Input,
                    Value = cpnyID,
                },
                new SqlParameter()
                {
                    ParameterName = "MenuID",
                    Direction = ParameterDirection.Input,
                    Value = menuID,
                },
                new SqlParameter()
                {
                    ParameterName = "Approve",
                    Direction = ParameterDirection.Input,
                    Value = approve,
                }
            };

            CDataProvider dataProvider = new CDataProvider();
            _appLogger.SqlLogger.LogSql(dataProvider.SPParametersToString(ConstantsSP.EZIR_SP_APPROVE_ORGMODEL, listParam, "", ""));

            var dataSet = dataProvider.GetResponseFromExecutedSp(ConstantsSP.EZIR_SP_APPROVE_ORGMODEL, listParam, this._connectionStringSQLEntDataInfo2);

            _appLogger.SqlLogger.LogSql("ZIR_SP_APPROVE_ORGMODEL DONE");

            return dataSet;
        }

        public CResponseMessage ApproveSubCompany(int cpnyID, int menuID, int approve)
        {
            var listParam = new[]
                {
                new SqlParameter()
                {
                    ParameterName = "CpnyID",
                    Direction = ParameterDirection.Input,
                    Value = cpnyID,
                },
                new SqlParameter()
                {
                    ParameterName = "MenuID",
                    Direction = ParameterDirection.Input,
                    Value = menuID,
                },
                new SqlParameter()
                {
                    ParameterName = "Approve",
                    Direction = ParameterDirection.Input,
                    Value = approve,
                }
            };

            CDataProvider dataProvider = new CDataProvider();
            var dataSet = dataProvider.GetResponseFromExecutedSp(ConstantsSP.EZIR_SP_APPROVE_SUBCOMPANY, listParam, this._connectionStringSQLEntDataInfo2);

            _appLogger.SqlLogger.LogSql(dataProvider.SPParametersToString(ConstantsSP.EZIR_SP_APPROVE_SUBCOMPANY, listParam, "", ""));

            return dataSet;
        }

        public CResponseMessage ApproveCompanyHolder(int cpnyID, int menuID, int approve)
        {
            var listParam = new[]
                {
                new SqlParameter()
                {
                    ParameterName = "CpnyID",
                    Direction = ParameterDirection.Input,
                    Value = cpnyID,
                },
                new SqlParameter()
                {
                    ParameterName = "MenuID",
                    Direction = ParameterDirection.Input,
                    Value = menuID,
                },
                new SqlParameter()
                {
                    ParameterName = "Approve",
                    Direction = ParameterDirection.Input,
                    Value = approve,
                }
            };

            CDataProvider dataProvider = new CDataProvider();
            var dataSet = dataProvider.GetResponseFromExecutedSp(ConstantsSP.EZIR_SP_APPROVE_SH_CPNYHOLDER, listParam, this._connectionStringSQLEntDataInfo2);

            _appLogger.SqlLogger.LogSql(dataProvider.SPParametersToString(ConstantsSP.EZIR_SP_APPROVE_SH_CPNYHOLDER, listParam, "", ""));

            return dataSet;
        }

        public IEnumerable<ChangeInfo> GetListChangeInfo(int cpnyID)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "pcpnyid",
                        OracleDbType = OracleDbType.Int64,
                        Value = cpnyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZS_CHANGEINFO_SEARCH, dbParam, "", ""));

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringEzSearch, ConstantsSP.SPEZS_CHANGEINFO_SEARCH, dbParam);

                if (dataSet.Tables.Count > 0)
                {
                    var mapper = new DataNamesMapper<ChangeInfo>();

                    var changeInfos = mapper.Map(dataSet.Tables[0]);

                    return changeInfos;
                }

                return Enumerable.Empty<ChangeInfo>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<ChangeInfo>();
            }
        }

        public IEnumerable<CompanyApprove> GetListCompanyApprove()
        {
            OracleParameter[] dbParam = {
                new OracleParameter()
                {
                    ParameterName = "REFCURSOR",
                    Direction = ParameterDirection.Output,
                    OracleDbType = OracleDbType.RefCursor,
                }
            };

            var comapnyApproves = new List<CompanyApprove>();

            _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZS_APPROVE_COMPANY_SEARCH, dbParam, "", ""));

            var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringEzSearch, ConstantsSP.SPEZS_APPROVE_COMPANY_SEARCH, dbParam);

            try
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var comapnyApprove = new CompanyApprove();

                    comapnyApprove.CpnyID = this._common.GetSafeInt(row["acpnyid"]);
                    comapnyApprove.NameVN = this._common.GetSafeString(row["namevn"]);

                    comapnyApproves.Add(comapnyApprove);
                }
            }
            catch (Exception e)
            {
                _appLogger.ErrorLogger.LogError(e);

                return comapnyApproves;
            }

            return comapnyApproves;
        }

        public IEnumerable<FunctionApprove> GetListFunctionApprove(int cpnyID)
        {
            OracleParameter[] dbParam =
            {
                    new OracleParameter()
                    {
                        ParameterName = "PCpnyID",
                        OracleDbType = OracleDbType.Int32,
                        Value = cpnyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
            };

            var functionApproves = new List<FunctionApprove>();

            _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZS_APPROVE_FUNCTION_SEARCH, dbParam, "", ""));

            var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringEzSearch, ConstantsSP.SPEZS_APPROVE_FUNCTION_SEARCH, dbParam);

            try
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var functionApprove = new FunctionApprove();

                    functionApprove.CpnyID = this._common.GetSafeInt(row["acpnyid"]);
                    functionApprove.Function = this._common.GetSafeString(row["afunction"]);
                    functionApprove.LevelID = this._common.GetSafeString(row["alevelid"]);
                    functionApprove.DetailLevelID = this._common.GetSafeString(row["adetaillevelid"]);

                    functionApproves.Add(functionApprove);
                }
            }
            catch (Exception e)
            {
                _appLogger.ErrorLogger.LogError(e);
            }

            return functionApproves;
        }
    }
}
