using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CommonLib.Interfaces;
using CoreLib.Entities;
using CoreLib.Interfaces;
using DataServiceLib.Interfaces;
using DataServiceLib.Utilities;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;


namespace DataServiceLib.Implementations
{
    public class CBaseDataProvider : ICBaseDataProvider
    {
        private readonly IConfiguration _config;
        private readonly IErrorHandler _errorHandler;
        private readonly string _connectionString;
        private List<OracleParameter> _requiredParams;        

        // private OracleParameter usernameParam;
        public CBaseDataProvider(IConfiguration config, IErrorHandler errorHandler)
        {
            this._config = config;
            this._errorHandler = errorHandler;
            this._connectionString = this._config["ConnectionString"];
            this._requiredParams = new List<OracleParameter>();
        }

        public void SetRoleCode(string roleCode)
        {
            if (!string.IsNullOrEmpty(roleCode))
            {
                this._requiredParams.Add(new OracleParameter
                {
                    ParameterName = "p_RoleCode",
                    OracleDbType = OracleDbType.Varchar2,
                    Direction = ParameterDirection.Input,
                    Value = roleCode,
                });
            }
        }

        public void SetUsername(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                this._requiredParams.Add(new OracleParameter
                {
                    ParameterName = "p_USERNAME",
                    OracleDbType = OracleDbType.Varchar2,
                    Direction = ParameterDirection.Input,
                    Value = username,
                });
            }
        }

        public DataSet GetDatasetFromSP(string SPname, OracleParameter[] paramArr)
        {
            try
            {
                this.ConvertSPnameToLog(SPname, paramArr); // Ghi log đầu vào

                var paramList = paramArr.ToList();
                if (this._requiredParams.Count > 0)
                {
                    this._requiredParams.ForEach(param => { paramList.Add(param); });

                    // sau khi add thì clear luôn , set chỉ dùng 1 lần
                    this._requiredParams.Clear();
                }

                paramArr = paramList.ToArray();
                return OracleHelper.ExecuteDataset(this._connectionString, CommandType.StoredProcedure, SPname, paramArr);
            }
            catch (Exception e)
            {
                this._errorHandler.WriteToFile(e);
            }

            return new DataSet();
        }

        public DataSet GetDatasetFromSP(string connectionString, string SPname, OracleParameter[] paramArr)
        {
            try
            {
                this.ConvertSPnameToLog(SPname, paramArr); // Ghi log đầu vào

                var paramList = paramArr.ToList();
                if (this._requiredParams.Count > 0)
                {
                    this._requiredParams.ForEach(param => { paramList.Add(param); });

                    // sau khi add thì clear luôn , set chỉ dùng 1 lần
                    this._requiredParams.Clear();
                }

                paramArr = paramList.ToArray();
                return OracleHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, SPname, paramArr);
            }
            catch (Exception e)
            {
                this._errorHandler.WriteToFile(e);
            }

            return new DataSet();
        }

        public DataSet GetDatasetFromSPNoRole(string SPname, OracleParameter[] paramArr)
        {
            try
            {
                this.ConvertSPnameToLog(SPname, paramArr);

                return OracleHelper.ExecuteDataset(this._connectionString, CommandType.StoredProcedure, SPname, paramArr);
            }
            catch (Exception e)
            {
                this._errorHandler.WriteToFile(e);
            }

            return new DataSet();
        }

        public DataTable GetDataTableFromSP(string SPname, OracleParameter[] paramArr)
        {

            var dsData = this.GetDatasetFromSP(SPname, paramArr);
            if (dsData.Tables.Count > 0)
            {
                return dsData.Tables[0];
            }

            return null;
        }

        public DataRow GetDataRowFromSP(string SPname, OracleParameter[] paramArr)
        {

            var dtbData = this.GetDataTableFromSP(SPname, paramArr);
            if (dtbData.Rows.Count > 0)
            {
                return dtbData.Rows[0];
            }

            return null;
        }

        public bool ExecuteSP(string SPname, OracleParameter[] paramArr)
        {
            this.ConvertSPnameToLog(SPname, paramArr); // Ghi log đầu vào
            var paramList = paramArr.ToList();
            if (this._requiredParams.Count > 0)
            {
                this._requiredParams.ForEach(param => { paramList.Add(param); });

                // sau khi add thì clear luôn , set chỉ dùng 1 lần
                this._requiredParams.Clear();
            }

            paramArr = paramList.ToArray();

            return OracleHelper.ExecuteNonQuery(this._connectionString, CommandType.StoredProcedure, SPname, paramArr) > 0;
        }

        public bool ExecuteSP(string connectionString, string SPname, OracleParameter[] paramArr)
        {
            this.ConvertSPnameToLog(SPname, paramArr); // Ghi log đầu vào
            var paramList = paramArr.ToList();
            if (this._requiredParams.Count > 0)
            {
                this._requiredParams.ForEach(param => { paramList.Add(param); });

                // sau khi add thì clear luôn , set chỉ dùng 1 lần
                this._requiredParams.Clear();
            }

            paramArr = paramList.ToArray();

            return OracleHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, SPname, paramArr) > 0;
        }

        /// <summary>
        /// SP phải có 2 param p_MESSAGE và p_ERRORCODE.
        /// </summary>
        /// <param name="SPname"></param>
        /// <param name="paramArr"></param>
        /// <returns></returns>
        public CResponseMessage GetResponseFromExecutedSP(string SPname, OracleParameter[] paramArr)
        {
            var errorCode = new OracleParameter()
            {
                ParameterName = "p_ERRORCODE",
                OracleDbType = OracleDbType.Int32,
                Size = 25,
                Direction = ParameterDirection.Output,
            };
            var message = new OracleParameter()
            {
                ParameterName = "p_MESSAGE",
                OracleDbType = OracleDbType.Varchar2,
                Size = 200,
                Direction = ParameterDirection.Output,
            };

            var paramList = paramArr.ToList();
            paramList.Add(message);
            paramList.Add(errorCode);

            if (this._requiredParams.Count > 0)
            {
                this._requiredParams.ForEach(param => { paramList.Add(param); });

                // sau khi add thì clear luôn , set chỉ dùng 1 lần
                this._requiredParams.Clear();
            }

            paramArr = paramList.ToArray();
            try
            {
                this.ExecuteSP(SPname, paramArr);

                if (errorCode.Value != null && message.Value != null)
                {
                    return new CResponseMessage()
                    {
                        Code = int.Parse(errorCode.Value.ToString()),
                        Message = message.Value.ToString(),
                    };
                }

                return new CResponseMessage(-1, "Không thể thực thi SP");
            }
            catch (Exception e)
            {
                this._errorHandler.WriteToFile(e);

                return new CResponseMessage(-1, e.Message);
            }
        }

        public CResponseMessage GetResponseFromExecutedSP(string connectionString, string SPname, OracleParameter[] paramArr)
        {
            var errorCode = new OracleParameter()
            {
                ParameterName = "p_ERRORCODE",
                OracleDbType = OracleDbType.Int32,
                Size = 25,
                Direction = ParameterDirection.Output,
            };
            var message = new OracleParameter()
            {
                ParameterName = "p_MESSAGE",
                OracleDbType = OracleDbType.Varchar2,
                Size = 200,
                Direction = ParameterDirection.Output,
            };

            var paramList = paramArr.ToList();
            paramList.Add(message);
            paramList.Add(errorCode);

            if (this._requiredParams.Count > 0)
            {
                this._requiredParams.ForEach(param => { paramList.Add(param); });

                // sau khi add thì clear luôn , set chỉ dùng 1 lần
                this._requiredParams.Clear();
            }

            paramArr = paramList.ToArray();
            try
            {
                this.ExecuteSP(connectionString, SPname, paramArr);

                if (errorCode.Value != null && message.Value != null)
                {
                    return new CResponseMessage()
                    {
                        Code = int.Parse(errorCode.Value.ToString()),
                        Message = message.Value.ToString(),
                    };
                }

                return new CResponseMessage(-1, "Không thể thực thi SP");
            }
            catch (Exception e)
            {
                this._errorHandler.WriteToFile(e);

                return new CResponseMessage(-1, e.Message);
            }
        }

        public DataSet GetDatasetFromQuery(string strQuery)
        {
            try
            {
                return OracleHelper.ExecuteDataset(this._connectionString, CommandType.Text, strQuery);
            }
            catch (Exception e)
            {
                this._errorHandler.WriteToFile(e);
            }

            return new DataSet();
        }

        /// <summary>
        /// Ghi log tên sp và chuỗi đầu vào
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="paramArr"></param>
        public void ConvertSPnameToLog(string spName, OracleParameter[] paramArr)
        {
            try
            {
                string stringParam = string.Empty;

                foreach (OracleParameter re in paramArr)
                {
                    stringParam += re.ParameterName + ":" + re.Value + ";";
                }

                this._errorHandler.WriteStringToFile(spName, stringParam);
            }
            catch (Exception ex)
            {

            }

        }
    }
}
