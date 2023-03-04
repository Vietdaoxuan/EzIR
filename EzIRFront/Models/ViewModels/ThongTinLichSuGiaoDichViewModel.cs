using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRFront.Models.ViewModels
{
    public class ThongTinLichSuGiaoDichViewModel
    {      
        public DateTime? tradinG_DATE { get; set; } //Ngày giao dịch   
        
        public double? basiC_PRICE { get; set; } //Giá sàn     

        public double? opeN_PRICE { get; set; } //Giá mở cửa
        
        public double? closE_PRICE { get; set; } //Giá đóng cửa
        
        public double? highesT_PRICE { get; set; } //Giá cao nhất
        
        public double? lowesT_PRICE { get; set; } //Giá thấp nhất
        
        public double? ceilinG_PRICE { get; set; } //Giá trần
        
        public double? flooR_PRICE { get; set; } //Giá sàn
        
        public double? averagE_PRICE { get; set; } //Giá trung bình 
        
        public double? besT_OFFER_PRICE { get; set; } //Giá đặt bán tốt nhất của GD khớp lệnh (lô chẵn)
        
        public double? besT_OFFER_QTTY { get; set; } //Khối lượng đặt mua tốt nhất của GD khớp lệnh
        
        public double? besT_BID_PRICE { get; set; } //Giá đặt mua tốt nhất của GD khớp lệnh (lô chẵn)
        
        public double? besT_BID_QTTY { get; set; } //Khối lượng đặt mua tốt nhất của GD khớp lệnh (lô chẵn)
        
        public double? changE_VALUE { get; set; } //Giá thay đổi
        
        public double? changE_PERCENT { get; set; } //Phần trăm giá thay đổi
        
        public double? offeR_COUNT { get; set; } //Tổng số lệnh đặt bán của GD khớp lệnh lô chẵn
        
        public double? biD_COUNT { get; set; } //Tổng số lệnh đặt mua của GD khớp lệnh lô chẵn
        
        public double? totaL_OFFER_QTTY { get; set; } //Tổng KL đặt bán của GD khớp lệnh lô chẵn(trừ khối lượng sửa, hủy)
        
        public double? totaL_BID_QTTY { get; set; } //Tổng KL đặt mua của GD khớp lệnh lô chẵn(trừ khối lượng sửa, hủy)
        
        public double? subtractioN_BID_OFFER_QTTY { get; set; } //Khối lượng mua trừ Khối lượng bán
        
        public double? nM_TOTAL_TRADED_QTTY { get; set; } //Tổng khối lượng giao dịch thông thường của GD khớp lệnh lô chẵn
        
        public double? nM_TOTAL_TRADED_VALUE { get; set; } //Tổng giá trị giao dịch thông thường của GD khớp lệnh lô chẵn 
        
        public double? pT_TOTAL_TRADED_QTTY { get; set; } //Tổng khối lượng của giao dịch thỏa thuận (lô chẵn và lẻ)
        
        public double? pT_TOTAL_TRADED_VALUE { get; set; } //Tổng giá trị của giao dịch thỏa thuận (lô chẵn và lẻ)
        
        public double? totaL_TRADING_QTTY { get; set; } //Tổng khối lượng khớp lệnh thường và thỏa thuận 
        
        public double? totaL_TRADING_VALUE { get; set; } //Tổng giá trị khớp lệnh thường và thỏa thuận	
        
        public double? rooM_TOTAL { get; set; } //Tổng khối lượng nhà đầu tư NN được phép sở hữu
        
        public double? rooM_RATIO { get; set; } //Phần trăm (%) sở hữu của nhà đầu tư NN
        
        public double? rooM_CURRENT { get; set; } //Khối lượng còn lại nhà đầu tư NN được phép mua
        
        public double? rooM_REMAIN { get; set; } //Khối lượng sở hữu của nhà đầu tư NN
        
        public double? foreigN_BUY_NM_QTTY { get; set; } //Tổng khối lượng mua khớp của GD khớp lệnh của NĐTNN
        
        public double? foreigN_BUY_NM_VALUE { get; set; } //Tổng giá trị mua khớp của GD khớp lệnh của NĐTNN
        
        public double? foreigN_BUY_NM_RATIO { get; set; } //Phần trăm (%) giao dịch toàn thị trường, mua khớp của GD khớp lệnh của NĐTNN 
        
        public double? foreigN_SELL_NM_QTTY { get; set; } //Tổng khối lượng bán khớp của GD khớp lệnh của NĐTNN 
        
        public double? foreigN_SELL_NM_VALUE { get; set; } //Tổng giá trị bán khớp của GD khớp lệnh của NĐTNN
        
        public double? foreigN_SELL_NM_RATIO { get; set; }//Phần trăm (%) giao dịch toàn thị trường, bán khớp của GD khớp lệnh của NĐTNN 

        public double? adjusteD_CLOSING_PRICE { get; set; }// Giá đóng cửa điều chỉnh

        //lịch sử tăng vốn trả cổ tức
        public string stockCode { get; set; }  
        public string announcements { get; set; } // nội dung 
        public string code { get; set; } 
        public string date2 { get; set; }
        public string content { get; set; } // nội dung 
        public string ctmg { get; set; }
        public string date2Order { get; set; }
        public string url { get; set; }
        public string listedDate    { get; set; }
        public string exRDate       { get; set; } 
        public string numOfAddStoIss{ get; set; } // số cổ phiếu phát hành
        public string listedQty     { get; set; }
        public string exright_Date  { get; set; }
        public string sum_Cpcny     { get; set; }
        public string state_        { get; set; }
        public string link          { get; set; }

        public string textDate      { get; set; }

        public float qPlus { get; set; }

        public DateTime exRightDate { get; set; }
}
}
