namespace PodoMicroServices.Models
{
    public class BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty; 
        public int AppId { get; set; }
    }
}
