using CoreLib.Entities;
using EzIRSpecialist.Models.Admin;
using EzIRSpecialist.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Interfaces
{
    public interface IBannerContext : IDataAccess<IEnumerable<Banner>, CResponseMessage, BannerViewModel>
    {
    }
}
