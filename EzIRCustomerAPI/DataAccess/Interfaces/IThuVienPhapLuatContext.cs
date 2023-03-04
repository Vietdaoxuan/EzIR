using CoreLib.Entities;
using CoreLib.Entities.ModelViews;
using CoreLib.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI.DataAccess.Interfaces
{
    public interface IThuVienPhapLuatContext//: IDataAccess<IEnumerable<ThuVienPhapLuatModelView>, CResponseMessage, ThuVienPhapLuatViewModel>
    {
        Task<IEnumerable<ThuVienPhapLuatModelView>> Select(ThuVienPhapLuatViewModel thuvienphapluatviewmodel);        
    }
}
