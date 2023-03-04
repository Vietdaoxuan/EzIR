using CoreLib.DataTableToObject.Attrubutes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLib.Entities.ModelViews
{
    public class TemplateModelView
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

        [DataNames("TEMPLATETYPENAME")]
        public string TemplateTypeName { get; set; }

        [DataNames("COMPANYTYPETYPENAME")]
        public string CompanyTypeName { get; set; }

        [DataNames("COMPANYTYPETYPENAMEEN")]
        public string CompanyTypeNameen { get; set; }

        [DataNames("STT")]
        public int Stt { get; set; }

        [DataNames("INDEXVN")]
        public int IndexVN { get; set; }

        [DataNames("INDEXEV")]
        public int IndexEV { get; set; }

        [DataNames("INDEXEN")]
        public int IndexEN { get; set; }

        [DataNames("ACREATEBY")]
        public string CreateBy { get; set; }

        [DataNames("ACREATEON")]
        public DateTime? CreateOn { get; set; }

        [DataNames("AUPDATEBY")]
        public string UpdateBy { get; set; }

        [DataNames("AUPDATEON")]
        public DateTime? UpdateOn { get; set; }

        [DataNames("ATYPENAME")]
        public string TypeName { get; set; }

        [DataNames("ATYPENAMEEN")]
        public string TypeNameEN { get; set; }
    }
}
