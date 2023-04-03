using EuroBankAPI.DTOs;
using MimeKit;

namespace EuroBankAPI.Service.EmailService
{
    public class EmailService:IEmailService
    {
        private IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public void SendEmail(EmailDTO RequestMail)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetValue<string>("EmailConfig:EmailUserName")));
            email.To.Add(MailboxAddress.Parse(RequestMail.To));
            email.Subject = RequestMail.Subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = RequestMail.Body };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_config.GetValue<string>("EmailConfig:EmailHost"), 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetValue<string>("EmailConfig:EmailUserName"), _config.GetValue<string>("EmailConfig:EmailPassword"));
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
