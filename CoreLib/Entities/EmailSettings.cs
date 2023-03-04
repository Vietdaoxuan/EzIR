using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace CoreLib.Entities
{
    /// <summary>
    /// Config của service gửi mail.
    /// </summary>
    public class EmailSettings
    {
        public string Mail { get; set; }

        public string Cc { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
        public string Sender { get; set; }
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string Username { get; set; }
        public SecureString Password { get; set; }
    }
}
