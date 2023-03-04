using AuthenticateInternal;
using CommonLib;
using DataServiceLib;
using EzIRSpecialist.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace EzIRSpecialist
{
    public class Startup
    {
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
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddResponseCaching(options =>
            {
                options.MaximumBodySize = 1024; // Cache responses with a body size smaller than or equal to 1,024 bytes.
                options.UseCaseSensitivePaths = true; // Store the responses by case-sensitive paths. For example, /page1 and /Page1 are stored separately.
            });

            // Đa ngôn ngữ
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options => options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResource)))
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddHttpContextAccessor();
            services.AddSingleton(Configuration);
            services.AddCommonLib(Configuration);
            services.AddMemoryCache();            
            services.AddDataAccess();
            services.AddDataServices();
            services.AddSession();

            services.AddControllersWithViews();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == Environments.Development)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseStaticFiles();

            // Tạo các thư mục file tĩnh            
            Directory.CreateDirectory(Configuration["LogPath"]);

            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(
            //        Path.Combine(Directory.GetCurrentDirectory(), Configuration["UploadPath"])),
            //    RequestPath = "/" + Configuration["UploadPath"],
            //});


            app.UseCookiePolicy();

            app.UseSession();

            app.UseRouting();

            app.UseResponseCaching();

            app.UseAuthorization();

            // cho phép hiển thị file logo banner

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
                Path.Combine(Directory.GetCurrentDirectory(), this.Configuration["FileSendMailTemplatePath"])),
                RequestPath = "/" + this.Configuration["FileSendMailTemplatePath"],
                EnableDirectoryBrowsing = true
            });

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), this.Configuration["logopath"])),
                RequestPath = "/" + this.Configuration["logopath"],
                EnableDirectoryBrowsing = true
            });
            



              var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("vi-VN"),
                //new CultureInfo("en"),
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("vi-VN"),

                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,

                // UI strings that we have localized.
                SupportedUICultures = supportedCultures,
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
