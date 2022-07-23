using Newtonsoft.Json;

namespace PodoMicroServices.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        [JsonIgnore]
        public string Role { get; set; } = string.Empty;
        [JsonIgnore]
        public byte[]? PasswordHash { get; set; }
        [JsonIgnore]
        public byte[]? PasswordSalt { get; set; }
        public bool Accepted { get; set; } = false;
        [JsonIgnore]
        public List<App> Apps { get; set; } = new List<App>();

    }
}
