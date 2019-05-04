using System;
using System.Collections.Generic;
using DAL.Entities;
using DAL.Entities.UserInfo;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using DAL.Entities.UserInfo.Permission;

namespace DAL.Entities.UserInfo
{

    public class User : UserID
    {
        #region Properties

        public virtual UserApplicationInfo Application { get; set; }
		public virtual UserBaseInfo BaseInfo { get; set; }
		public virtual UserCompanyInfo CompanyInfo { get; set; }
		public virtual UserSocialInfo SocialInfo { get; set; }
		

		
        #endregion

    }


}
