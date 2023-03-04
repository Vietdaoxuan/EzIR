using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ViewModel
{
    public class UserViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool? Active { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateOn { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string EmailCc { get; set; }

        public string Phone { get; set; }

        public string Note { get; set; }

        public int? RegionID { get; set; }

        public int? CpnyID { get; set; }

        public int? Type { get; set; }

        public string RoleCode { get; set; }
    }
}
