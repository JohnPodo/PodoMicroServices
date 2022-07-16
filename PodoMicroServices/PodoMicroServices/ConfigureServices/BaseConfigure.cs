using Microsoft.EntityFrameworkCore;
using PodoMicroServices.DAL;
using PodoMicroServices.Services.FileServices;
using PodoMicroServices.Services.LogServices;

namespace PodoMicroServices.ConfigureServices
{
    public static class BaseConfigure
    {
        public static void ConfigureCustomServices(this IServiceCollection services,string conString)
        { 
            services.AddDbContext<PodoMicroServiceContext>(options => options.UseSqlServer(conString));
            ConfigureLogServices(services);
            ConfigureFileServices(services);
        }

        public static void ConfigureLogServices(this IServiceCollection services)
        {
            services.AddTransient<LogService>();
            services.AddHostedService<LogCleaner>(); 
        }

        public static void ConfigureFileServices(this IServiceCollection services)
        {
            services.AddTransient<FileService>(); 
        }
    }
}
