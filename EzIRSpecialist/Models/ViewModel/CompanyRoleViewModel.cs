using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ViewModel
{
    public class CompanyRoleViewModel
    {
        public int CompanyID { get; set; }

        public string RoleCompany { get; set; }

        public bool? Status { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateOn { get; set; }

        public string UserLogin { get; set; }

        public string RoleCode { get; set; }
    }
}
