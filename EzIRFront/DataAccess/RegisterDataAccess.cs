using CommonLib.Implementations;
using CoreLib.Interfaces;
using CoreLib.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzIRFront.DataAccess
{
    public static class RegisterDataAccess
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {
            
            services.AddTransient<IHttpService, HttpService>();
            return services;
        }
    }
}
