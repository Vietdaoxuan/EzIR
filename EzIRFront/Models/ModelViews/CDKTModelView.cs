using EzIRFront.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRFront.Models.ModelViews
{
    public class CDKTModelView
    {
        public List<CDKTViewModel> table { get; set; }
        public List<CDKTViewModel> table1 { get; set; }

        public int? MaxLevel { get; set; }
    }
}
