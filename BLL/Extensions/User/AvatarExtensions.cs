using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions
{
	public static class AvatarExtensions
	{

		/// <summary>
		/// 男生默认头像
		/// </summary>
		public static readonly string avatarMale = $"male_templete.png";
		/// <summary>
		/// 女生默认头像
		/// </summary>
		public static readonly string avatarFemale = $"female_templete.png";
		/// <summary>
		/// 未知默认头像
		/// </summary>
		public static readonly string avatarUnknown = $"unknown_templete.png";
		public static string AvatarRawPath { get; set; } = "images/avatar/";
		public static async Task Update(this Avatar avatar, IHostingEnvironment hostingEnvironment)
		{
			if (avatar == null || hostingEnvironment == null) return;
			var filePath = System.IO.Path.Combine(hostingEnvironment.WebRootPath, AvatarRawPath, avatar.FilePath);
			using (var s = new FileStream(filePath, FileMode.OpenOrCreate))
			{
				await s.WriteAsync(avatar.Img);
			}
		}
		private static readonly object fileLocker = new object();
		public static async Task<Avatar> Load(this Avatar avatar, User user, IHostingEnvironment hostingEnvironment, bool isFirstTimeLoad = true)
		{
			if (hostingEnvironment == null) return null;
			if (avatar == null)
			{
				if (user == null) return null;
				avatar = user.CreateTempAvatar(hostingEnvironment.WebRootPath);
			}

			var filePath = System.IO.Path.Combine(hostingEnvironment.WebRootPath, AvatarRawPath, avatar.FilePath);
			if (!File.Exists(filePath)) if (!isFirstTimeLoad) return null; else return await Load(null, user, hostingEnvironment, false).ConfigureAwait(false);
			lock (fileLocker)
			{
				using (var s = new FileStream(filePath, FileMode.Open))
				{
					var buffer = new byte[s.Length];
					s.Read(buffer);
					avatar.Img = buffer;
					return avatar;
				}
			}

		}
		private static Avatar CreateTempAvatar(this User user, string rootPath)
		{
			var realName = user.BaseInfo.RealName;
			if (realName == null) realName = "无名"; else realName = realName.Substring(0, 1);
			var a = new Avatar()
			{
				CreateTime = DateTime.Now,
				FilePath = $"{user.BaseInfo.Gender}_{realName}.png"
			};
			var fullpath = System.IO.Path.Combine(rootPath, AvatarRawPath, a.FilePath);
			if (!File.Exists(fullpath))
			{
				var bgPic = user.BaseInfo.Gender == GenderEnum.Male ? avatarMale : user.BaseInfo.Gender == GenderEnum.Female ? avatarFemale : avatarUnknown;
				CreateDefaultAvatar(fullpath, System.IO.Path.Combine(rootPath, AvatarRawPath, bgPic), realName);
			}
			return a;
		}
		/// <summary>
		/// 创建头像文件
		/// </summary>
		/// <param name="fullPath">头像文件位置</param>
		/// <param name="bgPic">头像采用的背景图片</param>
		/// <param name="frontPicContent">头像采用的前景字符</param>
		private static void CreateDefaultAvatar(string fullPath, string bgPic, string frontPicContent)
		{
			using (var map = new Bitmap(300, 300))
			{
				using (var g = Graphics.FromImage(map))
				{
					using (TextureBrush Txbrus = new TextureBrush(Image.FromFile(bgPic)))
					{
						Txbrus.WrapMode = System.Drawing.Drawing2D.WrapMode.TileFlipXY;
						g.FillRectangle(Txbrus, 0, 0, map.Width, map.Height);
						using (var font = new Font(SystemFonts.DefaultFont.Name, 144f))
						{
							var size = g.MeasureString(frontPicContent, font);
							g.DrawString(frontPicContent, font, Brushes.White, (float)((map.Width - size.Width) * 0.5), (float)((map.Height - size.Height) * 0.5));
							map.Save(fullPath);
						}
					}
				}
			}
		}
	}
}
