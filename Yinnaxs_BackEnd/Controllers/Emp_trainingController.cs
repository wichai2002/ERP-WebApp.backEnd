using System;
using Microsoft.AspNetCore.Authorization;
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
    public class TrainingController : Controller
    {
        private readonly ApplicationDbContext _emp_training_Context;

        public TrainingController (ApplicationDbContext applicationDbContext)
        {
            _emp_training_Context = applicationDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Emp_training>>> GetAllEmp_training()
        {
            var emp_traning = await _emp_training_Context.Emp_Training.ToListAsync();
            return Ok(emp_traning);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Emp_training>> GetEmp_trainingByID(int id)
        {
            var emp_training = await _emp_training_Context.Emp_Training.FindAsync(id);
            return Ok(emp_training);
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Emp_training>>> GetEmp_training_List()
        {
            var emp_training = await _emp_training_Context.Emp_Training.Join(_emp_training_Context.Departments,
                emp_teain => emp_teain.department_id, dep => dep.department_id,
                (_emp_train, _dep) => new
                {
                    train_id = _emp_train.train_id,
                    title = _emp_train.title,
                    date = _emp_train.date,
                    department_id = _emp_train.date,
                    time = _emp_train.time,
                    department_name = _dep.department_name
                }
                ).ToListAsync();

            return Ok(emp_training);
        }

        [HttpPost]
        public async Task<ActionResult<Emp_training>> CreateEmp_training(Emp_training emp_Training)
        {
            _emp_training_Context.Emp_Training.Add(emp_Training);
            await _emp_training_Context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllEmp_training), new { id = emp_Training.train_id, emp_Training });
        }


        //[HttpDelete]
        //public async Task<IActionResult 
    }
}

