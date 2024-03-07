
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Yinnaxs_BackEnd.Models
{

    [Table("emp_education")]
    public class Education
    {
        [Key]
        public int emp_education_id { get; set; }
        public int emp_gen_id { get; set; }
        public string? university_name { get; set;}
        public string? degree { get; set; }
        public string? program { get; set; }
        public double gpa { get; set; }
        public int grad_year { get; set; }

    }
}
