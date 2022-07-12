using MicroServices.LogService.Common;

namespace MicroServices.LogService.Services
{
    public class LogCleaner : BackgroundService
    {
        private LogService? _service;

        private PeriodicTimer Timer { get; set; }
        public LogCleaner()
        {
            Timer = new PeriodicTimer(TimeSpan.FromHours(12));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await Timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                await WriteToLog("Cleaner Started", Severity.Information);
                _service = new LogService(new DAL.LogContext());
                var result = await _service.DeleteLogs(); 
                await WriteToLog($"Success : {result.Item1}", result.Item1 ? Severity.Information : Severity.Error);
                await WriteToLog($"Messages : {result.Item2}", result.Item1 ? Severity.Information : Severity.Error);
                _service = null;
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
