using CoreLib.Entities;
using EzIRSpecialist.Models;
using EzIRSpecialist.Models.QuanLyTaiKhoan;
using EzIRSpecialist.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Interfaces
{
    public interface ICompanyContext : IDataAccess<IEnumerable<Company>, CResponseMessage, CompanyViewModel>
    {
        public IEnumerable<CompanyEzSearchTemp> ListCompany();
        public IEnumerable<CompanyEzSearchTemp> ListCompanyEdit();
        public IEnumerable<CompanyEzSearchTemp> ListCompanyEditSearch();
        public IEnumerable<CompanyEzSearchTemp> ListCompanyEzSearchTemp(int? cpnyid);
        public IEnumerable<CompanyEzSearchTemp> ListCompanyEzSearchTest();
        public List<IEnumerable<CompanyEzSearchTemp>> ListCommontype();
        public List<IEnumerable<CompanyEzSearchTemp>> ListCommontypeSearch();

    }
}
