using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ModelView
{
    public class UserModelView
    {
        [DataNames("AUSERNAME")]
        public string Username { get; set; }

        [DataNames("APASSWORD")]
        public string Password { get; set; }

        [DataNames("AACTIVE")]
        public bool? Active { get; set; }

        [DataNames("ACREATEBY")]
        public string CreateBy { get; set; }

        [DataNames("ACREATEON")]
        public DateTime? CreateOn { get; set; }

        [DataNames("AUPDATEBY")]
        public string UpdateBy { get; set; }

        [DataNames("AUPDATEON")]
        public DateTime? UpdateOn { get; set; }

        [DataNames("AFULLNAME")]
        public string FullName { get; set; }

        [DataNames("AEMAIL")]
        public string Email { get; set; }

        [DataNames("ACC")]
        public string EmailCc { get; set; }

        [DataNames("APHONE")]
        public string Phone { get; set; }

        [DataNames("ANOTE")]
        public string Note { get; set; }

        [DataNames("AREGIONID")]
        public int? RegionID { get; set; }
    }
}
