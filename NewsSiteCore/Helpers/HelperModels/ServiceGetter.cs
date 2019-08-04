using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.HelperModels.DependencyModels.Interfaces;
using System;

namespace Models.HelperModels
{
    public static class ServiceGetter
    {
        public static IHttpContextAccessor HttpContextAccessor { get; private set; }
        public static IServiceProviderAccessor ServiceProviderAccessor { get; private set; }

        public static IConfiguration Configuration { get; private set; }

        private static IServiceProvider ServiceProvider =>
            ServiceProviderAccessor?.ServiceProvider
                    ?? HttpContextAccessor?.HttpContext?.RequestServices;

        public static void SetAccessor(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;

            if (Configuration == null)
            {
                Configuration = GetService<IConfiguration>();
            }
        }
        public static void SetAccessor(IServiceProviderAccessor serviceProviderAccessor)
        {
            ServiceProviderAccessor = serviceProviderAccessor;

            if (Configuration == null)
            {
                Configuration = GetService<IConfiguration>();
            }
        }

        public static T GetService<T>()
            where T : class
        {
            return ServiceProvider?.GetService<T>();
        }
    }
}
