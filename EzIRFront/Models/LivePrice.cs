using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRFront.Models
{
    public class LivePrice
    {
        public string Code { get; set; }
        public string UpDown { get; set; }

        public string MatchPrice { get; set; }

        public string ChangePrice { get; set; }

        public string TotalQtty { get; set; }
        public string CenterNo { get; set; }
        public string Ceiling { get; set; }
        public string Floor { get; set; }
        public string RefPrice { get; set; }
        public string BuyPrice3 { get; set; }
        public string BuyQtty3 { get; set; }
        public string BuyPrice2 { get; set; }
        public string BuyQtty2 { get; set; }
        public string BuyPrice1 { get; set; }
        public string BuyQtty1 { get; set; }
        public string MatchQtty { get; set; }
        public string SellPrice1 { get; set; }
        public string SellQtty1 { get; set; }
        public string SellPrice2 { get; set; }
        public string SellQtty2 { get; set; }
        public string SellPrice3 { get; set; }
        public string SellQtty3 { get; set; }
        public string OpenPrice { get; set; }
        public string HighestPrice { get; set; }
        public string LowestPrice { get; set; }
        public string ForeignBuyQtty { get; set; }
        public string ForeignSellQtty { get; set; }



    }
}
