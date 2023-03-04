using CoreLib.Entities;
using CoreLib.Entities.ModelViews;
using CoreLib.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.DataAccess.Interfaces
{
    public interface ITemplateContext : IDataAccess<List<IEnumerable<TemplateModelView>>, CResponseMessage, TemplateViewModel>
    {

    }
}
