using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities
{
    public class KnowLedgeLevel
    {
        [DataNames("AKNOWLEDGELEVELID")]
        public int KNOWLEDGELEVELID { get; set; }

        [DataNames("ALEVELDESC")]
        public string LEVELDESC { get; set; }

        [DataNames("ADESRC")]
        public string DESRC { get; set; }

        [DataNames("ALEVELDESCEN")]
        public string LEVELDESCEN { get; set; }

        [DataNames("ADESRCEN")]
        public string DESRCEN { get; set; }
    }
}
