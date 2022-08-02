using PodoMicroServices.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodoMicroServices.Consumer.Models
{
    public class PodoLog
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty; 
        public Guid GroupSession { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public Severity Severity { get; set; }

        public string Message { get; set; } = String.Empty;
    }
}
