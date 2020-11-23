using System;
using BaGet.Core;
using BaGet.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace BaGet
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddBaGetWebApplication(
            this IServiceCollection services,
            Action<BaGetApplication> configureAction)
        {
            services
                .AddControllers()
                .AddApplicationPart(typeof(PackageContentController).Assembly)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });

            services.AddHttpContextAccessor();
            services.AddTransient<IUrlGenerator, BaGetUrlGenerator>();

            services.AddBaGetApplication(configureAction);
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add<MySampleActionFilter>();
            });

            return services;
        }

        public class MySampleActionFilter : IActionFilter
        {
            public void OnActionExecuted(ActionExecutedContext context)
            {
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                // Do something before the action executes.
                Console.WriteLine($"Action executing: {context.ActionDescriptor.DisplayName} {context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}");
            }
        }
    }
}
