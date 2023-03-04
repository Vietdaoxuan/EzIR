using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ViewModel
{
    public class UserGroupViewModel
    {        
        public string AUsername { get; set; }
     
        public string GroupCode { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        public string UserName { get; set; }

        public string RoleCode { get; set; }
    }
}
