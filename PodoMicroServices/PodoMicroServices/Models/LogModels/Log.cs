using PodoMicroServices.Dto.LogDto;

namespace PodoMicroServices.Models.LogModels
{
    public class Log : BaseModel
    { 
        public Guid GroupSession { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public Severity Severity { get; set; }

        public string Message { get; set; } = String.Empty;

        public Log()
        {

        }

        public Log(LogDto newLog, App app)
        {
            Name = newLog.Name;
            GroupSession = newLog.GroupSession;
            Created = newLog.Created;
            Severity = newLog.Severity;
            Message = newLog.Message;
            App = app;
        }
    }
}
