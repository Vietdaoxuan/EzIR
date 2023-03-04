using System;
using System.Collections.Generic;
using System.Text;

namespace DataServiceLib.Interfaces
{
    /// <summary>
    /// Mỗi ss khi thực thi trong db đều phải tryền vào roleCode - role của controller đang sử dụng và username của người dùng
    /// để ghi log và check quyền.
    /// </summary>
    public interface IRequiredDataSP
    {
        /// <summary>
        /// Gán username.
        /// </summary>
        /// <param name="username"></param>
        void SetUsername(string username);
    }
}
