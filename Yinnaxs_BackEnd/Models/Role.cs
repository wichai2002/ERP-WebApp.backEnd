using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yinnaxs_BackEnd.Models
{
	[Table("Role")]
	public class Role
	{
		[Key]
		public int role_id { get; set; }
		public string? position { get; set; }
		public int department_id { get; set; }
		public string? start_work { get; set; }
		public string? finish_work { get; set; }
        public string role { get; set; }
    }
}

