using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yinnaxs_BackEnd.Models
{
    [Table("empattend")]
    public class Emp_att
    {
        [Key]
        public int emp_gen_id { get; set; }
        public DateTime date { get; set; }
        public string emp_firstname { get; set; }
        public string emp_lastname { get; set; }
        public DateTime att_time_in { get; set; }
        public DateTime? att_time_out { get; set; }
        public DateTime att_date { get; set; }
        public string role { get; set; }
    }
}
