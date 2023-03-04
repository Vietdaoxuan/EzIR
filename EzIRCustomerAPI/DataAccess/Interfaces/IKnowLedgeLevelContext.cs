using CoreLib.Entities;
using CoreLib.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.DataAccess.Interfaces
{
    public interface IKnowLedgeLevelContext : IDataAccess<IEnumerable<KnowLedgeLevel>, CResponseMessage, KnowLedgeLevelViewModel>
    {
    }
}
