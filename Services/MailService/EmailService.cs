using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace colab_api.Services.MailService
{
    public class EmailService : IEmailService
    {
        private readonly string _fromAddress = "website.colab.service@gmail.com"; // Your "from" address
        private readonly string _fromPassword = "Fuse27271144"; // Fetch from environment variable
        private readonly string _displayName = "CO-LAB Service";

        public EmailService()
        {
            if (string.IsNullOrEmpty(_fromPassword))
            {
                throw new InvalidOperationException("SMTP password not configured in environment variables.");
            }
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var fromAddress = new MailAddress(_fromAddress, _displayName);
            var toAddress = new MailAddress(to);


            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("4f478b6d758217", "f38aed34d5baf9"),
                EnableSsl = true
            };
            client.Send(_fromAddress, to, subject, body);
            System.Console.WriteLine("Sent");
        }
    }
}
