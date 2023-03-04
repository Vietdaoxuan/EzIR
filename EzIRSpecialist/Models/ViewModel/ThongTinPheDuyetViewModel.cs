using CoreLib.DataTableToObject.Attrubutes;
using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ViewModel
{
    public class ThongTinPheDuyetViewModel
    {
        
        public int? InfoID { get; set; }

        public int? ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int? ParentID { get; set; }

        public int? MenuID { get; set; }

        public int? HasContent { get; set; }

        public int? ModuleNumber { get; set; }

        public DateTime? PostDate { get; set; }

        public int? AuthorID { get; set; }

        public int? ModifierID { get; set; }

        public int? CpnyID { get; set; }

        public int? IsApproved { get; set; }

        public int? GuideLines { get; set; }

        public string Language { get; set; }

        public string Note { get; set; }

        public int? Approve { get; set; }

        public int? Status { get; set; }

        public string Username { get; set; }

        public string RoleCode { get; set; }

        public string Expert { get; set; }

        public string StockCode { get; set; }

        public string CompanyName { get; set; }

        public string CompanyNameEN { get; set; }

        public string levelid { get; set; }

        public int? OrgModelID { get; set; }

        public string OrgModelDesc { get; set; }

        public string OrgModelPath { get; set; }

        public int? OrgType { get; set; }

        public string NOTE { get; set; }

        public int? APPROVE { get; set; }

        public string UserLogin { get; set; }

        public string imagelogo { get; set; }

        //thành phần lãnh đạo
        public int? MNGID { get; set; }

        public int? MNGERID { get; set; }

        public int? CPNYID { get; set; }

        public int? CORGID { get; set; }

        public string MNGERNAME { get; set; }

        public string NATIONALITY { get; set; }

        public int? KNOWLEDGELEVELID { get; set; }

        public int? ISLEGALREP { get; set; }

        public string DATEOFBIRTHVN { get; set; }

        public string KNOWLEDGESPECIALLEVEL { get; set; }

        public string KNOWLEDGESPECIALLEVELEN { get; set; }

        public string CVPATH { get; set; }

        public int? ORD { get; set; }

        public string MORGID { get; set; }
        
        public EmailSettings EmailSettings { get; set; }

        public List<ChangeInfo> ChangeInfos { get; set; }

        public List<CompanyEzSearchTemp> listTT { get; set; }

        public List<CompanyEzSearchTemp> listMCKEzSearch { get; set; }
    }
}
