using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yinnaxs_BackEnd.Models
{
    [Table("leaves")]
    public class Leaveday
    {
        [Key]
        public int leave_id { get; set; }
        public int emp_gen_id { get; set; }
        public int sick_leave { get; set;}
        public int personal_leave { get; set; }
        public int vacation_leave { get; set; }
    }
}
