using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ViewModel
{
    public class RoleGroupViewModel
    {        
        public string ARoleCode { get; set; }
        
        public string GroupCode { get; set; }

        public string RoleType { get; set; }

        public bool? View { get; set; }
       
        public bool? Edit { get; set; }
       
        public bool? Delete { get; set; }
       
        public bool? Special { get; set; }

        public string UserName { get; set; }

        public string RoleCode { get; set; }
    }
}
