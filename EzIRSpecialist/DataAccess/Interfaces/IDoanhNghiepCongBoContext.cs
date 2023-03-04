using CoreLib.Entities;
using EzIRSpecialist.Models.ModelView.QuanLyDoanhNghiepCongBo;
using EzIRSpecialist.Models.ViewModel.QuanLyDoanhNghiepCongBo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Interfaces
{
    public interface IDoanhNghiepCongBoContext : IDataAccess<IEnumerable<DoanhNghiepCongBoModelView>, CResponseMessage, DoanhNghiepCongBoViewModel>
    {
        Task<DataTable> ListDoanhNghiepCongBo(DoanhNghiepCongBoViewModel doanhNghiepCongBo);
    }
}
    