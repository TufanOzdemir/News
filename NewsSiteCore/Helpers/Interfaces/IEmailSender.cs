using System.Threading.Tasks;

namespace Interface.ServiceInterfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
