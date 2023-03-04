using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using DataServiceLib.Interfaces;
using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models;
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
    public class CommonTypeContext : ICommonTypeContext
    {
        private readonly IConfiguration _config;
        private readonly ICBaseDataProvider _cBaseDataProvider;
        private readonly IAppLogger _appLogger;
        private readonly ICommon _common;

        public CommonTypeContext(IConfiguration config, ICBaseDataProvider cBaseDataProvider, IAppLogger appLogger, ICommon common)
        {
            this._config = config;
            this._cBaseDataProvider = cBaseDataProvider;
            this._appLogger = appLogger;
            this._common = common;
        }
        public Task<CResponseMessage> Delete(CommonTypeViewModel commontype)
        {
            //throw new NotImplementedException();
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ATYPEID",
                        OracleDbType = OracleDbType.Int32,
                        Value = commontype.TypeID,
                    },
                };

                _cBaseDataProvider.SetUsername(commontype.UserLogin);
                _cBaseDataProvider.SetRoleCode(commontype.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_COMMONTYPE_DELETE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_COMMONTYPE_DELETE, dbParam, commontype.RoleCode, commontype.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể xóa danh mục!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<CResponseMessage> Insert(CommonTypeViewModel commonType)
        {
            //throw new NotImplementedException();
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ATYPECODE",
                        OracleDbType = OracleDbType.Int16,
                        Value = commonType.TypeCode,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ATYPENAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = commonType.TypeName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ATYPENAMEEN",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = commonType.TypeNameEN,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ATYPEDESCRIPTION",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = commonType.TypeDescription,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AORD",
                        OracleDbType = OracleDbType.Int32,
                        Value = commonType.Ord,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACATEGORY",
                        OracleDbType = OracleDbType.Int32,
                        Value = commonType.Category,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_APARENTID",
                        OracleDbType = OracleDbType.Int32,
                        Value = commonType.ParentID,
                    },
                };

                _cBaseDataProvider.SetUsername(commonType.UserLogin);
                _cBaseDataProvider.SetRoleCode(commonType.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_COMMONTYPE_INSERT, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_COMMONTYPE_INSERT, dbParam, commonType.RoleCode, commonType.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể thêm mới danh mục!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<IEnumerable<CommonType>> Select(CommonTypeViewModel commonType)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ACATEGORY",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = commonType.ListCategory,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                };

                _cBaseDataProvider.SetUsername(commonType.UserLogin);
                _cBaseDataProvider.SetRoleCode(commonType.RoleCode);

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_COMMONTYPE_GET_BY_CATEGORY, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_COMMONTYPE_GET_BY_CATEGORY, dbParam, commonType.RoleCode, commonType.UserLogin));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));
                    var mapper = new DataNamesMapper<CommonType>();

                    var commontype = mapper.Map(dataSet.Tables[0]);
                    return Task.FromResult(commontype);
                }


                return Task.FromResult(Enumerable.Empty<CommonType>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<CommonType>());
            }
        }

        public Task<CResponseMessage> Update(CommonTypeViewModel commonType)
        {
            //throw new NotImplementedException();
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ATYPEID",
                        OracleDbType = OracleDbType.Int32,
                        Value = commonType.TypeID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ATYPECODE",
                        OracleDbType = OracleDbType.Int32,
                        Value = commonType.TypeCode,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ATYPENAME",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = commonType.TypeName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ATYPENAMEEN",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = commonType.TypeNameEN,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ATYPEDESCRIPTION",
                        OracleDbType = OracleDbType.NVarchar2,
                        Value = commonType.TypeDescription,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_AORD",
                        OracleDbType = OracleDbType.Int32,
                        Value = commonType.Ord,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACATEGORY",
                        OracleDbType = OracleDbType.Int32,
                        Value = commonType.Category,
                    },
                     new OracleParameter()
                    {
                        ParameterName = "P_APARENTID",
                        OracleDbType = OracleDbType.Int32,
                        Value = commonType.ParentID,
                    },
                };

                _cBaseDataProvider.SetUsername(commonType.UserLogin);
                _cBaseDataProvider.SetRoleCode(commonType.RoleCode);

                var cResponseMessage = _cBaseDataProvider.GetResponseFromExecutedSP(ConstantsSP.SPEZIR_COMMONTYPE_UPDATE, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_COMMONTYPE_UPDATE, dbParam, commonType.RoleCode, commonType.UserLogin));

                return Task.FromResult(cResponseMessage);
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                var cResponseMessage = new CResponseMessage { Code = -3, Message = "Lỗi không thể update danh mục!" };

                return Task.FromResult(cResponseMessage);
            }
        }

        public Task<IEnumerable<CommonType>> SelectById(CommonTypeViewModel commonType)
        {
            try
            {
                OracleParameter[] dbParam =
                {
                    new OracleParameter()
                    {
                        ParameterName = "P_ATYPEID",
                        OracleDbType = OracleDbType.Int32,
                        Value = commonType.TypeID,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ATYPECODE",
                        OracleDbType = OracleDbType.Int32,
                        Value = commonType.TypeCode,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ATYPENAME",
                        OracleDbType = OracleDbType.Varchar2,
                        Value = commonType.TypeName,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "P_ACATEGORY",
                        OracleDbType = OracleDbType.Int32,
                        Value = commonType.Category,
                    },
                    new OracleParameter()
                    {
                        ParameterName = "REFCURSOR",
                        Direction = ParameterDirection.Output,
                        OracleDbType = OracleDbType.RefCursor,
                    },
                };

                _cBaseDataProvider.SetUsername(commonType.UserLogin);
                _cBaseDataProvider.SetRoleCode(commonType.RoleCode);

                var dataSet = _cBaseDataProvider.GetDatasetFromSP(ConstantsSP.SPEZIR_COMMONTYPE_SEARCH, dbParam);

                _appLogger.SqlLogger.LogSql(_common.SPParametersToString(ConstantsSP.SPEZIR_COMMONTYPE_SEARCH, dbParam, commonType.RoleCode, commonType.UserLogin));

                if (dataSet.Tables.Count > 0)
                {
                    _appLogger.SqlLogger.LogSql(_common.GetResultInfo(dataSet));
                    var mapper = new DataNamesMapper<CommonType>();

                    var commontype = mapper.Map(dataSet.Tables[0]);
                    return Task.FromResult(commontype);
                }


                return Task.FromResult(Enumerable.Empty<CommonType>());
            }
            catch (Exception ex)
            {
                _appLogger.ErrorLogger.LogError(ex);

                return Task.FromResult(Enumerable.Empty<CommonType>());
            }
        }
    }
}
