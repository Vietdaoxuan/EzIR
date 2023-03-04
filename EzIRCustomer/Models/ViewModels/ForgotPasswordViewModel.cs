using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomer.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        public string Email { get; set; }

        public string Username { get; set; }

        public string GCaptcha { get; set; }

        public string RoleCode { get; set; }
    }
}
