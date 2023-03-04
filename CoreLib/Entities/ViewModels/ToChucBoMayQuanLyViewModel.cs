using CoreLib.Entities.ModelViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Entities.ViewModels
{
    public class ToChucBoMayQuanLyViewModel
    {
        public int? ID { get; set; }
        public int? OrgModelID { get; set; }
        public int? CpnyID { get; set; }
        public int? MenuID { get; set; }
        public string OrgModelDesc { get; set; }
        public string OrgModelPath { get; set; }
        public string OrgModelDescNote { get; set; }
        public int? OrgType { get; set; }
        public string NOTE { get; set; }
        public int? APPROVE { get; set; }
        public string RoleCode { get; set; }
        public string UserLogin { get; set; }

        public IEnumerable<ToChucBoMayQuanLyModelView> ToChucBoMayQuanLys { get; set; }

        public ToChucBoMayQuanLyModelView ToChucBoMayQuanLy { get; set; }
        public string imagelogo { get; set; }

        public List<ChangeInfo> ChangeInfos { get; set; }

        // prop gửi mail
        public EmailSettings EmailSettings { get; set; }
    }
}
