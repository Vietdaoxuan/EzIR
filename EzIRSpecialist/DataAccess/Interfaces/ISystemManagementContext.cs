using CoreLib.Entities;
using EzIRSpecialist.Models.Admin;
using EzIRSpecialist.Models.ModelView;
using EzIRSpecialist.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Interfaces
{
    public interface ISystemManagementContext
    {
        // THÔNG TIN NGƯỜI DÙNG
        IEnumerable<UserModelView> GetListUser(UserViewModel searchOptions);

        // thông tin nhóm và role
        IEnumerable<GroupViewModel> GetListRole();

        CResponseMessage GetListRoleByUser(RoleGroupViewModel roleGroupViewModel);

        // phân nhóm người dùng
        
    }
}
