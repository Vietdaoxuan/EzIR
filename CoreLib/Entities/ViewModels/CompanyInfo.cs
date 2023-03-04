using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities.ViewModels
{
    public class CompanyInfo
    {
        public CompanyEzSearchTemp Company { get; set; }

        public List<DevelopProgress> DevelopProgresses { get; set; }

        public EmailSettings EmailSettings { get; set; }
    }
}
