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
    public interface IChuyenVienContext : IDataAccess<IEnumerable<ChuyenVienModelView>, CResponseMessage, ChuyenVienViewModel>
    {

    }
}
