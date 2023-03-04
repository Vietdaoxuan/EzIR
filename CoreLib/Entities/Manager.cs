using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities
{
    public class Manager
    {
        [DataNames("AMNGID")]
        public int? MNGID { get; set; }

        [DataNames("AMNGERID")]
        public int? MNGERID { get; set; }

        [DataNames("ACPNYID")]
        public int? CPNYID { get; set; }

        [DataNames("ACORGID")]
        public int? CORGID { get; set; }

        [DataNames("AMNGERNAME")]
        public string MNGERNAME { get; set; }

        [DataNames("ANATIONALITY")]
        public string NATIONALITY { get; set; }

        [DataNames("AKNOWLEDGELEVELID")]
        public int? KNOWLEDGELEVELID { get; set; }

        [DataNames("AISLEGALREP")]
        public int? ISLEGALREP { get; set; }

        [DataNames("ADATEOFBIRTHVN")]
        public string DATEOFBIRTHVN { get; set; }

        [DataNames("AKNOWLEDGESPECIALLEVEL")]
        public string KNOWLEDGESPECIALLEVEL { get; set; }

        [DataNames("AKNOWLEDGESPECIALLEVELEN")]
        public string KNOWLEDGESPECIALLEVELEN { get; set; }

        [DataNames("ACVPATH")]
        public string CVPATH { get; set; }

        [DataNames("AORD")]
        public int? ORD { get; set; }

        [DataNames("ANOTE")]
        public string NOTE { get; set; }

        [DataNames("AAPPROVE")]
        public int? APPROVE { get; set; }

        [DataNames("AMORGDESC")]
        public string MORGDESC { get; set; }

        [DataNames("LISTMANAGERORGOFMANAGER")]
        public string LISTMANAGERORGOFMANAGER { get; set; }

        [DataNames("LISTMANAGERORGOFMANAGEREN")]
        public string LISTMANAGERORGOFMANAGEREN { get; set; }

        [DataNames("LISTMANAGERORG")]
        public string LISTMANAGERORG { get; set; }
    }
}
