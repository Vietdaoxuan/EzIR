using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CoreLib.DataTableToObject.Attrubutes;

namespace EzIRSpecialist.Models.Support
{
    public class JobsStatus
    {
        [DataNames("AQUERY")]
        public string AQUERY { get; set; }

        [DataNames("AID")]
        public DateTime AID { get; set; }

        [DataNames("STATUS")]
        public string STATUS { get; set; }

        public List<JobsStatus> listjs;
    }
}
