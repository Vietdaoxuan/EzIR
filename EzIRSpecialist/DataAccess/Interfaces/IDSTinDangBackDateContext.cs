using EzIRSpecialist.Models;
using EzIRSpecialist.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Interfaces
{
    public interface IDSTinDangBackDateContext
    {
        public Task<IEnumerable<DSTinDangBackDate>> ListTinDangBackDate(DSTinDangBackDateViewModel dSTinDangBackDateViewModel);

        public Task<DataTable> ListNewsBackDateExcel(DSTinDangBackDateViewModel dSTinDangBackDateViewModel);
    }
}
