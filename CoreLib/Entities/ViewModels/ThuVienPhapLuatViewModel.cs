using CoreLib.Entities.ModelViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities.ViewModels
{
    public class ThuVienPhapLuatViewModel
    {
        //public ThuVienPhapLuatModelView thuvienphapluattemp { get; set; }

        //public int ID { get; set; }

        public int TypeDoc { get; set; }

        public string Company { get; set; }

        public string No { get; set; }

        //public DateTime DatePub { get; set; }

        public string TextNote { get; set; }

        //public DateTime DateEff { get; set; }

        public int YearDateEff { get; set; }

        public int YearDatePub { get; set; }

        //public string RoleCode { get; set; }

        //public string UserLogin { get; set; }

        public string Path { get; set; }
    }
}
