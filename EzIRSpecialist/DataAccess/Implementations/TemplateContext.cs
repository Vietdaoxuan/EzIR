using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models.ModelView;
using EzIRSpecialist.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Implementations
{
    public class TemplateContext : ITemplateContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public TemplateContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;
        }
        public Task<CResponseMessage> Delete(TemplateViewModel template)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ATEMPLATEID",
                        OracleDbType = OracleDbType.Int32,
                        Value = template.TemplateID,
                    }
                };

                _cBaseDataProvider.SetUsername(template.UserLogin);
                _cBaseDataProvider.SetRoleCode(template.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_TEMPLATE_DELETE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_TEMPLATE_DELETE, dbParam, template.RoleCode, template.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể xóa biểu mẫu!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<CResponseMessage> Insert(TemplateViewModel template)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ACOMPANYTYPE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = template.CompanyTypelist,
                    },
                    //new OracleParameter()
                    //{
                    //    ParameterName = "P_ACOMPANYTYPE",
                    //    OracleDbType = OracleDbType.Int32,
                    //    Value = template.CompanyType,
                    //},
                    new OracleParameter()
                    {
                        ParameterName = "P_ATEMPLATETYPE",
                        OracleDbType = OracleDbType.Int32,
                        Value = template.TemplateType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ATITLE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = template.Title,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADETAIL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = template.Detail,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACCPL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = template.CCPL,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AFILENAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = template.FileName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APATH",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = template.Path,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AURL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = template.Url,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AORD",
                        OracleDbType = OracleDbType.Int32,
                        Value = template.Order,
                    }
                };

                _cBaseDataProvider.SetUsername(template.UserLogin);
                _cBaseDataProvider.SetRoleCode(template.RoleCode);

                //var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_TEMPLATE_INSERT, dbParam);

                //_appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_TEMPLATE_INSERT, dbParam, template.RoleCode, template.UserLogin));


                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_TEMPLATE_INSERT, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_TEMPLATE_INSERT, dbParam, template.RoleCode, template.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể thêm mới biểu mẫu!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<List<IEnumerable<TemplateModelView>>> Select(TemplateViewModel template)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ATEMPLATEID",
                        OracleDbType = OracleDbType.Int32,
                        Value = template.TemplateID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACOMPANYTYPE",
                        OracleDbType = OracleDbType.Int32,
                        Value = template.CompanyType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ATITLE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = template.Title,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADETAIL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = template.Detail,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACCPL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = template.CCPL,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSORVN",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOREN",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSORSN",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                };

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_TEMPLATE_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_TEMPLATE_SEARCH, dbParam, template.RoleCode, template.UserLogin));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));
                    var mapper = new DataNamesMapper<TemplateModelView>();
                    List<IEnumerable<TemplateModelView>> listTem = new List<IEnumerable<TemplateModelView>>();
                    var TemplateVN = mapper.Map(dataSet.Tables[0]);
                    var TemplateEN = mapper.Map(dataSet.Tables[1]);
                    var TemplateSN = mapper.Map(dataSet.Tables[2]);
                    listTem.Add(TemplateVN);
                    listTem.Add(TemplateEN);
                    listTem.Add(TemplateSN);
                    return Task.FromResult(listTem);
                }


                return Task.FromResult(new List<IEnumerable<TemplateModelView>>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(new List<IEnumerable<TemplateModelView>>());
            }
        }

        public IEnumerable SelectIndex(TemplateViewModel template)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ACOMPANYTYPE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = template.CompanyTypelist,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "REFCURSORVN",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOREN",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSORSN",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                };

                var dataInDex = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_TEMPLATE_SEARCH_INDEX, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_TEMPLATE_SEARCH_INDEX, dbParam, template.RoleCode, template.UserLogin));

                if (dataInDex.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataInDex));

                    var mapper = new DataNamesMapper<TemplateModelView>();
                   List<List<TemplateModelView>> listIndex = new List<List<TemplateModelView>>();
                    var Indexs0 = mapper.Map(dataInDex.Tables[0]).ToList();
                    var Indexs1 = mapper.Map(dataInDex.Tables[1]).ToList();
                    var Indexs2 = mapper.Map(dataInDex.Tables[2]).ToList();

                    listIndex.Add(Indexs0);
                    listIndex.Add(Indexs1);
                    listIndex.Add(Indexs2);
                    return listIndex;
                }
                return Enumerable.Empty<TemplateModelView>();
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Enumerable.Empty<TemplateModelView>();
            }
        }

        public Task<CResponseMessage> Update(TemplateViewModel template)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ATEMPLATEID",
                        OracleDbType = OracleDbType.Int32,
                        Value = template.TemplateID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACOMPANYTYPE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = template.CompanyTypelist,
                    },
                    //new OracleParameter()
                    //{
                    //    ParameterName = "P_ACOMPANYTYPE",
                    //    OracleDbType = OracleDbType.Int32,
                    //    Value = template.CompanyType,
                    //},
                    new OracleParameter()
                    {
                        ParameterName = "P_ATEMPLATETYPE",
                        OracleDbType = OracleDbType.Int32,
                        Value = template.TemplateType,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AORD",
                        OracleDbType = OracleDbType.Int32,
                        Value = template.Order,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ATITLE",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = template.Title,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ADETAIL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = template.Detail,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACCPL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = template.CCPL,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AFILENAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = template.FileName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_APATH",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = template.Path,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AURL",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = template.Url,
                    }
                };

                _cBaseDataProvider.SetUsername(template.UserLogin);
                _cBaseDataProvider.SetRoleCode(template.RoleCode);

                //var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_TEMPLATE_UPDATE, dbParam);

                //_appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_TEMPLATE_UPDATE, dbParam, template.RoleCode, template.UserLogin));

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_TEMPLATE_UPDATE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_TEMPLATE_UPDATE, dbParam, template.RoleCode, template.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể sửa biểu mẫu!" };

                return Task.FromResult(cResponseMessage);
            }
        }
    }
}
