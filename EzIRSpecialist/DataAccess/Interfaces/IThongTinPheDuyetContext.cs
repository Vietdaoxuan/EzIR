using CommonLib.Interfaces;
using CommonLib.Interfaces.Logger;
using CoreLib.DataTableToObject.Mapping;
using CoreLib.Entities;
using CoreLib.Entities.ModelViews;
using CoreLib.Entities.ViewModels;
using DataServiceLib.Interfaces;
using EzIRSpecialist.Commons;
using EzIRSpecialist.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Interfaces
{
    public interface IThongTinPheDuyetContext
    {
        IEnumerable<InfoSheet> SelectEzIR(ThongTinPheDuyetViewModel thongTin);
        IEnumerable<InfoSheet> SelectEzSearch(ThongTinPheDuyetViewModel thongTin);
        IEnumerable<ToChucBoMayQuanLyModelView> SelectBMQLEzIR(ThongTinPheDuyetViewModel toChucBoMayQuanLyViewModel);
        IEnumerable<ToChucBoMayQuanLyModelView> SelectBMQLEzSearch(ThongTinPheDuyetViewModel toChucBoMayQuanLyViewModel);
        public IEnumerable<DevelopProgress> ListDevelopProgressEzIR(ThongTinPheDuyetViewModel thongTin);
        public IEnumerable<DevelopProgress> ListDevelopProgressEzSearch(ThongTinPheDuyetViewModel thongTin);
        public IEnumerable<CompanyEzSearchTemp> ListCompanyEzIR(ThongTinPheDuyetViewModel thongTin);
        public IEnumerable<CompanyEzSearchTemp> ListCompanyEzSearch(ThongTinPheDuyetViewModel thongTin);
        public IEnumerable<Manager> SelectTPLDEzIR(ThongTinPheDuyetViewModel thongTin);
        public IEnumerable<Manager> SelectTPLDEzSearch(ThongTinPheDuyetViewModel thongTin);
        public List<IEnumerable<SubCompany>> SelectCCSHEzSearch(ThongTinPheDuyetViewModel thongTin);
    }
}
