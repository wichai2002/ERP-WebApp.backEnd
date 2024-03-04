using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yinnaxs_BackEnd.Context;
using Yinnaxs_BackEnd.Models;

namespace Yinnaxs_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EdittimeController : Controller
    {
        private readonly ApplicationDbContext _edittime;
        public EdittimeController(ApplicationDbContext edittimeContext)
        {
            _edittime = edittimeContext;
        }


        public class Editclass
        {
            public int id { get; set; }
            public string? time_start { get; set; }
            public string? time_end { get; set; }
        }

        [HttpGet("getrole/{did}")]
        public async Task<ActionResult<IEnumerable<Leave>>> GetRoles(int did)
        {
            var getroles = await _edittime.Departments.Where(a => a.department_id == did).Join(_edittime.Roles,
                a => a.department_id,
                b => b.department_id,
                (de,ro) => new
                {
                    //start_time = ro.start_work.Replace(".", ":").PadLeft(5, '0'),
                    //end_time = ro.finish_work.Replace(".", ":").PadLeft(5, '0'),
                   ro
                }).ToListAsync();

            return Ok(getroles); //return 200
        }

        [HttpGet("getdepart")]
        public async Task<ActionResult<IEnumerable<Leave>>> GetDepart(int did)
        {
            var getdepart = await _edittime.Departments.ToListAsync();

            return Ok(getdepart); //return 200
        }

        [HttpGet("gettime/{rid}")]
        public async Task<ActionResult<IEnumerable<Leave>>> Gettime(int rid)
        {
            var gettime = await _edittime.Roles.Where(a => a.role_id == rid).Select(a => new
            {
                time_start = a.start_work.Replace(".", ":").PadLeft(5, '0'),
                time_end = a.finish_work.Replace(".", ":").PadLeft(5, '0')
            }).FirstOrDefaultAsync();

            return Ok(gettime); //return 200
        }


        [HttpPut("updatetime")]

        public async Task<ActionResult<IEnumerable<Payroll>>> Updatetime([FromBody] Editclass editclass)
        {
            var transaction = _edittime.Database.BeginTransaction();
            try
            {
                int id = editclass.id;
                string start_time = editclass.time_start.Replace(":", ".");
                string end_time = editclass.time_end.Replace(":", ".");

                var updatetime = await _edittime.Roles.Where(a => a.role_id == id).FirstOrDefaultAsync();
                updatetime.start_work = start_time;
                updatetime.finish_work = end_time;

                await _edittime.SaveChangesAsync();
                await transaction.CommitAsync();
                Console.WriteLine(start_time + " " + end_time);
                return Ok(start_time);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest(ex);
            }
        }



    }


}
