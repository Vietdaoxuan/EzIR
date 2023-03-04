using CoreLib.DataTableToObject.Attrubutes;
using CoreLib.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ViewModel
{
    public class ThongTinChungViewModel
    {
        public List<CompanyEzSearchTemp> listTTC { get; set; }

        public List<DevelopProgress> listCMLS { get; set; }
    }
}
