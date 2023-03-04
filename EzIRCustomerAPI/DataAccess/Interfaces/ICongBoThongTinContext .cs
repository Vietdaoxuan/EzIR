using CoreLib.Entities;
using CoreLib.Entities.ViewModels;
using DataServiceLib.Interfaces;
using EzIRCustomerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.DataAccess.Interfaces
{
   
    public interface ICongBoThongTinContext : IDataAccess<IEnumerable<CongBoThongTin>, CResponseMessage, CongBoThongTinViewModel>
    {

        void SetRoleCode(string roleCode);

        void SetUsername(string username);
    }
}
