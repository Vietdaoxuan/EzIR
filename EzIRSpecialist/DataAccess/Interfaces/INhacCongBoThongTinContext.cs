using CoreLib.Entities;
using EzIRSpecialist.Models;
using EzIRSpecialist.Models.BaoCaoTienIch;
using EzIRSpecialist.Models.ModelView;
using EzIRSpecialist.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Interfaces
{
    public interface INhacCongBoThongTinContext : IDataAccess<IEnumerable<NhacCongBoThongTin>, CResponseMessage, NhacCongBoThongTinViewModel>
    {
        public IEnumerable<NhacCongBoThongTinModelView> ListCompanyID(int listcbtt, NhacCongBoThongTinViewModel nhacCongBoThongTinViewModel);

        public Task<IEnumerable<NhacCongBoThongTin>> GetTypeCompany(NhacCongBoThongTinViewModel nhacCongBoThongTinViewModel);
    }
}
