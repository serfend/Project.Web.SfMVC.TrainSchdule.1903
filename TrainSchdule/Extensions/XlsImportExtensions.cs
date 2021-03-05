using BLL.Helpers;
using BLL.Interfaces.File;
using Magicodes.ExporterAndImporter.Core.Models;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels;

namespace TrainSchdule.Extensions
{
    /// <summary>
    /// Excel导入拓展
    /// </summary>
    public static class XlsImportExtensions
    {
        /// <summary>
        /// 检查是否存在导入错误
        /// 当出现无效的上传时保存一份错误原因到文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="import"></param>
        /// <param name="labelingPath">返回</param>
        public static void CheckError<T>(this ImportResult<T> import,string labelingPath)where T:class
        {
            var ModelState = new ModelStateDictionary();
            if (import.HasError)
            {
                using var sr = new MemoryStream();
                foreach (var e in import.TemplateErrors) ModelState.AddModelError($"模板字段{e.ColumnName} 需求字段{e.RequireColumnName}", e.Message);
                foreach (var e in import.RowErrors) ModelState.AddModelError($"数据:{e.RowIndex}行", string.Join(";", e.FieldErrors.Select(i => $"{i.Key}:{i.Value}")));
                ModelState.AddModelError("LabelPath", labelingPath);
                var m = ModelState.ToModel(ActionStatusMessage.StaticMessage.XlsInvalid);
                throw new ModelStateException(m);
            }
        }
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="importer"></param>
        /// <param name="inputStream"></param>
        public static async Task<ImportResult<T>> ImportWithErrorCheck<T>(this ExcelImporter importer,Stream inputStream) where T:class,new()
        {
            var id = $"{Guid.NewGuid()}.xlsx";
            var raw_path = $"temp\\{id}";
            var path  =Path.Combine(Directory.GetCurrentDirectory(), raw_path);
            var upload_path = $"{path}.upload";
            var datas = new byte[inputStream.Length];
            inputStream.Read(datas);
            using(var f = File.OpenWrite(upload_path))
                using (var bw = new BinaryWriter(f))
                    bw.Write(datas);
            var import = await importer.Import<T>(upload_path, raw_path);
            import.CheckError(raw_path);
            return import;
        }
    }
}
