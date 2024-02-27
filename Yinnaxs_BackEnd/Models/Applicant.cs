using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yinnaxs_BackEnd.Models
{
	[Table("applicant")]
	public class Applicant
	{
		[Key]
		public int applicant_id { get; set; }
		public string? department { get; set; }
		public string? role { get; set; }
		public string? first_name { get; set; }
		public string? last_name { get; set; }
		public int age { get; set; }
		public DateTime? date_of_birth { get; set; }
		public string? email { get; set; }
		public string? phone { get; set; }
		public string? resume_about_me { get; set; }
		public string? resume_file { get; set; }
		public string? status { get; set; }
		public bool? accept { get; set; }
		public DateTime? application_date { get; set; }
		public string? education_1 { get; set; }
        public string? education_2 { get; set; }
        public string? education_3 { get; set; }
		public string? nationality { get; set; }
		public string? gender { get; set; }
		public string? nickname { get; set; }
		public DateTime? hire_date { get; set; }
    }
}

