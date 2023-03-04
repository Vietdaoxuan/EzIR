using CoreLib.Entities;
using CoreLib.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.DataAccess.Interfaces
{
    public interface ICommonTypeContext : IDataAccess<IEnumerable<CommonType>, CResponseMessage, CommonTypeViewModel>
    {
        //public DataSet Select(CommonTypeViewModel commonType);
    }
}
