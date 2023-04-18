using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using Services;
using UnitOfWork.Database;
using UnitOfWork.Interface;

namespace Product.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            
            services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                                new HeaderApiVersionReader("x-api-version"),
                                                                new MediaTypeApiVersionReader("x-api-version"));
            });

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Sales Website API",
                        Version = "1.0",
                        Description = "Sales Website API"
                    });
               });

            services.AddInfrastructureServices();
            return services;
        }

        private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWorkDatabase>();
            services.AddTransient<IProductServices, ProductServices>();
            return services;
        }
    }
}
