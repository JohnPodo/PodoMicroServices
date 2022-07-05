using MicroServices.LogService.Common;
using MicroServices.LogService.DAL;
using MicroServices.LogService.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.LogService.Services
{
    public class LogFetcher
    {
        private readonly LogContext _context;
        private readonly LogService _service;

        public LogFetcher(LogContext context,LogService service)
        {
            _service = service;
            _context = context;
            _service.Initialize("LogFetcher", Guid.NewGuid());
        }

        public async Task<BaseDataResponse<List<Log>>> GetLogs(int appId)
        {
            if (appId == 0) return new BaseDataResponse<List<Log>>("No App has Id of 0");
            if (_context is null) return new BaseDataResponse<List<Log>>("Database Context is null");
            if (_context.Logs is null) return new BaseDataResponse<List<Log>>("Logs Db Set is null");
            var logs = await _context.Logs.Where(l=>l.AppId == appId).AsNoTracking().ToListAsync();
            return new BaseDataResponse<List<Log>>(logs);
        }

        public async Task<BaseDataResponse<List<Log>>> GetLogs(Guid session, int appId)
        {
            if (session == Guid.Empty) return new BaseDataResponse<List<Log>>("No Sessiong was provided");
            if (_context is null) return new BaseDataResponse<List<Log>>("Database Context is null");
            if (_context.Logs is null) return new BaseDataResponse<List<Log>>("Logs Db Set is null");
            var logs = await _context.Logs.Where(l => l.GroupSession == session).Where(l => l.AppId == appId).AsNoTracking().ToListAsync();
            return new BaseDataResponse<List<Log>>(logs);
        }

        public async Task<BaseDataResponse<List<Log>>> GetLogs(int appId,Severity severity)
        {
            if (appId == 0) return new BaseDataResponse<List<Log>>("No App has Id of 1");
            if (_context is null) return new BaseDataResponse<List<Log>>("Database Context is null");
            if (_context.Logs is null) return new BaseDataResponse<List<Log>>("Logs Db Set is null");
            var logs = await _context.Logs.Where(l => l.AppId == appId).Where(l => l.Severity == severity).AsNoTracking().ToListAsync();
            return new BaseDataResponse<List<Log>>(logs);
        }
    }
}
