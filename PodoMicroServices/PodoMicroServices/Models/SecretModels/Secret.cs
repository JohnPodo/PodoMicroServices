using PodoMicroServices.Common.Dto.SecretDto;

namespace PodoMicroServices.Models.SecretModels
{
    public class Secret : BaseModel
    {
        public string Value { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? ExpiresIn { get; set; }

        public Secret()
        {

        }

        public Secret(SecretDto dto, App app)
        {
            Value = dto.Value;
            App = app;
            ExpiresIn = dto.ExpiresIn;
            CreatedAt = dto.CreatedAt;
            Name = dto.Name;
        }
    }
}
