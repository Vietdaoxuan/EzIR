using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities.ViewModels
{
    public class TinTucViewModel
    {
        public int IDX { get; set; }
        public int SID { get; set; }
        public string Title { get; set; }
        public string TitleFolder { get; set; }
        public int? FolderLuutru { get; set; }
        public int? FiscYear { get; set; }
        public string DatePub { get; set; }
        public string Stock_code { get; set; }
        public string Body { get; set; }
        public int ClickDownload { get; set; }
        public string URL { get; set; }
        public string PicURL { get; set; }
        public string Source { get; set; }
        public string PostBy { get; set; }
        public string OpenTable { get; set; }
        public int? MaxYearBCTC { get; set; }
        public int? MaxYearHDQT { get; set; }
        public int? MaxYearDHCD { get; set; }
        public string newimg { get; set; }

        // lưu trữ 
        public int? language { get; set; }

        public int? MFolder { get; set; }

        

       
    }
}
