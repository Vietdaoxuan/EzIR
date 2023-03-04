using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models;
using EzIRSpecialist.Models.BaoCaoTienIch;
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
    public class NhacCongBoThongTinContext : INhacCongBoThongTinContext
    {

        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public NhacCongBoThongTinContext(ICommon common, IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;

        }

        public Task<CResponseMessage> Delete(NhacCongBoThongTinViewModel obj)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NhacCongBoThongTin>> GetTypeCompany(NhacCongBoThongTinViewModel viewModel)
        {
            try
            {

                OracleParameter[] dbParam =
                {
                   new OracleParameter()
                    {
                        ParameterName = "P_COMPANYTYPE",
                        OracleDbType = OracleDbType.Int32,
                        Value = viewModel.ACompanyType,
                    },
                   new OracleParameter()
                    {
                        ParameterName = "P_EXPERT",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = viewModel.AExpert,
                    },
                   new OracleParameter()
                    {
                        ParameterName = "P_LEVEL",
                        OracleDbType = OracleDbType.Int32,
                        Value = viewModel.Alevel,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_NIENDOBCTC",
                        OracleDbType = OracleDbType.Int32,
                        Value = viewModel.AniendoBCTC,
                    },

                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                //var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_COMPANY_GET_COMPANYTYPE, dbParam);

                //_appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_COMPANY_GET_COMPANYTYPE, dbParam, "", ""));

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_COMPANY_SEARCH1, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_COMPANY_SEARCH1, dbParam, "", ""));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<NhacCongBoThongTin>();

                    var remindcbtt = mapper.Map(dataSet.Tables[0]);

                    return Task.FromResult(remindcbtt);
                }

                return Task.FromResult(Enumerable.Empty<NhacCongBoThongTin>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<NhacCongBoThongTin>());
            }
        }

        public Task<CResponseMessage> Insert(NhacCongBoThongTinViewModel obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NhacCongBoThongTinModelView> ListCompanyID(int listcbtt, NhacCongBoThongTinViewModel nhacCongBoThongTinViewModel)
        {

            try
            {
                OracleParameter[] dbParam =
                {

                    new OracleParameter()
                    {
                        ParameterName = "P_ARULEID",
                        OracleDbType = OracleDbType.Int16,
                        Value = listcbtt,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_ALOAIHINH",
                        OracleDbType = OracleDbType.Int16,
                        Value = nhacCongBoThongTinViewModel.TypeID,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_USERNAME",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = nhacCongBoThongTinViewModel.UserName,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_ROLECODE",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = nhacCongBoThongTinViewModel.RoleCode,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },

            };


                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_REMIND_CBTT_SEND_MAIL, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_REMIND_CBTT_SEND_MAIL, dbParam, "", ""));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<NhacCongBoThongTinModelView>();

                    var companys = mapper.Map(dataSet.Tables[0]);

                    return companys;
                }

                return Enumerable.Empty<NhacCongBoThongTinModelView>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<NhacCongBoThongTinModelView>();
            }

        }

        public Task<IEnumerable<NhacCongBoThongTin>> Select(NhacCongBoThongTinViewModel nhacCongBoThongTinViewModel)
        {
            try
            {

                OracleParameter[] dbParam =
                {
                   new OracleParameter()
                    {
                        ParameterName = "P_MACK",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = nhacCongBoThongTinViewModel.AStockCode,
                    },
                   new OracleParameter()
                    {
                        ParameterName = "P_ACOMPANYTYPE",
                        OracleDbType = OracleDbType.Int32,
                        Value = nhacCongBoThongTinViewModel.ALoaiDoanhNghiep,
                    },
                   new OracleParameter()
                    {
                        ParameterName = "P_FROMDATE",
                        OracleDbType = OracleDbType.Date,
                        Value = nhacCongBoThongTinViewModel.FormDate,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_TODATE",
                        OracleDbType = OracleDbType.Date,
                        Value = nhacCongBoThongTinViewModel.ToDate,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_NIENDOBCTC",
                        OracleDbType = OracleDbType.Int32,
                        Value = nhacCongBoThongTinViewModel.AniendoBCTC,
                    },
                   new OracleParameter()
                    {
                        ParameterName = "P_LEVEL",
                        OracleDbType = OracleDbType.Int32,
                        Value = nhacCongBoThongTinViewModel.Alevel,
                    },
                   new OracleParameter()
                    {
                        ParameterName = "P_EXPERT",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = nhacCongBoThongTinViewModel.AExpert,
                    },
                   new OracleParameter()
                    {
                        ParameterName = "P_DOITUONGAPDUNG",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = nhacCongBoThongTinViewModel.AObject,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }
                };

                _cBaseDataProvider.SetUsername(nhacCongBoThongTinViewModel.UserName);
                _cBaseDataProvider.SetRoleCode(nhacCongBoThongTinViewModel.RoleCode);


                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_REMIND_CBTT_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_REMIND_CBTT_SEARCH, dbParam, nhacCongBoThongTinViewModel.RoleCode, nhacCongBoThongTinViewModel.UserName));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<NhacCongBoThongTin>();

                    var remindcbtt = mapper.Map(dataSet.Tables[0]);

                    return Task.FromResult(remindcbtt);
                }

                return Task.FromResult(Enumerable.Empty<NhacCongBoThongTin>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<NhacCongBoThongTin>());
            }
        }
        public Task<CResponseMessage> Update(NhacCongBoThongTinViewModel obj)
        {
            throw new NotImplementedException();
        }
    }
}
