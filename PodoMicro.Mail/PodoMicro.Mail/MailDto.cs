using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodoMicro.Mail
{
    public class MailDto
    {
        /// <summary>
        /// Your E-mail
        /// </summary>
        public string SenderMail { get; set; }
        /// <summary>
        /// List of emails to receive email
        /// </summary>
        public List<string> Receivers { get; set; }
        /// <summary>
        /// Choose the provider of e-mail. Default is gmail.com. Provider must be like "providerName.com" or "providerName.gr"
        /// </summary>
        public string Provider { get; set; }
        /// <summary>
        /// Encoding of E-mail. Default Encoding is UTF-8
        /// </summary>
        public Encoding Encoding { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        /// <summary>
        /// From which port the email will be sent. Default is 587
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Subject of email
        /// </summary>
        public string Subject { get; set; }
    }
}
