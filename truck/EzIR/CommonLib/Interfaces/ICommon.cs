using CoreLib.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Interfaces
{
    public interface ICommon
    {
        string HashPassword(string password, string salt);

        bool ValidatePassword(string password, string salt, string correctHash);

        string CreateMD5(string input);

        string GetDeepCaller();

        string GetCaller(int level = 2);

        string FormatDate(string date);

        DateTime ConvertStringFromNewsToDateTime(string date);

        string ToQueryString(object obj);

        /// <summary>
        /// Lưu lại file trên server.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="_hostingEnvironment"></param>
        /// <param name="_config"></param>
        /// <returns></returns>
        Task<CResponseMessage> SaveExcelFile(IFormFile file, IHostingEnvironment _hostingEnvironment, IConfiguration _config);

        /// <summary>
        /// Lưu lại file vào thư mục trên server.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="targetFolder"></param>
        /// <param name="hostingEnvironment"></param>
        /// <returns></returns>
        Task<CResponseMessage> SaveFile(IFormFile file, string targetFolder, IHostingEnvironment hostingEnvironment);

        Task<bool> CheckGCaptcha(string secretKey, string captchaResponse);
       
        void CreateJsonFile(JToken obj, string fileName = null);
    }
}
