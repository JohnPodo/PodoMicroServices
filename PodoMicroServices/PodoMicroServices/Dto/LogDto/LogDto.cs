using PodoMicroServices.Models.LogModels;

namespace PodoMicroServices.Dto.LogDto
{
    public class LogDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;

        public Guid GroupSession { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public Severity Severity { get; set; }

        public string Message { get; set; } = String.Empty; 
    }
}
