using EzIRSpecialist.Models;
using EzIRSpecialist.Models.ModelView;
using EzIRSpecialist.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Interfaces
{
    public interface IDSTinDangTheoKhachHangContext
    {
        public Task<IEnumerable<DSTinDangTheoKhachHang>> ListTinDang(DSTinDangTheoKhachHangViewModel dSTinDangTheoKhachHangViewModel);

        Task<DataTable> ListTinDangExcel(DSTinDangTheoKhachHangViewModel dSTinDangTheoKhachHangViewModel);
    }

}
