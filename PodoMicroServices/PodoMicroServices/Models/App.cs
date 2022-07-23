namespace PodoMicroServices.Models
{
    public class App
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public User? User { get; set; }
    }
}
