using System;
using System.Collections.Generic;
using System.Data;
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
    public class ApplicantController : Controller
    {
        private readonly ApplicationDbContext _applicant_Context;
         
        public ApplicantController(ApplicationDbContext applicationDbContext)
        {
            _applicant_Context = applicationDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Applicant>>> GetAll()
        {
            var applicant = await _applicant_Context.Applicants.ToListAsync();

            return Ok(applicant);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Applicant>> GetApplicantByID(int id)
        {
            var appllicant = await _applicant_Context.Applicants.FindAsync(id);

            return Ok(appllicant);
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Applicant>>> GetApplicantList()
        {
            var apllicant = await _applicant_Context.Applicants.Select(app => new
            {
                applicant_id = app.applicant_id,
                first_name = app.first_name,
                last_name = app.last_name,
                application_date = app.application_date,
                role = app.role,
                date_of_birth = app.date_of_birth,
                status = app.status

            }).ToListAsync();

            return Ok(apllicant);
        }

        [HttpPost]
        public async Task<ActionResult<Applicant>> CreateApplicant(Applicant applicant)
        {
            _applicant_Context.Applicants.Add(applicant);
           await _applicant_Context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = applicant.applicant_id, applicant });
        }
    }
}

