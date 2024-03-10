using Microsoft.AspNetCore.Mvc;
using Yinnaxs_BackEnd.Context;
using Microsoft.EntityFrameworkCore;
using Yinnaxs_BackEnd.Models;
using Yinnaxs_BackEnd.Utility;
using Microsoft.AspNetCore.Authorization;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Yinnaxs_BackEnd.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppoinmentController : Controller
    {
        private readonly ApplicationDbContext _appointmentDbContext;

        public AppoinmentController (ApplicationDbContext applicationDbContext)
        {
            _appointmentDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentAll()
        {
            var appointment = await _appointmentDbContext.Appointments.ToListAsync();

            return Ok(appointment);
        }

        // api/Appoinment/:id
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointmentByID(int id)
        {
            var appointment = await _appointmentDbContext.Appointments.FindAsync(id);

            if (appointment != null)
            {
                return Ok(appointment);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointment_List()
        {
            var appointment_list = await _appointmentDbContext.Appointments.Join(_appointmentDbContext.Applicants,
                appo => appo.applicant_id,
                appli => appli.applicant_id,
                (_appo, _appli) => new
                {
                    appo_id = _appo.appo_id,
                    applicant_id = _appo.applicant_id,
                    date = _appo.date,
                    time = _appo.time,
                    role = _appli.role,
                    appomented = _appo.appointmented,
                    first_name = _appli.first_name,
                    last_name = _appli.last_name

                }).Where(app => app.appomented == false).ToListAsync();

            if (appointment_list != null)
            {
                return Ok(appointment_list);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Appointment>> CreateAppoinment(Appointment appointment)
        {
            var transaction = _appointmentDbContext.Database.BeginTransaction();
            
            try
            {
                Request.Headers.TryGetValue("emp_gen_id", out var headerValue);
                Request.Headers.TryGetValue("hrName", out var emp_name);

                appointment.interviewer = emp_name;
                _appointmentDbContext.Add(appointment);
                await _appointmentDbContext.SaveChangesAsync();

                // fetch table
                var _apllicant = await _appointmentDbContext.Applicants
                    .Where(app => app.applicant_id == appointment.applicant_id).FirstOrDefaultAsync();

                // change value
                _apllicant.status = "Appointmented";
                _appointmentDbContext.Update(_apllicant);
                // save new value
                await _appointmentDbContext.SaveChangesAsync();

                EmailSender emailSender = new EmailSender();

               bool sendEmail =  emailSender.sendEmail_Invite_Interview_To_Applicant
                    (emp_name,
                    $"{_apllicant.first_name} {_apllicant.last_name}",
                    _apllicant.email,
                    _apllicant.role,
                    appointment.date,
                    appointment.time);

                if (sendEmail == true)
                {
                    // commit transaction
                    await transaction.CommitAsync();
                }
                else
                {
                    throw new Exception("Send Email error");
                }


                // return http code 201
                return CreatedAtAction(nameof(GetAppointmentAll), new { id = appointment.appo_id, appointment });
            }
            catch (Exception error)
            {
                // roleback transaction, have some exception
                await transaction.RollbackAsync();
                return BadRequest(error);
            }
        }

    }
}

