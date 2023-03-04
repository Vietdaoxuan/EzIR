using CoreLib.Entities;
using EzIRSpecialist.Models.BaoCaoTienIch;
using EzIRSpecialist.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Interfaces
{
    public interface IRulesContext : IDataAccess<IEnumerable<Rules>, CResponseMessage, RulesViewModel>
    {
        Task<DataTable> Listrules(RulesViewModel rulesViewModel);
    }
}
