using System.Threading.Tasks;

namespace EmailService
{
    public interface IEmailSender
    {
        bool SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}
