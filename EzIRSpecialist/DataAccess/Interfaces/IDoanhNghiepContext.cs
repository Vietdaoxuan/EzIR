using CoreLib.Entities;
using EzIRSpecialist.Models;
using EzIRSpecialist.Models.ModelView.QLTKModelView;
using EzIRSpecialist.Models.ViewModel;
using EzIRSpecialist.Models.ViewModel.QLTKViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Interfaces
{
    public interface IDoanhNghiepContext : IDataAccess<IEnumerable<DoanhNghiepModelView>, CResponseMessage, DoanhNghiepViewModel>
    {
        public Task<IEnumerable<DoanhNghiepModelView>> SelectCustomerByExpert(DoanhNghiepViewModel qLTKDoanhNghiep);

        public Task<IEnumerable<CompanyRole>> SelectCompanyRole();

        public Task<CResponseMessage> UpdateCompanyRole(CompanyRoleViewModel companyRole);

        public Task<CResponseMessage> InsertCompanyClient(CompanyClientViewModel companyClient);

        public Task<CResponseMessage> UpdateCompanyClient(CompanyClientViewModel companyClient);
    }
}
