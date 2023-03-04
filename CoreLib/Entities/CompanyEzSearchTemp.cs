using CoreLib.DataTableToObject.Attrubutes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities
{
    public class CompanyEzSearchTemp
    {

        [DataNames("AMaCK")]
        public string AMack { get; set; }

        [DataNames("Acpnyid")]
        public double? ACpnyID { get; set; }

        [DataNames("Namevn")]
        public string NameVN { get; set; }

        [DataNames("Nameen")]
        public string NameEN { get; set; }

        [DataNames("Aname_Vn")]
        public string AName_VN { get; set; }

        [DataNames("Aname_En")]
        public string AName_EN { get; set; }

        [DataNames("Aname_Short")]
        public string AName_Short { get; set; }

        [DataNames("IsExist")]
        public int? IsExist { get; set; }

        [DataNames("Astock_Code")]
        public string AStockCode { get; set; }


        [DataNames("aname")]
        public string aname { get; set; }

        [DataNames("Adetail_Url")]
        public string ADetail_URL { get; set; }

        [DataNames("Aprovinceid")]
        public int? AProvinceID { get; set; }

        [DataNames("Aheadoffice")]
        public string AHeadOffice { get; set; }

        [DataNames("Aphone")]
        public string APhone { get; set; }

        [DataNames("Afax")]
        public string AFax { get; set; }

        [DataNames("Aemail")]
        public string AEmail { get; set; }

        [DataNames("Awebsite")]
        public string AWebsite { get; set; }

        [DataNames("Alogopath")]
        public string ALogoPath { get; set; }

        [DataNames("Acapital")]
        public double? ACapital { get; set; }

        [DataNames("Areg_Capital")]
        public double? AReg_Capital { get; set; }

        [DataNames("Aorg_Price")]
        public double? AOrg_Price { get; set; }

        [DataNames("Aunit")]
        public string AUnit { get; set; }

        [DataNames("Apost_To")]
        public string APost_To { get; set; }

        [DataNames("Apost_Date")]
        public DateTime? APost_Date { get; set; }

        [DataNames("Aqty")]
        public double? AQty { get; set; }

        [DataNames("Afirst_Price")]
        public double? AFirst_Price { get; set; }

        [DataNames("Areg_F_No")]
        public string AReg_F_No { get; set; }

        [DataNames("Areg_F_Date")]
        public DateTime? AReg_F_Date { get; set; }

        [DataNames("Areg_F_Position")]
        public string AReg_F_Position { get; set; }

        [DataNames("Areg_Biz_No")]
        public string AReg_Biz_No { get; set; }

        [DataNames("Areg_Biz_Date")]
        public DateTime? AReg_Biz_Date { get; set; }

        [DataNames("Areg_Biz_Position")]
        public string AReg_Biz_Position { get; set; }

        [DataNames("Abiography")]
        public string ABiography { get; set; }

        [DataNames("Abiography_En")]
        public string ABiography_EN { get; set; }

        [DataNames("Asummary")]
        public string ASummary { get; set; }

        [DataNames("Asummary_En")]
        public string ASummary_EN { get; set; }

        [DataNames("Aservebank")]
        public string AServerBank { get; set; }

        [DataNames("Aservebanken")]
        public string AServerBankEN { get; set; }

        [DataNames("Abankrate")]
        public string ABankRate { get; set; }

        [DataNames("Acorgid")]
        public int? ACorgID { get; set; }

        [DataNames("Acissueorgid")]
        public int? ACissueorgID { get; set; }

        [DataNames("Aupd_Date")]
        public DateTime? AUpd_Date { get; set; }

        [DataNames("Ataxcode")]
        public string ATaxCode { get; set; }

        [DataNames("Aministrydesct")]
        public string AMinistryDesct { get; set; }

        [DataNames("Astock_Id")]
        public double? AStock_ID { get; set; }

        [DataNames("Aisdeleted")]
        public bool? AIs_Deleted { get; set; }

        [DataNames("Avisionstatement")]
        public string AVisionStatement { get; set; }

        [DataNames("Auserid")]
        public double? AUserID { get; set; }

        [DataNames("Acpnytype")]
        public int? ACpnyType { get; set; }

        [DataNames("Afeedbackemail")]
        public string AFeedbackEmail { get; set; }

        [DataNames("Aheadofficeen")]
        public string AHeadOfficeEN { get; set; }

        [DataNames("Areg_F_Positionen")]
        public string AReg_F_PositionEN { get; set; }

        [DataNames("Areg_Biz_Positionen")]
        public string Areg_Biz_Positionen { get; set; }

        [DataNames("Avisionstatementen")]
        public string AVisionStatementEN { get; set; }

        [DataNames("Alogoimage")]
        public byte[] ALogoImage { get; set; }

        [DataNames("ADESC")]
        public string CompanyTypeDesc { get; set; }

        [DataNames("AStatus")]
        public int? AStatus { get; set; }

        [DataNames("AMenuid")]
        public double? AMenuid { get; set; }

        [DataNames("ANOTE")]
        public string ANote { get; set; }

        [DataNames("AAPPROVE")]
        public string AApprove { get; set; }

        public IFormFile ImageFile  { get; set; }

        public string Language { get; set; }

        public string Username { get; set; }

        public string RoleCode { get; set; }

        [DataNames("ATypecode")]
        public string ATypecode { get; set; }

        [DataNames("Atypename")]
        public string ATypename { get; set; }

        [DataNames("AORD")]
        public int? AORD { get; set; }

        [DataNames("ACATEGORY")]
        public int? ACATEGORY { get; set; }

        [DataNames("ALEVELID")]
        public string ALEVELID { get; set; }

        [DataNames("ADETAILLEVELID")]
        public string ADetaillevelid { get; set; }
        
        [DataNames("minisnamevn")]
        public string minisnamevn { get; set; }
        
        [DataNames("minisnameen")]
        public string minisnameen { get; set; }


    }
}
