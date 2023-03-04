using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models.BaoCaoTienIch;
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
    public class RulesContext : IRulesContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public RulesContext(ICommon common, IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;

        }

        private OracleParameter[] InitDbParam(RulesViewModel rulesviewmodel)
        {
            return new[]
            {

                    new OracleParameter()
                    {
                        ParameterName = "P_ARULEID",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.ARuleID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACOMPANYTYPE",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.ACompanyType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADOCTYPE",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.ADocType,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_ANewType",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.ANewType,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_ADEADLINE",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.ADeadline,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADATE",
                        OracleDbType = OracleDbType.Date,
                        Value = rulesviewmodel.ADate,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADATELINEEXTEND",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.ADatelineExtend,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_ADATEEXTEND",
                        OracleDbType = OracleDbType.Date,
                        Value = rulesviewmodel.ADateExtend,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_FROMDATE",
                        OracleDbType = OracleDbType.Date,
                        Value = rulesviewmodel.FormDate,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_TODATE",
                        OracleDbType = OracleDbType.Date,
                        Value = rulesviewmodel.ToDate,
                    },
                      new OracleParameter()
                    {
                        ParameterName = "P_NIENDO",
                        OracleDbType = OracleDbType.Int32,
                        Value = rulesviewmodel.NienDo,
                    },
                       new OracleParameter()
                    {
                        ParameterName = "P_BIEUMAUSO",
                        OracleDbType = OracleDbType.Int32,
                        Value = rulesviewmodel.BieuMauSo,
                    },
                       new OracleParameter()
                    {
                        ParameterName = "P_DOITUONGAPDUNG",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.AObject,
                    },
                      new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    }

            };
        }


        public Task<CResponseMessage> Delete(RulesViewModel rulesviewmodel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ARULEID",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.ARuleID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACPNYTYPE",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.ACompanyType,
                    },
                };

                _cBaseDataProvider.SetUsername(rulesviewmodel.UserName);
                _cBaseDataProvider.SetRoleCode(rulesviewmodel.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_RULES_DELETE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_RULES_DELETE, dbParam, rulesviewmodel.RoleCode, rulesviewmodel.UserName));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể xóa rules!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<CResponseMessage> Insert(RulesViewModel rulesviewmodel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_TEMPID",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.BieuMauSo,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_NIENDOBCTC",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.NienDo,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_LOAIHINH",
                        OracleDbType = OracleDbType.Clob,
                        Value = rulesviewmodel.LoaiHinhIDs,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_AQDCBTT",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = rulesviewmodel.Aqdcbtt,
                    },
                      new OracleParameter()
                    {
                        ParameterName = "P_ACCPLCBTT",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = rulesviewmodel.Accplcbtt,
                    },
                         new OracleParameter()
                    {
                        ParameterName = "P_ADEADLINE",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.ADeadline,
                    },
                         new OracleParameter()
                    {
                        ParameterName = "P_ADATE",
                        OracleDbType = OracleDbType.Date,
                        Value = rulesviewmodel.ADate,
                    },
                              new OracleParameter()
                    {
                        ParameterName = "P_ADATELINEEXTEND",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.ADatelineExtend,
                    },
                              new OracleParameter()
                    {
                        ParameterName = "P_ADATEEXTEND",
                        OracleDbType = OracleDbType.Date,
                        Value = rulesviewmodel.ADateExtend,
                    },
                               new OracleParameter()
                    {
                        ParameterName = "P_ADOCTYPE",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.ADocType,
                    },
                               new OracleParameter()
                    {
                        ParameterName = "P_AOBJECT",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.AObject,
                    },
                               new OracleParameter()
                    {
                        ParameterName = "P_ANEWTYPE",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.ANewType,
                    }
                };

                _cBaseDataProvider.SetUsername(rulesviewmodel.UserName);
                _cBaseDataProvider.SetRoleCode(rulesviewmodel.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_RULES_INSERT, dbParam);

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -1, Message = "Lỗi không thể cập nhập rules!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<IEnumerable<Rules>> Select(RulesViewModel rulesviewmodel)
        {
            try
            {
                var dbParam = this.InitDbParam(rulesviewmodel);


                _cBaseDataProvider.SetUsername(rulesviewmodel.UserName);
                _cBaseDataProvider.SetRoleCode(rulesviewmodel.RoleCode);

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_RULES_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_RULES_SEARCH, dbParam, rulesviewmodel.RoleCode, rulesviewmodel.UserName));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));

                    var mapper = new DataNamesMapper<Rules>();

                    var rule = mapper.Map(dataSet.Tables[0]);

                    return Task.FromResult(rule);
                }

                return Task.FromResult(Enumerable.Empty<Rules>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<Rules>());
            }
        }

        public Task<CResponseMessage> Update(RulesViewModel rulesviewmodel)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ARULEID",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.ARuleID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_TEMPID",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.BieuMauSo,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_NIENDOBCTC",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.NienDo,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_LOAIHINH",
                        OracleDbType = OracleDbType.Clob,
                        Value = rulesviewmodel.LoaiHinhIDs,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADOCTYPE",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.ADocType,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_ANEWTYPE",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.ANewType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AQDCBTT",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = rulesviewmodel.Aqdcbtt,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACCPLCBTT",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = rulesviewmodel.Accplcbtt,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_ADEADLINE",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.ADeadline,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADATE",
                        OracleDbType = OracleDbType.Date,
                        Value = rulesviewmodel.ADate,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADATELINEEXTEND",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.ADatelineExtend,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_ADATEEXTEND",
                        OracleDbType = OracleDbType.Date,
                        Value = rulesviewmodel.ADateExtend,
                    },new OracleParameter()
                    {
                        ParameterName = "P_AOBJECT",
                        OracleDbType = OracleDbType.Int16,
                        Value = rulesviewmodel.AObject,
                    }
                };

                _cBaseDataProvider.SetUsername(rulesviewmodel.UserName);
                _cBaseDataProvider.SetRoleCode(rulesviewmodel.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_RULES_UPDATE, dbParam);

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -1, Message = "Lỗi không thể cập nhập rules!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<DataTable> Listrules(RulesViewModel rulesviewmodel)
        {
            var dbParam = this.InitDbParam(rulesviewmodel);

            _cBaseDataProvider.SetUsername(rulesviewmodel.UserName);
            _cBaseDataProvider.SetRoleCode(rulesviewmodel.RoleCode);

            var datatb = _cBaseDataProvider.GetDataTableFromSP(ConstantsSP.SPEZIR_RULES_SEARCH, dbParam);

            return Task<DataTable>.FromResult(datatb);

        }
    }
}
