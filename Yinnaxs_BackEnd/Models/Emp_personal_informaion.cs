using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yinnaxs_BackEnd.Models
{
	[Table("emp_personal_information")]
	public class Emp_personal_informaion
	{
		[Key]
		public int emp_per_id { get; set; }
		public int emp_gen_id { get; set; }
		public string? personal_id { get; set; }
		public bool married { get; set; }
		public int? children { get; set; }
		public string? bank_account { get; set; }
		public string? address { get; set; }
		public DateTime hire_date { get; set; }
		public string? resign_date { get; set; }
    }
}

