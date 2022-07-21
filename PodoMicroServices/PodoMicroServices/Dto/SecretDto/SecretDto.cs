namespace PodoMicroServices.Dto.SecretDto
{
    public class SecretDto
    {
        public string Name { get; set; } = String.Empty;
        public string Value { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public TimeSpan ExpiresIn { get; set; } = TimeSpan.FromDays(730);
    }
}
