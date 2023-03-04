using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.DataAccess.Interfaces
{
    public interface IChangeInfoContext
    {
        //IEnumerable<ChangeInfo> Select(ChangeInfo changeInfo);

        CResponseMessage Insert(ChangeInfo changeInfo);

        CResponseMessage Update(ChangeInfo changeInfo);
    }
}
