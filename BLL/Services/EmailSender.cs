using System.Threading.Tasks;
using BLL.Interfaces;

namespace BLL.Services
{
    /// <summary>
    /// This class is used by the application to send email for account confirmation and password reset.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
			//TODO 此处添加 邮箱认证功能
            return Task.CompletedTask;
        }
    }
}
