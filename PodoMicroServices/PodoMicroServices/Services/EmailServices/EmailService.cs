using MailKit.Net.Smtp;
using MimeKit;
using PodoMicroServices.Dto.EmailDto;
using PodoMicroServices.Models;

namespace PodoMicroServices.Services.EmailServices
{
    public class EmailService
    {
        internal MimeMessage CreateEmail(EmailDto mailRequest)
        { 
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(mailRequest.From));
            message.To.Add(MailboxAddress.Parse(mailRequest.To));

            if (mailRequest.Subject != null)
            {
                message.Subject = mailRequest.Subject;
            }

            if (mailRequest.Cc != null)
            {
                foreach (var item in mailRequest.Cc)
                {
                    if (item != null)
                    {
                        message.Cc.Add(MailboxAddress.Parse(item));
                    }
                }
            }

            if (mailRequest.Bcc != null)
            {
                foreach (var item in mailRequest.Bcc)
                {
                    if (item != null)
                    {
                        message.Bcc.Add(MailboxAddress.Parse(item));
                    }
                }
            }

            var builder = new BodyBuilder();

            if (mailRequest.IsHtml)
            {
                builder.HtmlBody = mailRequest.HtmlBody;
            }
            else
            {
                builder.TextBody = mailRequest.Body;
            }

            if (mailRequest.Attachments != null)
            {
                foreach (var item in mailRequest.Attachments)
                {
                    if (item != null)
                    {
                        builder.Attachments.Add(item);
                    }
                }
            }

            if (mailRequest.AttachmentsBase64 != null)
            {
                foreach (var item in mailRequest.AttachmentsBase64)
                {
                    if (item.Value != null)
                    {
                        byte[] bytes = Convert.FromBase64String(item.Value);
                        builder.Attachments.Add(item.Key, bytes);
                    }
                }
            }

            message.Body = builder.ToMessageBody();

            return message;
        }

        public async Task<BaseResponse> SendEmail(EmailDto mailRequest)
        {
            BaseResponse response = new BaseResponse();
             
            var message = CreateEmail(mailRequest);

            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Connect(mailRequest.ServerOptions.Host, mailRequest.ServerOptions.Port, mailRequest.ServerOptions.SecureSocketOptions);
            smtpClient.Authenticate(mailRequest.ServerOptions.Username, mailRequest.ServerOptions.Password);

            string sendResponse = await smtpClient.SendAsync(message);

            await smtpClient.DisconnectAsync(true);

            response.Success = true;
            response.Message = sendResponse;

            return response;
        }

        public BaseResponse ValidateDto(EmailDto dto)
        {
            if (dto is null) return new BaseResponse("No Data Given");
            if (dto.ServerOptions is null) return new BaseResponse("No Server Options Given");
            if (string.IsNullOrWhiteSpace(dto.ServerOptions.Username)) return new BaseResponse("Username in Server Options is necessary");
            if (string.IsNullOrWhiteSpace(dto.ServerOptions.Password)) return new BaseResponse("Password in Server Options is necessary");
            if (string.IsNullOrWhiteSpace(dto.ServerOptions.Host)) return new BaseResponse("Host in Server Options is necessary");
            if (string.IsNullOrWhiteSpace(dto.From)) return new BaseResponse("From is necessary");
            if (string.IsNullOrWhiteSpace(dto.To)) return new BaseResponse("To is necessary");
            return new BaseResponse();
        }
    }

}
