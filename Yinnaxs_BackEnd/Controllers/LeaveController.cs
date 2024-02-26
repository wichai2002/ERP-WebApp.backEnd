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
    public class LeaveController : Controller
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
                (_table1, _table2) => new { role_name = _table2.position, _table1._gen, _table1._per }
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
                (_Roles, _department) => new
                {
                    _Roles,
                    _department
                }).ToListAsync();

            return Ok(leaveday); //return 200
        }


        [HttpGet("diffdate/{id}")]
        public async Task<ActionResult<IEnumerable<Leave>>> GetDiffDate(int id)
        {
            //.Where(a => a.emp_gen_id == id)
            var leaveday = await _leave.Leaves.Where(a => a.emp_gen_id == id & a.status == 1).ToListAsync();

            List<TimeSpan> diffList = new List<TimeSpan>();


            foreach (var item in leaveday)
            {

                TimeSpan diff1 = item.end_leave - item.start_leave;

                diffList.Add(diff1);
            };
            //TimeSpan diff1 = leaveday.start_leave - leaveday.end_leave;

            return Ok(new { diff = diffList, le = leaveday }); //return 200
        }


        [HttpPut("confirm/{id}/{rid}/{tpy}/{num}")]
        public async Task<ActionResult<IEnumerable<Leave>>> Updateleave(int id, int rid, int tpy, int num)
        {
            var transaction = _leave.Database.BeginTransaction();

            try
            {
                var calulation_Leave = await _leave.Leavedays.Where(a => a.emp_gen_id == id).FirstOrDefaultAsync();


                var change_status = await _leave.Leaves.Where(a => a.leave_req_number == rid).FirstOrDefaultAsync();

                if (num == 1)
                {
                    change_status.status = 1;
                    _leave.Update(change_status);
                }
                else
                {
                    change_status.status = 2;
                    _leave.Update(change_status);
                }


                TimeSpan diff = change_status.end_leave - change_status.start_leave;
                int diffday = diff.Days;
                if (tpy == 4 && num == 1)
                {
                    var coun = calulation_Leave.sick_leave;
                    calulation_Leave.sick_leave = coun - diffday;
                    if (calulation_Leave.sick_leave < 0)
                    {
                        calulation_Leave.sick_leave = 0;
                    }
                    _leave.Update(calulation_Leave);
                }

                else if (tpy == 5 && num == 1)
                {
                    var coun = calulation_Leave.personal_leave;
                    calulation_Leave.personal_leave = coun - diffday;
                    if (calulation_Leave.personal_leave < 0)
                    {
                        calulation_Leave.personal_leave = 0;
                    }
                    _leave.Update(calulation_Leave);
                }
                else if (tpy == 6 && num == 1)
                {
                    var coun = calulation_Leave.vacation_leave;
                    calulation_Leave.vacation_leave = coun - diffday;
                    if (calulation_Leave.vacation_leave < 0)
                    {
                        calulation_Leave.vacation_leave = 0;
                    }
                    _leave.Update(calulation_Leave);
                };


                await _leave.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(calulation_Leave);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex);
            }
        }


    }
}
