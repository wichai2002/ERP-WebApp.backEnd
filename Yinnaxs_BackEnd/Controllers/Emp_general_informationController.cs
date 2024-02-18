using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yinnaxs_BackEnd.Context;
using Yinnaxs_BackEnd.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Yinnaxs_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Emp_general_informationController : ControllerBase
    {
        private readonly ApplicationDbContext _emp_Gen_InformationContext;

        public Emp_general_informationController(ApplicationDbContext emp_Gen_InformationContext)
        {
            _emp_Gen_InformationContext = emp_Gen_InformationContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Emp_general_information>>> GetAllEmp_gen_info()
        {
            var emp_gen_info = await _emp_Gen_InformationContext.Emp_General_Information.ToListAsync();

            return Ok(emp_gen_info);
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Emp_general_information>>> GetListName()
        {
            var listResutlt = await _emp_Gen_InformationContext.Emp_General_Information
                 .Join(_emp_Gen_InformationContext.Emp_Personal_Informaion,
                    gen => gen.emp_gen_id,
                    per => per.emp_gen_id,
                    (_gen, _per) => new
                    {
                        emp_gen_id = _gen.emp_gen_id,
                        first_name = _gen.first_name,
                        last_name = _gen.last_name,
                        email = _gen.email,
                        hire_date = _per.hire_date
                    }
                 ).ToListAsync();

            return Ok(listResutlt);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Emp_general_information>> GetEmp_gen_infoById(int id)
        {
            var emp_gen_info_one = await _emp_Gen_InformationContext.Emp_General_Information.FindAsync(id);

            if (emp_gen_info_one == null)
            {
                return BadRequest();
            }

            return Ok(emp_gen_info_one);
        }

        [HttpPost]
        public async Task<ActionResult<Emp_general_information>> CreateEnp_gen_info(Emp_general_information emp_General_Information)
        {
            _emp_Gen_InformationContext.Emp_General_Information.Add(emp_General_Information);
            await _emp_Gen_InformationContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllEmp_gen_info), new { id = emp_General_Information.emp_gen_id, emp_General_Information });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmp_gen_info(int id, Emp_general_information emp_General_Information)
        {
            if (id != emp_General_Information.emp_gen_id)
            {
                return BadRequest();
            }

            _emp_Gen_InformationContext.Entry(emp_General_Information).State = EntityState.Modified;

            try
            {
                await _emp_Gen_InformationContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExitEmp_gen_info(id))
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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmp_gen_info(int id)
        {
            var delete_emp_gen = await _emp_Gen_InformationContext.Departments.FindAsync(id);

            if (delete_emp_gen == null)
            {
                return BadRequest();
            }

            _emp_Gen_InformationContext.Remove(delete_emp_gen);
            _emp_Gen_InformationContext.SaveChanges();
            return NoContent();
        }


        private bool ExitEmp_gen_info(int id)
        {
            return _emp_Gen_InformationContext.Emp_General_Information.Any(e => e.emp_gen_id == id);
        }
    }

}

