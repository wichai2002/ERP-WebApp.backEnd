using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yinnaxs_BackEnd.Models
{
    [Table("emp_depart")]
    public class Emp_depart
    {
        [Key]
        public int emp_gen_id { set; get; }
        public int role_id { get; set; }
        public bool emp_status { get; set; }
        public int department_id { get; set; }
    }
}