using System.Threading.Tasks;
using TrainSchdule.BLL.Interfaces;

namespace TrainSchdule.BLL.Services
{
    /// <summary>
    /// This class is used by the application to send email for account confirmation and password reset.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }
    }
}
