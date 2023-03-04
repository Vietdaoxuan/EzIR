using EzIRSpecialist.Commons;
using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models.ModelView.QLTKModelView;
using EzIRSpecialist.Models.ViewModel.QLTKViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Components
{
    public class DoanhNghiepDetailViewComponent : ViewComponent
    {
        private readonly IDoanhNghiepContext _DoanhNghiepContext;

        public DoanhNghiepDetailViewComponent(IDoanhNghiepContext DoanhNghiepContext)
        {
            _DoanhNghiepContext = DoanhNghiepContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? id)
        {
            var viewModel = new DoanhNghiepViewModel { CompanyID = id};
            var data = await _DoanhNghiepContext.Select(viewModel);
            var listData = data.ToList().FirstOrDefault();
            return this.View(listData);
        }
    }
}
