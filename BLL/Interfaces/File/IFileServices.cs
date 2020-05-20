using DAL.Entities.FileEngine;
using DAL.QueryModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

		/// <summary>
		/// 获取文件夹下的文件列表
		/// </summary>
		/// <param name="filepath"></param>
		/// <param name="pages"></param>
		/// <returns></returns>
		Tuple<IQueryable<UserFileInfo>, int> FolderFiles(string filepath, QueryByPage pages);

		/// <summary>
		/// 通过路径获取文件节点，不存在的路径将会被创建
		/// </summary>
		/// <param name="path"></param>
		/// <param name="mkdWhenNotExist"></param>
		/// <param name="current">当前所在节点</param>
		/// <returns></returns>
		UserFileInfo GetNode(string path, bool mkdWhenNotExist, UserFileInfo current);

		/// <summary>
		/// 获取文件夹的子文件夹
		/// </summary>
		/// <param name="filepath"></param>
		/// <param name="pages"></param>
		/// <returns></returns>
		Tuple<IEnumerable<string>, int> Folders(string filepath, QueryByPage pages);

		void RemoveTimeoutUploadStatus();

		/// <summary>
		/// 通过HttpContext获取当前正在上传的文件
		/// </summary>
		/// <returns></returns>
		UploadCache Stauts();

		/// <summary>
		/// 获取文件信息（currentId不可为null）
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="current">需要指定当前父目录，调用GetNode</param>
		/// <returns></returns>

		UserFileInfo LoadFromCurrentPath(string filename, UserFileInfo current);

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

		/// <summary>
		/// 刪除文件
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		bool Remove(UserFileInfo file);
	}
}