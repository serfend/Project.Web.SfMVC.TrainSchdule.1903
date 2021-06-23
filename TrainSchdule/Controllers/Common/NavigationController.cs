using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.Common;
using DAL.Data;
using DAL.Entities.Common;
using DAL.Entities.Permisstions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.Extensions.Common;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class NavigationController
    {
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="menuName"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult List(string menuName)
        {
            return new JsonResult(new EntitiesListViewModel<CommonNavigate>(context.CommonNavigates.Where(n => n.Parent == menuName).OrderByDescending(i=>i.Priority)));
        }
        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Info([FromBody] CommonNavigateViewModel model)
        {
            var currentUser = currentUserService.CurrentUser;
            dataUpdateServices.Update(new BLL.Extensions.Common.EntityModifyExtensions.DataUpdateModel<CommonNavigate>() { 
                Item = model.Data,
                AuthUser = currentUser,
                BeforeAdd = v => { },
                BeforeModify = (cur, prev) =>
                {
                    prev.Alias = cur.Alias;
                    prev.Description = cur.Description;
                    prev.Icon = cur.Icon;
                    prev.Parent = cur.Parent;
                    prev.Priority = cur.Priority;
                    prev.Svg = cur.Svg;
                    prev.Url = cur.Url;
                },
                Db = context.CommonNavigates,
                UpdateJudge =  new BLL.Extensions.Common.EntityModifyExtensions.PermissionJudgeItem<CommonNavigate>()
                {
                    CompanyGetter = c=> "root",
                    Description = "菜单",
                    Flag = BLL.Extensions.Common.EntityModifyExtensions.PermissionFlag.None,
                    Permission = ApplicationPermissions.Resources.Menu.Item,
                },
                QueryItemGetter = c=>c.Name == model.Data.Name
            });
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class CommonNavigateViewModel : GoogleAuthViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "菜单信息未填写")]

        public CommonNavigate Data { get; set; }
    }
    /// <summary>
    /// 菜单管理
    /// </summary>

    [Route("[controller]/[action]")]
    [ApiController]
    public partial class NavigationController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IUsersService usersService;
        private readonly IUserActionServices userActionServices;
        private readonly ICurrentUserService currentUserService;
        private readonly IGoogleAuthService googleAuthService;
        private readonly IDataUpdateServices dataUpdateServices;

        /// <summary>
        /// 
        /// </summary>
        public NavigationController(ApplicationDbContext context, IUsersService usersService, IUserActionServices userActionServices, ICurrentUserService currentUserService, IGoogleAuthService googleAuthService,IDataUpdateServices dataUpdateServices)
        {
            this.context = context;
            this.usersService = usersService;
            this.userActionServices = userActionServices;
            this.currentUserService = currentUserService;
            this.googleAuthService = googleAuthService;
            this.dataUpdateServices = dataUpdateServices;
        }
    }
}
