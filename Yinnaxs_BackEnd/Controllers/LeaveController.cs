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
    public class LeaveController :Controller
    {
        private readonly ApplicationDbContext _leave;
        public LeaveController(ApplicationDbContext leaveContext)
        {
            _leave = leaveContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Leave>>> GetLeaves()
        {
            var leave = await _leave.Leaves.ToListAsync();

            return Ok(leave); //return 200
        }

        [HttpGet("check")]
        public async Task<ActionResult<IEnumerable<Leave>>> GetLeaves2()
        {
            var listResult = await _leave.Emp_General_Information.Join(_leave.Leaves,
                gen => gen.emp_gen_id,
                per => per.emp_gen_id,
                (_gen, _per) => new
                {
                    _gen,
                    _per
                }
                ).Join(_leave.Roles,
                gen => gen._gen.role_id,
                per => per.role_id,
                (_table1, _table2) => new { role_name = _table2.position, _table1._gen,_table1._per }
                ).ToListAsync();

            

            return Ok(listResult); //return 200
        }

        [HttpGet("day/{id}")]
        public async Task<ActionResult<IEnumerable<Leave>>> GetLeavesDay(int id)
        {
            var leaveday = await _leave.Leavedays.Where(a => a.emp_gen_id == id).ToListAsync();

            return Ok(leaveday); //return 200
        }

        [HttpGet("dayper/{id}")]
        public async Task<ActionResult<IEnumerable<Leave>>> GetLeavesDayper(int id)
        {
            var leaveday = await _leave.Emp_General_Information.Where(a => a.emp_gen_id == id).ToListAsync();

            return Ok(leaveday); //return 200
        }


        [HttpGet("department/{id}")]
        public async Task<ActionResult<IEnumerable<Leave>>> GetDepartment(int id)
        {
            var leaveday = await _leave.Roles.Where(a => a.role_id == id).Join(_leave.Departments,
                gen => gen.department_id,
                per => per.department_id,
                (_Roles,_department) => new
                {
                    _Roles,_department
                }).ToListAsync();

            return Ok(leaveday); //return 200
        }


        [HttpGet("diffdate/{id}")]
        public async Task<ActionResult<IEnumerable<Leave>>> GetDiffDate(int id)
        {
            var leaveday = await _leave.Leaves.Where(a => a.emp_gen_id == id).ToListAsync();

            return Ok(leaveday); //return 200
        }



    }
}
