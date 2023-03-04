using CoreLib.Entities;
using EzIRSpecialist.Models.Admin;
using EzIRSpecialist.Models.ModelView;
using EzIRSpecialist.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Interfaces
{
    public interface IUserGroupContext : IDataAccess<IEnumerable<UserGroupModelView>, CResponseMessage, UserGroupViewModel>
    {
    }
}
