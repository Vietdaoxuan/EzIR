using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommonLib;
using CommonLib.Interfaces.Logger;
using CoreLib.Configs;
using CoreLib.Interfaces;
using CoreLib.Services;
using CoreLib.SharedKernel;
using EzIRCustomer.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace EzIRCustomer
{
    public class Startup
    {
        private List<CultureInfo> supportedCultures = new List<CultureInfo> { new CultureInfo("vi-VN"), new CultureInfo("en-US") };

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConfigProvider.Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                //options.CheckConsentNeeded = context => true;

                // Cho biết khách hàng chỉ nên gửi cookie với các yêu cầu "cùng trang web".
                options.MinimumSameSitePolicy = SameSiteMode.None;

                // HttpOnly có tác dụng làm cho cookie chỉ được thao tác bởi server mà không bị thao tác bởi các script phía người dùng.
                options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
             
                // chỉ truy cập cookie từ các máy chủ có giao thức an toàn HTTPS
                //  options.Secure = CookieSecurePolicy.Always;
            });

            //Lấy địa chỉ ip user
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy => { policy.AllowAnyHeader().AllowAnyMethod(); });
            });

            services.AddHttpContextAccessor();

            services.AddCommonLib(Configuration);

            services.AddSingleton<IHttpService, HttpService>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    // Cookie settings
                    options.Cookie.HttpOnly = true;
                    options.LoginPath = "/Login";
                    options.AccessDeniedPath = "/";
                });

            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(2440); options.Cookie.HttpOnly = true; });

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMemoryCache();

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options => options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResource)))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("vi-VN");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAppLogger appLogger, IHttpContextAccessor _httpContextAccessor)
        {
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(supportedCultures[0]),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures,
                RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new CookieRequestCultureProvider()
                }
            });

            app.UseCors();

            app.UseCookiePolicy();

            app.UseSession();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            //CurrentDirectoryHelpers.SetCurrentDirectory();

            // Tạo các thư mục file tĩnh
            Directory.CreateDirectory(Configuration["UploadImagePath"]);
            Directory.CreateDirectory(Configuration["UploadFilePath"]);
            Directory.CreateDirectory(Configuration["LogPath"]);

            // cho phép hiển thị file img
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), this.Configuration["UploadOrgModelPath"])),
                RequestPath = "/" + this.Configuration["UploadOrgModelPath"],
                EnableDirectoryBrowsing = true
            });

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), this.Configuration["UploadPath"])),
                RequestPath = "/" + this.Configuration["UploadPath"],
                EnableDirectoryBrowsing = true
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.Use(next => context =>
            {
                try
                {
                    //giá trị để check session trước khi gán
                    var session_before = context.Session.GetString(SessionTypes.TOKEN);
                    //bỏ comment khi muốn authen
                    if (!string.IsNullOrEmpty(context.Request.Cookies[CookieTypes.TOKEN]))
                    {
                        context.Session.SetString(SessionTypes.TOKEN, context.Request.Cookies[CookieTypes.TOKEN]);

                        // Kiểm tra nếu cookies rỗng thì không gán                                 
                        if (context.Request.Cookies[CookieTypes.USERNAME] != null
                            && context.Request.Cookies[CookieTypes.FULL_NAME] != null
                            && context.Request.Cookies[CookieTypes.EMAIL] != null
                            && context.Request.Cookies[CookieTypes.CPNY_ID] != null
                            && context.Request.Cookies[CookieTypes.STOCK_CODE] != null
                            && context.Request.Cookies[CookieTypes.STOCK_NAME] != null
                            && context.Request.Cookies[CookieTypes.COMPANY_TYPE] != null
                            && context.Request.Cookies[CookieTypes.COMPANY_TYPE_NAME] != null
                            && context.Request.Cookies[CookieTypes.MAIL_SPECIALIST] != null
                            && context.Request.Cookies[CookieTypes.MAIL_SPECIALISTCC] != null
                            && context.Request.Cookies[CookieTypes.EXPERT_NAME] != null
                            && context.Request.Cookies[CookieTypes.EXPERT_PHONE] != null
                            && context.Request.Cookies[CookieTypes.SEED] != null)
                        {
                            context.Session.SetString(SessionTypes.USERNAME, context.Request.Cookies[CookieTypes.USERNAME]);
                            context.Session.SetString(SessionTypes.FULL_NAME, context.Request.Cookies[CookieTypes.FULL_NAME]);
                            context.Session.SetString(SessionTypes.EMAIL, context.Request.Cookies[CookieTypes.EMAIL]);
                            context.Session.SetString(SessionTypes.CPNY_ID, context.Request.Cookies[CookieTypes.CPNY_ID]);
                            context.Session.SetString(SessionTypes.STOCK_CODE, context.Request.Cookies[CookieTypes.STOCK_CODE]);
                            context.Session.SetString(SessionTypes.STOCK_NAME, context.Request.Cookies[CookieTypes.STOCK_NAME]);
                            context.Session.SetString(SessionTypes.COMPANY_TYPE, context.Request.Cookies[CookieTypes.COMPANY_TYPE]);
                            context.Session.SetString(SessionTypes.COMPANY_TYPE_NAME, context.Request.Cookies[CookieTypes.COMPANY_TYPE_NAME]);
                            context.Session.SetString(SessionTypes.MAIL_SPECIALIST, context.Request.Cookies[CookieTypes.MAIL_SPECIALIST]);
                            context.Session.SetString(SessionTypes.MAIL_SPECIALISTCC, context.Request.Cookies[CookieTypes.MAIL_SPECIALISTCC]);
                            context.Session.SetString(SessionTypes.EXPERT_NAME, context.Request.Cookies[CookieTypes.EXPERT_NAME]);
                            context.Session.SetString(SessionTypes.EXPERT_PHONE, context.Request.Cookies[CookieTypes.EXPERT_PHONE]);
                            context.Session.SetString(SessionTypes.SEED, context.Request.Cookies[CookieTypes.SEED]);

                            // IHttpContextAccessor(nếu dùng IHttpContextAccessor để set session thì sẽ dùng được trong Injection)
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.TOKEN, context.Request.Cookies[CookieTypes.TOKEN]);
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.USERNAME, context.Request.Cookies[CookieTypes.USERNAME]);
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.FULL_NAME, context.Request.Cookies[CookieTypes.FULL_NAME]);
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.EMAIL, context.Request.Cookies[CookieTypes.EMAIL]);
                            _httpContextAccessor.HttpContext.Session.SetInt32(SessionTypes.CPNY_ID, int.Parse(context.Request.Cookies[CookieTypes.CPNY_ID] ?? "0"));
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.STOCK_NAME, context.Request.Cookies[CookieTypes.STOCK_NAME]);
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.STOCK_CODE, context.Request.Cookies[CookieTypes.STOCK_CODE]);
                            _httpContextAccessor.HttpContext.Session.SetInt32(SessionTypes.COMPANY_TYPE, int.Parse(context.Request.Cookies[CookieTypes.COMPANY_TYPE] ?? "0"));
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.COMPANY_TYPE_NAME, context.Request.Cookies[CookieTypes.COMPANY_TYPE_NAME]);
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.COMPANY_TYPE_NAMEEN, context.Request.Cookies[CookieTypes.COMPANY_TYPE_NAMEEN] ?? "");
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.MAIL_SPECIALIST, context.Request.Cookies[CookieTypes.MAIL_SPECIALIST]);
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.MAIL_SPECIALISTCC, context.Request.Cookies[CookieTypes.MAIL_SPECIALISTCC]);
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.EXPERT_NAME, context.Request.Cookies[CookieTypes.EXPERT_NAME]);
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.EXPERT_PHONE, context.Request.Cookies[CookieTypes.EXPERT_PHONE]);
                            _httpContextAccessor.HttpContext.Session.SetString(SessionTypes.SEED, context.Request.Cookies[CookieTypes.SEED]);
                        }

                    }

                    //giá trị để check session sau khi gán
                    var session_after = context.Session.GetString(SessionTypes.TOKEN);

                    //appLogger.InfoLogger.LogInfo("Session:" + session);

                    if (string.IsNullOrEmpty(session_before) && session_after != session_before)
                    {
                        // bỏ đăng nhập khi session rỗng
                        context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        context.Session.Clear();

                        // nếu 
                        if (!context.Request.Path.Value.ToLower().Contains("login") && !context.Request.Path.Value.ToLower().Contains("forgotpassword"))
                            context.Response.Redirect("/login");

                    }

                    return next(context);
                }
                catch (Exception ex)
                {
                    appLogger.ErrorLogger.LogError(ex);
                    context.Response.Redirect("/login");
                    return next(context);
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
