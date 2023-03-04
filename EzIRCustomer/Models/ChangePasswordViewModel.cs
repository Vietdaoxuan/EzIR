using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomer.Models
{
    public class ChangePasswordViewModel
    {
        public string Username { get; set; }


        public string OldPassword { get; set; }


        public string NewPassword { get; set; }

        public string Email { get; set; }

        public string ReEnterPassword
        {
            get; set;
        }
        public int? Cpnyid { get; set; }
        public string RoleCode { get; set; }

        public string GCaptcha { get; set; }


    }
}
