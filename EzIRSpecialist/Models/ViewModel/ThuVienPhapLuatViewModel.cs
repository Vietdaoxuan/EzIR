using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ViewModel
{
    public class ThuVienPhapLuatViewModel
    {
        public int? ID { get; set; }

        public int? TypeDoc { get; set; }

        public string TypeName { get; set; }

        public string Company { get; set; }

        public string No { get; set; }
        
        public int? YearPub { get; set; }

        public int? YearEff { get; set; }

        public DateTime DatePub { get; set; }

        public string TextNote { get; set; }

        public DateTime DateEff { get; set; }

        public string FileName { get; set; }

        public string OldFileName { get; set; }

        public string Path { get; set; }

        public string Url { get; set; }

        public int? Order { get; set; }

        public string OldUrl { get; set; }

        public string OldPath { get; set; }

        public string UserLogin { get; set; }

        public string RoleCode { get; set; }

        public List<CommonType> listTypeDoc { get; set; }
    }
}
