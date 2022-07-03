using MicroServices.LogService.DAL;

namespace MicroServices.LogService.Services
{
    public class LogService
    {
        private readonly LogContext _context;

        public LogService(LogContext context)
        {
            _context = context;
        }


    }
}
