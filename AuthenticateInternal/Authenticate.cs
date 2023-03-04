using System;
using System.Data;
using Microsoft.AspNetCore.Http;
using Oracle.ManagedDataAccess.Client;

namespace AuthenticateInternal
{
    public class Authenticate
    {
        string strConn = ConfigProvider.Configuration["keyAuthenconnString"];

        //Check dải mảng vào Internal -- xem có phải vào từ mạng ngoài k hay mạng LAN
        public bool Check_PublicInterNet(HttpRequest currentPage)
        {
            bool Check = false;
            string[] IP_LAN = new string[] { "10.26.2", "10.26.4", "10.26.6", "10.26.8", "10.26.9", "10.26.10", "10.26.11", "10.26.12" };
            string IpClient = currentPage.HttpContext.Connection.RemoteIpAddress.ToString();
            //string IpClient = "10.26.4";
            foreach (string arrIPLan in IP_LAN)
            {
                if (IpClient.StartsWith(arrIPLan))
                {
                    Check = false;
                    return Check;
                }
                else
                {
                    Check = true;
                }
            }
            return Check;
        }

        //Check quyền vào Internal từ mạng ngoài
        public int Check_Permission(string UserName, ref string ReturnMess)
        {
            int iReturnCode = -1;
            try
            {
                OracleParameter[] arrParams = new OracleParameter[3];
                arrParams[0] = new OracleParameter("p_ausername", OracleDbType.Varchar2, 500);
                arrParams[0].Direction = ParameterDirection.Input;
                arrParams[0].Value = UserName;

                arrParams[1] = new OracleParameter("p_returncode", OracleDbType.Int16);
                arrParams[1].Direction = ParameterDirection.Output;
                arrParams[1].Value = DBNull.Value;

                arrParams[2] = new OracleParameter("p_returnMess", OracleDbType.Varchar2, 500);
                arrParams[2].Direction = ParameterDirection.Output;
                arrParams[2].Value = DBNull.Value;

                OracleHelper.ExecuteNonQuery(strConn, CommandType.StoredProcedure, "authen.spauth_internal_public_check", arrParams);
                iReturnCode = Convert.ToInt16(arrParams[1].Value.ToString());
                ReturnMess = arrParams[2].Value.ToString();
                CLogAuthentication.LogSQL(CLogAuthentication.GetDeepCaller(), "authen.spauth_internal_public_check \r\n" + "p_ausername: " + UserName + "\r\n p_returncode: " + iReturnCode + "\r\n ReturnMsg: " + ReturnMess);
                return iReturnCode;
            }
            catch (Exception ex)
            {
                ReturnMess = ex.Message;
                iReturnCode = -1;
                CLogAuthentication.LogError(CLogAuthentication.GetDeepCaller(), ex.Message);
                return iReturnCode;
            }
        }


        //Check session đăng nhập khi login
        public int Check_Session(HttpRequest currentPage, ref string loginname, ref string username, ref string returnMess)
        {
            int iReturnCode = -1;

            try
            {
                OracleParameter[] arrParams = new OracleParameter[9];
                arrParams[0] = new OracleParameter("p_aipclient", OracleDbType.Varchar2, 500);
                arrParams[0].Direction = ParameterDirection.Input;
                arrParams[0].Value = currentPage.HttpContext.Connection.LocalIpAddress.ToString();

                arrParams[1] = new OracleParameter("p_aipserver", OracleDbType.Varchar2, 500);
                arrParams[1].Direction = ParameterDirection.Input;
                arrParams[1].Value = currentPage.HttpContext.Connection.RemoteIpAddress.ToString().ToString();

                arrParams[2] = new OracleParameter("p_atoken", OracleDbType.Varchar2, 500);
                arrParams[2].Direction = ParameterDirection.Input;
                arrParams[2].Value = currentPage.Cookies["cookieToken"] ?? "blank"; ;

                arrParams[3] = new OracleParameter("p_auseragent", OracleDbType.Varchar2, 500);
                arrParams[3].Direction = ParameterDirection.Input;
                arrParams[3].Value = currentPage.Headers["User-Agent"];

                arrParams[4] = new OracleParameter("p_abrowser", OracleDbType.Varchar2, 500);
                arrParams[4].Direction = ParameterDirection.Input;
                arrParams[4].Value = string.Empty;

                arrParams[5] = new OracleParameter("p_aloginname", OracleDbType.Varchar2, 500);
                arrParams[5].Direction = ParameterDirection.Output;
                arrParams[5].Value = DBNull.Value;

                arrParams[6] = new OracleParameter("p_ausername", OracleDbType.Varchar2, 500);
                arrParams[6].Direction = ParameterDirection.Output;
                arrParams[6].Value = DBNull.Value;

                arrParams[7] = new OracleParameter("p_returncode", OracleDbType.Int16);
                arrParams[7].Direction = ParameterDirection.Output;
                arrParams[7].Value = DBNull.Value;

                arrParams[8] = new OracleParameter("p_returnMess", OracleDbType.Varchar2, 500);
                arrParams[8].Direction = ParameterDirection.Output;
                arrParams[8].Value = DBNull.Value;

                OracleHelper.ExecuteNonQuery(strConn, CommandType.StoredProcedure, "authen.spauth_internal_confirmcookie", arrParams);

                loginname = arrParams[5].Value.ToString();
                username = arrParams[6].Value.ToString();
                iReturnCode = Convert.ToInt16(arrParams[7].Value.ToString());
                returnMess = arrParams[8].Value.ToString();
                CLogAuthentication.LogSQL(CLogAuthentication.GetDeepCaller(), currentPage.Cookies["cookieToken"] + "\r\n p_auseragent: " + currentPage.Headers["User-Agent"] + "\r\n p_abrowser: " + string.Empty + "\r\n p_aloginname: " + loginname + "\r\n  p_ausername: " + username + "\r\n p_returncode: " + iReturnCode + "\r\n p_returnMess: " + returnMess);
                return iReturnCode;

            }
            catch (Exception ex)
            {
                loginname = null;
                username = null;
                iReturnCode = -1;
                returnMess = ex.Message;
                CLogAuthentication.LogError(CLogAuthentication.GetDeepCaller(), ex.Message);
                return iReturnCode;
            }
        }

        //Logout clear session
        public int Internal_Logout(HttpRequest currentPage, int iSource, ref string returnMess)
        {
            int iReturnCode = -1;

            try
            {
                OracleParameter[] arrParams = new OracleParameter[11];
                arrParams[0] = new OracleParameter("p_asource", OracleDbType.Int16);
                arrParams[0].Direction = ParameterDirection.Input;
                arrParams[0].Value = iSource;

                arrParams[1] = new OracleParameter("p_aipclient", OracleDbType.Varchar2, 500);
                arrParams[1].Direction = ParameterDirection.Input;
                arrParams[1].Value = currentPage.HttpContext.Connection.RemoteIpAddress.ToString().ToString();

                arrParams[2] = new OracleParameter("p_aipserver", OracleDbType.Varchar2, 500);
                arrParams[2].Direction = ParameterDirection.Input;
                arrParams[2].Value = currentPage.HttpContext.Connection.LocalIpAddress.ToString();

                arrParams[3] = new OracleParameter("p_atoken", OracleDbType.Varchar2, 500);
                arrParams[3].Direction = ParameterDirection.Input;
                arrParams[3].Value = currentPage.Cookies["cookieToken"] ?? "blank"; ;

                arrParams[4] = new OracleParameter("p_areferer", OracleDbType.Varchar2, 500);
                arrParams[4].Direction = ParameterDirection.Input;
                arrParams[4].Value = currentPage.Headers["Referer"];

                arrParams[5] = new OracleParameter("p_auseragent", OracleDbType.Varchar2, 500);
                arrParams[5].Direction = ParameterDirection.Input;
                arrParams[5].Value = currentPage.Headers["User-Agent"];

                arrParams[6] = new OracleParameter("p_abrowser", OracleDbType.Varchar2, 500);
                arrParams[6].Direction = ParameterDirection.Input;
                arrParams[6].Value = string.Empty;

                arrParams[7] = new OracleParameter("p_abrowsername", OracleDbType.Varchar2, 500);
                arrParams[7].Direction = ParameterDirection.Input;
                arrParams[7].Value = string.Empty;

                arrParams[8] = new OracleParameter("p_abrowservers", OracleDbType.Varchar2, 500);
                arrParams[8].Direction = ParameterDirection.Input;
                arrParams[8].Value = string.Empty;

                arrParams[9] = new OracleParameter("p_returncode", OracleDbType.Int16);
                arrParams[9].Direction = ParameterDirection.Output;
                arrParams[9].Value = DBNull.Value;

                arrParams[10] = new OracleParameter("p_returnMess", OracleDbType.Varchar2, 4000);
                arrParams[10].Direction = ParameterDirection.Output;
                arrParams[10].Value = DBNull.Value;

                OracleHelper.ExecuteNonQuery(strConn, CommandType.StoredProcedure, "authen.spauth_internal_logout", arrParams);
                iReturnCode = Int16.Parse(arrParams[9].Value.ToString());
                returnMess = arrParams[10].Value.ToString();
                CLogAuthentication.LogSQL(CLogAuthentication.GetDeepCaller(), currentPage.Cookies["cookieToken"] + "\r\n p_auseragent: " + currentPage.Headers["User-Agent"] + "\r\n p_abrowser: " + string.Empty + "\r\n p_returncode: " + iReturnCode + "\r\n p_returnMess: " + returnMess);

                return iReturnCode;
            }
            catch (Exception ex)
            {
                iReturnCode = -1;
                returnMess = ex.Message;
                CLogAuthentication.LogError(CLogAuthentication.GetDeepCaller(), ex.Message);
                return iReturnCode;
            }
        }

        public int Check_Session(HttpRequest currentPage, ref string loginname, ref string local, ref string ussertype, ref string subcstcencde, ref string username, ref string returnMess)
        {
            int iReturnCode = -1;

            try
            {
                OracleParameter[] arrParams = new OracleParameter[12];
                arrParams[0] = new OracleParameter("p_aipclient", OracleDbType.Varchar2, 500);
                arrParams[0].Direction = ParameterDirection.Input;
                arrParams[0].Value = currentPage.HttpContext.Connection.LocalIpAddress.ToString();

                arrParams[1] = new OracleParameter("p_aipserver", OracleDbType.Varchar2, 500);
                arrParams[1].Direction = ParameterDirection.Input;
                arrParams[1].Value = currentPage.HttpContext.Connection.RemoteIpAddress.ToString().ToString();

                arrParams[2] = new OracleParameter("p_atoken", OracleDbType.Varchar2, 500);
                arrParams[2].Direction = ParameterDirection.Input;
                arrParams[2].Value = currentPage.Cookies["cookieToken"] ?? "blank";

                arrParams[3] = new OracleParameter("p_auseragent", OracleDbType.Varchar2, 500);
                arrParams[3].Direction = ParameterDirection.Input;
                arrParams[3].Value = currentPage.Headers["User-Agent"];

                arrParams[4] = new OracleParameter("p_abrowser", OracleDbType.Varchar2, 500);
                arrParams[4].Direction = ParameterDirection.Input;
                arrParams[4].Value = string.Empty;

                arrParams[5] = new OracleParameter("p_aloginname", OracleDbType.Varchar2, 500);
                arrParams[5].Direction = ParameterDirection.Output;
                arrParams[5].Value = DBNull.Value;

                arrParams[6] = new OracleParameter("p_alocal", OracleDbType.Varchar2, 500);
                arrParams[6].Direction = ParameterDirection.Output;
                arrParams[6].Value = DBNull.Value;

                arrParams[7] = new OracleParameter("p_ausertype", OracleDbType.Varchar2, 500);
                arrParams[7].Direction = ParameterDirection.Output;
                arrParams[7].Value = DBNull.Value;

                arrParams[8] = new OracleParameter("p_asubcstcencde", OracleDbType.Varchar2, 500);
                arrParams[8].Direction = ParameterDirection.Output;
                arrParams[8].Value = DBNull.Value;

                arrParams[9] = new OracleParameter("p_ausername", OracleDbType.Varchar2, 500);
                arrParams[9].Direction = ParameterDirection.Output;
                arrParams[9].Value = DBNull.Value;

                arrParams[10] = new OracleParameter("p_returncode", OracleDbType.Int16);
                arrParams[10].Direction = ParameterDirection.Output;
                arrParams[10].Value = DBNull.Value;

                arrParams[11] = new OracleParameter("p_returnMess", OracleDbType.Varchar2, 500);
                arrParams[11].Direction = ParameterDirection.Output;
                arrParams[11].Value = DBNull.Value;

                OracleHelper.ExecuteNonQuery(strConn, CommandType.StoredProcedure, "authen.spauth_internal_confirmcookie2", arrParams);

                loginname = arrParams[5].Value.ToString();
                local = arrParams[6].Value.ToString();
                ussertype = arrParams[7].Value.ToString();
                subcstcencde = arrParams[8].Value.ToString();
                username = arrParams[9].Value.ToString();
                iReturnCode = Convert.ToInt16(arrParams[10].Value.ToString());
                returnMess = arrParams[11].Value.ToString();
                CLogAuthentication.LogSQL(CLogAuthentication.GetDeepCaller(), currentPage.Cookies["cookieToken"] + "\r\n p_auseragent: " + currentPage.Headers["User-Agent"] + "\r\n p_abrowser: " + string.Empty + "\r\n p_aloginname: " + loginname + "\r\n p_returncode: " + iReturnCode + "\r\n p_returnMess: " + returnMess);
                return iReturnCode;

            }
            catch (Exception ex)
            {
                loginname = null;
                username = null;
                local = "HN";
                ussertype = "6";
                subcstcencde = "NONE";
                iReturnCode = -1;
                returnMess = ex.Message;
                CLogAuthentication.LogError(CLogAuthentication.GetDeepCaller(), ex.Message);
                return iReturnCode;
            }
        }
    }
}
