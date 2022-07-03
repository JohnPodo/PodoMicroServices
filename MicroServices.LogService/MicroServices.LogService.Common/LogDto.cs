using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroServices.LogService.Common
{
    public class LogDto
    {
        public string Name { get; set; } = String.Empty;

        public Guid GroupSession { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public Severity Severity { get; set; }

        public string Message { get; set; } = String.Empty;
         
        public int AppId { get; set; }
    }
}
