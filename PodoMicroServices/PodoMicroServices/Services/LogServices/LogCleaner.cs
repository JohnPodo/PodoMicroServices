using Microsoft.EntityFrameworkCore;
using PodoMicroServices.Models.LogModels;

namespace PodoMicroServices.Services.LogServices
{
    public class LogCleaner : BackgroundService
    {
        private LogService _service;

        private PeriodicTimer Timer { get; set; }
        public LogCleaner(IConfiguration config)
        {
            Timer = new PeriodicTimer(TimeSpan.FromHours(12));
            _service = new LogService(new DAL.PodoMicroServiceContext(new DbContextOptionsBuilder().UseSqlServer(config["Base:ConnectionString"]).Options));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await Timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                await WriteToLog("Cleaner Started", Severity.Information); 
                var result = await _service.DeleteLogs();
                await WriteToLog($"Success : {result.Item1}", result.Item1 ? Severity.Information : Severity.Error);
                await WriteToLog($"Messages : {result.Item2}", result.Item1 ? Severity.Information : Severity.Error);
                await WriteToLog("Cleaner Finished", Severity.Information);
            }
        }

        private async Task WriteToLog(string message, Severity severity)
        {
            if (_service is not null)
                _ = await _service.WriteToLog(message, severity);
        }
    }
}
