using CommonLib.Interfaces;
using CoreLib.Entities;
using CoreLib.SharedKernel;
using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Text.RegularExpressions;

namespace CommonLib.Implementations
{
    public class CCommon : ICommon
    {
        public string HashPassword(string password, string salt)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var saltBytes = Encoding.UTF8.GetBytes(salt);

            var argon2 = new Argon2i(passwordBytes)
            {
                DegreeOfParallelism = 16,
                MemorySize = 8192,
                Iterations = 40,
                Salt = saltBytes,
            };

            var hash = argon2.GetBytes(32);

            // Convert the byte array to hexadecimal string
            var stringBuilder = new StringBuilder();
            foreach (var t in hash)
            {
                stringBuilder.Append(t.ToString("X2"));
            }

            return stringBuilder.ToString();
        }

        public bool ValidatePassword(string password, string salt, string correctHash)
        {
            return this.HashPassword(password, salt).Equals(correctHash);
        }

        public string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }

        public string GetDeepCaller()
        {
            string strCallerName = string.Empty;
            for (int i = 3; i >= 2; i--)
            {
                strCallerName += this.GetCaller(i) + "=>";
            }

            // returns a composite of the namespace, class and method name.
            return strCallerName;
        }

        /// <summary>
        /// get caller function name.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public string GetCaller(int level = 2)
        {
            var m = new StackTrace().GetFrame(level).GetMethod();

            if (m.DeclaringType == null)
            {
                return string.Empty; // 9:33 AM 6/18/2014 Exception Details: System.NullReferenceException: Object reference not set to an instance of an object.
            }

            // .Name is the name only, .FullName includes the namespace
            var className = m.DeclaringType.FullName;

            // the method/function name you are looking for.
            var methodName = m.Name;

            // returns a composite of the namespace, class and method name.
            return className + "->" + methodName;
        }

        public string FormatDate(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                return null;
            }
            else
            {
                string[] s = new string[3];
                for (int i = 0; i < 3; i++)
                {
                    s[i] = string.Empty;
                }

                string str = string.Empty;
                int n = 0;
                for (int i = 0; i < date.Length; i++)
                {
                    if (date[i] != '/')
                    {
                        s[n] += date[i];
                    }
                    else
                    {
                        n++;
                    }
                }

                str = s[2] + '/' + s[1] + '/' + s[0];
                return str;
            }
        }

        public DateTime ConvertStringFromNewsToDateTime(string strDateTime)
        {
            DateTimeFormatInfo usDtfi = new CultureInfo("vi-VN", false).DateTimeFormat;
            return Convert.ToDateTime(strDateTime, usDtfi);
        }

        public string ToQueryString(object obj)
        {
            var result = new List<string>();
            var props = obj.GetType().GetProperties().Where(p => p.GetValue(obj, null) != null);
            foreach (var p in props)
            {
                var value = p.GetValue(obj, null);
                var enumerable = value as ICollection;
                if (enumerable != null)
                {
                    result.AddRange(from object v in enumerable select string.Format("{0}={1}", p.Name, HttpUtility.UrlEncode(v.ToString())));
                }
                else
                {
                    result.Add(string.Format("{0}={1}", p.Name, HttpUtility.UrlEncode(value.ToString())));
                }
            }

            return string.Join("&", result.ToArray());
        }

        public async Task<CResponseMessage> SaveExcelFile(IFormFile file, IHostingEnvironment hostingEnvironment, IConfiguration config)
        {
            var excelFileExtension = ".xlsx";

            var response = new CResponseMessage();
            var validFileFomat = new[] { excelFileExtension };
            response.Code = -1;

            try
            {
                var fileName = file.FileName;

                var configstringfolder = config["ExcelFilePath"];
                configstringfolder = configstringfolder.Replace(@"/", @"\");
                var filePath = hostingEnvironment.ContentRootPath;
                var name = fileName.Remove(file.FileName.Length - 5);

                var fileExtention = file.FileName.Substring(file.FileName.Length - 5, 5);
                if (!validFileFomat.Contains(fileExtention.ToLower()))
                {
                    response.Message = "NOT_A_VALID_FORMAT";
                    return response;
                }

                // check và đặt tên file để không trùng tên cũ
                var i = 1;
                var filePathsave = Path.Combine(filePath, configstringfolder, name + i + excelFileExtension);
                while (File.Exists(filePathsave))
                {
                    i++;
                    filePathsave = Path.Combine(filePath, configstringfolder, name + i + excelFileExtension);
                }

                using (var stream = new FileStream(filePathsave, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                response.Code = 0;
                response.Message = "UPLOAD_FILE_SUCCESS";
                response.Data = filePathsave;
            }
            catch (Exception exception)
            {
                response.Code = -1;
                response.Message = "UPLOAD_FILE_ERROR";
            }

            return response;
        }
        public async Task<CResponseMessage> CopyFile(IFormFile file, string targetFolder, IHostingEnvironment hostingEnvironment, IConfiguration _configuration)
        {
            var response = new CResponseMessage { Code = -1 };

            try
            {
                targetFolder = targetFolder.Replace(@"/", @"\");

                var filePath = hostingEnvironment.ContentRootPath;

                // check và đặt tên file để không trùng tên cũ
                var i = 1;
                var fileName = $"File_{i}_{file.FileName.Replace(" ", "")}";
                var filePathsave = Path.Combine(filePath, targetFolder, fileName);
                while (File.Exists(filePathsave))
                {
                    i++;
                    fileName = $"File_{i}_{file.FileName}";
                    filePathsave = Path.Combine(filePath, targetFolder, fileName);
                }

                using (var stream = new FileStream(filePathsave, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                //var filesPath = hostingEnvironment.ContentRootPath + @"\" + targetFolder + @"\" + file.FileName;                
                var SubPath = targetFolder + @"\" + fileName;

                // cắt UploadOrgModelPath lấy ra thư mục ImgOrgModel;
                /*string inputString = targetFolder; ;
                string[] parts = Regex.Split(inputString, @"\\");
                var shortenedFolder = parts[1];
                var SubPath = shortenedFolder + @"\" + fileName;*/

                if (File.Exists(filePathsave))
                {
                    //Copy file từ server chứa code sang server moi truong
                    CommonUtil.CopyFileUploaded(filePathsave, SubPath, _configuration);

                    //Xóa file vừa copy
                    File.Delete(filePathsave);
                }

                response.Code = 0;
                 response.Message = "UPLOAD_FILE_SUCCESS";
                 response.Data = Path.Combine(targetFolder, fileName);
             }
             catch (Exception e)
             {
                 response.Code = -1;
                 response.Message = e.Message;
             }

             return response;
        }

        public async Task<CResponseMessage> CopyFileNoDelete(IFormFile file, string targetFolder, IHostingEnvironment hostingEnvironment, IConfiguration _configuration)
        {
            var response = new CResponseMessage { Code = -1 };

            try
            {
                targetFolder = targetFolder.Replace(@"/", @"\");

                var filePath = hostingEnvironment.ContentRootPath;

                // check và đặt tên file để không trùng tên cũ
                var i = 1;
                var fileName = $"{file.FileName.Replace(" ", "")}";
                var filePathsave = Path.Combine(filePath, targetFolder, fileName);
                while (File.Exists(filePathsave))
                {
                    i++;
                    fileName = $"{file.FileName}";
                    filePathsave = Path.Combine(filePath, targetFolder, fileName);
                }

                using (var stream = new FileStream(filePathsave, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                //var filesPath = hostingEnvironment.ContentRootPath + @"\" + targetFolder + @"\" + file.FileName;                
                var SubPath = targetFolder + @"\" + fileName;

                // cắt UploadOrgModelPath lấy ra thư mục ImgOrgModel;
                /*string inputString = targetFolder; ;
                string[] parts = Regex.Split(inputString, @"\\");
                var shortenedFolder = parts[1];
                var SubPath = shortenedFolder + @"\" + fileName;*/

                if (File.Exists(filePathsave))
                {
                    //Copy file từ server chứa code sang server moi truong
                    CommonUtil.CopyFileUploaded(filePathsave, SubPath, _configuration);

                    //Xóa file vừa copy
                  
                }

                response.Code = 0;
                response.Message = "UPLOAD_FILE_SUCCESS";
                response.Data = Path.Combine(targetFolder, fileName);
            }
            catch (Exception e)
            {
                response.Code = -1;
                response.Message = e.Message;
            }

            return response;
        }
        public async Task<CResponseMessage> SaveFile(IFormFile file, string targetFolder, IHostingEnvironment hostingEnvironment)
        {
            var response = new CResponseMessage { Code = -1 };

            try
            {
                targetFolder = targetFolder.Replace(@"/", @"\");

                var filePath = hostingEnvironment.ContentRootPath;

                // check và đặt tên file để không trùng tên cũ
                var i = 1;
                var fileName = $"File_{i}_{file.FileName.Replace(" ", "")}";
                var filePathsave = Path.Combine(filePath, targetFolder, fileName);
                while (File.Exists(filePathsave))
                {
                    i++;
                    fileName = $"File_{i}_{file.FileName}";
                    filePathsave = Path.Combine(filePath, targetFolder, fileName);
                }

                using (var stream = new FileStream(filePathsave, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                response.Code = 0;
                response.Message = "UPLOAD_FILE_SUCCESS";
                response.Data = Path.Combine(targetFolder, fileName);
            }
            catch (Exception e)
            {
                response.Code = -1;
                response.Message = e.Message;
            }

            return response;
        }       

        /*public async Task<CResponseMessage> SaveBieuMau(IFormFile file, string baseFolder, string basePath)
        {
            var response = new CResponseMessage { Code = -1 };
            try
            {

                //var filePath = hostingEnvironment.ContentRootPath;
                String sDate = DateTime.Now.ToString();
                DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
                String dy = datevalue.Day.ToString();
                String mn = datevalue.Month.ToString();
                String yy = datevalue.Year.ToString();
                if (Convert.ToInt32(dy) <= 9) dy = "0" + dy;
                if (Convert.ToInt32(mn) <= 9) mn = "0" + mn;
                
                baseFolder = baseFolder.Replace(@"/", @"\");
                var folder = Path.Combine(basePath,baseFolder, Path.Combine(yy, Path.Combine(mn, dy)));

                *//*var filePath = basePath + @"Upload\"+ baseFolder + @"\" + file.FileName;
                var SubPath = @"Upload\" + baseFolder + @"\" + file.FileName;*/
                /*if (_configuration["MoiTruong"] == "1")*//*

                    //Copy file từ server chứa code sang server moi truong
                   *//* CommonUtil.CopyFileUploaded(filePath, SubPath);

                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }*//*


                // check và đặt tên file để không trùng tên cũ
                var i = 1;
                var fileName = $"File_{i}_{file.FileName.Replace(" ", "")}";
                var filePathsave = Path.Combine(basePath, folder, fileName);
                while (File.Exists(filePathsave))
                {
                    i++;
                    fileName = $"File_{i}_{file.FileName}";
                    filePathsave = Path.Combine(basePath, folder, fileName);
                }

                using (var stream = new FileStream(filePathsave, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                response.Code = 0;
                response.Message = "UPLOAD_FILE_SUCCESS";
                response.Data = filePathsave;
            }
            catch (Exception e)
            {
                response.Code = -1;
                response.Message = e.Message;
            }
            return response;
        }*/

        public async Task<bool> CheckGCaptcha(string secretKey, string captchaResponse)
        {
            var verifyHost = $@"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={captchaResponse}";
            try
            {
                var response = await HttpRequestFactory.Get(verifyHost);
                response.EnsureSuccessStatusCode();
                var result = JObject.Parse(await response.Content.ReadAsStringAsync());
                return (bool)result.SelectToken("success");
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public CResponseMessage CheckValidateExcelFile(IFormFile file, IHostingEnvironment _hostingEnvironment, IConfiguration _config)
        {
            var excelFileExtension = ".xlsx";
            CResponseMessage _response = new CResponseMessage();
            _response.Code = -1;
            string[] validImageFomat = new string[] { excelFileExtension };

            try
            {
                _response.Message = "File chưa upload";
                if (file == null)
                {
                    return _response;
                }

                string configstringfolder = _config["ExcelFilePath"];
                configstringfolder = configstringfolder.Replace(@"/", @"\");
                string filePath = _hostingEnvironment.ContentRootPath;
                string fileNamefull = file.FileName;
                string fileExtention = fileNamefull.Substring(file.FileName.Length - 5, 5).ToLower();
                string fileName = fileNamefull.Remove(file.FileName.Length - 5);

                // check và đặt tên file để không trùng tên cũ
                int i = 1;
                string filePathsave = Path.Combine(filePath, configstringfolder, fileName + i + excelFileExtension);
                while (File.Exists(filePathsave))
                {
                    i++;
                    filePathsave = Path.Combine(filePath, configstringfolder, fileName + i + excelFileExtension);
                }

                // lấy file đã tồn tại
                filePathsave = Path.Combine(filePath, configstringfolder, fileName + (i - 1) + excelFileExtension);

                // if (file.Length < 1) return _response;
                if (!File.Exists(filePathsave))
                {
                    _response.Message = " File không tồn tại";
                    return _response;
                }

                if (!validImageFomat.Contains(fileExtention))
                {
                    _response.Message = "Không phải định dạng file Excel " + excelFileExtension;
                    return _response;
                }

                _response.Code = 0;
                _response.Message = string.Empty;
                _response.Data = filePathsave;
            }
            catch (Exception)
            {
                _response.Code = -1;
                _response.Message = "Có lỗi trong quá trình Upload File";
            }

            return _response;
        }

        public DataTable DataTableFromExcelFile(string path, string sheetName)
        {
            // Khởi tạo data table
            DataTable dt = new DataTable();

            // Load file excel và các setting ban đầu
            using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
            {
                if (package.Workbook.Worksheets.Count < 1)
                {
                    // Log - Không có sheet nào tồn tại trong file excel của bạn
                    return null;
                }

                // Lấy Sheet đầu tiện trong file Excel để truy vấn // Truyền vào name của Sheet để lấy ra sheet cần, nếu name = null thì lấy sheet đầu tiên
                ExcelWorksheet workSheet = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == sheetName) ?? package.Workbook.Worksheets.FirstOrDefault();

                // Đọc tất cả các header
                foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
                {
                    dt.Columns.Add(firstRowCell.Text);
                }

                // Đọc tất cả data bắt đầu từ row thứ 2
                for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
                {
                    // Lấy 1 row trong excel để truy vấn
                    var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];

                    // tạo 1 row trong data table
                    var newRow = dt.NewRow();
                    foreach (var cell in row)
                    {
                        newRow[cell.Start.Column - 1] = cell.Text;
                    }

                    dt.Rows.Add(newRow);
                }
            }

            return dt;
        }

        public void CreateJsonFile(JToken obj, string fileName = null)
        {
            string uplDir = Path.Combine("JsonData");
            fileName = fileName + ".json";
            string filePath = Path.Combine(uplDir, fileName);
            string objstr = obj.ToString();
            string[] allFiles = System.IO.Directory.GetFiles("JsonData");
            System.IO.DirectoryInfo di = new DirectoryInfo("JsonData");
            foreach (FileInfo files in di.GetFiles())
            {
                if (File.Exists(Path.Combine(filePath)))
                {
                    File.Delete(Path.Combine(filePath));
                }
            }
            System.IO.File.WriteAllText(filePath, objstr, Encoding.UTF8);
        }

        public string GetResultInfo(DataSet dataSet)
        {
            var tableCount = 0;
            string infoString;
            string data = string.Empty;

            if (dataSet == null)
            {
                infoString = "dataSet=null";
            }
            else
            {
                infoString = $"dataSet.Tables.Count={dataSet.Tables.Count}; ";

                foreach (DataTable dataTable in dataSet.Tables)
                {
                    infoString += $"dataTable[{tableCount++}].Rows.Count={dataTable.Rows.Count}; ";
                }
            }

            return infoString;
        }

        public string SPParametersToString(string spName, OracleParameter[] paramArr, string roleCode, string userName)
        {
            const string CHAR_CRLF = "\r\n";
            const string CHAR_TAB = "\t";

            if (paramArr == null || paramArr.Length == 0) return string.Empty;

            var infoString = CHAR_CRLF;

            foreach (var parameter in paramArr)
            {
                infoString += CHAR_TAB + parameter.ParameterName + "='" + parameter.Value + "'," + CHAR_CRLF;
            }

            infoString += CHAR_TAB + $"P_ROLECODE={roleCode}," + CHAR_CRLF + CHAR_TAB + $"P_USERNAME={userName}";

            return spName + "=" + infoString;
        }

        public int GetSafeInt<T>(T col)
        {
            if (col != null)
            {
                bool isNumber = int.TryParse(col.ToString(), out int number);

                if (isNumber) return number;
            }

            return 0;
        }

        public string GetSafeString<T>(T col)
        {
            if (col != null)
            {
                return col.ToString();
            }

            return string.Empty;
        }
    }
}
