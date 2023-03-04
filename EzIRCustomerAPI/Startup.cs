using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;
using CoreLib.SharedKernel;
using DataServiceLib;
using EzIRCustomerAPI.DataAccess;
using EzIRCustomerAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

using static EzIRCustomerAPI.Services.IEmailSender;

namespace EzIRCustomerAPI
{
    public class Startup
    {

        public Microsoft.AspNetCore.Hosting.IHostingEnvironment HostingEnvironment { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            this.HostingEnvironment = env;
            Configuration = configuration;
            //ConfigProvider.Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddJwtAuthentication(Configuration);

            services.AddDataServices();

            services.AddCommonLib(Configuration);

            services.AddDataAccess();

            // ngăn không cho api tự động response bad request khi model state invalid
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            // tùy chọn response xml hoặc json tùy theo accept header của client
            services.AddMvc().AddXmlSerializerFormatters().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //services.AddSwaggerGen();

            services.AddSwaggerGen(c =>
            {
                //c.DescribeAllEnumsAsStrings();
                c.DescribeAllParametersInCamelCase();
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "EzIRCustomer API", Version = "v1.0" });
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement());

                //  c.IncludeXmlComments($@"{AppContext.BaseDirectory}\EzGsmApi.XML");
                //   c.IncludeXmlComments($@"{AppContext.BaseDirectory}\CoreLib.XML");
            });

            // Cấu hình các doamin được phép gọi đến API 
            services.AddCors(options =>
            {
                //options.AddPolicy("CorsPolicy", builder => builder.SetIsOriginAllowed(hostname => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
                //options.AddPolicy("CorsPolicy", builder => builder.WithOrigins("http://ezir.fpts.com.vn/", "http://localhost:5995", "https://localhost:44378/").AllowAnyMethod().AllowAnyHeader().AllowCredentials());                
                options.AddDefaultPolicy(builder => builder.WithOrigins("http://ezir.fpts.com.vn", "http://localhost:5991", "https://localhost:46378").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });

            services.AddControllers();

            // cài config mail server
            services.Configure<EmailSettings>(this.Configuration.GetSection("EmailSettings"));
            services.AddSingleton(Configuration.GetSection("EmailSettings").Get<EmailSettings>());
            services.AddSingleton<IEmailSender, EmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            //Tạo các thư mục file tĩnh            
            Directory.CreateDirectory(Configuration["LogFilePath"]);
            Directory.CreateDirectory(Configuration["LogPath"]);

            app.UseRouting();
            
            //app.UseCors("CorsPolicy");
            app.UseCors();

            //app.UseAuthorization();

            app.UseAuthentication();

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "EzIRCustomer API"); });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
