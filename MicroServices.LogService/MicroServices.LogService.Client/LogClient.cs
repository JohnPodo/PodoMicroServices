using MicroServices.LogService.Common;
using Newtonsoft.Json;
using RestSharp;

namespace MicroServices.LogService.Client
{
    public class LogClient
    {
        private readonly string _url;
        private readonly int _appId;
        private readonly string _name;
        private readonly Guid _session;

        public LogClient(string url, int appId,string name)
        {
            _url = url;
            _appId = appId;
            _name = name;
            _session = Guid.NewGuid();
        }

        public async Task<BaseDataResponse<List<LogDto>>?> GetLogs()
        {
            return await ExecuteRequest<BaseDataResponse<List<LogDto>>>("/GetLogs", null, Method.Get);
        }

        public async Task<BaseDataResponse<List<LogDto>>?> GetLogs(Guid session)
        {
            return await ExecuteRequest<BaseDataResponse<List<LogDto>>>($"/GetLogsPerSession/{session}", null, Method.Get);
        }

        public async Task<BaseDataResponse<List<LogDto>>?> GetLogs(Severity severity)
        {
            return await ExecuteRequest<BaseDataResponse<List<LogDto>>>($"/GetLogsPerSeverity/{severity}", null, Method.Get);
        }

        public async Task<BaseResponse?> DeleteLog(int id)
        {
            return await ExecuteRequest<BaseResponse>($"/DeleteLog/{id}", null, Method.Delete);
        }
        public async Task<BaseResponse?> WriteToLog(string message, Severity severity)
        {
            if (string.IsNullOrEmpty(message)) return new BaseResponse("Parameter:message came null");
            if (severity == Severity.NoLogging) return new BaseResponse("Parameter:severity came NoLogging");
            var dto = new LogDto();
            dto.AppId = _appId;
            dto.GroupSession = _session;
            dto.Severity = severity;
            dto.Message = message;
            dto.Name = _name; 
            return await ExecuteRequest<BaseResponse>($"/WriteLog", dto, Method.Post);
        }

        private async Task<T?> ExecuteRequest<T>(string extension, object? body, Method method) where T : class
        {
            try
            {
                var client = new RestClient(_url);
                var request = new RestRequest(extension, method);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("appId", _appId);
                if (body is not null)
                    request.AddBody(body);
                var result = await client.ExecuteAsync(request);
                if (result is not null)
                    if (!string.IsNullOrEmpty(result.Content))
                        return JsonConvert.DeserializeObject<T>(result.Content);
                return null;
            }
            catch
            {
                return null;
            }

        }
    }
}