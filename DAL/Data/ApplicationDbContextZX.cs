using DAL.DTO.ZX.MemberRate;
using DAL.Entities.Common.DataDictionary;
using DAL.Entities.ZX.Grade;
using DAL.Entities.ZX.MemberRate;
using DAL.Entities.ZX.Phy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DAL.Data
{
	partial class ApplicationDbContext
	{
		public DbSet<GradePhySubject> GradePhySubjects { get; set; }
		public DbSet<GradePhyStandard> GradePhyStandards { get; set; }
		public DbSet<GradePhyRecord> GradePhyRecords { get; set; }
		public DbSet<GradeExam> GradeExams { get; set; }
		public DbSet<NormalRate> NormalRates { get; set; }
	}
}