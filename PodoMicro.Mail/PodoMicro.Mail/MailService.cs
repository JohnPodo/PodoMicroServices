using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PodoMicro.Mail
{
    public class MailService
    {
        private MailMessage request;
        private SmtpClient client;

        public MailService(MailDto settings)
        {
            InstanciateService(settings);
        }

        private void InstanciateService(MailDto settings)
        {
            request = new MailMessage();
            client = new SmtpClient($"smtp.{settings.Provider}");
            request.From = new MailAddress(settings.SenderMail);
            settings.Receivers.ForEach(rec => request.To.Add(new MailAddress(rec)));
            client.Port = settings.Port;
            client.Credentials = new NetworkCredential(settings.Username, settings.Password);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            request.SubjectEncoding = settings.Encoding is null ? Encoding.UTF8 : settings.Encoding;
            request.BodyEncoding = settings.Encoding is null ? Encoding.UTF8 : settings.Encoding;
            request.Subject = settings.Subject;
        }

        public void AddBody(string body, bool isItHtml)
        {
            request.IsBodyHtml = isItHtml;
            request.Body = body;
        }

        public void AddAttachment(string content,string attachName)
        {
            Attachment newAttach = Attachment.CreateAttachmentFromString(content, attachName);
            request.Attachments.Add(newAttach);
        }

        public void SendEmail()
        {
            client.Send(request);
            if (client != null)
                client.Dispose();
            if (request != null)
                request.Dispose();
        }
    }
}
