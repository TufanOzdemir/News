using Interface.ServiceInterfaces;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Services.IdentityServices
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("Tufan.Ingamedemo@gmail.com", "DEne123!")
            };

            using (var mail = new MailMessage("Tufan.Ingamedemo@gmail.com", email)
            {
                Subject = subject,
                Body = message
            })
            {
                await smtpClient.SendMailAsync(mail);
            }
        }
    }
}
