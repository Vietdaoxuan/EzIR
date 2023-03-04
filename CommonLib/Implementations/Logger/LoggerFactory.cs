using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommonLib.Implementations.Logger
{
    public static class LoggerFactory
    {
        public const string TYPE_FOLDER_DEBUG = "DEBUG";

        public const string TYPE_FOLDER_INFO = "INFO";

        public const string TYPE_FOLDER_SQL = "SQL";

        public const string TYPE_FOLDER_ERROR = "ERROR";

        public const string
            CONFIG_PATH_POS =
                "Serilog:WriteTo:0:Args:path"; 

        private const string _dateFormat = "yyyyMMdd";
        public const string LOG_EXT = "js";

        public static ILogger CreateLogger(IConfiguration configuration, string subFolder, bool isRandomFileName = false)
        {
            string fullLogPath = GetFullPath(configuration.GetSection(CONFIG_PATH_POS).Value, subFolder, isRandomFileName);

            string dirPath = Path.GetDirectoryName(fullLogPath);

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            var serilog = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Async(a => a.File(fullLogPath))
                .CreateLogger();
            return serilog;
        }

        /// <summary>
        /// 2019-01-03 15:54:32 ngocta2
        /// get folder ghi log, tuy vao loai log ma folder khac nhau, nhung deu la folder con cua appPath
        /// </summary>
        /// <param name="rootFolder">D:\WebLog\S6G\CommonLib.Tests</param>
        /// <param name="subFolder">DEBUG/SQL/ERROR</param>
        /// <param name="isRandomFileName">Tên file ngẫu nhiên</param>
        /// <returns></returns>
        private static string GetFullPath(string rootFolder, string subFolder, bool isRandomFileName)
        {
            var random = isRandomFileName ? "_" + Guid.NewGuid().ToString().Substring(0, 8) : string.Empty;
            return $"{rootFolder}\\{subFolder}\\{DateTime.Now.ToString(_dateFormat)}{random}.{LOG_EXT}";
        }
    }
}
