using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Configs
{
    public static class SessionTypes
    {
        /// <summary>
        /// Token để truy cập api.
        /// </summary>
        public const string TOKEN = "Token";

        public const string USERNAME = "Username";

        public const string FULL_NAME = "FullName";

        public const string EMAIL = "Email";

        public const string CPNY_ID = "CpnyID";

        public const string PHONE = "Phone";

        public const string IP = "IP";

        public const string BROWSER = "BROWSER";

        public const string LOGINCOOKIES = "LoginCookies";

        public const string STOCK_CODE = "StockCode";

        public const string COMPANY_TYPE = "CompanyType";

        public const string COMPANY_TYPE_NAME = "CompanyTypeName";

        public const string COMPANY_TYPE_NAMEEN = "CompanyTypeNameEN";

        public const string STOCK_NAME = "StockName";

        public const string STOCK_NAMEEN = "StockNameEn";

        public const string LANGUAGE = "Language";

        public const string MAIL_SPECIALIST = "MailSpecialist";

        public const string MAIL_SPECIALISTCC = "MailSpecialistCC";

        public const string EXPERT_NAME = "ExpertName";

        public const string EXPERT_PHONE = "ExpertPhone";

        public const string CAPTCHA_CODE = "CaptchaCode";

        public const string LIST_ROLECODE = "LIST_ROLECODE";

        //Kiểm tra cho phép đăng nhập 1 tài khoản chỉ 1 máy
        public const string SEED = "Seed";
    }
}
