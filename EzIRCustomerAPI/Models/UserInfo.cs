using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.Models
{
    public class UserInfo
    {
        public string Username { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public int CpnyID { get; set; }

        public string StockCode { get; set; }

        public string StockName { get; set; }

        public string StockNameEn { get; set; }

        public int CompanyType { get; set; }

        public string CompanyTypeName { get; set; }

        public string CompanyTypeNameEN { get; set; }

        public string EmailSpecialist { get; set; }

        public string EmailSpecialistCC { get; set; }

        public string ExpertName { get; set; }

        public string ExpertPhone { get; set; }
    }
}
