using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Text;


namespace CoreLib.Entities.ModelViews
{
    public class RoleCompanyModelView
    {
        [DataNames("ACPNYID")]
        public int? aCpnyid { get; set; }

        [DataNames("AROLECODE")]
        public string aRolecode { get; set; }

        [DataNames("ASTATUS")]
        public int? aStatus { get; set; }

        [DataNames("ACREATEBY")]
        public string aCreateby { get; set; }

        [DataNames("ACREATEON")]
        public DateTime? aCreateon { get; set; }

        [DataNames("AUPDATEBY")]
        public string aUpdateby { get; set; }

        [DataNames("AUPDATEON")]
        public DateTime? aUpdateon { get; set; }
    }
}
