using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities.ViewModels
{
    public class ManagerOrgViewModel
    {
        public int? AMOrgID { get; set; }

        public string AMOrgCode { get; set; }

        public string AMOrgDesc { get; set; }

        public int? AMOrgGroupID { get; set; }

        public string ListManager { get; set; }

        public string RoleCode { get; set; }

        public string UserLogin { get; set; }

        public int? ABold { get; set; }

        public string AMOrgDescEN { get; set; }

        public List<ManagerOrg> listChucvu { get; set; }

        

    }
}
