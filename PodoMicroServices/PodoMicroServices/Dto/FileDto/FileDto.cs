namespace PodoMicroServices.Dto.FileDto
{
    public class FileDto
    {
        public string Name { get; set; } = String.Empty;
        public string Folder { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Alt { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
