using MicroServices.LogService.DAL;
using MicroServices.LogService.Services;

namespace MicroServices.LogService
{
    public static class ServicesExtension
    {
        public static void ConfigureCustomServices(this IServiceCollection services)
        {
            services.AddDbContext<LogContext>();
            services.AddTransient<Services.LogService>();
            services.AddTransient<Services.LogFetcher>();
        }
    }
}
