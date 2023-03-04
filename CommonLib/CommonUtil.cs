using CommonLib.Interfaces.Logger;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace CommonLib
{
    public static class CommonUtil
    {
        //private static readonly IConfiguration _configuration;
        //private static readonly IAppLogger _appLogger;
        //public static string TEMPLATE_DIR_DEST = _configuration["TEMPLATE_DIR_DEST"];
        //public static string TEMPLATE_URL_DEST = _configuration["TEMPLATE_URL_DEST"];
        //public static string TEMPLATE_URL_HOST = _configuration["TEMPLATE_URL_HOST"];
        //public static string CREDENTIALS_USERNAME = _configuration["CREDENTIALS_USERNAME"];
        //public static string CREDENTIALS_PASSWORD = _configuration["CREDENTIALS_PASSWORD"];
        //public static string CREDENTIALS_NETWORK = _configuration["CREDENTIALS_NETWORK"];


        /// <summary>
        /// 2017-12-08 10:56:41 ngocta2
        ///
        /// full logic:
        ///     + website host tai D:\website
        ///     + client upload file : "Báo cáo quý 3.pdf"
        ///     + server code save 2 file da upload vao D:\website\upload => upload la folder con cua folder cha D:\website
        ///     + vi du file da upload la => D:\website\upload\Bao_cao_quy_3_aabbcc.pdf
        ///     + folder dich can copy file vao la E:\file\(yyyy)\(mm)\(dd)\(filename) => (yyyy) va (mm) va (dd) la year, month, day cua thoi diem hien tai, (filename) la ten file da xu ly
        ///     + neu hom nay la ngay 8 thang 12 nam 2017 thi nghia la file copy file tu D:\website\upload vao E:\file\2017\12\08
        ///     + sau khi copy la E:\file\2017\12\08\Bao_cao_quy_3_aabbcc.pdf
        ///     + return string theo mau http://www.fpts.com.vn/FileStore2/File/(yyyy)/(mm)/(dd)/(filename). vi du: http://www.fpts.com.vn/FileStore2/File/2017/12/08/Bao_cao_quy_3_aabbcc.pdf
        ///
        /// yeu cau
        ///     + khai bao key TEMPLATE_DIR_DEST trong web.config chua gia tri "E:\file\(yyyy)\(mm)\(dd)\(filename)"
        ///     + khai bao key TEMPLATE_URL_DEST trong web.config chua gia tri "http://www.fpts.com.vn/FileStore2/File/(yyyy)/(mm)/(dd)/(filename)"
        /// </summary>
        /// <param name="strFullSoucePath">D:\website\upload\Bao_cao_quy_3_aabbcc.pdf</param>
        /// <returns>http://www.fpts.com.vn/FileStore2/File/2017/12/08/Bao_cao_quy_3_aabbcc.pdf</returns>
        public static string CopyFileUploaded(string strFullSoucePath, string SubPath, IConfiguration _configuration)
        {
            //_appLogger.InfoLogger.LogInfo(strFullSoucePath);
            try
            {
                //SubPath  : Reports\PDF\test_af2f73.pdf  || Reports\PDF\HoaDonHuy\test_af2f73.pdf
                //gan gia tri cho path
                string strFullDestURL, strPath;
                string strSubFolder;            //Reports\PDF\HoaDonHuy
                string[] arrstrTemplatePath = _configuration["TEMPLATE_DIR_DEST"].Split('|');          //C:\file\(yyyy)\(mm)\(dd)\|D:\file\(yyyy)\(mm)\(dd
                //string[] arrstrTemplatePath = TEMPLATE_DIR_DEST.Split('|');          //C:\file\(yyyy)\(mm)\(dd)\|D:\file\(yyyy)\(mm)\(dd)\
                string strFileName = System.IO.Path.GetFileName(strFullSoucePath);     //00000008_15.pdf                                                                               //   CLog.LogEx("DEBUG.js", "Common.TEMPLATE_DIR_DEST = " + Common.TEMPLATE_DIR_DEST);
                strSubFolder = SubPath.Replace(strFileName, "");                                                                                                                                         // copy 1 file vao X folder dich
                foreach (string strTemplatePath in arrstrTemplatePath)                          // C:\file\(yyyy)\(mm)\(dd)\
                {
                    if (strTemplatePath != "")                                                      // blank string thi ko copy nua vi du trong config la "C:\file\|"
                    {
                        //2018-04-03 17:29:03 ngocta2 fix error : Message = Access to the path '\\172.16.0.18\e$\FileStore2\File\2018\04\03\' is denied.
                        NetworkCredential netCredential = new System.Net.NetworkCredential(_configuration["CREDENTIALS_USERNAME"], _configuration["CREDENTIALS_PASSWORD"]);
                        using (new CNetworkConnection(_configuration["CREDENTIALS_NETWORK"], netCredential)) // webserver host tai 51 , file up len 51, tu 51 copy sang 18 phai cung cap NetworkCredential
                        {
                            strPath = strTemplatePath + strSubFolder;                                                   // C:\file\2017\12\16\  + Reports\PDF\HoaDonHuy
                            //.Replace("(yyyy)", datDatePublish.Year.ToString())
                            //.Replace("(mm)", CBase.Right("00" + datDatePublish.Month.ToString(), 2))
                            //.Replace("(dd)", CBase.Right("00" + datDatePublish.Day.ToString(), 2));
                            // CLog.LogEx("DEBUG.js", "strPath = " + strPath);
                            //// Use Path class to manipulate file and directory paths.
                            string strFullDestPath = Path.Combine(strPath, strFileName);                // C:\file\2017\12\16\test_af2f73.pdf
                                                                                                        //    CLog.LogEx("DEBUG.js", "strFullDestPath = " + strFullDestPath);
                            //_appLogger.InfoLogger.LogInfo(strFullDestPath);                                                                         // To copy a folder's contents to a new location:
                                                                                                                                   // Create a new target folder, if necessary.
                            if (!Directory.Exists(strPath))
                                Directory.CreateDirectory(strPath);
                            //     CLog.LogEx("DEBUG.js", "strFullSoucePath => strFullDestPath = " + strFullSoucePath + " => " + strFullDestPath);
                            // To copy a file to another location and
                            // overwrite the destination file if it already exists.
                            File.Copy(strFullSoucePath, strFullDestPath, true);                         // H:\Project\TP_ENT\ENT\WebApp\Uploads\test_af2f73.pdf => C:\file\2017\12\16\test_af2f73.pdf
                        }
                    }
                }

                // tao 1 URL duy nhat
                strFullDestURL = _configuration["TEMPLATE_URL_DEST_SERVER"]
                    //.Replace("(yyyy)", datDatePublish.Year.ToString())
                    //.Replace("(mm)", CBase.Right("00" + datDatePublish.Month.ToString(), 2))
                    //.Replace("(dd)", CBase.Right("00" + datDatePublish.Day.ToString(), 2))
                    .Replace("(filename)", SubPath);
               // _appLogger.InfoLogger.LogInfo(strFullDestURL);
                return strFullDestURL;                                                          // http://www.fpts.com.vn/FileStore2/File/2017/12/16/test_af2f73.pdf
            }
            catch (Exception ex)
            {
                // CLog.LogError(CBase.GetDeepCaller(), CBase.GetDetailError(ex));
                //_appLogger.ErrorLogger.LogError(ex);   // Ghi thông tin ra file
                return null;
            }
        }
        public static string ReturnLanguge(string URL)
        {
            if (URL.Contains("culture=en-US"))
                return "en";
            else
                return "vn";
        }
    }
}
