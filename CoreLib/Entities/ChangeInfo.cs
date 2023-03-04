using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities
{
    public class ChangeInfo
    {
        [DataNames("AID")]
        public int? ID { get; set; }

        [DataNames("ACPNYID")]
        public int? CpnyID { get; set; }

        [DataNames("AKEY")]
        public string Key { get; set; }

        [DataNames("AVALUE")]
        public string Value { get; set; }

        [DataNames("ASTATUS")]
        public int? Status { get; set; }

        [DataNames("AKEYFUNCTION")]
        public string KeyFunction { get; set; }

        [DataNames("AMENUID")]
        public int? MenuID { get; set; }

        [DataNames("AFUNCTION")]
        public string Function { get; set; }

        [DataNames("ALEVELID")]
        public string LevelID { get; set; }

        [DataNames("ADETAILLEVELID")]
        public string DetailLevelID { get; set; }

        public string Username { get; set; }

        public string RoleCode { get; set; }
    }
}
