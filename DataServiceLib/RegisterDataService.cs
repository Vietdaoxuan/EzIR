using DataServiceLib.Implementations;
using DataServiceLib.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DataServiceLib
{
    public static class RegisterDataService
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddTransient<ICBaseDataProvider, CBaseDataProvider>();

            return services;
        }
    }
}
