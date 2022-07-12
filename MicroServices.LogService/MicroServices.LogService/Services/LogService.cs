using MicroServices.LogService.Common;
using MicroServices.LogService.DAL;
using MicroServices.LogService.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroServices.LogService.Services
{
    public class LogService
    {
        private readonly LogContext _context;

        private string _name = string.Empty;
        private Guid _session = Guid.Empty;

        public LogService(LogContext context)
        {
            _context = context;
        }

        internal void Initialize(string name, Guid session)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }
            if (session == Guid.Empty)
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }
            _name = name;
            _session = session;

            _ = WriteToLog($"Logger {name} instanciated with group session : {session}", Severity.Information).Result;
        }

        private Log FormatNewLogModel(string message, Severity severity)
        {
            return new Log()
            {
                AppId = 1,
                Severity = severity,
                Created = DateTime.Now,
                Name = _name,
                GroupSession = _session,
                Message = message

            };
        }

        internal (bool, string) ValidateLogDto(LogDto dto)
        {
            if (dto is null) return (false, "Missing Argument");
            if (dto.GroupSession == Guid.Empty
                || dto.AppId < 2
                || dto.Severity == Severity.NoLogging
                || string.IsNullOrEmpty(dto.Name)
                || string.IsNullOrEmpty(dto.Message)
                ) return (false, "False Arguments");
            return (true, string.Empty);
        }

        internal async Task<(bool, string)> WriteToLog(string message, Severity severity)
        {
            return await WriteToLog(FormatNewLogModel(message, severity));
        }

        internal async Task<(bool, string)> WriteToLog(Log newLog)
        {
            try
            {
                if (newLog is null) return (false, "Object is null");
                if (_context is null) return (false, "Database Context is null");
                if (_context.Logs is null) return (false, "Logs Db Set is null");
                await _context.Logs.AddAsync(newLog);
                await _context.SaveChangesAsync();
                return (true, String.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"Exception Caught with message : {ex.Message}");
            }
        }

        internal async Task<(bool, string)> DeleteLog(int id)
        {
            try
            {
                if (id == 0) return (false, "Could not delete that log");
                if (_context is null) return (false, "Database Context is null");
                if (_context.Logs is null) return (false, "Logs Db Set is null");
                var desiredLog = _context.Logs.Where(l => l.Id == id).FirstOrDefault();
                if (desiredLog is null) return (false, $"No Log with id : {id}");
                _context.Logs.Remove(desiredLog);
                await _context.SaveChangesAsync();
                return (true, String.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"Exception Caught with message : {ex.Message}");
            }
        }

        internal async Task<(bool, string)> DeleteLogs()
        {
            try
            {
                if (_context is null) return (false, "Database Context is null");
                if (_context.Logs is null) return (false, "Logs Db Set is null");
                var desiredLogs = await _context.Logs.Where(l => l.Created.Date.CompareTo(DateTime.Now) < 7).ToListAsync();
                if (desiredLogs is null) return (false, $"No Logs found");
                _context.Logs.RemoveRange(desiredLogs);
                await _context.SaveChangesAsync();
                return (true, String.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"Exception Caught with message : {ex.Message}");
            }
        }
    }
}
