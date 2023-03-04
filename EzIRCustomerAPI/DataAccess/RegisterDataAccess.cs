using EzIRCustomerAPI.DataAccess.Implementations;
using EzIRCustomerAPI.DataAccess.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EzIRCustomerAPI.DataAccess
{
    public static class RegisterDataAccess
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {
            services.AddTransient<ILoginContext, LoginContext>();
            services.AddTransient<ICustomerContext, CustomerContext>();
            services.AddTransient<IInfoSheetContext, InfoSheetContext>();
            services.AddTransient<ICommonTypeContext, CommonTypeContext>();
            services.AddTransient<ICongBoThongTinContext, CongBoThongTinContext>();
            services.AddTransient<ITemplateContext, TemplateContext>();
            services.AddTransient<IToChucBoMayQuanLyContext, ToChucBoMayQuanLyContext>();
            services.AddTransient<ICoCauSoHuuContext, CoCauSoHuuContext>();
            services.AddTransient<IManagerOrgContext, ManagerOrgContext>();
            services.AddTransient<IManagerContext, ManagerContext>();
            services.AddTransient<IKnowLedgeLevelContext, KnowLedgeLevelContext>();
            services.AddTransient<ICoCauSoHuuContext, CoCauSoHuuContext>();

            services.AddTransient<IChangeInfoContext, ChangeInfoContext>();
            services.AddTransient<ICompanyContext, CompanyContext>();
            services.AddTransient<IThuVienPhapLuatContext, ThuVienPhapLuatContext>();

            return services;
        }
    }
}
