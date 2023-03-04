using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using CoreLib.Entities.ModelViews;
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

        public Task<CResponseMessage> Delete(TemplateViewModel obj)
        {
            throw new NotImplementedException();
        }

        public Task<CResponseMessage> Insert(TemplateViewModel obj)
        {
            throw new NotImplementedException();
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
                    /*new OracleParameter()
                    {
                        ParameterName = "P_ATEMPLATETYPE",
                        OracleDbType = OracleDbType.Int32,
                        Value = template.TemplateType,
                    },*/
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

        public Task<CResponseMessage> Update(TemplateViewModel obj)
        {
            throw new NotImplementedException();
        }
    }
}
