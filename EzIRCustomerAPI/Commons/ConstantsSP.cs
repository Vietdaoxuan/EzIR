using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.Commons
{
    public static class ConstantsSP
    {
        // SP Login
        public const string SPEZIR_CUSTOMER_LOGIN = "SPEZIR_CUSTOMER_LOGIN";
        public const string SPEZIR_CUSTOMER_SEED_SEARCH = "SPEZIR_CUSTOMER_SEED_SEARCH";

        // SP Customer
        public const string SPEZIR_CUSTOMER_SEARCH = "SPEZIR_CUSTOMER_SEARCH";

        //SP Company
        public const string SPEZIR_COMPANYROLE_SEARCH = "SPEZIR_COMPANYROLE_SEARCH";
        public const string SPEZIR_ROLE_GET_BY_COMPANY = "SPEZIR_ROLE_GET_BY_COMPANY";

        // SP ChangePassword
        public const string SPEZIR_CUSTOMER_CHANGE_PASSWORD = "SPEZIR_CUSTOMER_CHANGE_PASSWORD";

        //SP Forgotpassword
        public const string SPEZIR_CUSTOMER_FORGOT_PASSWORD = "SPEZIR_CUSTOMER_FORGOT_PASSWORD";

        // SP Infosheet - EzIR
        public const string SPEZIR_INFOSHEET_SEARCH = "SPEZIR_INFOSHEET_SEARCH";
        public const string SPEZIR_INFOSHEET_INSERT = "SPEZIR_INFOSHEET_INSERT";
        public const string SPEZIR_INFOSHEET_UPDATE = "SPEZIR_INFOSHEET_UPDATE";

        // SP Infosheet - EzSearch 
        public const string SPEZS_IR_INFOSHEET_SEARCH = "SPEZS_IR_INFOSHEET_SEARCH";

        //SP Công bố thông tin
        public const string SPEZIR_NEWS_SEARCH = "Spezir_News_Search";
        public const string SPEZIR_NEWS_INSERT = "Spezir_News_Insert";
        public const string SPEZIR_NEWS_UPDATE = "Spezir_News_Update";
        public const string SPEZIR_NEWS_DELETE = "Spezir_News_Delete";

        //SP Danh mục biểu mẫu
        public const string SPEZIR_TEMPLATE_SEARCH = "spezir_template_search";


        //SP Tổ chức bộ máy quản lý - EzIR
        public const string SPEZIR_ORGMODEL_SEARCH = "SPEZIR_ORGMODEL_SEARCH";
        public const string SPEZIR_ORGMODEL_UPDATE = "SPEZIR_ORGMODEL_UPDATE";
        public const string SPEZIR_ORGMODEL_INSERT = "SPEZIR_ORGMODEL_INSERT";

        //SP Tổ chức bộ máy quản lý - EzSearch
        public const string SPEZS_IR_ORGMODEL_SEARCH = "SPEZS_IR_ORGMODEL_SEARCH";

        //SP Chức vụ thành phần lãnh đạo - EzSearch
        public const string SPEZS_IR_MANAGER_SEARCH = "SPEZS_IR_MANAGER_SEARCH"; // SP bên EzSearch nhưng lấy data bên EzIR
        public const string SPEZS_IR_MANAGER_CP_SEARCH = "SPEZS_IR_MANAGER_CP_SEARCH";
        public const string SPEZS_IR_MANAGERORG_SEARCH = "SPEZS_IR_MANAGERORG_SEARCH";
        public const string SPEZS_IR_KNOWLEDGELEVEL_SEARCH = "SPEZS_IR_KNOWLEDGELEVEL_SEARCH";

        //SP Chức vụ thành phần lãnh đạo - EzIR     
        public const string SPEZIR_MANAGER_INSERT = "SPEZIR_MANAGER_INSERT";
        public const string SPEZIR_MANAGER_UPDATE = "SPEZIR_MANAGER_UPDATE";


        //SP CommonType
        public const string SPEZIR_COMMONTYPE_GET_BY_CATEGORY = "SPEZIR_COMMONTYPE_GET_BY_CATEGORY";
        public const string SPEZIR_COMMONTYPE_GET_BY_ID = "SPEZIR_COMMONTYPE_GET_BY_ID";
        public const string SPEZIR_COMMONTYPE_INSERT = "SPEZIR_COMMONTYPE_INSERT";
        public const string SPEZIR_COMMONTYPE_UPDATE = "SPEZIR_COMMONTYPE_UPDATE";
        public const string SPEZIR_COMMONTYPE_DELETE = "SPEZIR_COMMONTYPE_DELETE";

        #region sp cũ của db temp (bỏ)
        //sp đồng bộ thông tin
        public const string SPEZSTEMP_COMPANY_SYNC = "SPEZSTEMP_COMPANY_SYNC";

        public const string SPEZSTEMP_MANAGER_SYNC = "SPEZSTEMP_MANAGER_SYNC";

        public const string SPEZSTEMP_MANAGERORG_SYNC = "SPEZSTEMP_MANAGERORG_SYNC";

        public const string SPEZSTEMP_ORGMODEL_SYNC = "SPEZSTEMP_ORGMODEL_SYNC";

        public const string SPEZSTEMP_SH_CPNYHOLDER_SYNC = "SPEZSTEMP_SH_CPNYHOLDER_SYNC";

        public const string SPEZSTEMP_SUBCOMPANY_SYNC = "SPEZSTEMP_SUBCOMPANY_SYNC";

        public const string SPEZSTEMP_INFOSHEET_SYNC = "SPEZSTEMP_INFOSHEET_SYNC";

        public const string SPEZSTEMP_MENUS_SYNC = "SPEZSTEMP_MENUS_SYNC";

        public const string SPEZSTEMP_PAR_SUBCOMPANYTYPE_SYNC = "SPEZSTEMP_PAR_SUBCOMPANYTYPE_SYNC";
        #endregion sp cũ của db temp (bỏ)

        //SP cơ cấu sở hữu
        public const string SPEZS_IR_SUBCOMPANY_SEARCH = "SPEZS_IR_SUBCOMPANY_SEARCH";
        public const string SPEZIR_SUBCOMPANY_INSERT = "SPEZIR_SUBCOMPANY_INSERT";
        public const string SPEZIR_SUBCOMPANY_UPDATE = "SPEZIR_SUBCOMPANY_UPDATE";
        public const string SPEZIR_SH_HOLDER_INSERT = "SPEZIR_SH_HOLDER_INSERT";
        public const string SPEZIR_SH_HOLDER_UPDATE = "SPEZIR_SH_HOLDER_UPDATE";
        public const string SPEZS_IR_PAR_SUBCOMPANYTYPE_SEARCH = "SPEZS_IR_PAR_SUBCOMPANYTYPE_SEARCH";
        //SP lĩnh vực&công nghiệp
        public const string SPEZS_MINISTRYLIST = "SPEZS_MINISTRYLIST";

        //SP Thông tin sửa đổi ( TEZS_CHANGEINFO ) 
        public const string SPEZIR_CHANGEINFO_INSERT = "SPEZIR_CHANGEINFO_INSERT";
        public const string SPEZIR_CHANGEINFO_UPDATE = "SPEZIR_CHANGEINFO_UPDATE";
        //public const string SPEZSTEMP_CHANGEINFO_SEARCH = "SPEZSTEMP_CHANGEINFO_SEARCH";

        //SP Company, DevelopProgress trong DB EzIR
        public const string SPEZIR_COMPANY_LIST = "SPEZIR_COMPANY_LIST";
        public const string SPEZIR_DEVELOPPROGRESS_LIST = "SPEZIR_DEVELOPPROGRESS_LIST";
        public const string SPEZIR_DEVELOPPROGRESS_UPDATE = "SPEZIR_DEVELOPPROGRESS_UPDATE";
        public const string SPEZIR_DEVELOPPROGRESS_INSERT = "SPEZIR_DEVELOPPROGRESS_INSERT";
        public const string SPEZIR_COMPANY_UPDATE = "SPEZIR_COMPANY_UPDATE";

        //SP Company, DevelopProgress trong DB EzSearch
        public const string SPEZS_IR_COMPANY_LIST = "SPEZS_IR_COMPANY_LIST";
        public const string SPEZS_IR_DEVELOPPROGRESS_LIST = "SPEZS_IR_DEVELOPPROGRESS_LIST";
        public const string SPEZS_COMPANYTYPELIST = "SPEZS_COMPANYTYPELIST";

        //sp Jobs Status
        public const string SPEZSTEMP_JOBS_STATUS = "SPEZSTEMP_JOBS_STATUS";

        //sp sync
        public const string SPEZSTEMP_CALL_SYNC = "SPEZSTEMP_CALL_SYNC";

        //Thư viện pháp luật
        public const string SPEZIR_LIBOFLAW_SEARCH = "SPEZIR_LIBOFLAW_SEARCH";
    }
}
