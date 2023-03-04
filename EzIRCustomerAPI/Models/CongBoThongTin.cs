using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.Models
{
    public class CongBoThongTin
    {
        [DataNames("ANEWID")]
        public int aNewID { get; set; }//NUMBER

        [DataNames("ASID")] 
        public int aSID { get; set; }//NUMBER
        [DataNames("ACPNYID")] 
        public int aCpnyID { get; set; }//NUMBER
        [DataNames("ADATEPUB")] 
        public DateTime? aDatePub { get; set; }//DATETIME
        [DataNames("ADOCTYPE")] 
        public int aDocType { get; set; }//NUMBER
        [DataNames("ANEWTYPE")] 
        public int aNewType { get; set; }//NUMBER
        [DataNames("ATITLE")] 
        public string aTitle { get; set; }//VARCHAR2
        [DataNames("ALANGUAGE")] 
        public int aLanguage { get; set; }//NUMBER
        [DataNames("AYEAR")] 
        public int ayear { get; set; }//NUMBER
        [DataNames("ANOTE")] 
        public string aNote { get; set; }//VARCHAR2
        [DataNames("AISDELETE")] 
        public int aIsDelete { get; set; }//NUMBER
        [DataNames("AFILENAME")] 
        public string aFileName { get; set; }//VARCHAR2
        [DataNames("APATH")] 
        public string aPath { get; set; }//VARCHAR2
        [DataNames("AURL")] 
        public string aURL { get; set; }//VARCHAR2
        [DataNames("ACREATEBY")] 
        public string aCreateBy { get; set; }//VARCHAR2
        [DataNames("ACREATEON")] 
        public DateTime? aCreateOn { get; set; }//DATETIME
        [DataNames("AUPDATEBY")] 
        public string aUpdateBy { get; set; }//VARCHAR2
        [DataNames("AUPDATEON")] 
        public DateTime? aUpdateOn { get; set; }//DATETIME
        [DataNames("AUSERTYPE")] 
        public int aUserType { get; set; }//NUMBER
        [DataNames("USERPOST")]
        public string aUserPort { get; set; }

        [DataNames("AloaiTaiLieu")]
        public string atypename { get; set; }//VARCHAR2

        [DataNames("AloaiTaiLieuEN")]
        public string atypenameen { get; set; }

        [DataNames("AloaiTin")]
        public string anewtypename { get; set; }

        [DataNames("AloaiTinEN")]
        public string anewtypenameen { get; set; }
    }
}
