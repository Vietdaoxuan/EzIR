using CoreLib.Entities;
using CoreLib.Entities.ModelViews;
using CoreLib.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.DataAccess.Interfaces
{
    public interface IToChucBoMayQuanLyContext
    {

        IEnumerable<ToChucBoMayQuanLyModelView> SelectEzIR(ToChucBoMayQuanLyViewModel toChucBoMayQuanLyViewModel);

        IEnumerable<ToChucBoMayQuanLyModelView> SelectEzSearch(ToChucBoMayQuanLyViewModel toChucBoMayQuanLyViewModel);

        CResponseMessage Insert(ToChucBoMayQuanLyViewModel toChucBoMayQuanLyViewModel);

        CResponseMessage Update(ToChucBoMayQuanLyViewModel ToChucBoMayQuanLyViewModel);

        CResponseMessage Delete(int id);

        void SetRoleCode(string roleCode);
        void SetUsername(string username);
    }
}
