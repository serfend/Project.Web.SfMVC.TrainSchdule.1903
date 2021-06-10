using DAL.DTO.ZX.MemberRate;
using DAL.Entities.Common.DataDictionary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
	public partial class ApplicationDbContext
	{

		public const string normarRateLevel = "NormalRateLevel";
		private void Configuration_ZX(ModelBuilder builder)
		{
			var data = builder.Entity<CommonDataDictionary>();

			var actions = new List<CommonDataDictionary>(){
			new CommonDataDictionary()
			{
				Alias="不称职",
				Description="没有达到岗位基本要求，需备注不称职原因",
				Color="#e60039",
				Key=LevelAssign.L1.ToString(),
				Value=(int)LevelAssign.L1,
			},
			new CommonDataDictionary()
			{
				Alias="较差",
				Description="基本达到岗位要求",
				Color="#c85554",
				Key=LevelAssign.L2.ToString(),
				Value=(int)LevelAssign.L2,
			},
			new CommonDataDictionary()
			{
				Alias="称职",
				Description="完全达到本职岗位的所有要求",
				Color="#337d56",
				Key=LevelAssign.L3.ToString(),
				Value=(int)LevelAssign.L3,
			},
			new CommonDataDictionary()
			{
				Alias="良好",
				Description="达到并高于本岗位所有要求的标准",
				Color="#9ec8da",
				Key=LevelAssign.L4.ToString(),
				Value=(int)LevelAssign.L4,
			},
			new CommonDataDictionary()
			{
				Alias="优秀",
				Description="表现突出，大幅超出当前岗位要求。需备注优秀原因。",
				Color="#6ff9c1",
				Key=LevelAssign.L5.ToString(),
				Value=(int)LevelAssign.L5,
			},
			new CommonDataDictionary()
			{
				Alias="无",
				Description="未选择",
				Color="#cbe2e4",
				Key=LevelAssign.None.ToString(),
				Value=(int)LevelAssign.None,
			},
			};
			foreach (var d in actions)
			{
				d.Id = dataId++;
				d.GroupName = normarRateLevel;
			}
			data.HasData(actions);

		}
	}
}
