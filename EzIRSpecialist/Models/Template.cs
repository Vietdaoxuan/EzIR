using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.Models
{
    public class Template
    {
        [DataNames("ATEMPLATEID")]
        public int? TemplateID { get; set; }

        [DataNames("ACOMPANYTYPE")]
        public int? CompanyType { get; set; }

        [DataNames("ATEMPLATETYPE")]
        public int? TemplateType { get; set; }

        [DataNames("ATITLE")]
        public string Title { get; set; }

        [DataNames("ADETAIL")]
        public string Detail { get; set; }

        [DataNames("ACCPL")]
        public string CCPL { get; set; }

        [DataNames("AFILENAME")]
        public string FileName { get; set; }

        [DataNames("APATH")]
        public string Path { get; set; }

        [DataNames("AURL")]
        public string Url { get; set; }

        [DataNames("ACREATEBY")]
        public string CreateBy { get; set; }

        [DataNames("ACREATEON")]
        public DateTime? CreateOn { get; set; }

        [DataNames("AUPDATEBY")]
        public string UpdateBy { get; set; }

        [DataNames("AUPDATEON")]
        public DateTime? UpdateOn { get; set; }
    }
}
