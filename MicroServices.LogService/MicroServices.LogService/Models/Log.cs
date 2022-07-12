using MicroServices.LogService.Common;

namespace MicroServices.LogService.Models
{
    public class Log
    {
        public int Id { get; set; }

        public string Name { get; set; } = String.Empty;

        public Guid GroupSession { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public Severity Severity { get; set; }

        public string Message { get; set; } = String.Empty;

        public int AppId { get; set; }

        public Log()
        {

        }

        public Log(LogDto newLog)
        { 
            Name = newLog.Name;
            GroupSession = newLog.GroupSession;
            Created = newLog.Created;
            Severity = newLog.Severity;
            Message = newLog.Message;
            AppId = newLog.AppId;
        }
    }
}
