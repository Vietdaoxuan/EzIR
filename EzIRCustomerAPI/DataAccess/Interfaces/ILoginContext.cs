using CoreLib.Entities;
using EzIRCustomerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.DataAccess.Interfaces
{
    public interface ILoginContext
    {
        CResponseMessage CheckLogin(LoginModel loginModel);

        CResponseMessage CheckSeed(LoginModel loginModel);
    }
}
