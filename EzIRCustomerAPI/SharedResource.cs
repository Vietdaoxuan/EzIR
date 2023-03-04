using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRCustomerAPI
{
    /// <summary>
    /// Đa ngôn ngữ
    /// Class để nhóm tất cả các string lại trong 1 file resource duy nhất , inject  IStringLocalizer<SharedResource> sharedLocalizer
    /// để sử dụng , VD: sharedLocalizer['LOGIN']
    /// nếu trong view thì sử dụng @inject IHtmlLocalizer<SharedResource> SharedLocalizer
    /// link : https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-2.2
    /// </summary>
    public class SharedResource
    {
    }
}
