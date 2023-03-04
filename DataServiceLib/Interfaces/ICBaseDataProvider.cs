using CoreLib.Entities;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace DataServiceLib.Interfaces
{
    public interface ICBaseDataProvider
    {
        void SetRoleCode(string roleCode);

        void SetUsername(string username);

        DataSet GetDatasetFromSP(string SPname, OracleParameter[] paramArr);

        DataSet GetDatasetFromSP(string connectionString, string SPname, OracleParameter[] paramArr);

        DataSet GetDatasetFromQuery(string strQuery);

        /// <summary>
        /// Dành cho báo cáo
        /// </summary>
        /// <param name="SPname"></param>
        /// <param name="paramArr"></param>
        /// <returns></returns>
        DataSet GetDatasetFromSPNoRole(string SPname, OracleParameter[] paramArr);

        DataTable GetDataTableFromSP(string SPname, OracleParameter[] paramArr);

        DataRow GetDataRowFromSP(string SPname, OracleParameter[] paramArr);

        bool ExecuteSP(string SPname, OracleParameter[] paramArr);

        /// <summary>
        /// Thực thi sp ,
        /// SP tryền vào phải có 2 output parameter ,
        ///  @Message  nvarchar(200) output ,
        ///  @ErrorCode int output.
        /// </summary>
        /// <param name="SPname">Tên SP.</param>
        /// <param name="paramArr">Các para chứa dữ liệu.</param>
        /// <returns>Respose gồm mã lỗi và message. </returns>
        CResponseMessage GetResponseFromExecutedSP(string SPname, OracleParameter[] paramArr);

        CResponseMessage GetResponseFromExecutedSP(string connectionString, string SPname, OracleParameter[] paramArr);
    }
}
