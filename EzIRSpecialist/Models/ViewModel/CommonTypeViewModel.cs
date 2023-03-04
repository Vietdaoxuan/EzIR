using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ViewModel
{
    public class CommonTypeViewModel
    {
        public int? TypeID { get; set; }

        public int? TypeCode { get; set; }

        public string TypeName { get; set; }

        public string TypeNameEN { get; set; }

        public string TypeDescription { get; set; }

        public int? Ord { get; set; }

        public int? Category { get; set; }

        public int? ParentID { get; set; }

        public string ListCategory { get; set; }

        public string UserLogin { get; set; }

        public string RoleCode { get; set; }

        public CResponseMessage ResponseMessage { get; set; }

        public CommonType MyProperty { get; set; }
    }
}
