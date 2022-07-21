using PodoMicroServices.Dto.SecretDto;

namespace PodoMicroServices.Models.SecretModels
{
    public class Secret : BaseModel
    {
        public string Value { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public TimeSpan ExpiresIn { get; set; } = TimeSpan.FromDays(730);

        public Secret()
        {

        }

        public Secret(SecretDto dto, int appId)
        {
            Value = dto.Value;
            AppId = appId;
            ExpiresIn = dto.ExpiresIn;
            CreatedAt = dto.CreatedAt;
            Name = dto.Name;
        }
    }
}
