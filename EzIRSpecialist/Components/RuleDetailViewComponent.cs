using EzIRSpecialist.DataAccess.Interfaces;
using EzIRSpecialist.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Components
{
    public class RuleDetailViewComponent : ViewComponent
    {
        private readonly IRulesContext _rulesContext;

        public RuleDetailViewComponent(IRulesContext rulesContext)
        {
            this._rulesContext = rulesContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? id)
        {
            var viewModel = new RulesViewModel { ARuleID = id };
            var data = await _rulesContext.Select(viewModel);
            var loaiHinh = string.Empty;

            var rule = data.ToList().FirstOrDefault();

            if (data != null && data.Count() > 0)
            {                
                foreach (var item in data)
                {
                    loaiHinh += item.AloaiDoanhNghiep + ", ";
                }

                rule.LoaiHinh = loaiHinh;
            }

            return this.View(rule);
        }
    }
}
