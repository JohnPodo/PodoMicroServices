using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodoMicroServices.Consumer.Models
{
    public class PodoSecret
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty; 
        public string Value { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? ExpiresIn { get; set; }
    }
}
