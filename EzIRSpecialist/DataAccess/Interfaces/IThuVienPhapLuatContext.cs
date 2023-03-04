using CoreLib.Entities;
using EzIRSpecialist.Models.ModelView;
using EzIRSpecialist.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Interfaces
{
    public interface IThuVienPhapLuatContext : IDataAccess<IEnumerable<ThuVienPhapLuatModelView>, CResponseMessage, ThuVienPhapLuatViewModel>

    {
        Task<IEnumerable<ThuVienPhapLuatModelView>> SelectIndex(ThuVienPhapLuatViewModel thuVien);
    }
}
