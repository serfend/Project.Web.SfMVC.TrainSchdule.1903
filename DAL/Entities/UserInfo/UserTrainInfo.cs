using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.UserInfo
{
	/// <summary>
	/// TODO 用户受训情况
	/// </summary>
	public class UserTrainInfo : BaseEntity
	{
		public virtual IEnumerable<Train> Trains { get; set; }
	}

	public class Train : BaseEntity
	{
		public DateTime Time_Begin { get; set; }
		public DateTime Time_End { get; set; }
		public string TrainName { get; set; }
		public virtual TrainRank TrainRank { get; set; }
		public virtual TrainType TrainType { get; set; }
	}

	public class TrainType
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Key]
		public string Code { get; set; }

		public string Name { get; set; }
	}

	public class TrainRank
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Key]
		public string Code { get; set; }

		public string Name { get; set; }
	}
}