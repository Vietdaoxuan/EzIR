using CommonLib.Implementations;
using CommonLib.Interfaces;
using CoreLib.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Text;

namespace CommonLib
{
    public static class RegisterCommonLib
    {
        /// <summary>
        ///  them ghi log ra file va cac ham tien ich.
        /// </summary>
        /// <param name="services">services.</param>
        /// <param name="configuration">config.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddCommonLib(this IServiceCollection services, IConfiguration configuration)
        {
            // tạo logger.
            if (configuration != null)
            {
                ILogger logger = new LoggerConfiguration()
                    .WriteTo.File(configuration["LogFilePath"], encoding: Encoding.UTF8, rollingInterval: RollingInterval.Day)
                    .CreateLogger();

                // chỉ có 1 logger trên toàn bộ ứng dụng
                services.AddSingleton(logger);
            }

            services.AddSingleton<ISerilogProvider, CSerilog>();

            services.AddTransient<ICommon, CCommon>();

            services.AddSingleton<IErrorHandler, CErrorHandler>();            

            return services;
        }
    }
}
