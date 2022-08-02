using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodoMicroServices.Consumer.Models
{
    public class PodoFile
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty; 
        public string Folder { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Alt { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
