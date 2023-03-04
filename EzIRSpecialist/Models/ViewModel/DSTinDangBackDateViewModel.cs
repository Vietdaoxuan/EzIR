using CoreLib.Entities;
using EzIRSpecialist.Models.QuanLyTaiKhoan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ViewModel
{
    public class DSTinDangBackDateViewModel
    {
        public string UserName { get; set; }

        public string RoleCode { get; set; }

        public string? AStockCode { get; set; }

        public int ACompanyType { get; set; }

        public string AExpert { get; set; }

        public int ARegion { get; set; }

        public DateTime? AFromDate { get; set; }

        public DateTime? AToDate { get; set; }

        public int ADocType { get; set; }
        public int ANewType { get; set; }

        public IEnumerable<Company> ListMaCk { get; set; }

        public IEnumerable<CommonType> ListDocType { get; set; }

        public IEnumerable<CommonType> ListCompanyType { get; set; }

        public IEnumerable<CommonType> ListLoaiTin { get; set; }

        public IEnumerable<CommonType> ListRegion { get; set; }


    }
}
