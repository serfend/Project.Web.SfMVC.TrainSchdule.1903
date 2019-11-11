namespace DAL.Entities.UserInfo
{

	public class User : UserID
    {
        #region Properties

        public virtual UserApplicationInfo Application { get; set; }
		public virtual UserBaseInfo BaseInfo { get; set; }
		public virtual UserCompanyInfo CompanyInfo { get; set; }
		public virtual UserSocialInfo SocialInfo { get; set; }
		public virtual UserTrainInfo TrainInfo { get; set; }
		public virtual UserDiyInfo DiyInfo { get; set; }
		#endregion

	}


}
