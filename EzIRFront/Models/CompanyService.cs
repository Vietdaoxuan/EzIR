using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace EzIRFront.Models
{
   
    public  class CompanyService
    {

        
        public string astock_CODE { get; set; }

       
        public string astockname { get; set; }

       
        public string aposT_TO { get; set; }

    
        public string acpnytype { get; set; }

        public int acpnyid { get; set; }

        public CompanyService companyService { get; set; }

        //logo công ty
        public string alogopath { get; set; }
        public string awebsite { get; set; }
    }
}
