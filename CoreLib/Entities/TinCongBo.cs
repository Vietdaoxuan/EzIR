using CoreLib.DataTableToObject.Attrubutes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities
{
    public class TinCongBo
    {
        [DataNames("ANEWID")]
        public int? aNewID { get; set; }

        [DataNames("ASID")]
        public int? asid { get; set; }

        [DataNames("ACPNYTD")]
        public int aCpnyID { get; set; }

        [DataNames("ADATEPUB")]
        public DateTime? aDatePub { get; set; }

        [DataNames("ADOCTYPE")]
        public int? aDocType { get; set; }

        [DataNames("ANEWTYPE")]
        public int aNewType { get; set; }

        [DataNames("ATITLE")]
        public string aTitle { get; set; }

        [DataNames("ALANGUAGE")]
        public int aLanguage { get; set; }

        [DataNames("AYEAR")]
        public int ayear { get; set; }

        [DataNames("ANOTE")]
        public string aNote { get; set; }

        [DataNames("AISDELETE")]
        public int aIsDelete { get; set; }

        [DataNames("AFILENAME")]
        public string aFileName { get; set; }

        [DataNames("APATH")]
        public string aPath { get; set; }

        [DataNames("AURL")]
        public string aurl { get; set; }

        [DataNames("ACREATEBY")]
        public string aCreateBy { get; set; }

        [DataNames("ACREATEON")]
        public DateTime? aCreateOn { get; set; }

        [DataNames("AUPDATEBY")]
        public string aUpdateBy { get; set; }

        [DataNames("AUPDATEON")]
        public DateTime? aUpdateOn { get; set; }

        [DataNames("USERPOST")]
        public string aUserPort { get; set; }

        [DataNames("AUSERTYPE")]
        public int aUserType { get; set; }

        [DataNames("AloaiTaiLieu")]
        public string atypename { get; set; }

        [DataNames("AloaiTaiLieuEN")]
        public string atypenameen { get; set; }

        [DataNames("AloaiTin")]
        public string anewtypename { get; set; }

        [DataNames("AloaiTinEN")]
        public string anewtypenameen { get; set; }

        [DataNames("ASTOCKCODE")]
        public string aStockCode { get; set; }
    }
}
