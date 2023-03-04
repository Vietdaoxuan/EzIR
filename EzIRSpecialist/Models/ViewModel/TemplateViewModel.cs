using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ViewModel
{
    public class TemplateViewModel
    {
        public int? TemplateID { get; set; }

        public int? CompanyType { get; set; }

        public int? TemplateType { get; set; }

        public string Title { get; set; }

        public string Detail { get; set; }

        public string CCPL { get; set; }

        public string FileName { get; set; }

        public string OldFileName { get; set; }

        public string Path { get; set; }

        public string Url { get; set; }

        public int? Order { get; set; }

        public string OldUrl { get; set; }

        public string OldPath { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateOn { get; set; }

        public string UserLogin { get; set; }

        public string RoleCode { get; set; }

        public string CompanyTypelist { get; set; }
        public List<CommonType> listCompanyType { get; set; }

        public List<CommonType> listTemplateType { get; set; }
    }
}
