using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yinnaxs_BackEnd.Models
{
    [Table("attendance")]
    public class Attendance
    {
        [Key]
        public int atten_id { get; set; }
        public int emp_gen_id { get; set; }
        public string? time_in { get; set; }
        public string? time_out { get; set; }
        public DateTime date { get; set; }
        
    }
}

