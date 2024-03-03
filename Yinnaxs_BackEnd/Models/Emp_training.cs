using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yinnaxs_BackEnd.Models
{
	[Table("emp_training")]
	public class Emp_training
	{
		[Key]
		public int train_id { get; set; }
		public string? title { get; set; }
		public DateTime date { get; set; }
		public string? time { get; set; }
		public int department_id { get; set; }
    }
}

