using PodoMicroServices.Common;

namespace PodoMicroServices.Services.LogServices
{
    public class LogCleaner : SuperCleaner
    {
        public LogCleaner(IConfiguration config) : base(config)
        {
        }

        protected override async Task LocalExecution()
        {
            var result = await _logService.DeleteLogs();
            await WriteToLog($"Success : {result.Item1}", result.Item1 ? Severity.Information : Severity.Error);
            await WriteToLog($"Messages : {result.Item2}", result.Item1 ? Severity.Information : Severity.Error);
        }
    }
}
