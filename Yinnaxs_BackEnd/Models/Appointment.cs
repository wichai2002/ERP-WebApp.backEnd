using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yinnaxs_BackEnd.Models
{
	[Table("appointment")]
	public class Appointment
	{
		[Key]
		public int appo_id { get; set; }
		public DateTime date { get; set; }
		public string? time { get; set; }
		public int applicant_id { get; set; }
		public string? interviewer { get; set; }
		public bool appointmented { get; set; }
	}
}

