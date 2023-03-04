using CoreLib.Entities;
using EzIRSpecialist.Models.BaoCaoTienIch;
using EzIRSpecialist.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Interfaces
{
    public interface INhatKyHoatDongContext : IDataAccess<IEnumerable<NhatKyHoatDong>, CResponseMessage, NhatKyHoatDongViewModel>
    {

    }
}
