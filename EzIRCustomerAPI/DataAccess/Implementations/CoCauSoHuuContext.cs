using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using EzIRCustomerAPI.Commons;
using EzIRCustomerAPI.DataAccess.Interfaces;
using EzIRCustomerAPI.Models;
using EzIRCustomerAPI.Models.ViewModels;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.DataAccess.Implementations
{
    public class CoCauSoHuuContext : ICoCauSoHuuContext
    {        
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;
        private readonly string connectionStringDBEzSearch = string.Empty;
        public CoCauSoHuuContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;
            this.connectionStringDBEzSearch = _config["ConnectionStringEzSearch"];
        }

        public Task<List<IEnumerable<SubCompany>>> Select(CoCauSoHuuViewModel coCauSoHuuViewModel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                     new OracleParameter()
                     {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int16,
                        Value = coCauSoHuuViewModel.CpnyID,
                     },

                       new OracleParameter()
                     {
                         ParameterName = "REFCURSORSUBSIDIARIES",
                         Direction = ParameterDirection.Output,
                         OracleDbType = OracleDbType.RefCursor,
                     },

                     new OracleParameter()
                     {
                         ParameterName = "REFCURSORPARTNER",
                         Direction = ParameterDirection.Output,
                         OracleDbType = OracleDbType.RefCursor,
                     },

                      new OracleParameter()
                     {
                         ParameterName = "REFCURSORSHAREHOLDER",
                         Direction = ParameterDirection.Output,
                         OracleDbType = OracleDbType.RefCursor,
                     }

                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(connectionStringDBEzSearch, ConstantsSP.SPEZS_IR_SUBCOMPANY_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZS_IR_SUBCOMPANY_SEARCH, dbParam, "", ""));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<SubCompany>();
                    List<IEnumerable<SubCompany>> listCcsh = new List<IEnumerable<SubCompany>>();
                    var listCongtyCon = mapper.Map(dataSet.Tables[0]);
                    var listCongtyLienKet = mapper.Map(dataSet.Tables[1]);
                    var listCoDongLon = mapper.Map(dataSet.Tables[2]);
                    listCcsh.Add(listCongtyCon);
                    listCcsh.Add(listCongtyLienKet);
                    listCcsh.Add(listCoDongLon);
                    return Task.FromResult(listCcsh);
                }

                return Task.FromResult(new List<IEnumerable<SubCompany>>());
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(new List<IEnumerable<SubCompany>>());
            }

        }

        public IEnumerable<Ministry> Select_Misnitry()
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

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(connectionStringDBEzSearch, ConstantsSP.SPEZS_MINISTRYLIST, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZS_MINISTRYLIST, dbParam, "", ""));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<Ministry>();

                    var ministries = mapper.Map(dataSet.Tables[0]);

                    return ministries;
                }
                return Enumerable.Empty<Ministry>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<Ministry>();
            }

        }

        private OracleParameter[] InitDbParam_SubCompany(SubCompany subCompany)
        {
            return new[]
            {
                    new OracleParameter()
                    {
                        ParameterName = "P_ASUBCOMPANYNAME",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = subCompany.asubcompanyname,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_ADDRESS",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = subCompany.aaddress,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ASUBCOMPANYNAMEEN",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = subCompany.asubcompanyen,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_ADDRESSEN",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = subCompany.aaddressen,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_MINISTRY",
                        OracleDbType = OracleDbType.Int64,
                        Value = subCompany.aministryid,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_ASHARERATE",
                        OracleDbType = OracleDbType.Decimal,
                        Value = subCompany.asharerate,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int64,
                        Value = subCompany.acompanyid,
                    },
                      new OracleParameter()
                    {
                        ParameterName = "P_ASUBCOMPANYID",
                        OracleDbType = OracleDbType.Int64,
                        Value = subCompany.asubcompanyid,
                    },
                       new OracleParameter()
                    {
                        ParameterName = "P_ASUBCOMPANYTYPEID",
                        OracleDbType = OracleDbType.Int64,
                        Value = subCompany.asubcompanytypeid,
                    },

                          new OracleParameter()
                    {
                        ParameterName = "P_AAPPROVE",
                        OracleDbType = OracleDbType.Int64,
                        Value = subCompany.aapprove,
                    },
                            new OracleParameter()
                    {
                        ParameterName = "P_ANOTE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = subCompany.anote,
                    },
                            new OracleParameter()
                    {
                        ParameterName = "P_AORDER",
                        OracleDbType = OracleDbType.Int64,
                        Value = subCompany.aorder,
                    },

            };
        }

        private OracleParameter[] InitDbParam_ShareHolder(ShareHolder shareHolder)
        {
            return new[]
            {
                new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int16,
                        Value = shareHolder.acpnyid,
                    },

                    /* new OracleParameter()
                    {
                        ParameterName = "P_ASHERID",
                        OracleDbType = OracleDbType.Int16,
                        Value = shareHolder.asherid,
                    },*/
                     new OracleParameter()
                    {
                        ParameterName = "P_ASHNAME",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = shareHolder.ashname,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_ACURSHARENO",
                        OracleDbType = OracleDbType.Int16,
                        Value = shareHolder.acurshareno,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_ACURSHARERATE",
                        OracleDbType = OracleDbType.Decimal,
                        Value = shareHolder.acursharerate,
                    },

                      new OracleParameter()
                    {
                        ParameterName = "P_AAPPROVE",
                        OracleDbType = OracleDbType.Int64,
                        Value = shareHolder.aapprove,
                    },

                      new OracleParameter()
                    {
                        ParameterName = "P_ANOTE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = shareHolder.anote,
                    },
                        new OracleParameter()
                    {
                        ParameterName = "P_ASHERID",
                        OracleDbType = OracleDbType.Int64,
                        Value = shareHolder.asherid,
                    },
                        new OracleParameter()
                    {
                        ParameterName = "P_AORDER",
                        OracleDbType = OracleDbType.Int64,
                        Value = shareHolder.aorder,
                    },

            };
        }

        public CResponseMessage InsertSubcompany(SubCompany subCompany)
        {
            try
            {
                var dbParam = this.InitDbParam_SubCompany(subCompany);

                _cBaseDataProvider.SetRoleCode(subCompany.RoleCode);
                _cBaseDataProvider.SetUsername(subCompany.UserName);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_SUBCOMPANY_INSERT, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_SUBCOMPANY_INSERT, dbParam, subCompany.RoleCode, subCompany.UserName));

                return cResponseMessage;
            }
            catch (Exception ex)
            {

                _appLogger.ErrorLogger.LogError(ex);

                return new CResponseMessage { Code = -3, Message = "MSG_TEZIR_AN_ERROR_OCCURRED" };
            }

        }

        public CResponseMessage UpdateSubcompany(SubCompany subCompany)
        {
            try
            {
                var dbParam = this.InitDbParam_SubCompany(subCompany);

                _cBaseDataProvider.SetRoleCode(subCompany.RoleCode);
                _cBaseDataProvider.SetUsername(subCompany.UserName);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_SUBCOMPANY_UPDATE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_SUBCOMPANY_UPDATE, dbParam, subCompany.RoleCode, subCompany.UserName));

                return cResponseMessage;
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return new CResponseMessage { Code = -3, Message = "MSG_TEZIR_AN_ERROR_OCCURRED" };
            }

        }

        public CResponseMessage InsertShareHolder(ShareHolder shareHolder)
        {
            try
            {
                var dbParam = this.InitDbParam_ShareHolder(shareHolder);

                _cBaseDataProvider.SetRoleCode(shareHolder.RoleCode);
                _cBaseDataProvider.SetUsername(shareHolder.UserName);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_SH_HOLDER_INSERT, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_SH_HOLDER_INSERT, dbParam, shareHolder.RoleCode, shareHolder.UserName));

                return cResponseMessage;
            }   
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return new CResponseMessage { Code = -3, Message = "MSG_TEZIR_AN_ERROR_OCCURRED" };
            }


        }

        public CResponseMessage UpdateShareHolder(ShareHolder shareHolder)
        {
            try
            {
               
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int16,
                        Value = shareHolder.acpnyid,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_ASHERID",
                        OracleDbType = OracleDbType.Int16,
                        Value = shareHolder.asherid,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_ASHNAME",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = shareHolder.ashname,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_ACURSHARENO",
                        OracleDbType = OracleDbType.Int16,
                        Value = shareHolder.acurshareno,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_ACURSHARERATE",
                        OracleDbType = OracleDbType.Int16,
                        Value = shareHolder.acursharerate,
                    },

                      new OracleParameter()
                    {
                        ParameterName = "P_AAPPROVE",
                        OracleDbType = OracleDbType.Int64,
                        Value = shareHolder.aapprove,
                    },

                      new OracleParameter()
                    {
                        ParameterName = "P_ANOTE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = shareHolder.anote,
                    },
                        new OracleParameter()
                    {
                        ParameterName = "P_AORDER",
                        OracleDbType = OracleDbType.Int64,
                        Value = shareHolder.aorder,
                    },

                };

                _cBaseDataProvider.SetRoleCode(shareHolder.RoleCode);
                _cBaseDataProvider.SetUsername(shareHolder.UserName);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_SH_HOLDER_UPDATE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_SH_HOLDER_UPDATE, dbParam, shareHolder.RoleCode, shareHolder.UserName));

                return cResponseMessage;
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return new CResponseMessage { Code = -3, Message = "MSG_TEZIR_AN_ERROR_OCCURRED" };
            }
        }

        public IEnumerable<SubCompany> Select_SubCompanyType(CoCauSoHuuViewModel coCauSoHuuViewModel)
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

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(connectionStringDBEzSearch, ConstantsSP.SPEZS_IR_PAR_SUBCOMPANYTYPE_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZS_IR_PAR_SUBCOMPANYTYPE_SEARCH, dbParam, "", ""));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<SubCompany>();

                    var subcompanytype = mapper.Map(dataSet.Tables[0]);

                    return subcompanytype;
                }
                return Enumerable.Empty<SubCompany>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<SubCompany>();
            }
        }
    }
}
