using CoreLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRSpecialist.DataAccess.Interfaces
{
    public interface IApproveContext
    {
        /// <summary>
        /// Danh sách các công ty bị thay đổi thông tin 
        /// </summary>
        /// <returns>IEnumerable<ComapnyApprove></returns>
        IEnumerable<CompanyApprove> GetListCompanyApprove();

        /// <summary>
        /// Danh sách các danh mục bị thay đổi thông tin
        /// </summary>
        /// <param name="cpnyID"></param>
        /// <returns>IEnumerable<FunctionApprove></returns>
        IEnumerable<FunctionApprove> GetListFunctionApprove(int cpnyID);

        /// <summary>
        /// Thông tin các bản ghi, trường bị thay đổi
        /// </summary>
        /// <param name="cpnyID"></param>
        /// <returns>IEnumerable<ChangeInfo></returns>
        IEnumerable<ChangeInfo> GetListChangeInfo(int cpnyID);

        /// <summary>
        /// Phê duyệt thông tin free-text, infosheet
        /// </summary>
        /// <param name="cpnyID"></param>
        /// <param name="menuID"></param>
        /// <param name="approve"></param>
        /// <param name="authorID"></param>
        /// <param name="modifierID"></param>
        /// <returns>CResponseMessage</returns>
        CResponseMessage ApproveInfoSheet(int cpnyID, int menuID, int approve, int authorID, int modifierID);

        /// <summary>
        /// Phê duyệt thông tin Thông tin chung
        /// </summary>
        /// <param name="cpnyID"></param>
        /// <param name="menuID"></param>
        /// <param name="approve"></param>
        /// <returns>CResponseMessage</returns>
        CResponseMessage ApproveInfoCompany(int cpnyID, int menuID, int approve);

        /// <summary>
        /// Phê duyệt các mốc lịch sử
        /// </summary>
        /// <param name="cpnyID"></param>
        /// <param name="menuID"></param>
        /// <param name="approve"></param>
        /// <returns>CResponseMessage</returns>
        CResponseMessage ApproveDevelopprogess(int cpnyID, int menuID, int approve);

        /// <summary>
        /// Phê duyệt thông tin Bộ máy quản lý
        /// </summary>
        /// <param name="cpnyID"></param>
        /// <param name="menuID"></param>
        /// <param name="approve"></param>
        /// <returns>CResponseMessage</returns>
        CResponseMessage ApproveOrgModel(int cpnyID, int menuID, int approve);

        /// <summary>
        /// Phê duyệt thông tin Thành phần lãnh đạo
        /// </summary>
        /// <param name="cpnyID"></param>
        /// <param name="menuID"></param>
        /// <param name="approve"></param>
        /// <returns>CResponseMessage</returns>
        CResponseMessage ApproveManager(int cpnyID, int menuID, int approve);

        /// <summary>
        /// Phê duyệt thông tin công ty con - công ty liên kết
        /// </summary>
        /// <param name="cpnyID"></param>
        /// <param name="menuID"></param>
        /// <param name="approve"></param>
        /// <returns>CResponseMessage</returns>
        CResponseMessage ApproveSubCompany(int cpnyID, int menuID, int approve);

        /// <summary>
        /// Phê duyệt thông tịn cổ đông
        /// </summary>
        /// <param name="cpnyID"></param>
        /// <param name="menuID"></param>
        /// <param name="approve"></param>
        /// <returns></returns>
        CResponseMessage ApproveCompanyHolder(int cpnyID, int menuID, int approve);
    }
}
