using System;
using System.Collections.Generic;
using System.Data;
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
    public class AttendanceController : ControllerBase
    {
        private readonly ApplicationDbContext _attendanceDbContext;

        public AttendanceController(ApplicationDbContext applicationDbContext)
        {
            _attendanceDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAll()
        {
            var attendance = await _attendanceDbContext.Attendances.ToListAsync();

            return Ok(attendance);
        }

        [HttpGet("attenemp")]
        public async Task<ActionResult<IEnumerable<Emp_att>>> GetAllAttendance_with_emp()
        {
            var attendance_with_emp = await _attendanceDbContext.Attendances
                .Join(
                    _attendanceDbContext.Emp_General_Information,
                    attenData => attenData.emp_gen_id,
                    empData => empData.emp_gen_id,
                    (attenData, empData) => new { attenData, empData }
                )
                .Join(
                    _attendanceDbContext.Roles,
                    joinResult => joinResult.empData.role_id,
                    roleData => roleData.role_id,
                    (joinResult, roleData) => new Emp_att
                    {

                        emp_gen_id = joinResult.attenData.emp_gen_id,
                        emp_firstname = joinResult.empData.first_name,
                        emp_lastname = joinResult.empData.last_name,
                        att_time_in = DateTime.Parse(joinResult.attenData.time_in),
                        att_time_out = !string.IsNullOrEmpty(joinResult.attenData.time_out)
                            ? DateTime.Parse(joinResult.attenData.time_out)
                            : (DateTime?)null,
                        att_date = joinResult.attenData.date,
                        role = roleData.position
                    }
                )
                .ToListAsync();

            return Ok(attendance_with_emp);
        }

        [HttpPost]
        public async Task<ActionResult> AddAttendance([FromBody] Attendance newAttendance)
        {
            try
            {
                // Check if time_out is null or empty
                if (string.IsNullOrEmpty(newAttendance.time_out))
                {
                    newAttendance.time_out = null; // Set to null to allow Entity Framework to save in the database
                }

                _attendanceDbContext.Attendances.Add(newAttendance);
                await _attendanceDbContext.SaveChangesAsync();

                return Ok("Attendance added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
