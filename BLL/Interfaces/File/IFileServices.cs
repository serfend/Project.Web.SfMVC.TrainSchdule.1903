using DAL.Entities.FileEngine;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.File
{
	public interface IFileServices
	{
		/// <summary>
		/// 上传文件
		/// </summary>
		/// <param name="file"></param>
		/// <param name="path"></param>
		/// <param name="filename">用户自定义名称，无名称则用文件名</param>
		/// <param name="uploadStatusId">断点续传需要传入id</param>
		/// <param name="clientKey">文件所有者认证</param>
		/// <returns></returns>
		Task<UserFileInfo> Upload(IFormFile file, string path, string filename, Guid uploadStatusId, Guid clientKey);

		void RemoveTimeoutUploadStatus();

		/// <summary>
		/// 通过HttpContext获取当前正在上传的文件
		/// </summary>
		/// <returns></returns>
		UploadCache Stauts();

		/// <summary>
		/// 获取文件信息
		/// </summary>
		/// <param name="path"></param>
		/// <param name="filename"></param>
		/// <returns></returns>

		UserFileInfo Load(string path, string filename);

		/// <summary>
		/// 下载文件
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		UserFile Download(Guid id);

		/// <summary>
		/// 获取文件信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		UserFileInfo FileInfo(Guid id);
	}
}