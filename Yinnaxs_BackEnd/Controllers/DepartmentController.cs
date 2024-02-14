using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yinnaxs_BackEnd.Context;
using Yinnaxs_BackEnd.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Yinnaxs_BackEnd.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _departmentContext;

        public DepartmentController(ApplicationDbContext departmentContext)
        {
            _departmentContext = departmentContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            var department = await _departmentContext.Departments.ToListAsync();

            return Ok(department);
        }
       
        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartmentById(int id)
        {
            var department_one = await _departmentContext.Departments.FindAsync(id);

            if (department_one == null)
            {
                return NotFound();
            }

            return Ok(department_one);
        }


        // POST api/values
        [HttpPost]
        public async Task<ActionResult<Department>> CreateDepartment(Department department)
        {
            _departmentContext.Departments.Add(department);
            await _departmentContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDepartments), new {id = department.department_id, department});
        }

        // PUT api/values/5

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDepartment(int id, Department department)

        {
            if (id != department.department_id)
            {
                return BadRequest();
            }

            _departmentContext.Entry(department).State = EntityState.Modified;

            try
            {
                await _departmentContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleateDepartment(int id)
        {
            var department = await _departmentContext.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            _departmentContext.Remove(department);
            await _departmentContext.SaveChangesAsync();

            return NoContent();
        }

        private bool DepartmentExists(int id)
        {
            return _departmentContext.Departments.Any(e => e.department_id == id);
        }
    }
}
