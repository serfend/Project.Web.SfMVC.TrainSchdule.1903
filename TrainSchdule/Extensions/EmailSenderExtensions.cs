using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BLL.Interfaces;

namespace TrainSchdule.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class EmailSenderExtensions
    {
        #region Logic

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailSender"></param>
        /// <param name="email"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "验证邮箱",
                $"验证邮件已发送至注册邮箱<a href='{HtmlEncoder.Default.Encode(link)}'>点击此处</a>进行验证");
        }

        #endregion
    }
}
