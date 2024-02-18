using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yinnaxs_BackEnd.Models
{							 
	[Table("departments")]
	public class Department
	{
		[Key]
		public int department_id { get; set; }
		public string? department_name { get; set; }
		public double base_salary { get; set; }
		public double limit_total_salary { get; set; }
	}
}

