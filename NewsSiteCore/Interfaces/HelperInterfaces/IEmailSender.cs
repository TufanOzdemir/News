using System.Threading.Tasks;

namespace Interface.HelperInterfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
