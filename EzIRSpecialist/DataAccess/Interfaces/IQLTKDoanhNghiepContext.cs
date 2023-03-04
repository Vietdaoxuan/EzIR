using CoreLib.Entities;
using EzIRSpecialist.Models.ModelView.QLTKModelView;
using EzIRSpecialist.Models.QuanLyTaiKhoan;
using EzIRSpecialist.Models.ViewModel.QLTKViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Interfaces
{
    public interface IQLTKDoanhNghiepContext : IDataAccess<IEnumerable<QLTKDoanhNghiepModelView>, CResponseMessage, QLTKDoanhNghiepViewModel>
    {

    }
}
