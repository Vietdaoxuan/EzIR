using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRFront.Models
{
    public class ThongTinViewModel
    {
        public int? aauthorid { get; set; }

        public string acontent { get; set; }

        public int? acpnyid { get; set; }

        public int? aguidelines { get; set; }

        public int? ahascontent { get; set; }

        public int? aid { get; set; }

        public int? aisapproved { get; set; }

        public string alanguage { get; set; }

        public int? amenuid { get; set; }

        public int? amodifierid { get; set; }

        public int? amodulenumber { get; set; }

        public int? aparentid { get; set; }

        public DateTime? apostdate { get; set; }

        public string atitle { get; set; }

        public int? aapprove { get; set; }

        //các mốc lịch su
        public int? aeventid { get; set; }
        public string aeventdate { get; set; }
        public string aeventdesc { get; set; }
        public string aeventdateen { get; set; }
        public string aeventdescen { get; set; }
        // TCBMQL
        public int? aorgmodelid { get; set; }
        public string aorgmodeldesc { get; set; }
        public string aorgmodelpath { get; set; }
        public int? aorgtype { get; set; }

        //TPLD
        public int? amngerid { get; set; }
        public int? acorgid { get; set; }
        public string amngername { get; set; }
        public string anationality { get; set; }
        public int? aknowledgelevelid { get; set; }
        public int? aislegalrep { get; set; }
        public string adateofbirthvn { get; set; }
        public string aknowledgespeciallevel { get; set; }
        public string aknowledgespeciallevelen { get; set; }
        public string acvpath { get; set; }
        public int? aord { get; set; }
        public string listmanagerorg { get; set; }
        public string listmanagerorgofmanager { get; set; }  
        public string listmanagerorgofmanageren { get; set; }

        //TTC

        public string amack { get; set; }

        public string namevn { get; set; }


        public string nameen { get; set; }


        public string anamE_VN { get; set; }


        public string anamE_EN { get; set; }

 
        public string anamE_SHORT { get; set; }

      
        public int? isexist { get; set; }


        public string astocK_CODE { get; set; }


        public string adetail_url { get; set; }


        public int? aprovinceid { get; set; }

        
        public string aheadoffice { get; set; }


        public string aphone { get; set; }


        public string afax { get; set; }

 
        public string aemail { get; set; }

 
        public string awebsite { get; set; }

 
        public string alogopath { get; set; }
  
        public double? acapital { get; set; }


        public double? areG_CAPITAL { get; set; }


        public double? aorG_PRICE { get; set; } // giá chào sàn

     
        public string aunit { get; set; }


        public string aposT_TO { get; set; } // nơi niêm yết

      
        public DateTime? aposT_DATE { get; set; } // ngày niêm yết

        public double? aqty { get; set; } // khối lượng đang niêm yết hiện tại

        public double? afirsT_PRICE { get; set; } // mệnh giá


        public string areG_F_NO { get; set; }


        public DateTime? areG_F_DATE { get; set; }


        public string areG_F_POSITION { get; set; }

     
        public string areG_BIZ_NO { get; set; }

     
        public DateTime? areG_BIZ_DATE { get; set; }

 
        public string areG_BIZ_POSITION { get; set; }


        public string abiography { get; set; }


        public string abiography_en { get; set; }

       
        public string asummary { get; set; }

     
        public string asummarY_EN { get; set; }

      
        public string aserverbank { get; set; }

 
        public string aserverbanken { get; set; }


        public string abankrate { get; set; }


        public int? acissueorgid { get; set; }

    
        public DateTime? aupD_DATE { get; set; }

  
        public string ataxcode { get; set; }

      
        public string aministrydesct { get; set; }

    
        public double? astocK_ID { get; set; }

  
        public bool? ais_deleted { get; set; }

     
        public string avisionstatement { get; set; }


        public double? auserid { get; set; }

        public int? acpnytype { get; set; }
          
        public string afeedbackemail { get; set; }

   
        public string aheadofficeen { get; set; }
       
        public string areG_BIZ_POSITIONEN { get; set; }
           
        public string avisionstatementen { get; set; }

         public byte[] alogoimage { get; set; }

      
        public string companytypedesc { get; set; }

    
        public int? astatus { get; set; }

     
        public string anote { get; set; }

        public IFormFile imagefile { get; set; }

        public string language { get; set; }

        public string username { get; set; }

        public string rolecode { get; set; }

 
        public string atypecode { get; set; }


        public string atypename { get; set; }


        public string cpnytypename { get; set; }

        public int? acategory { get; set; }


        public string alevelid { get; set; }


        public string adetaillevelid { get; set; }

        public string high52 { get; set; }

        public string low52 { get; set; }

        public string mktCap { get; set; }

        public string listed { get; set; }

        public string last4qEPSAdj { get; set; }

        public string last4qPEAdj { get; set; }

        public string klgD_30_Days { get; set; }

        public string tlshnn { get; set; }

        public string gtSoSach { get; set; }

        public string periodBVocp { get; set; }

        public string vBVPeriod { get; set; } // TungLX 09/01/2023: sửa lại tên giá trị đúng với api trả ra vbvperiod -> vBVPeriod

        public string beta { get; set; }

        public float pT_TOTAL_TRADED_QTTY { get; set; }

        public string minisnamevn { get; set; } //QuangKS add 16:02 22/08/2022 Dành cho ngành

        public string minisnameen { get; set; } //QuangKS add 16:02 22/08/2022 

    }
}
