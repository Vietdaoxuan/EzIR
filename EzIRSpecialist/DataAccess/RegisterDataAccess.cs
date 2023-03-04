using CommonLib.Implementations;
using CoreLib.Interfaces;
using CoreLib.Services;
using EzIRCustomerAPI.Services;
using EzIRSpecialist.DataAccess.Implementations;
using EzIRSpecialist.DataAccess.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EzIRCustomerAPI.Services.IEmailSender;

namespace EzIRSpecialist.DataAccess
{
    public static class RegisterDataAccess
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {
            services.AddTransient<IUserContext, UserContext>();
            services.AddTransient<IGroupContext, GroupContext>();
            services.AddTransient<IUserGroupContext, UserGroupContext>();
            services.AddTransient<IRoleGroupContext, RoleGroupContext>();
            services.AddTransient<ISystemManagementContext, SystemManagementContext>();
            services.AddTransient<IBannerContext, BannerContext>();
            services.AddTransient<IChuyenVienContext, ChuyenVienContext>();
            services.AddTransient<INhatKyHoatDongContext, NhatKyHoatDongContext>();
            services.AddTransient<IQLTKDoanhNghiepContext, QLTKDoanhNghiepContext>();
            services.AddTransient<ICommonTypeContext, CommonTypeContext>();
            services.AddTransient<ICompanyContext, CompanyContext>();

            services.AddTransient<IDoanhNghiepContext, DoanhNghiepContext>();
            services.AddTransient<INhacCongBoThongTinContext, NhacCongBoThongTinContext>(); 
            services.AddTransient<IDoanhNghiepCongBoContext, DoanhNghiepCongBoContext>(); 
            services.AddTransient<ITemplateContext, TemplateContext>(); 
            services.AddTransient<IThuVienPhapLuatContext, ThuVienPhapLuatContext>(); 


            services.AddTransient<IRulesContext, RulesContext>();
            services.AddTransient<IErrorHandler, CErrorHandler>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddTransient<ISupportContext, SupportContext>();
            services.AddTransient<IDSTinDangTheoKhachHangContext, DSTinDangTheoKhachHangContext>();
            services.AddTransient<IDSTinDangBackDateContext, DSTinDangBackDateContext>();
            services.AddTransient<IThongTinPheDuyetContext, ThongTinPheDuyetContext>();

            services.AddTransient<IApproveContext, ApproveContext>();

            services.AddTransient<IHttpService, HttpService>();
            return services;
        }
    }
}
