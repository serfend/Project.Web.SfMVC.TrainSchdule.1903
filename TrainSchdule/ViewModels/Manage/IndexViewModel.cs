using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TrainSchdule.DAL.Entities;

namespace TrainSchdule.WEB.ViewModels.Manage
{
    public class IndexViewModel
    {
        [Display(Name = "姓名")]
        public string RealName { get; set; }
		[Display(Name = "头像")]
        public string Avatar { get; set; }
		[DisplayName("用户名")]
        public string Username { get; set; }
		[DisplayName("签名")]
        public string About { get; set; }
		[DisplayName("性别")]
        public GenderEnum Gender { get; set; }
        //public string Gender { get; set; }
        [Url, Display(Name = "个人主页")]
        public string WebSite { get; set; }
        public bool PrivateAccount { get; set; }
        //public bool IsEmailConfirmed { get; set; }
        //[Required, EmailAddress]
        //public string Email { get; set; }
        //[Phone, Display(Name = "Phone number")]
        //public string PhoneNumber { get; set; }
        public string StatusMessage { get; set; }
    }
}
