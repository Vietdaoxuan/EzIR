using CoreLib.Entities;
using EzIRCustomerAPI.Models;
using EzIRCustomerAPI.Models.ModelViews;
using EzIRCustomerAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.DataAccess.Interfaces
{
    public interface ICustomerContext
    {
        IEnumerable<CustomerModelView> Select(CustomerViewModel customer);
        CResponseMessage ChangePassword(ChangePasswordViewModel changePasswordViewModel);
        CResponseMessage ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel);
        void SetRoleCode(string roleCode);
        void SetUsername(string value);
    }
}
