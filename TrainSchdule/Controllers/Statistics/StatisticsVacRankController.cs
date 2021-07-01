using Abp.Extensions;
using BLL.Extensions.Common;
using BLL.Extensions.Statistics;
using BLL.Interfaces;
using BLL.Interfaces.IVacationStatistics.Rank;
using DAL.DTO.Statistics.Rank;
using DAL.Entities.Permisstions;
using DAL.Entities.Vacations.Statistics.Rank;
using DAL.Entities.ZX.MemberRate;
using DAL.QueryModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Statistics;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.Controllers.Statistics
{

    /// <summary>
    /// 申请 - 排行榜
    /// </summary>
	[Route("[controller]/[action]")]
    public partial class StatisticsVacRankController : Controller
    {
        private readonly IStatisticsApplyRankServices rankServices;
        private readonly ICurrentUserService currentUserService;
        private readonly IUserActionServices userActionServices;

        /// <summary>
        /// 
        /// </summary>
        public StatisticsVacRankController(IStatisticsApplyRankServices rankServices, ICurrentUserService currentUserService, IUserActionServices userActionServices)
        {
            this.rankServices = rankServices;
            this.currentUserService = currentUserService;
            this.userActionServices = userActionServices;
        }
    }

    public partial class StatisticsVacRankController
    {
        /// <summary>
        /// 获取申请的排行榜
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IActionResult List([FromBody] QueryRankListViewModel model)
        {
            var currentUser = currentUserService.CurrentUser;
            if (model.Company == null) model.Company = new QueryByString() { Value = currentUser.CompanyInfo.CompanyCode };
            else
            {
                // 暂时不要求权限，能选中单位即允许访问
                var authCompany = model.Company.Value.IsNullOrEmpty() ? "root" : model.Company.Value;
                var permit = userActionServices.Permission(currentUser, ApplicationPermissions.Apply.Item, PermissionType.Read, new List<string>() { authCompany }, "排名[未启用校验]", out var _);
            }
            var list = rankServices.QueryRankList(model, out var totalCount);
            return new JsonResult(new EntitiesListViewModel<AppliesRankDto>(list.Select(i => i.ToDto())));
        }
        /// <summary>
        /// 获取带本人信息的排行榜
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IActionResult ListWithSelf([FromBody] QueryRankListViewModel model)
        {
            var currentUser = currentUserService.CurrentUser;
            if (model.Company == null) model.Company = new QueryByString() { Value = currentUser.CompanyInfo.CompanyCode };
            else
            {
                // 暂时不要求权限，能选中单位即允许访问
                var authCompany = model.Company.Value.IsNullOrEmpty() ? "root" : model.Company.Value;
                var permit = userActionServices.Permission(currentUser, ApplicationPermissions.Apply.Item, PermissionType.Read, new List<string>() { authCompany }, "排名[未启用校验]", out var _);
            }
            var list = rankServices.QueryRankList(model, out var totalCount);
            model.User = new QueryByString() { Value = currentUser.Id };
            var self = rankServices.QueryRankList(model, out var _);
            var dto = new Tuple<IEnumerable<StatisticsApplyRankItem>, StatisticsApplyRankItem>(list, self.FirstOrDefault() ?? new StatisticsApplyRankItem()
            {
                User = currentUser,
                UserId = currentUser.Id,
                Rank = -1,
                Status = "无排名",
                UserRealName = currentUser.BaseInfo.RealName,
                UserCompany = currentUser.CompanyInfo.Company?.Name
            });
            return new JsonResult(new EntityViewModel<AppliesRankListDto>(dto.ToDto(totalCount)));
        }
        /// <summary>
        /// 获取种类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RankTypes()
        {
            var dict = new List<RatingTypeDataModel>();
            foreach (RatingType s in ratingTypes)
            {
                MemberInfo[] mi = ratingType.GetMember(s.ToString());
                if (mi != null && mi.Length > 0 && Attribute.GetCustomAttribute(mi[0], typeof(DisplayAttribute)) is DisplayAttribute attr)
                    if (s.ToString() != "Once") dict.Add(new RatingTypeDataModel()
                    {
                        Id = (int)s,
                        Name = s.ToString(),
                        Alias = attr.Name,
                        ShortAlias = attr.ShortName
                    });
            }
            return new JsonResult(new EntitiesListViewModel<RatingTypeDataModel>(dict));
        }
    }

    public partial class StatisticsVacRankController
    {
        private static Type ratingType = typeof(RatingType);
        private static Array ratingTypes = Enum.GetValues(ratingType);

        /// <summary>
        /// 日期转期数
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DateToRound(DateTime date)
        {
            var list = new List<RatingType>();
            foreach (RatingType s in ratingTypes)
                list.Add(s);
            return new JsonResult(new EntitiesListViewModel<RatingRoundDescriptionDataModel>(list.Select(i =>
            {
                var attr = (ratingType.GetMember(i.ToString())?[0]?.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute);
                int round = date.RoundOfDateTime(i);
                var r = new RatingRoundDescriptionDataModel()
                {
                    Date = date,
                    Round = round,
                    Type = new RatingTypeDataModel()
                    {
                        Id = (int)i,
                        Name = i.ToString(),
                        Alias = attr?.Name,
                        ShortAlias = attr?.ShortName,
                    },
                    RoundToRoundIndex = round.RoundToRoundIndex(i),
                    RoundIndexToRound = round.RoundIndexToRound(i),
                    NextRound = round.NextRound(i),
                    LastRound = round.NextRound(i, -1),
                };
                return r;
            })));
        }
        /// <summary>
        /// 期数转日期
        /// </summary>
        /// <param name="round"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RoundToDate(int round)
        {
            var list = new List<RatingType>();
            foreach (RatingType s in ratingTypes)
                list.Add(s);
            return new JsonResult(new EntitiesListViewModel<RatingRoundDescriptionDataModel>(list.Select(i =>
            {
                var attr = (ratingType.GetMember(i.ToString())?[0]?.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute);
                var r = new RatingRoundDescriptionDataModel()
                {
                    Date = round.DateTimeRangeOfRound(i).Item1,
                    Round = round,
                    Type = new RatingTypeDataModel()
                    {
                        Id = (int)i,
                        Name = i.ToString(),
                        Alias = attr?.Name,
                        ShortAlias = attr?.ShortName,
                    },
                    RoundToRoundIndex = round.RoundToRoundIndex(i),
                    RoundIndexToRound = round.RoundIndexToRound(i),
                    NextRound = round.NextRound(i),
                    LastRound = round.NextRound(i, -1),
                };
                return r;
            })));
        }
    }
}
