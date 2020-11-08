using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces.Permission
{
	public interface IPermissionServices
	{
		/// <summary>
		/// 获取所有系统已有的权限列表 建议Singleton初始化
		/// </summary>
		/// <returns></returns>
		List<Tuple<string, PermissionDescription>> AllPermission();
	}
}