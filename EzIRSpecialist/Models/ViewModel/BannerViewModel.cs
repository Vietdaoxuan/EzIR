using CoreLib.Entities;
using EzIRSpecialist.Models.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models.ViewModel
{
    public class BannerViewModel
    {

        public int AbannerID { get; set; }
        public string Atitle { get; set; }

        public string AfileName { get; set; }

        public string Aurl { get; set; }

        public string Apath { get; set; }

        public string Acreateby { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime AcreateOn { get; set; }

        public string AupdateBY { get; set; }

        public DateTime AupdateOn { get; set; }

        public IEnumerable<Banner> Banners { get; set; }

        public CResponseMessage ResponseMessage { get; set; }

        public string UserName { get; set; }

        public string RoleCode { get; set; }

        public Banner Banner { get; set; }

        public string imagelogo { get; set; }

        public byte[] ALogo { get; set; }
    }
}
