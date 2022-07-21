using MailKit.Security;

namespace PodoMicroServices.Dto.EmailDto
{
    public class EmailDto
    {
        public string To { get; set; } = String.Empty;
        public string From { get; set; } = String.Empty;
        public string Subject { get; set; } = string.Empty;
        public List<string> Cc { get; set; } = new List<string>();
        public List<string> Bcc { get; set; } = new List<string>();
        public string Body { get; set; } = string.Empty;
        public bool IsHtml { get; set; }
        public string HtmlBody { get; set; } = String.Empty;
        public List<string> Attachments { get; set; } = new List<string>();
        public Dictionary<string, string> AttachmentsBase64 { get; set; } = new Dictionary<string, string>();
        public ServerOptions ServerOptions { get; set; } = new ServerOptions();

    }

    public class ServerOptions
    {
        public string Host { get; set; } = String.Empty;
        public int Port { get; set; } = 587;
        public SecureSocketOptions SecureSocketOptions { get; set; } = SecureSocketOptions.Auto;
        public string Username { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }

}
