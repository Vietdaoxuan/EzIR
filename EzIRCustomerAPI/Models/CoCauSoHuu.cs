using CoreLib.DataTableToObject.Attrubutes;
using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.Models
{
    public class CoCauSoHuu
    {
        public SubCompany SubCompany { get; set; } 

        public Ministry ministry { get; set; }

        public ShareHolder ShareHolder { get; set; }

        public string UserName { get; set; }

        public string RoleCode { get; set; }

        public List<SubCompany> ListSubCompany { get; set; }

        public List<ShareHolder> ListShareHolder { get; set; }

        public EmailSettings emailSettings { get; set; }

    }
}
