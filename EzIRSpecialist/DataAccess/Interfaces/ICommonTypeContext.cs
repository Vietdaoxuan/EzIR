using CoreLib.Entities;
using EzIRSpecialist.Models;
using EzIRSpecialist.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Interfaces
{
    public interface ICommonTypeContext : IDataAccess<IEnumerable<CommonType>, CResponseMessage, CommonTypeViewModel>
    {
        public Task<IEnumerable<CommonType>> SelectById(CommonTypeViewModel commonType);

    }
}
