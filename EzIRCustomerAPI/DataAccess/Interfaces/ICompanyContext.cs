using CoreLib.Entities;
using CoreLib.Entities.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.DataAccess.Interfaces
{
    public interface ICompanyContext
    {
        IEnumerable<CompanyEzSearchTemp> ListCompanyEzIR(int? cpnyID, int? approve);

        IEnumerable<CompanyEzSearchTemp> ListCompanyEzSearch(int? cpnyID);

        IEnumerable<DevelopProgress> ListDevelopProgressEzIR(int? cpnyID, int? status, string language);

        IEnumerable<DevelopProgress> ListDevelopProgressEzSearch(int? cpnyID, string language);

        IEnumerable<CompanyType> ListCompanyType();

        IEnumerable<RoleCompanyModelView> GetRoleCompany(int? cpnyid);

        CResponseMessage UpdateCompanyInfomation(CompanyEzSearchTemp company);

        CResponseMessage UpdateDevelopProgress(DevelopProgress developProgress);

        CResponseMessage InsertDevelopProgress(DevelopProgress developProgress);

        void SetRoleCode(string roleCode);

        void SetUsername(string username);
    }
}
