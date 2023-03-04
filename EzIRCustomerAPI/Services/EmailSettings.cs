using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.Services
{
    /// <summary>
    /// Config của service gửi mail.
    /// </summary>
    public class EmailSettings
    {
        public string MailServer { get; set; }

        public int MailPort { get; set; }

        public string SenderName { get; set; }

        public string Sender { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
