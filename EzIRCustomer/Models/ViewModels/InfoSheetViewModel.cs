using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomer.Models.ViewModels
{
    public class InfoSheetViewModel
    {
        public int? ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int? ParentID { get; set; }

        public int? MenuID { get; set; }

        public bool? HasContent { get; set; }

        public int? ModuleNumber { get; set; }

        public DateTime? PostDate { get; set; }

        public int? AuthorID { get; set; }

        public int? ModifierID { get; set; }

        public int? CpnyID { get; set; }

        public bool? IsApproved { get; set; }

        public int? GuideLines { get; set; }

        public string Language { get; set; }

        public string Note { get; set; }

        public int? Approve { get; set; }

        public string Username { get; set; }

        public string RoleCode { get; set; }

        public List<ChangeInfo> ChangeInfos { get; set; }

        // prop gửi mail
        public EmailSettings EmailSettings { get; set; }
    }
}
