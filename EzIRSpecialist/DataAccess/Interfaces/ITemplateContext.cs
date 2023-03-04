using CoreLib.Entities;
using EzIRSpecialist.Models.ModelView;
using EzIRSpecialist.Models.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Interfaces
{
    public interface ITemplateContext : IDataAccess<List<IEnumerable<TemplateModelView>>, CResponseMessage, TemplateViewModel>
    {
        IEnumerable SelectIndex(TemplateViewModel template);
    }
}
