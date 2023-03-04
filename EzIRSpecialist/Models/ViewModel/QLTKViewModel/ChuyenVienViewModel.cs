using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ViewModel.QLTKViewModel
{
    public class ChuyenVienViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public int? Active { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateOn { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string EmailCc { get; set; }

        public string Phone { get; set; }

        public string Note { get; set; }

        public int? RegionID { get; set; }

        public string Expert { get; set; }

        public string UserLogin { get; set; }

        public string RoleCode { get; set; }

        public List<CommonType> listStatus { get; set; }

        public List<CommonType> listRegion { get; set; }
    }
}
