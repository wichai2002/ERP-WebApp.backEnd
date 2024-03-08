// systerm packages
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// project packages
using Yinnaxs_BackEnd.Context;
using Yinnaxs_BackEnd.Models;
using Yinnaxs_BackEnd.Utility;
using System.Net;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Yinnaxs_BackEnd.Controllers
{
    [Route("api/[controller]")]
    public class ApplicantController : Controller
    {
        private readonly ApplicationDbContext _applicant_Context;
        private EmailSender emailSender;

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
                resume_file = app.resume_file,
                first_name = app.first_name,
                last_name = app.last_name,
                application_date = app.application_date,
                role = app.role,
                date_of_birth = app.date_of_birth,
                status = app.status,
                hire_date = Convert.ToString(app.hire_date)

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

        [HttpPut("Announcement/{id:int}/{accept:bool}")]
        public async Task<ActionResult<Applicant>> AnnouncementInterview(int id,bool accept, Applicant applicant)
        {

            var transaction = _applicant_Context.Database.BeginTransaction();
  
            Request.Headers.TryGetValue("hrName", out var emp_name);

            if (applicant == null)
            {
                return BadRequest();
            }
            try
            {
                Console.WriteLine(id);
                Console.WriteLine(applicant.hire_date);
                Console.WriteLine(applicant.email);
                Console.WriteLine(accept);

                var _applicant = await _applicant_Context.Applicants
                .Where(app => app.applicant_id == id).FirstOrDefaultAsync();
                
                if (_applicant == null)
                {
                    return BadRequest("Applicant is null");
                }

                _applicant.hire_date = applicant.hire_date;
                _applicant.status = "Interviewed";
                _applicant.accept = accept;

                _applicant_Context.Update(_applicant);

                await _applicant_Context.SaveChangesAsync();

                var _appointment = await _applicant_Context.Appointments.
                    Where(appo => appo.applicant_id == id).FirstOrDefaultAsync();

                if (_appointment == null)
                {
                    return BadRequest();
                }

                _appointment.appointmented = true;

                _applicant_Context.Update(_appointment);

                await _applicant_Context.SaveChangesAsync();

                emailSender = new EmailSender();

                bool sendEmail = emailSender.sendEmail_Accept_Applicant(
                    emp_name,
                    $"{_applicant.first_name} {_applicant.last_name}",
                    applicant.email,
                    _applicant.role,
                    _applicant.hire_date,
                    accept);

                if (sendEmail == true)
                {
                   await transaction.CommitAsync();
                }
                else
                {
                    throw new DbUpdateConcurrencyException();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();

                if (!ApplicantExit(id))
                {
                    // status 400
                    return BadRequest();
                }
                else
                {
                    throw ;
                }
            }

            // status 204
            return NoContent();
        }

        private bool ApplicantExit(int id)
        {
            return _applicant_Context.Applicants.Any(app => app.applicant_id == id);
        }
    }
}

