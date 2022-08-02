using Microsoft.EntityFrameworkCore;
using PodoMicroServices.DAL;
using PodoMicroServices.Services;
using PodoMicroServices.Services.EmailServices;
using PodoMicroServices.Services.FileServices;
using PodoMicroServices.Services.LogServices;
using PodoMicroServices.Services.SecretServices;

namespace PodoMicroServices.ConfigureServices
{
    public static class BaseConfigure
    {
        public static void ConfigureCustomServices(this IServiceCollection services, string conString)
        {
            services.AddDbContext<PodoMicroServiceContext>(options => options.UseSqlServer(conString));
            services.AddTransient<EmailService>();
            services.AddTransient<UserService>();
            ConfigureLogServices(services);
            ConfigureFileServices(services);
            ConfigureSecretServices(services);
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
        public static void ConfigureSecretServices(this IServiceCollection services)
        {
            services.AddTransient<SecretService>();
            services.AddHostedService<SecretCleaner>();
        }
    }
}
