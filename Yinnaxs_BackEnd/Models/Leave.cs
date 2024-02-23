using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yinnaxs_BackEnd.Models
{
    [Table("leave_request")]
    public class Leave
    {
        [Key]


        public int leave_req_number { get; set; }
        public int emp_gen_id { get; set; }
        public string type { get; set; }

        public int status {  get; set; }
        public DateTime start_leave { get; set; }
        public DateTime end_leave { get; set;}
        public int year { get; set; }

    }
}
