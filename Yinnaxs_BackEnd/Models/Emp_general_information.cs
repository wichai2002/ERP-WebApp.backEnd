using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yinnaxs_BackEnd.Models
{
	[Table("emp_general_information")]
	public class Emp_general_information
	{
		[Key]
		public int emp_gen_id { set; get; }
		public string? first_name { set; get; }
		public string? last_name { set; get; }
		public string? nick_name { set; get; }
		public int age { get; set; }
		public string? gender { get; set; }
		public string? date_of_birth { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
		public int role_id { get; set; }
		public string? image_profile { get; set; }
		public string? nationality { get; set; }
		public bool emp_status { get; set; }

    }
}

