using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TrainSchdule.BLL.Interfaces;

namespace TrainSchdule.WEB.Extensions
{
    public static class EmailSenderExtensions
    {
        #region Logic

        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "验证邮箱",
                $"验证邮件已发送至注册邮箱<a href='{HtmlEncoder.Default.Encode(link)}'>点击此处</a>进行验证");
        }

        #endregion
    }
}
