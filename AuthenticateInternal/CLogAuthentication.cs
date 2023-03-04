using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AuthenticateInternal
{
    public class CLogAuthentication
    {
        // lock thread
        public static string LogPath = "\\LoginLogs\\ERROR\\(yyyy)\\(MM)\\(dd)\\";
        public static string BASE_LOG_PATH_SQL = "\\LoginLogs\\SQL\\(yyyy)\\(MM)\\(dd)\\";
        internal static string NO_LOG_SET = "0";
        internal static string SINGLE_THREAD = "0";

        internal static string TEMPLATE_LOG_DATA = "(Time)|(Title)|(Detail)";
        internal static string LOG_EXT = ".txt";
        internal static string LOG_EXT_SQL = ".sql";

        internal const string CHAR_CRLF = "\r\n";
        internal const string CHAR_TAB = "\t";
        internal const string BASE_APP_NAME = "IsAuthenticate";
        internal const string BASE_LOG_MULTI_THREAD = "0";
        static private readonly object m_objLocker = new object();

        /// <summary>
        /// log thong tin ERROR vao C:\
        ///     <!-- C:\Log\MyApp\ERROR\2014\05\17\1gzjxwqq.p0b  |  C:\Log\MyApp\Error\2014-05-17.txt -->
        ///     <add key="BASE_LOG_PATH_ERROR"    value="C:\\Log\\MyTestApp\\ERROR\\(yyyy)\\(MM)\\(dd)\\" />
        /// </summary>
        /// <param name="strFunctionName"></param>
        /// <param name="strErrorMesssage"></param>
        public static void LogError(string strFunctionName, string strErrorMesssage)
        {
            System.Diagnostics.Debug.Print("===========================" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "========================\r\nLogError=>" + strErrorMesssage);
            if ("1" == NO_LOG_SET) return;
            //------------------------------------------------------------------------------------
            //Log(BASE_LOG_PATH_ERROR, strFunctionName, strErrorMesssage);
            LogModule2FileName(LogPath, strFunctionName, strErrorMesssage);
        }
        public static string GetDeepCaller()
        {
            string strCallerName = "";
            for (int i = 3; i >= 2; i--)
                strCallerName += GetCaller(i) + "=>";

            //returns a composite of the namespace, class and method name.
            return strCallerName;
        }
        public static string GetCaller(int level = 2)
        {
            var m = new StackTrace().GetFrame(level).GetMethod();

            if (m.DeclaringType == null) return ""; //9:33 AM 6/18/2014 Exception Details: System.NullReferenceException: Object reference not set to an instance of an object.

            // .Name is the name only, .FullName includes the namespace
            var className = m.DeclaringType.FullName;

            //the method/function name you are looking for.
            var methodName = m.Name;

            //returns a composite of the namespace, class and method name.
            return className + "->" + methodName;
        }
        /// <summary>
        /// log thong tin SQL vao C:\
        /// </summary>
        /// <param name="strVarName">v1</param>
        /// <param name="strSQLscript">select * from table</param>
        public static void LogSQL(string strVarName, string strSQLscript)
        {
            if (LogPath == NO_LOG_SET) return;
            //------------------------------------------------------------------------------------
            Log(BASE_LOG_PATH_SQL, strVarName, strSQLscript);
        }

        public static void LogSQL2(string strVarName, string strSQLscript, string strAccountNo)
        {
            if (LogPath == NO_LOG_SET) return;
            //------------------------------------------------------------------------------------
            Log2(LogPath, strVarName, strSQLscript, strAccountNo, LOG_EXT_SQL);
        }

        /// <summary>
        /// log thong tin khac vao C:\
        /// </summary>
        /// <param name="strTitle"></param>
        /// <param name="strDetail"></param>
        public static void LogText(string strTitle, string strDetail)
        {
            Log(LogPath, strTitle, strDetail);
        }
        public static string Right(string value, int length)
        {
            return value.Substring(value.Length - length);
        }


        /// <summary>
        /// ghi log file
        /// </summary>
        /// <param name="strPathTemplate"></param>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        private static void Log(string strPathTemplate, string str1, string str2)
        {
            try
            {
                string strPath = strPathTemplate.Replace("(AppName)", BASE_APP_NAME);
                string strLine = "";

                if (BASE_LOG_MULTI_THREAD != SINGLE_THREAD)
                {
                    // multi - ko duoc write chung 1 file - error "file locked by other process"
                    strPath = strPath
                    .Replace("(yyyy)", DateTime.Now.Year.ToString())
                    .Replace("(MM)", Right("00" + DateTime.Now.Month.ToString(), 2))
                    .Replace("(dd)", Right("00" + DateTime.Now.Day.ToString(), 2));
                    strPath += Path.GetRandomFileName();
                }
                else
                {
                    // single
                    strPath = strPath.Replace("(yyyy)\\(MM)\\(dd)\\", "(yyyy)-(MM)-(dd)");
                    strPath = strPath
                    .Replace("(yyyy)", DateTime.Now.Year.ToString())
                    .Replace("(MM)", Right("00" + DateTime.Now.Month.ToString(), 2))
                    .Replace("(dd)", Right("00" + DateTime.Now.Day.ToString(), 2));
                    strPath += LOG_EXT;
                }


                // data
                strLine = TEMPLATE_LOG_DATA
                    .Replace("(Time)", DateTime.Now.ToString("HH:mm:ss.") + DateTime.Now.Millisecond.ToString("000"))
                    .Replace("(Title)", str1)
                    .Replace("(Detail)", str2);

                // check folder
                CheckDirectory(strPath);

                // write
                StreamWriter fs = new StreamWriter(strPath, true); // append
                fs.WriteLine(strLine);
                fs.Close();

            }
            catch (Exception)
            {
                // do nothing
            }
            finally
            {

            }
        }

        /// <summary>
        /// 2018-08-27 10:50:47 ngocta2
        /// Log2() chi tiet hon Log()
        /// </summary>
        /// <param name="strPathTemplate">D:\WebLog\(AppName)\SQL\(yyyy)\(MM)\(dd)\</param>
        /// <param name="strTitle">TransferHist</param>
        /// <param name="strDetail">SQL script...</param>
        /// <param name="strAccountNo">058C108101 | Api_Transfer_Next_Working_Day</param>
        /// <param name="strExt">.sql</param>
        private static void Log2(string strPathTemplate, string strTitle, string strDetail, string strAccountNo, string strExt)
        {
            try
            {
                string strPath = strPathTemplate.Replace("(AppName)", BASE_APP_NAME);
                string strLine = "";
                string strFileName = strAccountNo;

                // full path = strPath + strFileName + "." + strExt

                if (BASE_LOG_MULTI_THREAD != SINGLE_THREAD)
                {
                    // multi - ko duoc write chung 1 file - error "file locked by other process"
                    strPath = strPath
                    .Replace("(yyyy)", DateTime.Now.Year.ToString())
                    .Replace("(MM)", Right("00" + DateTime.Now.Month.ToString(), 2))
                    .Replace("(dd)", Right("00" + DateTime.Now.Day.ToString(), 2));
                    strPath += Path.GetRandomFileName();
                }
                else
                {
                    // single
                    //strPath = strPath.Replace("(yyyy)\\(MM)\\(dd)\\", "(yyyy)-(MM)-(dd)");
                    strPath = strPath
                    .Replace("(yyyy)", DateTime.Now.Year.ToString())
                    .Replace("(MM)", Right("00" + DateTime.Now.Month.ToString(), 2))
                    .Replace("(dd)", Right("00" + DateTime.Now.Day.ToString(), 2));
                    strPath += strFileName + strExt; // D:\WebLog\DataService\SQL\2018\08\27\058C108101.sql | D:\WebLog\DataService\SQL\2018\08\27\Api_Transfer_Next_Working_Day.sql
                }


                // data
                strLine = TEMPLATE_LOG_DATA
                    .Replace("(Time)", DateTime.Now.ToString("HH:mm:ss.") + DateTime.Now.Millisecond.ToString("000"))
                    .Replace("(Title)", strTitle)
                    .Replace("(Detail)", strDetail);

                // check folder
                CheckDirectory(strPath);

                // write
                StreamWriter fs = new StreamWriter(strPath, true); // append
                fs.WriteLine(strLine);
                fs.Close();

            }
            catch (Exception)
            {
                // do nothing
            }
            finally
            {

            }
        }


        /// <summary>
        /// log cao cap, chi tiet hon: thuong dung de log cac du lieu big, phai chia nho file nhu log SQL (single file = 800MB)
        /// 1 file chi ghi 1 lan, ko ghi nhieu lan (nhieu line) trong 1 file
        /// <add key="BASE_LOG_PATH_EX"       value="C:\Log\(AppName)\(Type)\(yyyy)\(MM)\(dd)\(FileName).txt" />
        /// <add key="BASE_TEMPLATE_LOG_EX_FILENAME"      value="(thread)__(type)__(hh)_(mm)_(ss)__(random).txt" />
        /// </summary>
        /// <param name="strFolder">SQL/ERROR/TEXT</param>
        /// <param name="strFileNameEx">1_security_14_33_59_afj43laf</param>
        /// <param name="strBody">SQL script</param>
        public static string LogEx(string strFileNameEx, string strBody)
        {
            try
            {
                lock (m_objLocker)
                {
                    // C:\Log\5G_QuoteFeeder_HOSE\SQL\2014\12\04\1__security__14_34_59__afj43laf.txt
                    string strPath = LogPath
                        .Replace("(AppName)", BASE_APP_NAME)
                        .Replace("(yyyy)", DateTime.Now.Year.ToString())
                        .Replace("(MM)", Right("00" + DateTime.Now.Month.ToString(), 2))
                        .Replace("(dd)", Right("00" + DateTime.Now.Day.ToString(), 2))
                        .Replace("(FileName)", strFileNameEx)
                        ;

                    // check folder
                    CheckDirectory(strPath);

                    // neu noi dung ko co gi thi chi lay path (ko write)
                    if (strBody == "")
                        return strPath;

                    // write
                    StreamWriter fs = new StreamWriter(strPath, true); // append
                    fs.WriteLine(DateTime.Now.ToString("HH:mm:ss.") + DateTime.Now.Millisecond.ToString("000") + " => " + strBody);
                    fs.Close();

                    // 
                    return strPath;
                }
            }
            catch (Exception)
            {
                // do nothing
                return "";
            }
            finally
            {

            }
        }

        /// <summary>
        /// 2017-11-02 15:54:57 ngocta2
        /// ghi data vao file de co luc load lai, ko ghi cac moc time (de debug) vao trong noi dung file
        /// su dung chung folder LogEx
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="strData"></param>
        /// <returns></returns>
        public static string LogDataSave(string strFileName, string strData)
        {
            try
            {
                lock (m_objLocker)
                {
                    // D:\LOG\StockHNX2\LogEx\2017\11\02\BASKET_HNX.js
                    string strPath = LogPath
                        .Replace("(AppName)", BASE_APP_NAME)
                        .Replace("(yyyy)", DateTime.Now.Year.ToString())
                        .Replace("(MM)", Right("00" + DateTime.Now.Month.ToString(), 2))
                        .Replace("(dd)", Right("00" + DateTime.Now.Day.ToString(), 2))
                        .Replace("(FileName)", strFileName)
                        ;

                    // check folder
                    CheckDirectory(strPath);

                    // neu noi dung ko co gi thi chi lay path (ko write)
                    if (strData == "")
                        return strPath;

                    // neu file ton tai thi xoa truoc khi write
                    if (File.Exists(strPath))
                        File.Delete(strPath);

                    // write
                    StreamWriter fs = new StreamWriter(strPath, true); // append
                    fs.WriteLine(strData);
                    fs.Close();

                    // return path
                    return strPath;
                }
            }
            catch (Exception)
            {
                // do nothing
                return "";
            }
            finally
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public static string LogDataLoad(string strFileName)
        {
            try
            {
                lock (m_objLocker)
                {
                    // D:\LOG\StockHNX2\LogEx\2017\11\02\BASKET_HNX.js
                    string strPath = LogPath
                        .Replace("(AppName)", BASE_APP_NAME)
                        .Replace("(yyyy)", DateTime.Now.Year.ToString())
                        .Replace("(MM)", Right("00" + DateTime.Now.Month.ToString(), 2))
                        .Replace("(dd)", Right("00" + DateTime.Now.Day.ToString(), 2))
                        .Replace("(FileName)", strFileName)
                        ;

                    // check folder
                    CheckDirectory(strPath);

                    // read
                    string strData = "";
                    using (StreamReader sr = new StreamReader(strPath))
                    {
                        strData = sr.ReadToEnd();
                    }

                    return strData;
                }
            }
            catch (Exception)
            {
                // do nothing
                return "";
            }
            finally
            {

            }
        }

        /// <summary>
        /// tuong tu LogEx
        /// </summary>
        /// <param name="strFileNameEx"></param>
        /// <param name="strBody"></param>
        /// <param name="arrHeader"></param>
        public static void LogCSV(string strFileNameEx, string strBody, string[] arrHeader)
        {
            try
            {
                // C:\Log\5G_QuoteFeeder_HOSE\SQL\2014\12\04\1__security__14_34_59__afj43laf.txt
                string strPath = LogPath
                    .Replace("(AppName)", BASE_APP_NAME)
                    .Replace("(yyyy)", DateTime.Now.Year.ToString())
                    .Replace("(MM)", Right("00" + DateTime.Now.Month.ToString(), 2))
                    .Replace("(dd)", Right("00" + DateTime.Now.Day.ToString(), 2))
                    .Replace("(FileName)", strFileNameEx)
                    ;

                // check folder
                CheckDirectory(strPath);

                // tao row header neu file chua ton tai
                string strHeader = "";
                if (!File.Exists(strPath))
                {
                    strHeader = "Time";
                    foreach (string str in arrHeader)
                        strHeader += "," + str;
                }

                // write
                StreamWriter fs = new StreamWriter(strPath, true); // append
                if (strHeader != "")
                    fs.WriteLine(strHeader);
                fs.WriteLine(DateTime.Now.ToString("HH:mm:ss.") + DateTime.Now.Millisecond.ToString("000") + "," + strBody);
                fs.Close();

            }
            catch (Exception)
            {
                // do nothing
            }
            finally
            {

            }
        }

        //<add key="BASE_TEMPLATE_LOG_EX_FILENAME"      value="(thread)__(type)__(hh)_(mm)_(ss)__(random).(ext)" />
        //"1__security__14_33_55__abcd7890"
        //CLog.LogEx("TEST", "1__security__14_33_55__abcd7890", "Tạm ứng cổ tức năm đợt 2 năm 2014 bằng tiền mặt, 2.000đồng/cổ phiếu");
        public static string GetLogExFileName(int intThread, string strType, string strExt)
        {
            try
            {
                string strFileName = LogPath;
                strFileName = strFileName
                    .Replace("(thread)", intThread.ToString())
                    .Replace("(type)", strType)
                    .Replace("(hh)", Right("00" + DateTime.Now.Hour.ToString(), 2))
                    .Replace("(mm)", Right("00" + DateTime.Now.Minute.ToString(), 2))
                    .Replace("(ss)", Right("00" + DateTime.Now.Second.ToString(), 2))
                    .Replace("(random)", System.IO.Path.GetRandomFileName())
                    .Replace("(ext)", strExt);
                return strFileName;
            }
            catch (Exception ex)
            {
                CLogAuthentication.LogError(GetDeepCaller(), GetDetailError(ex));
                return "";
            }
        }
        public static string GetDetailError(Exception ex)
        {
            string strDetailError = "" +
                "\r\nSource\t\t= " + ex.Source +
                "\r\nTargetSite\t= " + ex.TargetSite +
                "\r\nMessage\t\t= " + ex.Message +
                "\r\nStackTrace\t= " + ex.StackTrace;
            return strDetailError;
        }

        /// <summary>
        /// chua co dir thi create dir
        /// chu y: phai co quyen write
        /// </summary>
        /// <param name="strFilePath"> C:\Log\MyApp\Error\2014-05-17.txt </param>
        public static void CheckDirectory(string strFilePath)
        {
            try
            {
                FileInfo f = new FileInfo(strFilePath);
                if (!f.Directory.Exists)
                {
                    f.Directory.Create();
                }
            }
            catch (Exception)
            {
                // do nothing
            }
        }

        /// <summary>
        /// 2015-05-04 14:32:33 ngocta2
        /// </summary>
        /// <param name="strFullPathTemplate"></param>
        /// <param name="strBody"></param>
        /// <param name="blnIsAppend"></param>
        public static void WriteFile(string strFullPathTemplate, string strBody, bool blnIsAppend)
        {
            try
            {
                // C:\Log\5G_QuoteFeeder_HOSE\SQL\2014_12_04.txt
                string strPath = strFullPathTemplate
                    .Replace("(AppName)", BASE_APP_NAME)
                    .Replace("(yyyy)", DateTime.Now.Year.ToString())
                    .Replace("(MM)", Right("00" + DateTime.Now.Month.ToString(), 2))
                    .Replace("(dd)", Right("00" + DateTime.Now.Day.ToString(), 2))
                    ;

                // check folder
                CheckDirectory(strPath);

                // write
                StreamWriter fs = new StreamWriter(strPath, blnIsAppend); // append
                fs.WriteLine(strBody);
                fs.Close();

            }
            catch (Exception)
            {
                // do nothing
            }
            finally
            {

            }
        }

        /// <summary>
        /// 2015-05-04 14:41:33 ngocta2
        /// chi can read 1 dong
        /// </summary>
        /// <param name="strFullPathTemplate"></param>
        /// <returns></returns>
        public static string ReadFile(string strFullPathTemplate)
        {
            try
            {
                // C:\Log\5G_QuoteFeeder_HOSE\SQL\2014_12_04.txt
                string strPath = strFullPathTemplate
                    .Replace("(AppName)", BASE_APP_NAME)
                    .Replace("(yyyy)", DateTime.Now.Year.ToString())
                    .Replace("(MM)", Right("00" + DateTime.Now.Month.ToString(), 2))
                    .Replace("(dd)", Right("00" + DateTime.Now.Day.ToString(), 2))
                    ;

                // check folder
                CheckDirectory(strPath);

                // read
                string strBody = "";
                using (StreamReader sr = new StreamReader(strPath))
                {
                    strBody = sr.ReadToEnd();
                }

                return strBody;
            }
            catch (Exception)
            {
                // do nothing
                return "";
            }
            finally
            {

            }
        }


        /// <summary>
        /// 2015-08-27 09:12:23 ngocta2
        /// tuong tu log binh thuong nhung ghi luon ten Module vao FileName (de random filename kho nhan ra file can tim)
        /// chi su dung cho log error
        /// C:\Log\StockQuote\ERROR\2015\08\27\bwd0pgfa.jma (09:03:23.996|QuoteBaseLib.BLL.CComparer`1->CompareDataD=>QuoteBaseLib.BLL.CReaderBase`1->FindOldRecord)
        /// C:\Log\StockQuote\ERROR\2015\08\27\090323996__QuoteBaseLib.BLL.CComparer_CompareDataD_QuoteBaseLib.BLL.CReaderBase_FindOldRecord__bwd0pgfa.jma
        /// </summary>
        /// <param name="strPathTemplate"></param>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        private static void LogModule2FileName(string strPathTemplate, string str1, string str2)
        {
            try
            {
                string strPath = strPathTemplate.Replace("(AppName)", BASE_APP_NAME);
                string strLine = "";

                if (BASE_LOG_MULTI_THREAD != SINGLE_THREAD)
                {
                    // multi - ko duoc write chung 1 file - error "file locked by other process"
                    strPath = strPath
                    .Replace("(yyyy)", DateTime.Now.Year.ToString())
                    .Replace("(MM)", Right("00" + DateTime.Now.Month.ToString(), 2))
                    .Replace("(dd)", Right("00" + DateTime.Now.Day.ToString(), 2));
                    //strPath += Path.GetRandomFileName();
                }
                else
                {
                    // single
                    //strPath = strPath.Replace("(yyyy)\\(MM)\\(dd)\\", "(yyyy)-(MM)-(dd)");  // 2016-06-23 10:41:53 ngocta2 bo dong nay
                    strPath = strPath
                    .Replace("(yyyy)", DateTime.Now.Year.ToString())
                    .Replace("(MM)", Right("00" + DateTime.Now.Month.ToString(), 2))
                    .Replace("(dd)", Right("00" + DateTime.Now.Day.ToString(), 2));
                    //strPath += LOG_EXT;
                }

                // xu ly them path
                str1 = str1.Replace("`1", "");
                str1 = str1.Replace("=>", "_");
                str1 = str1.Replace("->", "_");
                strPath += "\\";
                //strPath += DateTime.Now.ToString("HHmmss");
                //strPath += "__";
                strPath += str1;
                //strPath += "__";
                //strPath += Path.GetRandomFileName();
                strPath += ".log";
                strPath = FixPath(strPath);

                // data
                strLine = TEMPLATE_LOG_DATA
                    .Replace("(Time)", DateTime.Now.ToString("HH:mm:ss.") + DateTime.Now.Millisecond.ToString("000"))
                    .Replace("(Title)", str1)
                    .Replace("(Detail)", str2);

                // check folder
                CheckDirectory(strPath);

                // write
                StreamWriter fs = new StreamWriter(strPath, true); // append
                fs.WriteLine(strLine);
                fs.Close();

            }
            catch (Exception)
            {
                // do nothing
            }
            finally
            {

            }
        }


        /// <summary>
        /// bo ky tu ko hop le
        /// C:\Log\Monitor\ERROR\2016-04-08\103843__System.Threading.QueueUserWorkItemCallback_WaitCallback_Context_Monitor.Hubs.CMonitor5G_<InitPubSubData>b__0___xgbmys1i.5tx
        /// Illegal characters in path.
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static string FixPath(string strPath)
        {
            try
            {
                strPath = strPath.Replace("<", "(");
                strPath = strPath.Replace(">", ")");
                strPath = strPath.Replace("`", "_");

                return strPath;
            }
            catch (Exception ex)
            {
                CLogAuthentication.LogError(GetDeepCaller(), GetDetailError(ex));
                return strPath;
            }
        }
    }
}
