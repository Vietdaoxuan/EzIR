using CoreLib.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities
{
        public class TinTucWebApiNew
        {
            public int Code { get; set; }
            public string Message { get; set; }
            public clsData Data { get; set; }
        }
        public class clsData
        {
            public List<TinTucViewModel> Table1 { get; set; }
            public List<TinTucViewModel> Table2 { get; set; }
            public List<TinTucViewModel> Table3 { get; set; }
            public List<TinTucViewModel> Table4 { get; set; }

        }
    
    
}
