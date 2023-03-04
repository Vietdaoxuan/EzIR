using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.Models
{
    public class LoginModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string IP { get; set; }

        public string Browser { get; set; }

        public string Seed { get; set; }
    }
}
