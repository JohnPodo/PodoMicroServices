using Microsoft.EntityFrameworkCore;
using PodoMicroServices.Models.LogModels;
using PodoMicroServices.Services.LogServices;

namespace PodoMicroServices.Services
{
    public abstract class SuperCleaner : BackgroundService
    {
        protected LogService _logService; 
        private PeriodicTimer Timer { get; set; }
        public SuperCleaner(IConfiguration config)
        {
            Timer = new PeriodicTimer(TimeSpan.FromHours(12));
            _logService = new LogService(new DAL.PodoMicroServiceContext(new DbContextOptionsBuilder().UseSqlServer(config["Base:ConnectionString"]).Options));
            _logService.Initialize(this.GetType().Name, Guid.NewGuid());
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await Timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await WriteToLog("Cleaner Started", Severity.Information);
                    await LocalExecution();
                    await WriteToLog("Cleaner Finished", Severity.Information);
                }
                catch (Exception ex)
                {
                    await WriteToLog($"Exception Caught with message {ex.Message}", Severity.Fatal);
                }
            }
        }

        protected abstract Task LocalExecution();

        protected async Task WriteToLog(string message, Severity severity)
        {
            if (_logService is not null)
                _ = await _logService.WriteToLog(message, severity);
        }
    }
}
