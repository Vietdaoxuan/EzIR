using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Configs
{
    public class WebApiConfigs
    {

        /// <summary>
        /// Đường link webapi
        /// </summary>

      

        ///api get stocode
        public const string Link_WebApi_StockCode = "/api/companyinfo/listcompany";

        //api get banner from project ezirspecialist
        public const string Link_WebApi_Banner_List = "/api/getdatafromezir/listbanner";
        //logo đối tác doanh nghiệp

        //api get enterprise from  project ezirspecialist
        public const string LINK_WEBAPI_ENTERPRISE_LIST = "/api/getdatafromezir/listquanlydoanhnghiep";

        public const string Link_WebApi_logo_List = "/api/getdatafromezir/listlogocompanypartner";
        // tin tức cổng thông tin
        public const string Link_WebApi_TinTucVN = "/v1/mobile/list?folder=86";

        public const string Link_WebApi_TinTucEN = "/v1/mobile/list?folder=506";

        // Bản tin IR home
        public const string Link_WebApi_BanTinIRHomeVN = "/v1/mobile/list?folder=86&code=AAA&pageSize=10&selectedPage=1&cbtt=1&newsType=2";

        public const string Link_WebApi_BanTinIRHomeEN = "/v1/mobile/list?folder=86&code=AAA&pageSize=10&selectedPage=1&cbtt=1&newsType=2";
        //Tin tức home
        public const string Link_WebApi_TinTucHomeVN = "/v1/mobile/list?folder=86&code=--&pageSize=8&selectedPage=1&cbtt=0&from=01-01-1970&to=01-01-3000&newsType=1";

        public const string Link_WebApi_TinTucHomeEN = "/v1/mobile/list?folder=506&code=--&pageSize=8&selectedPage=1&cbtt=0&from=01-01-1970&to=01-01-3000&newsType=1";

        public const string Link_WebApi_CMLS = "/api/companyinfo/cacmoclichsu";
        // Tầm nhìn chiến lược,...
        public const string Link_WebApi_Listinfosheet = "/api/getdatafromezir/listinfosheet";
        //TCBMQL
        public const string Link_WebApi_TCBMQL = "/api/getdatafromezir/listorgmodel";
        //TPLD
        public const string Link_WebApi_TPLD = "/api/getdatafromezir/listmanager";
        //Thông tin chung
        public const string Link_WebApi_TTC = "/api/companyinfo/thongtin";
        //Lịch sử giao dịch
        public const string Link_WebApi_Listpricehistory = "/api/getdatafromezir/listpricehistory";
        //Thông tin giao dịch
        public const string Link_WebApi_TTGD = "/api/companyfinance/tabratioview_ezanalyze";

        public const string Link_WebApi_TTGD_GDTT = "/api/getdatafromezir/listpricehistory";

        //Phân tích của fpts
        public const string Link_WebApi_CBTT = "/api/getdatafromezir/listcongbothongtin";

        //Tổng quan - cơ cấu sở hữu
        public const string Link_WebApi_TQ_CCSH = "/api/getdatafromezir/listsubcompany";

        // Lịch sử tăng vốn
        public const string Link_WebApi_LS_TANGVON = "/api/companyinfo/lichsutangvon";

        //Lịch sử trả cổ tức
        public const string Link_WebApi_LS_TRACOTUC = "/api/companyinfo/lichsutracotuc";


        //Tài chính
        public const string Link_WebApi_TCHSTC = "/api/companyfinance/hesotaichinh";
        public const string Link_WebApi_TCCDKT = "/api/companyfinance/candoiketoan";
        public const string Link_WebApi_TCKHTC = "/api/companyfinance/kehoachkinhdoanh";
        public const string Link_WebApi_KQKD = "/api/companyfinance/ketquakinhdoanh";
        public const string Link_WebApi_LuuChuyenICF = "/api/companyfinance/icf";
        public const string Link_WebApi_LuuChuyenDCF = "/api/companyfinance/dcf";


        public string BASE_URL { get; set; }

        public String API_TinTuc { get; set; }

        public String API_BanTin { get; set; }

        public string API_PRICE { get; set; }

        public string API_KLGDTT { get; set; }

        public string API_BETA { get; set; }
    }
}
