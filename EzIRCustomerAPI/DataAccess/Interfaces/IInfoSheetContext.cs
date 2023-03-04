using CoreLib.Entities;
using EzIRCustomerAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.DataAccess.Interfaces
{
    public interface IInfoSheetContext
    {
        IEnumerable<InfoSheet> SelectEzIR(InfoSheetViewModel infoSheet);

        IEnumerable<InfoSheet> SelectEzSearch(InfoSheetViewModel infoSheet);

        CResponseMessage Insert(InfoSheetViewModel infoSheet);

        CResponseMessage Update(InfoSheetViewModel infoSheet);

        CResponseMessage Delete(int id);

        void SetRoleCode(string roleCode);

        void SetUsername(string username);
    }
}
