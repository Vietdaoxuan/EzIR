using CoreLib.Entities;
using CoreLib.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.DataAccess.Interfaces
{
    public interface IManagerContext : IDataAccess<IEnumerable<Manager>, CResponseMessage, ManagerViewModel>
    {

        IEnumerable<Manager> SelectEzIR(ManagerViewModel manager);

        IEnumerable<Manager> SelectEzSearch(ManagerViewModel manager);

        void SetRoleCode(string roleCode);

        void SetUsername(string username);
    }
}
