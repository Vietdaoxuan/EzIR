using System.IO;
using CommonLib;
using CoreLib.SharedKernel;
using CoreLib.Interfaces;
using CoreLib.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization.Routing;
using EzIRFront.DataAccess;

namespace EzIRFront
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
            services.AddResponseCaching(options =>
            {
                options.MaximumBodySize = 1024; // Cache responses with a body size smaller than or equal to 1,024 bytes.
                options.UseCaseSensitivePaths = true; // Store the responses by case-sensitive paths. For example, /page1 and /Page1 are stored separately.
            });



            //Đa ngôn ngữ
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options => options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResource)))
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddHttpContextAccessor();

            services.AddSingleton(Configuration);
            services.AddCommonLib(Configuration);
            services.AddTransient<IExportExcel, ExportExcel>();
            
            services.AddMemoryCache();
            services.AddSession();
            services.AddSingleton<IHttpService, HttpService>();

            services.AddControllersWithViews();


            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    options.DefaultRequestCulture = new RequestCulture("vi-VN");
            //    options.SupportedCultures = supportedCultures;
            //    options.SupportedUICultures = supportedCultures;
            //});

            services.Configure<RequestLocalizationOptions>(
            options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                   new CultureInfo("vi-VN"),
                   new CultureInfo("en-US"),
                };
                options.DefaultRequestCulture = new RequestCulture(culture: "vi-VN", uiCulture: "vi-VN");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                //options.RequestCultureProviders.Clear();
                //options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider());
                //options.RequestCultureProviders.Insert(1, new QueryStringRequestCultureProvider());
                //options.RequestCultureProviders.Insert(2, new CookieRequestCultureProvider());
                //options.RequestCultureProviders.Insert(3, new AcceptLanguageHeaderRequestCultureProvider());
                //services.AddSingleton(options);
            });

            // Cho phép chèn IFRAME
            services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);
            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(2440); options.Cookie.HttpOnly = true; });

            // Tắt block cookie bên thứ 3 (site mình) của IFRAME
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.OnAppendCookie = cookieContext =>
                    cookieContext.CookieOptions.SameSite = DisallowsSameSiteNone(cookieContext.Context) ? SameSiteMode.Unspecified : SameSiteMode.None;
                options.OnDeleteCookie = cookieContext =>
                    cookieContext.CookieOptions.SameSite = DisallowsSameSiteNone(cookieContext.Context) ? SameSiteMode.Unspecified : SameSiteMode.None;
                options.Secure = CookieSecurePolicy.Always; // required for chromium-based browsers

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //  app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Tạo các thư mục file tĩnh
            //Directory.CreateDirectory(Configuration["UploadPath"]);
            //Directory.CreateDirectory(Configuration["ImportExcelPath"]);
            Directory.CreateDirectory(Configuration["LogPath"]);

            app.UseRouting();

            app.UseResponseCaching();

            app.UseAuthorization();
            app.UseCookiePolicy();

            //app.UseRequestLocalization(new RequestLocalizationOptions
            //{
            //    DefaultRequestCulture = new RequestCulture(supportedCultures[0]),
            //    // Formatting numbers, dates, etc.
            //    SupportedCultures = supportedCultures,
            //    // UI strings that we have localized.
            //    SupportedUICultures = supportedCultures,
            //    RequestCultureProviders = new List<IRequestCultureProvider>
            //    {

            //        new QueryStringRequestCultureProvider (),
            //        new CookieRequestCultureProvider(),
            //        new AcceptLanguageHeaderRequestCultureProvider()
            //    }
            //});


            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(supportedCultures[0]),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures,
                RequestCultureProviders = new List<IRequestCultureProvider>
                {

                    new QueryStringRequestCultureProvider (),
                    //new CookieRequestCultureProvider(),
                    //new AcceptLanguageHeaderRequestCultureProvider()
                }
            });
            // cho phép hiển thị file logo banner


            //app.UseFileServer(new FileServerOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //    Path.Combine(Directory.GetCurrentDirectory(), this.Configuration["SpecialistPath"]).Replace("EzIRFront\\", "")),
            //    RequestPath = "/" + this.Configuration["SpecialistPath"],
            //    EnableDirectoryBrowsing = true
            //});



            app.UseSession();

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
               Path.Combine(Directory.GetCurrentDirectory(), this.Configuration["LogoBannerPath"])),
                RequestPath = "/" + this.Configuration["LogoBannerPath"],
                EnableDirectoryBrowsing = true
            });

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
     Path.Combine(Directory.GetCurrentDirectory(), this.Configuration["UploadFPA"])),
                RequestPath = "/" + this.Configuration["UploadFPA"],
                EnableDirectoryBrowsing = true
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller=ThongTinDoanhNghiep}/{astock_CODE}",
                     defaults: new { controller = "ThongTinDoanhNghiep", action = "Index" });
                endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=ThongTinDoanhNghiep}/{astock_CODE}/{culture}",
                    defaults: new { controller = "ThongTinDoanhNghiep", action = "Index" });
                endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }

        //Kiểm tra và cho phép các thiết bị sử dụng bên IFRAME sử dụng cookie bên thứ 3 (site mình)
        private static bool DisallowsSameSiteNone(HttpContext httpContext)
        {
            var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

            if (string.IsNullOrEmpty(userAgent))
            {
                return false;
            }

            // Cover all iOS based browsers here. This includes:
            // - Safari on iOS 12 for iPhone, iPod Touch, iPad
            // - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
            // - Chrome on iOS 12 for iPhone, iPod Touch, iPad
            // All of which are broken by SameSite=None, because they use the iOS networking stack
            if (userAgent.Contains("CPU iPhone OS 12") || userAgent.Contains("iPad; CPU OS 12"))
            {
                return true;
            }

            // Cover Mac OS X based browsers that use the Mac OS networking stack. This includes:
            // - Safari on Mac OS X.
            // This does not include:
            // - Chrome on Mac OS X
            // Because they do not use the Mac OS networking stack.
            if (userAgent.Contains("Macintosh; Intel Mac OS X 10_14") &&
                userAgent.Contains("Version/") && userAgent.Contains("Safari"))
            {
                return true;
            }

            // Cover Chrome 50-69, because some versions are broken by SameSite=None, 
            // and none in this range require it.
            // Note: this covers some pre-Chromium Edge versions, 
            // but pre-Chromium Edge does not require SameSite=None.
            if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
            {
                return true;
            }

            return false;
        }
    }
}
