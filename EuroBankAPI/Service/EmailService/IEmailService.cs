using EuroBankAPI.DTOs;

namespace EuroBankAPI.Service.EmailService
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO RequestMail);

    }
}
