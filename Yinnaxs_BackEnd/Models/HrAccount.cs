using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Yinnaxs_BackEnd.Models
{
	[Table("hr_account")]
	public class HrAccount
	{
		[Key]
		public int hr_id { get; set; }
        public int emp_gen_id { get; set; }
		public string? password { get; set; }
	}
}

