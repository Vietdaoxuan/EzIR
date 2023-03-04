using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models;
using EzIRSpecialist.Models.QuanLyTaiKhoan;
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
    public class CompanyContext : ICompanyContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;
        private readonly string _connectionStringDBSearch = string.Empty;
        private readonly string _connectionStringDBSearchTemp = string.Empty;

        public CompanyContext(ICommon common, IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;
            this._connectionStringDBSearch = _config["ConnectionStringEzSearch"];
        }

        public Task<CResponseMessage> Delete(CompanyViewModel obj)
        {
            throw new NotImplementedException();
        }

        public Task<CResponseMessage> Insert(CompanyViewModel obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CompanyEzSearchTemp> ListCompanyEzSearchTemp(int? cpnyid)
        {
            try
            {

                OracleParameter[] dbParam =
                {
                     new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = cpnyid,
                    },

                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBSearch, ConstantsSP.SPEZS_IR_COMPANY_LIST, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZS_IR_COMPANY_LIST, dbParam, "", ""));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<CompanyEzSearchTemp>();

                    var companys = mapper.Map(dataSet.Tables[0]);

                    return companys;
                }

                return Enumerable.Empty<CompanyEzSearchTemp>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<CompanyEzSearchTemp>();
            }
        }

        public IEnumerable<CompanyEzSearchTemp> ListCompanyEzSearchTest()
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

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBSearch, ConstantsSP.SPEZS_IR_COMPANY_LIST_QLDN, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZS_IR_COMPANY_LIST_QLDN, dbParam, "", ""));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<CompanyEzSearchTemp>();

                    var companys = mapper.Map(dataSet.Tables[0]);

                    return companys;
                }

                return Enumerable.Empty<CompanyEzSearchTemp>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<CompanyEzSearchTemp>();
            }
        }

        public IEnumerable<CompanyEzSearchTemp> ListCompany()
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = null,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBSearch, ConstantsSP.SPEZIR_COMPANY_LIST, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_COMPANY_LIST, dbParam, "", ""));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<CompanyEzSearchTemp>();

                    var companys = mapper.Map(dataSet.Tables[0]);

                    return companys;
                }

                return Enumerable.Empty<CompanyEzSearchTemp>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<CompanyEzSearchTemp>();
            }
        }

        /// <summary>
        /// <name>danh sach công tyt theo db EzIR</name>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CompanyEzSearchTemp> ListCompanyEdit()
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


                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_COMPANY_LIST_EDIT, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_COMPANY_LIST_EDIT, dbParam, "", ""));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<CompanyEzSearchTemp>();

                    var companys = mapper.Map(dataSet.Tables[0]);

                    return companys;
                }

                return Enumerable.Empty<CompanyEzSearchTemp>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<CompanyEzSearchTemp>();
            }
        }

        /// <summary>
        /// <name>danh sach công ty theo db EzSearchTemp</name>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CompanyEzSearchTemp> ListCompanyEditSearch()
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

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBSearch, ConstantsSP.SPEZS_IR_COMPANY_LIST_EDIT, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZS_IR_COMPANY_LIST_EDIT, dbParam, "", ""));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<CompanyEzSearchTemp>();

                    var companys = mapper.Map(dataSet.Tables[0]);

                    return companys;
                }

                return Enumerable.Empty<CompanyEzSearchTemp>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<CompanyEzSearchTemp>();
            }
        }

        /// <summary>
        /// <name>danh sach thong tin trang xem thong tin phe duyet theo db EzIR</name>
        /// </summary>
        /// <returns></returns>
        public List<IEnumerable<CompanyEzSearchTemp>> ListCommontype()
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSORTT",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSORTTCT",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_COMMONTYPE_LIST, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_COMMONTYPE_LIST, dbParam, "", ""));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<CompanyEzSearchTemp>();
                    List<IEnumerable<CompanyEzSearchTemp>> list = new List<IEnumerable<CompanyEzSearchTemp>>();
                    var listTT = mapper.Map(dataSet.Tables[0]);
                    var listTTCT = mapper.Map(dataSet.Tables[1]);
                    list.Add(listTT);
                    list.Add(listTTCT);
                    return list;
                }

                return new List<IEnumerable<CompanyEzSearchTemp>>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return new List<IEnumerable<CompanyEzSearchTemp>>();
            }
        }

        /// <summary>
        /// <name>danh sach thong tin trang xem thong tin phe duyet theo db EzSearch</name>
        /// </summary>
        /// <returns></returns>
        public List<IEnumerable<CompanyEzSearchTemp>> ListCommontypeSearch()
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSORTT",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSORTTCT",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBSearch, ConstantsSP.SPEZS_IR_COMMONTYPE_LIST, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZS_IR_COMMONTYPE_LIST, dbParam, "", ""));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<CompanyEzSearchTemp>();
                    List<IEnumerable<CompanyEzSearchTemp>> list = new List<IEnumerable<CompanyEzSearchTemp>>();
                    var listTT = mapper.Map(dataSet.Tables[0]);
                    var listTTCT = mapper.Map(dataSet.Tables[1]);
                    list.Add(listTT);
                    list.Add(listTTCT);
                    return list;
                }

                return new List<IEnumerable<CompanyEzSearchTemp>>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return new List<IEnumerable<CompanyEzSearchTemp>>();
            }
        }


        public Task<IEnumerable<Company>> Select(CompanyViewModel companyviewmodel)
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


                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_COMPANY_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_COMPANY_SEARCH, dbParam, companyviewmodel.RoleCode, companyviewmodel.UserName));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<Company>();

                    var company = mapper.Map(dataSet.Tables[0]);

                    return Task.FromResult(company);
                }

                return Task.FromResult(Enumerable.Empty<Company>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<Company>());
            }
        }



        public Task<CResponseMessage> Update(CompanyViewModel obj)
        {
            throw new NotImplementedException();
        }
    }
}
