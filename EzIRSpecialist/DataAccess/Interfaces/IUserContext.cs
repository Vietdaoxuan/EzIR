using CoreLib.Entities;
using EzIRSpecialist.Models.Admin;
using EzIRSpecialist.Models.ViewModel;
using System.Collections.Generic;

namespace EzIRSpecialist.DataAccess.Interfaces
{
    public interface IUserContext : IDataAccess<IEnumerable<User>, CResponseMessage, UserViewModel>
    {

    }
}
