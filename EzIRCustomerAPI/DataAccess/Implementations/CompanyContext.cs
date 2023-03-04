using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using CoreLib.Entities.ModelViews;
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
    public class CompanyContext : ICompanyContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;
        private readonly string _connectionStringDBEzSearch = string.Empty;

        public CompanyContext(ICommon common, IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;
            this._connectionStringDBEzSearch = _config["ConnectionStringEzSearch"];
        }

        // DB: EzIR
        public IEnumerable<CompanyEzSearchTemp> ListCompanyEzIR(int? cpnyID, int? approve)
        {
            try
            {

                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int64,
                        Value = cpnyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APPROVE",
                        OracleDbType = OracleDbType.Int64,
                        Value = approve,
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

        // DB: EzSearch
        public IEnumerable<CompanyEzSearchTemp> ListCompanyEzSearch(int? cpnyID)
        {
            try
            {

                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
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

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBEzSearch, ConstantsSP.SPEZS_IR_COMPANY_LIST, dbParam);

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

        // lấy bản ghi của bảng tezs_companytype ở db EzSearch 
        public IEnumerable<CompanyType> ListCompanyType()
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

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBEzSearch, ConstantsSP.SPEZS_COMPANYTYPELIST, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZS_COMPANYTYPELIST, dbParam, "", ""));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<CompanyType>();

                    var companyType = mapper.Map(dataSet.Tables[0]);

                    return companyType;
                }

                return Enumerable.Empty<CompanyType>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<CompanyType>();
            }
        }

        // lấy role company thông qua cpnyid
        public IEnumerable<RoleCompanyModelView> GetRoleCompany(int? cpnyid)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_CpnyID",
                        Direction = ParameterDirection.Input,
                        OracleDbType = OracleDbType.Int64,
                        Value = cpnyid,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_ROLE_GET_BY_COMPANY, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_ROLE_GET_BY_COMPANY, dbParam, "", ""));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<RoleCompanyModelView>();

                    var cpnyrole = mapper.Map(dataSet.Tables[0]);

                    return cpnyrole;
                }

                return Enumerable.Empty<RoleCompanyModelView>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<RoleCompanyModelView>();
            }
        }

        // Các mốc lịch sử - DB: EzIR
        public IEnumerable<DevelopProgress> ListDevelopProgressEzIR(int? cpnyID, int? status, string language)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int64,
                        Value = cpnyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_STATUS",
                        OracleDbType = OracleDbType.Int16,
                        Value = status,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_LANGUAGE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = language,
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

        // Các mốc lịch sử - DB: EzSearch
        public IEnumerable<DevelopProgress> ListDevelopProgressEzSearch(int? cpnyID, string language)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int64,
                        Value = cpnyID,
                    },                    
                    new OracleParameter()
                    {
                        ParameterName = "P_LANGUAGE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = language,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(_connectionStringDBEzSearch, ConstantsSP.SPEZS_IR_DEVELOPPROGRESS_LIST, dbParam);

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


        public CResponseMessage UpdateCompanyInfomation(CompanyEzSearchTemp company)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ANAME_VN",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AName_VN,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANAME_EN",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AName_EN,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ANAME_SHORT",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AName_Short,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ASTOCK_CODE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AStockCode,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADETAIL_URL",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.ADetail_URL,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APROVINCEID",
                        OracleDbType = OracleDbType.Int32,
                        Value = company.AProvinceID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AHEADOFFICE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AHeadOffice,
                    },                                       
                    new OracleParameter()
                    {
                        ParameterName = "P_APHONE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.APhone,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AFAX",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AFax,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AEMAIL",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AEmail,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AWEBSITE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AWebsite,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ALOGOPATH",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.ALogoPath,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACAPITAL",
                        OracleDbType = OracleDbType.Int32,
                        Value = company.ACapital,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AREG_CAPITAL",
                        OracleDbType = OracleDbType.Int32,
                        Value = company.AReg_Capital,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AORG_PRICE",
                        OracleDbType = OracleDbType.Int32,
                        Value = company.AOrg_Price,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AUNIT",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AUnit,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APOST_TO",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.APost_To,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APOST_DATE",
                        OracleDbType = OracleDbType.Date,
                        Value = company.APost_Date,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AQTY",
                        OracleDbType = OracleDbType.Int32,
                        Value = company.AQty,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AFIRST_PRICE",
                        OracleDbType = OracleDbType.Int64,
                        Value = company.AFirst_Price,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AREG_F_NO",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AReg_F_No,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AREG_F_DATE",
                        OracleDbType = OracleDbType.Date,
                        Value = company.AReg_F_Date,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AREG_F_POSITION",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AReg_F_Position,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AREG_BIZ_NO",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AReg_Biz_No,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AREG_BIZ_DATE",
                        OracleDbType = OracleDbType.Date,
                        Value = company.AReg_Biz_Date,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AREG_BIZ_POSITION",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AReg_Biz_Position,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ABIOGRAPHY",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.ABiography,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ABIOGRAPHY_EN",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.ABiography_EN,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ASUMMARY",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.ASummary,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ASUMMARY_EN",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.ASummary_EN,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ASERVEBANK",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AServerBank,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ASERVEBANKEN",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AServerBankEN,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ABANKRATE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.ABankRate,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACORGID",
                        OracleDbType = OracleDbType.Int64,
                        Value = company.ACorgID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACISSUEORGID",
                        OracleDbType = OracleDbType.Int64,
                        Value = company.ACissueorgID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AUPD_DATE",
                        OracleDbType = OracleDbType.Date,
                        Value = company.AUpd_Date,
                    },                    
                    new OracleParameter()
                    {
                        ParameterName = "P_ATAXCODE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.ATaxCode,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AMINISTRYDESCT",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AMinistryDesct,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ASTOCK_ID",
                        OracleDbType = OracleDbType.Int64,
                        Value = company.AStock_ID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AISDELETED",
                        OracleDbType = OracleDbType.Int64,
                        Value = company.AIs_Deleted,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AVISIONSTATEMENT",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AVisionStatement,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AUSERID",
                        OracleDbType = OracleDbType.Int64,
                        Value = company.AUserID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACPNYTYPE",
                        OracleDbType = OracleDbType.Int64,
                        Value = company.ACpnyType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AFEEDBACKEMAIL",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AFeedbackEmail,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AHEADOFFICEEN",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AHeadOfficeEN,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AREG_F_POSITIONEN",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AReg_F_PositionEN,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AREG_BIZ_POSITIONEN",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.Areg_Biz_Positionen,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AVISIONSTATEMENTEN",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.AVisionStatementEN,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ALOGOIMAGE",
                        OracleDbType = OracleDbType.Blob,
                        Value = company.ALogoImage,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACPNYID",
                        OracleDbType = OracleDbType.Int64,
                        Value = company.ACpnyID,
                    },                    
                    new OracleParameter()
                    {
                        ParameterName = "P_ANOTE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = company.ANote,
                    },                    
                    
                };

                _cBaseDataProvider.SetRoleCode(company.RoleCode);
                _cBaseDataProvider.SetUsername(company.Username);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_COMPANY_UPDATE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_COMPANY_UPDATE, dbParam, company.RoleCode, company.Username));

                return cResponseMessage;
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return new CResponseMessage { Code = -3, Message = "MSG_TEZIR_AN_ERROR_OCCURRED" };
            }
        }

        public CResponseMessage InsertDevelopProgress(DevelopProgress developProgress)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_EVENTID",
                        OracleDbType = OracleDbType.Int64,
                        Value = developProgress.EventID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int64,
                        Value = developProgress.CpnyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_EVENTDATE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = developProgress.EventDate,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_EVENTDESC",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = developProgress.EventDesc,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_EVENTDATEEN",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = developProgress.EventDateEN,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_EVENTDESCEN",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = developProgress.EventDescEN,
                    },                    
                    new OracleParameter()
                    {
                        ParameterName = "P_APPROVE",
                        OracleDbType = OracleDbType.Int16,
                        Value = developProgress.Approve,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_NOTE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = developProgress.Note,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_EVENTORDER",
                        OracleDbType = OracleDbType.Int64,
                        Value = developProgress.EventOrder,
                    },
                };

                _cBaseDataProvider.SetRoleCode(developProgress.RoleCode);
                _cBaseDataProvider.SetUsername(developProgress.Username);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_DEVELOPPROGRESS_INSERT, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_DEVELOPPROGRESS_INSERT, dbParam, developProgress.RoleCode, developProgress.Username));

                return cResponseMessage;
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return new CResponseMessage { Code = -3, Message = "MSG_TEZIR_AN_ERROR_OCCURRED" };
            }
        }

        public CResponseMessage UpdateDevelopProgress(DevelopProgress developProgress)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_EVENTID",
                        OracleDbType = OracleDbType.Int64,
                        Value = developProgress.EventID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_CPNYID",
                        OracleDbType = OracleDbType.Int64,
                        Value = developProgress.CpnyID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_EVENTDATE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = developProgress.EventDate,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_EVENTDESC",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = developProgress.EventDesc,
                    },                    
                    new OracleParameter()
                    {
                        ParameterName = "P_APPROVE",
                        OracleDbType = OracleDbType.Int16,
                        Value = developProgress.Approve,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_NOTE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = developProgress.Note,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_EVENTORDER",
                        OracleDbType = OracleDbType.Int64,
                        Value = developProgress.EventOrder,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_LANGUAGE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = developProgress.Language,
                    },
                };

                _cBaseDataProvider.SetRoleCode(developProgress.RoleCode);
                _cBaseDataProvider.SetUsername(developProgress.Username);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_DEVELOPPROGRESS_UPDATE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_DEVELOPPROGRESS_UPDATE, dbParam, developProgress.RoleCode, developProgress.Username));

                return cResponseMessage;
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return new CResponseMessage { Code = -3, Message = "MSG_TEZIR_AN_ERROR_OCCURRED" };
            }
        }

        

        public void SetRoleCode(string roleCode)
        {
            this._cBaseDataProvider.SetRoleCode(roleCode);
        }

        public void SetUsername(string username)
        {
            this._cBaseDataProvider.SetUsername(username);
        }

        
    }
}
