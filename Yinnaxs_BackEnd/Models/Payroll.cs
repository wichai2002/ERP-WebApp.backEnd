using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yinnaxs_BackEnd.Models
{
    [Table("payroll")]
    public class Payroll
    {
        [Key]
        public int payroll_id { get; set; }
        public int emp_gen_id { get; set; }
        public double salary { get; set; }

        public DateTime? salary_update { get; set; }
        public double? salary_up { get; set; }
        public double? bonus_per_year { get; set; }

    }
}
