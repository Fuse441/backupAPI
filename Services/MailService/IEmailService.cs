using System.Net.Mail;

namespace colab_api.Services.MailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
