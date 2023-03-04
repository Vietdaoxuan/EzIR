using CoreLib.Entities;
using CoreLib.ViewModel;
using EzIRCustomerAPI.Models;
using EzIRCustomerAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.DataAccess.Interfaces
{
    public interface ICoCauSoHuuContext
    {
        Task<List<IEnumerable<SubCompany>>> Select(CoCauSoHuuViewModel coCauSoHuuViewModel);

        IEnumerable<Ministry> Select_Misnitry();

        IEnumerable<SubCompany> Select_SubCompanyType(CoCauSoHuuViewModel coCauSoHuuViewModel);

        CResponseMessage InsertSubcompany(SubCompany subCompany);

        CResponseMessage UpdateSubcompany(SubCompany subCompany);

        CResponseMessage InsertShareHolder(ShareHolder shareHolder);

        CResponseMessage UpdateShareHolder(ShareHolder shareHolder);
    }
}
