using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreLib.Entities
{
    public class CaptchaResult
    {        
        public string CaptchaCode { get; set; }

        public byte[] CaptchaByteData { get; set; }

        public string CaptchBase64Data => Convert.ToBase64String(CaptchaByteData);

        public DateTime Timestamp { get; set; }
    }
}
