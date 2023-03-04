using CoreLib.Entities;
using EzIRSpecialist.Models.Admin;
using EzIRSpecialist.Models.BaoCaoTienIch;
using EzIRSpecialist.Models.QuanLyTaiKhoan;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ViewModel
{
    public class NhatKyHoatDongViewModel
    {
        public string UserName { get; set; }

        public string RoleCode { get; set; }

        public string Astockcode { get; set; }

        public string Ausername { get; set; }

        public string Aloaitaikhoan { get; set; }
        public string ActionC { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FormDate { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ToDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ADatePub { get; set; }

        public string Atypename { get; set; }

        public NhatKyHoatDong NhatKyHoatDong { get; set; }

        public IEnumerable<CommonType> ListAction{ get; set; }

        public IEnumerable<Company> ListMaCk { get; set; }

      



    }
}
