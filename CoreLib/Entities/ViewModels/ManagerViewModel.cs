using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities.ViewModels
{
    public class ManagerViewModel
    {
        public int? ID { get; set; }
        public int? MNGID { get; set; }

        public int? MNGERID { get; set; }

        public int? CPNYID { get; set; }

        public int? CORGID { get; set; }

        public string MNGERNAME { get; set; }

        public string NATIONALITY { get; set; }

        public int? KNOWLEDGELEVELID { get; set; }

        public int? ISLEGALREP { get; set; }

        public string DATEOFBIRTHVN { get; set; }

        public string KNOWLEDGESPECIALLEVEL { get; set; }

        public string KNOWLEDGESPECIALLEVELEN { get; set; }

        public string CVPATH { get; set; }

        public int? ORD { get; set; }

        public string NOTE { get; set; }

        public int? APPROVE { get; set; }

        public string RoleCode { get; set; }

        public string UserLogin { get; set; }

        public string MORGID { get; set; }

        public List<ChangeInfo> ChangeInfos { get; set; }

        public EmailSettings EmailSettings { get; set; }
    }
}
