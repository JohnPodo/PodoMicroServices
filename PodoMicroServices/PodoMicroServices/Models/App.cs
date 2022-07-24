using Newtonsoft.Json;

namespace PodoMicroServices.Models
{
    public class App
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        [JsonIgnore]
        public User? User { get; set; }
    }
}
