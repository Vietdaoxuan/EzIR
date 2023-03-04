using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreLib.Entities
{
    public class DevelopProgress
    {
        [DataNames("AEVENTID")]
        public int? EventID { get; set; }

        [DataNames("ACPNYID")]
        public int? CpnyID { get; set; }

        [DataNames("AEVENTORDER")]
        public int? EventOrder { get; set; }

        [DataNames("AEVENTDATE")]
        public string EventDate { get; set; }

        [DataNames("AEVENTDESC")]
        public string EventDesc { get; set; }

        [DataNames("AEVENTDATEEN")]
        public string EventDateEN { get; set; }

        [DataNames("AEVENTDESCEN")]
        public string EventDescEN { get; set; }

        [DataNames("ANOTE")]
        public string Note { get; set; }

        [DataNames("AAPPROVE")]
        public int? Approve { get; set; }

        public string Language { get; set; }

        public string Username { get; set; }

        public string RoleCode { get; set; }
    }
}
