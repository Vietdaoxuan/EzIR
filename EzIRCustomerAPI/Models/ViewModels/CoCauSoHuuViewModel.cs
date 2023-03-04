using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.Models.ViewModels
{
    public class CoCauSoHuuViewModel
    {
        public int? CpnyID { get; set; }

        public int? MinistryID { get; set; }
       
        public int? Asubcompanyid { get; set; }

        public string Asubcompanyname { get; set; }

        public string Aaddress { get; set; }

        public int Aministryid { get; set; }

        public int Asharerate { get; set; }

        public int Asherid { get; set; }

        public int Acurshareno { get; set; }

        public int Acursharerate { get; set; }

        public int Aorder { get; set; }

        public string UserName { get; set; }

        public string RoleCode { get; set; }

        public List<CoCauSoHuu> ListCoCauSoHuu { get; set; }

        public string Ashname { get; set; }
    }
}
