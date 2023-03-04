using CoreLib.Entities;
using EzIRSpecialist.Models.Support;
using EzIRSpecialist.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Interfaces
{
    public interface ISupportContext
    {
        Task<CResponseMessage> XuLyDongBo(string Loai);

        public Task<IEnumerable<JobsStatus>> Select(JobsStatusViewModel jobsStatusViewModel);
    }
}