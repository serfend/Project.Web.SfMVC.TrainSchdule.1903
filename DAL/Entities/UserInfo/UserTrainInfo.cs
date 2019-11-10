﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities.UserInfo
{
	/// <summary>
	/// TODO 用户受训情况
	/// </summary>
	public class UserTrainInfo: BaseEntity
	{
		public IEnumerable<Train> Trains { get; set; }
	}
	public class Train: BaseEntity
	{
		public DateTime Time_Begin { get; set; }
		public DateTime Time_End { get; set; }
		public string TrainName { get; set; }
		public TrainRank TrainRank { get; set; }
		public TrainType TrainType { get; set; }
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