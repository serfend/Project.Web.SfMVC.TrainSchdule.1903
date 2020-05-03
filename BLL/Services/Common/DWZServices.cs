using BLL.Extensions.Common;
using BLL.Extensions.CreateClientInfo;
using BLL.Helpers;
using BLL.Interfaces.Common;
using Castle.Core.Internal;
using DAL.Data;
using DAL.Entities.Common;
using DAL.Entities.UserInfo;
using DAL.QueryModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Services.Common
{
	public class DWZServices : IDWZServices
	{
		private readonly ApplicationDbContext context;
		private readonly IHttpContextAccessor httpContextAccessor;

		public DWZServices(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
		{
			this.context = context;
			this.httpContextAccessor = httpContextAccessor;
		}

		public ShortUrl Create(User createBy, string target, DateTime Expire, string key)
		{
			if (target == null) throw new ArgumentNullException(nameof(target));
			if (createBy == null) throw new ArgumentNullException(nameof(createBy));
			if (key != null && key.Length > 255) throw new ActionStatusMessageException(ActionStatusMessage.Static.ResourceOutofSize);
			var checkExist = Load(key);
			if (checkExist != null) throw new ActionStatusMessageException(ActionStatusMessage.Static.ResourceAllReadyExist);
			var httpContext = httpContextAccessor.HttpContext;
			var m = httpContext.ClientInfo<ShortUrl>();
			m.Create = DateTime.Now;
			m.CreateBy = createBy;
			m.Expire = Expire;
			m.Target = target;
			if (key != null && key.Length > 0)
				m.Key = key;
			else
			{
				string result = string.Empty;
				using (var sha1 = SHA256.Create())
				{
					var s1 = sha1.ComputeHash(Encoding.UTF8.GetBytes(target));
					result = BitConverter.ToString(s1).Replace("-", "").ToLower().Substring(0, 6);
					checkExist = Load(result);
					if (checkExist != null)
					{
						if (checkExist.Target == target) return checkExist;
						else throw new ActionStatusMessageException(ActionStatusMessage.Static.ResourceAllReadyExist);
					}
				};

				m.Key = result;
			}
			context.CommonShortUrl.Add(m);
			context.SaveChanges();
			return m;
		}

		public string Open(ShortUrl model, User ViewUser)
		{
			if (model == null) return null;
			var open = httpContextAccessor.HttpContext.ClientInfo<ShortUrlStatistics>();
			open.Create = DateTime.Now;
			open.Url = model;
			open.ViewBy = ViewUser;
			context.CommonShortUrlStatistics.Add(open);
			context.SaveChanges();
			return model.Target;
		}

		public ShortUrl Load(string key)
		{
			if (key.IsNullOrEmpty()) return null;
			var m = context.CommonShortUrlDb.Where(c => c.Key == key).FirstOrDefault();
			return m;
		}

		public IEnumerable<ShortUrl> Query(QueryDwzViewModel model, out int totalCount)
		{
			if (model == null)
			{
				totalCount = 0;
				return null;
			}
			var res = context.CommonShortUrlDb;
			if (model.Create != null) res = res.Where(s => s.Create > model.Create.Start).Where(s => s.Create < model.Create.End);
			if (model.CreateBy != null) res = res.Where(s => s.CreateBy.Id == model.CreateBy.Value);
			if (model.Device != null) res = res.Where(s => s.Device == model.Device.Value);
			if (model.Key != null) res = res.Where(s => s.Key == model.Key.Value);
			if (model.Target != null) res = res.Where(s => s.Target.Contains(model.Target.Value));
			if (model.Ip != null) res = res.Where(s => s.Ip == model.Ip.Value);

			res = res.SplitPage(model.Pages, out totalCount);
			return res.ToList();
		}

		public void Remove(ShortUrl model)
		{
			if (model == null) return;
			model.Remove();
			context.CommonShortUrl.Update(model);
			context.SaveChanges();
		}

		public IEnumerable<ShortUrlStatistics> QueryStatistics(ShortUrl shortUrl, QueryDwzStatisticsViewModel model, out int totalCount)
		{
			if (model == null)
			{
				totalCount = 0;
				return null;
			}
			var res = context.CommonShortUrlStatistics.Where(s => s.Url.Id == shortUrl.Id);
			if (model.Create != null) res = res.Where(s => s.Create > model.Create.Start).Where(s => s.Create < model.Create.End);
			if (model.ViewBy != null) res = res.Where(s => s.ViewBy.Id == model.ViewBy.Value);
			if (model.Device != null) res = res.Where(s => s.Device == model.Device.Value);
			if (model.Ip != null) res = res.Where(s => s.Ip == model.Ip.Value);

			res = res.SplitPage(model.Pages, out totalCount);
			return res.ToList();
		}
	}
}