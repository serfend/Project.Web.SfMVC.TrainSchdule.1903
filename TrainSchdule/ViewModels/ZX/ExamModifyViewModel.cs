using DAL.Data;
using DAL.DTO.ZX.Grade;
using DAL.Entities.ZX.Grade;
using DAL.Entities.ZX.Phy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.ZX
{
	/// <summary>
	/// 编辑考核
	/// </summary>
	public class ExamModifyViewModel : GoogleAuthViewModel
	{
		/// <summary>
		/// 数据
		/// </summary>
		[Required]
		public ExamDTO Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public static class GradeExtensions
	{
		/// <summary>
		/// to model , if previous exist in db , use it
		/// </summary>
		/// <param name="vm"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public static GradeExam ToModel(this ExamModifyViewModel vm, ApplicationDbContext context)
		{
			if (vm == null) return null;
			var model = vm.Data;
			if (model == null) return null;
			var m = context.GradeExams.Where(e => e.Name == model.Name).FirstOrDefault();
			if (m == null) m = new GradeExam();
			m.Name = model.Name;
			m.IsRemoved = model.IsRemoved;
			m.HandleBy = context.AppUsers.Where(u => u.Id == model.HoldBy).FirstOrDefault();
			m.HoldBy = context.Companies.Where(c => c.Code == model.HoldBy).FirstOrDefault();
			m.Create = model.Create ?? DateTime.MinValue;
			m.CreateBy = context.AppUsers.Where(u => u.Id == model.CreateBy).FirstOrDefault();
			m.Description = model.Description;
			m.ExecuteTime = model.ExecuteTime ?? DateTime.MinValue;
			return m;
		}

		/// <summary>
		/// to model , if previous exist in db , use it
		/// </summary>
		/// <param name="model"></param>
		/// <param name="context"></param>
		/// <returns></returns>

		public static GradePhyRecord ToModel(this GradeRecordModifyViewModel model, ApplicationDbContext context)
		{
			if (model == null) return null;
			var m = context.GradePhyRecords.Where(r => r.Id == model.Id).FirstOrDefault();
			if (m == null) m = new GradePhyRecord();
			m.IsRemoved = model.IsRemoved;
			m.Create = model.Create;
			m.CreateBy = context.AppUsers.Where(u => u.Id == model.CreateBy).FirstOrDefault();
			m.Exam = context.GradeExams.Where(e => e.Name == model.Exam).FirstOrDefault();
			m.Id = model.Id;
			m.RawValue = model.RawValue;
			m.Remark = model.Remark;
			m.Score = model.Score;
			m.Subject = context.GradePhySubjects.Where(s => s.Name == model.Subject).FirstOrDefault();
			m.User = context.AppUsers.Where(u => u.Id == model.User).FirstOrDefault();
			return m;
		}
	}

	/// <summary>
	/// 成绩编辑
	/// </summary>
	public class GradeRecordModifyViewModel : GoogleAuthViewModel
	{
		/// <summary>
		/// id主键
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 是否已删除
		/// </summary>
		public bool IsRemoved { get; set; }

		/// <summary>
		/// 所属人
		/// </summary>
		public string User { get; set; }

		/// <summary>
		/// 创建人
		/// </summary>
		public string CreateBy { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime Create { get; set; }

		/// <summary>
		/// 科目名称
		/// </summary>
		public string Subject { get; set; }

		/// <summary>
		/// 考核名称
		/// </summary>
		public string Exam { get; set; }

		/// <summary>
		/// 得分
		/// </summary>
		public int Score { get; set; }

		/// <summary>
		/// 成绩原始值
		/// </summary>
		public string RawValue { get; set; }

		/// <summary>
		/// 用户备注标识
		/// </summary>
		public string Remark { get; set; }
	}
}