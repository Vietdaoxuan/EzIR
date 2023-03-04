using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using CoreLib.Entities.ModelViews;
using CoreLib.Entities.ViewModels;
using DataServiceLib.Interfaces;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
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
    public class ThongTinPheDuyetContext : IThongTinPheDuyetContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;
        private readonly string _connectionStringDBSearch = string.Empty;

        public ThongTinPheDuyetContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;
            this._connectionStringDBSearch = _config["ConnectionStringEzSearch"];
        }

        public IEnumerable<InfoSheet> SelectEzIR(ThongTinPheDuyetViewModel thongTin)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ID",
                        OracleDbType = OracleDbType.Int64,
                        Value = thongTin.ID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int64,
                        Value = thongTin.CpnyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_HASCONTENT",
                        OracleDbType = OracleDbType.Int64,
                        Value = thongTin.HasContent,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_MENUID",
                        OracleDbType = OracleDbType.Int64,
                        Value = thongTin.MenuID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_POSTDATE",
                        OracleDbType = OracleDbType.Date,
                        Value = thongTin.PostDate,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_LANGUAGE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = thongTin.Language,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                };

                var dataSetEzIR = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_INFOSHEET_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_INFOSHEET_SEARCH, dbParam, thongTin.RoleCode, thongTin.Username));

                if (dataSetEzIR.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSetEzIR));

                    var mapper = new DataNamesMapper<InfoSheet>();

                    var infoSheets = mapper.Map(dataSetEzIR.Tables[0]);

                    return infoSheets;
                }

                return Enumerable.Empty<InfoSheet>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<InfoSheet>();
            }
        }

        public IEnumerable<InfoSheet> SelectEzSearch(ThongTinPheDuyetViewModel thongTin)
        {
            try
            {
                OracleParameter[] dbParam =
                {

                   new OracleParameter()
                    {
                        ParameterName = "P_ID",
                        OracleDbType = OracleDbType.Int64,
                        Value = thongTin.ID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int64,
                        Value = thongTin.CpnyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_HASCONTENT",
                        OracleDbType = OracleDbType.Int64,
                        Value = thongTin.HasContent,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_MENUID",
                        OracleDbType = OracleDbType.Int64,
                        Value = thongTin.MenuID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_POSTDATE",
                        OracleDbType = OracleDbType.Date,
                        Value = thongTin.PostDate,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_LANGUAGE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = thongTin.Language,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",//
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                };

                var dataSetEzSearch = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBSearch, ConstantsSP.SPEZS_IR_INFOSHEET_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_INFOSHEET_SEARCH, dbParam, thongTin.RoleCode, thongTin.Username));


                if (dataSetEzSearch.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSetEzSearch));

                    var mapper = new DataNamesMapper<InfoSheet>();

                    var infoSheets = mapper.Map(dataSetEzSearch.Tables[0]);

                    return infoSheets;
                }
                return Enumerable.Empty<InfoSheet>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<InfoSheet>();
            }
        }

        public IEnumerable<ToChucBoMayQuanLyModelView> SelectBMQLEzIR(ThongTinPheDuyetViewModel toChucBoMayQuanLyViewModel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ORGMODELID",
                        OracleDbType = OracleDbType.Int32,
                        Value = toChucBoMayQuanLyViewModel.OrgModelID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = toChucBoMayQuanLyViewModel.CpnyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APPROVE",
                        OracleDbType = OracleDbType.Int32,
                        Value = toChucBoMayQuanLyViewModel.APPROVE,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_AORGTYPE",
                        OracleDbType = OracleDbType.Int32,
                        Value = toChucBoMayQuanLyViewModel.OrgType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "Refcursor",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_ORGMODEL_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_ORGMODEL_SEARCH, dbParam, toChucBoMayQuanLyViewModel.RoleCode, toChucBoMayQuanLyViewModel.UserLogin));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<ToChucBoMayQuanLyModelView>();

                    var rule = mapper.Map(dataSet.Tables[0]);

                    return rule;
                }

                return Enumerable.Empty<ToChucBoMayQuanLyModelView>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<ToChucBoMayQuanLyModelView>();
            }
        }
        public IEnumerable<ToChucBoMayQuanLyModelView> SelectBMQLEzSearch(ThongTinPheDuyetViewModel toChucBoMayQuanLyViewModel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "Porgmodelid",
                        OracleDbType = OracleDbType.Int32,
                        Value = toChucBoMayQuanLyViewModel.OrgModelID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "Pcpnyid",
                        OracleDbType = OracleDbType.Int32,
                        Value = toChucBoMayQuanLyViewModel.CpnyID,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "Porgtype",
                        OracleDbType = OracleDbType.Int32,
                        Value = toChucBoMayQuanLyViewModel.OrgType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "RefCursor",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBSearch, ConstantsSP.SPEZS_IR_ORGMODEL_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZS_IR_ORGMODEL_SEARCH, dbParam, toChucBoMayQuanLyViewModel.RoleCode, toChucBoMayQuanLyViewModel.UserLogin));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<ToChucBoMayQuanLyModelView>();

                    var rule = mapper.Map(dataSet.Tables[0]);

                    return rule;
                }

                return Enumerable.Empty<ToChucBoMayQuanLyModelView>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<ToChucBoMayQuanLyModelView>();
            }
        }

        public IEnumerable<DevelopProgress> ListDevelopProgressEzIR(ThongTinPheDuyetViewModel thongTin)
        {
            try
            {

                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int64,
                        Value = thongTin.CpnyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_STATUS",
                        OracleDbType = OracleDbType.Int16,
                        Value = thongTin.Approve,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_LANGUAGE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = thongTin.Language,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_DEVELOPPROGRESS_LIST, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_DEVELOPPROGRESS_LIST, dbParam, "", ""));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<DevelopProgress>();

                    var developProgresses = mapper.Map(dataSet.Tables[0]);

                    return developProgresses;
                }

                return Enumerable.Empty<DevelopProgress>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<DevelopProgress>();
            }
        }

        public IEnumerable<DevelopProgress> ListDevelopProgressEzSearch(ThongTinPheDuyetViewModel thongTin)
        {
            try
            {

                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int64,
                        Value = thongTin.CpnyID,
                    },
                   
                    new OracleParameter()
                    {
                        ParameterName = "P_LANGUAGE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = thongTin.Language,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBSearch, ConstantsSP.SPEZS_IR_DEVELOPPROGRESS_LIST, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZS_IR_DEVELOPPROGRESS_LIST, dbParam, "", ""));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<DevelopProgress>();

                    var developProgresses = mapper.Map(dataSet.Tables[0]);

                    return developProgresses;
                }

                return Enumerable.Empty<DevelopProgress>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<DevelopProgress>();
            }
        }

        public IEnumerable<CompanyEzSearchTemp> ListCompanyEzIR(ThongTinPheDuyetViewModel thongTin)
        {
            try
            {

                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int64,
                        Value = thongTin.CpnyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APPROVE",
                        OracleDbType = OracleDbType.Int64,
                        Value = thongTin.Status,
                    },

                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_COMPANY_LIST, dbParam);

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

        public IEnumerable<CompanyEzSearchTemp> ListCompanyEzSearch(ThongTinPheDuyetViewModel thongTin)
        {
            try
            {

                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int64,
                        Value = thongTin.CpnyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBSearch, ConstantsSP.SPEZS_IR_COMPANY_LIST, dbParam);

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

        public IEnumerable<Manager> SelectTPLDEzIR(ThongTinPheDuyetViewModel thongTin)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_AMNGID",
                        OracleDbType = OracleDbType.Int32,
                        Value = thongTin.MNGID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AMNGERID",
                        OracleDbType = OracleDbType.Int32,
                        Value = thongTin.MNGERID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = thongTin.CPNYID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APPROVE",
                        OracleDbType = OracleDbType.Int32,
                        Value = thongTin.APPROVE,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "Refcursor",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBSearch, ConstantsSP.SPEZS_IR_MANAGER_SEARCH, dbParam); //Lấy dữ liệu từ EzIR và EzSearch 

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_MANAGER_SEARCH, dbParam, thongTin.RoleCode, thongTin.UserLogin));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<Manager>();

                    var managers = mapper.Map(dataSet.Tables[0]).ToList();

                    return managers;
                }

                return Enumerable.Empty<Manager>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<Manager>();
            }
        }

        public IEnumerable<Manager> SelectTPLDEzSearch(ThongTinPheDuyetViewModel thongTin)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int32,
                        Value = thongTin.CPNYID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AMNGERID",
                        OracleDbType = OracleDbType.Int32,
                        Value = thongTin.MNGERID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "Refcursor",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBSearch, ConstantsSP.SPEZS_IR_MANAGER_CP_SEARCH, dbParam);// lấy dữ liệu từ ezsearch

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_MANAGER_SEARCH, dbParam, thongTin.RoleCode, thongTin.UserLogin));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<Manager>();

                    var managers = mapper.Map(dataSet.Tables[0]).ToList();

                    return managers;
                }

                return Enumerable.Empty<Manager>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<Manager>();
            }
        }

        public List<IEnumerable<SubCompany>> SelectCCSHEzSearch(ThongTinPheDuyetViewModel thongTin)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                     new OracleParameter()
                     {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int16,
                        Value = thongTin.CpnyID,
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

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBSearch, ConstantsSP.SPEZS_IR_SUBCOMPANY_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZS_IR_SUBCOMPANY_SEARCH, dbParam, "", ""));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<SubCompany>();
                    List<IEnumerable<SubCompany>> listCcsh = new List<IEnumerable<SubCompany>>();
                    var listCongtyCon = mapper.Map(dataSet.Tables[0]);
                    var listCongtyLienKet = mapper.Map(dataSet.Tables[1]);
                    var listCoDongLon = mapper.Map(dataSet.Tables[2]);
                    listCcsh.Add(listCongtyCon.Where(x=>x.aapprove==1||x.aapprove==2));
                    listCcsh.Add(listCongtyLienKet.Where(x => x.aapprove == 1 || x.aapprove == 2));
                    listCcsh.Add(listCoDongLon.Where(x => x.aapprove == 1 || x.aapprove == 2));
                    return listCcsh;
                }

                return new List<IEnumerable<SubCompany>>();
            }

            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return new List<IEnumerable<SubCompany>>();
            }

        }
    }
}
