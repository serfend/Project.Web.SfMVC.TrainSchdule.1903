using BLL.Interfaces.File;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions
{
	/// <summary>
	/// 头像
	/// </summary>
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

		/// <summary>
		/// 创建临时头像文件
		/// </summary>
		/// <param name="realName"></param>
		/// <param name="gender"></param>
		/// <param name="rootPath"></param>
		/// <returns></returns>
		public static Avatar CreateTempAvatar(this string realName, GenderEnum gender, string rootPath)
		{
			if (realName == null) realName = "无名"; else realName = realName.Substring(0, 1);
			var a = new Avatar()
			{
				CreateTime = DateTime.Now,
				FilePath = $"{gender.ToString()}_{realName}.png"
			};
			var bgPic = gender == GenderEnum.Male ? avatarMale : gender == GenderEnum.Female ? avatarFemale : avatarUnknown;
			CreateDefaultAvatar(a, System.IO.Path.Combine(rootPath, AvatarRawPath, bgPic), realName);
			return a;
		}

		/// <summary>
		/// 创建头像文件
		/// </summary>
		/// <param name="avatar">头像实体</param>
		/// <param name="bgPic">头像采用的背景图片</param>
		/// <param name="frontPicContent">头像采用的前景字符</param>
		private static void CreateDefaultAvatar(Avatar avatar, string bgPic, string frontPicContent)
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
							using (MemoryStream stream = new MemoryStream())
							{
								map.Save(stream, ImageFormat.Png);
								avatar.Img = new byte[stream.Length];
								stream.Seek(0, SeekOrigin.Begin);
								stream.Read(avatar.Img, 0, Convert.ToInt32(stream.Length));
							}
						}
					}
				}
			}
		}
	}
}