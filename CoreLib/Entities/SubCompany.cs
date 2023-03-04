using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities
{
    public class SubCompany
    {
        /// <summary>
        /// Công ty con - công ty liên kết
        /// </summary>
        [DataNames("ASUBCOMPANYID")]
        public int asubcompanyid { get; set; }

        [DataNames("ASUBCOMPANYNAME")]
        public string asubcompanyname { get; set; }

        [DataNames("AADDRESS")]
        public string aaddress { get; set; }

        [DataNames("ANAMEVN")]
        public string anamevn { get; set; }

        //Nắm giữ
        [DataNames("ASHARERATE")]
        public decimal asharerate { get; set; }

        //Cổ đông
        [DataNames("ASHERID")]
        public int asherid { get; set; }

        //Số lượng cổ phần nắm giữ
        [DataNames("ACURSHARENO")]
        public long acurshareno { get; set; }

        [DataNames("ASHNAME")]
        public string ashname { get; set; }

        //Tỷ lệ
        [DataNames("ACURSHARERATE")]
        public decimal acursharerate { get; set; }

        //Thuộc ngành
        [DataNames("AMINISTRYID")]
        public int aministryid { get; set; }

        [DataNames("APARENTID")]
        public int aparentid { get; set; }

        [DataNames("ACOMPANYID")]
        public int? acompanyid { get; set; }


        [DataNames("AAPPROVE")]
        public int aapprove { get; set; }

        [DataNames("ASUBCOMPANYTYPEID")]
        public int asubcompanytypeid { get; set; }

        [DataNames("ASUBCOMPANYTYPEDESC")]
        public string asubcompanytypedesc { get; set; }

        [DataNames("ANOTE")]
        public string anote { get; set; }

        [DataNames("AADDRESSEN")]
        public string aaddressen { get; set; }

        [DataNames("ASUBCOMPANYNAMEEN")]
        public string asubcompanyen { get; set; }

        [DataNames("AORDER")]
        public int? aorder { get; set; }

        public string UserName { get; set; }

        public string RoleCode { get; set; }

       
    }
}
