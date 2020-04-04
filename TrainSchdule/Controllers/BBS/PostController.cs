using BLL.Helpers;
using DAL.QueryModel;
using Microsoft.AspNetCore.Mvc;
using System;
using TrainSchdule.ViewModels.BBS;

namespace TrainSchdule.Controllers.BBS
{
	/// <summary>
	/// 动态（朋友圈模型）管理
	/// </summary>
	[Route("[controller]/[action]")]
	public class PostController : Controller
	{
		/// <summary>
		/// 发布动态
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(ActionStatusMessage), 0)]
		public IActionResult Post([FromBody]PostCreateDataModel model)
		{
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 按条件筛选动态列表
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(ActionStatusMessage), 0)]
		public IActionResult Post([FromBody]QueryPostViewModel model)
		{
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 删除动态
		/// 使用当前登录用户权限进行删除
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete]
		[ProducesResponseType(typeof(ActionStatusMessage), 0)]
		public IActionResult Post(Guid id)
		{
			return new JsonResult(ActionStatusMessage.Success);
		}
	}
}