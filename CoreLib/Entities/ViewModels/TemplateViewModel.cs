using CoreLib.Entities;
using CoreLib.Entities.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLib.Entities.ViewModels
{
    public class TemplateViewModel
    {
        public TemplateModelView template { get; set; }
        public int? CompanyID { get; set; }
        public int? TemplateID { get; set; }

        public int? CompanyType { get; set; }

        public string CompanyTypeTest { get; set; }

        public int? TemplateType { get; set; }

        public string Title { get; set; }

        public string Detail { get; set; }

        public string CCPL { get; set; }

        public string FileName { get; set; }

        public string Path { get; set; }

        public string Url { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateOn { get; set; }

        public string UserLogin { get; set; }

        public string RoleCode { get; set; }

        public string CompanyTypeName { get; set; }
                      
        public List<CommonType> listCompanyType { get; set; }

        public List<CommonType> listTemplateType { get; set; }
    }
}
