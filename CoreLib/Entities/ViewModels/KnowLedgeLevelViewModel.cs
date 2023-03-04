using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities.ViewModels
{
    public class KnowLedgeLevelViewModel
    {
        public int KNOWLEDGELEVELID { get; set; }

        public string LEVELDESC { get; set; }

        public string DESRC { get; set; }

        public string KnowLedgeLevels { get; set; }

        public string RoleCode { get; set; }

        public string UserLogin { get; set; }

        public string LEVELDESCEN { get; set; }

        public string DESRCEN { get; set; }

        public List<KnowLedgeLevel> ListKnowLedgeLevel { get; set; }
    }
}
