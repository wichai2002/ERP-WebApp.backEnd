using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Yinnaxs_BackEnd.Context;
using Yinnaxs_BackEnd.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Yinnaxs_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Emp_general_informationController : ControllerBase
    {
        public class Editclass
        {
            public int id { set; get; }
            public string? first_name { set; get; }
            public string? last_name { set; get; }
            public string? department { set; get; }
            public string? role { set; get; }
            public string? phone { set; get; }
            public string? email { set; get; }
            public string? nation { set; get; }
            public string? nick_name { set; get; }
            public string? address { set; get; }
            public bool married { set; get; }
            public int? children { set; get; }
            public string? account_number { set; get; }
            public int sickleave { get; set; }
            public int personalleave { get; set; }
            public int vacationleave { get; set; }
            public string? time_start { get; set; }
            public string? time_end { get; set; }
        }    

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

        [HttpGet("emp_list")]
        public async Task<ActionResult<IEnumerable<Emp_general_information>>> GetAllEmp_gen_info_list()
        {
            var emp_gen_info = await _emp_Gen_InformationContext.Emp_General_Information.Join(_emp_Gen_InformationContext.Roles,
                a => a.role_id,
                b => b.role_id,
                (emp,role) => new
                {
                    emp_id = emp.emp_gen_id,
                    fullname = emp.first_name + " "+ emp.last_name,
                    email = emp.email,  
                    role_name = role.position,
                    status = emp.emp_status
                }).Join(_emp_Gen_InformationContext.Emp_Personal_Informaion,
                a => a.emp_id,
                b => b.emp_gen_id,
                (emp,per) => new 
                {
                    emp_id = per.emp_gen_id,
                    fullname = emp.fullname,
                    email = emp.email,
                    role_name = emp.role_name,
                    join_date = per.hire_date.ToString("yyyy-MM-dd"),
                    status = emp.status

                }).ToListAsync();

            return Ok(emp_gen_info);
        }





        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Emp_general_information>>> GetListName()
        {
            var listResutlt = await _emp_Gen_InformationContext.Emp_General_Information
                 .Join(_emp_Gen_InformationContext.Emp_Personal_Informaion,
                    gen => gen.emp_gen_id, per => per.emp_gen_id,
                    (_gen, _per) => new
                    {
                        emp_gen_id = _gen.emp_gen_id,
                        first_name = _gen.first_name,
                        last_name = _gen.last_name,
                        email = _gen.email,
                        role_id = _gen.role_id,
                        hire_date = _per.hire_date,
                        department_id = 0
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


        [HttpGet("em_info/{id}")]
        public async Task<ActionResult<Emp_general_information>> GetEmp_gen_infoById_For_Edit(int id)
        {

            //table 1
            var emp_gen_info_one = await _emp_Gen_InformationContext.Emp_General_Information.Where(a => a.emp_gen_id == id)
                .Join(_emp_Gen_InformationContext.Roles,
                a => a.role_id,
                b => b.role_id,
                (info,role) => new
                {
                    roleId = info.role_id,
                    phone = info.phone,
                    nationality = info.nationality,
                    position = role.position,
                    departmentId = role.department_id
                })
                .Join(_emp_Gen_InformationContext.Departments,
                a => a.departmentId,
                b => b.department_id,
                (info,depart) => new
                {
                    departmentName = depart.department_name,
                    roleName = info.position,
                    phone = info.phone,
                    nation = info.nationality
                }).ToListAsync();
                



            //table 2
            var emp_gen_info_two = await _emp_Gen_InformationContext.Emp_General_Information.Where(a => a.emp_gen_id == id).
                Join(_emp_Gen_InformationContext.Emp_Personal_Informaion,
                a => a.emp_gen_id,
                b => b.emp_gen_id,
                (emp,per) => new
                {
                    emp_id = emp.emp_gen_id,
                    roleId = emp.role_id,
                    first_name = emp.first_name,
                    last_name = emp.last_name,
                    nickname = emp.nick_name,
                    gender = emp.gender,
                    birth = emp.date_of_birth.ToString("yyyy-MM-dd"),
                    email = emp.email,
                    address = per.address,
                    hire_date = per.hire_date.ToString("yyyy-MM-dd")
                }).Join(_emp_Gen_InformationContext.Roles,
                a => a.roleId,
                b => b.role_id,
                (one,two) => new 
                {
                    first_name = one.first_name,
                    last_name = one.last_name,
                    nickname = one.nickname,
                    gender = one.gender,
                    birth = one.birth,
                    email = one.email,
                    address = one.address,
                    hire_date = one.hire_date,
                    start_time = two.start_work.Replace(".",":").PadLeft(5, '0'),
                    end_time = two.finish_work.Replace(".", ":").PadLeft(5, '0')
                }).ToListAsync();

            //table 3
            var emp_gen_info_three = await _emp_Gen_InformationContext.Emp_General_Information.Where(a => a.emp_gen_id == id)
                .Join(_emp_Gen_InformationContext.Emp_Personal_Informaion,
                a => a.emp_gen_id,
                b => b.emp_gen_id,
                (info,per) => new
                {
                    empId = info.emp_gen_id,
                    married = per.married,
                    children = per.children,
                    fullname = info.first_name,
                    bankingNumber = per.bank_account,

                }).Join(_emp_Gen_InformationContext.Educations,
                a => a.empId,
                b => b.emp_gen_id,
                (one,two) => new
                {
                    married = one.married,
                    children = one.children,
                    fullname = one.fullname,
                    bankingNumber = one.bankingNumber,
                    university = two.university_name,
                    degree = two.degree,
                    program = two.program,
                    gpa = two.gpa,
                    graduration = two.grad_year
                }).ToListAsync();

            //table 4
            var emp_gen_info_four = await _emp_Gen_InformationContext.Leavedays.Where(a => a.emp_gen_id == id).ToListAsync();

            return Ok(new {employee_if = emp_gen_info_one,main_info = emp_gen_info_two,other_info = emp_gen_info_three,leave_day=emp_gen_info_four});
        }


        [HttpPut("reject/{id}")]
        public async Task<ActionResult<Emp_general_information>> Reject_Emp(int id)
        {

            var transaction = _emp_Gen_InformationContext.Database.BeginTransaction();
            try
            {

                var rejectEmp = await _emp_Gen_InformationContext.Emp_General_Information.Where(a => a.emp_gen_id == id).FirstOrDefaultAsync();
                var rejecttime = await _emp_Gen_InformationContext.Emp_Personal_Informaion.Where(a => a.emp_gen_id == id).FirstOrDefaultAsync();


                rejectEmp.emp_status = false;
                _emp_Gen_InformationContext.Update(rejectEmp);
                await _emp_Gen_InformationContext.SaveChangesAsync();

                rejecttime.resign_date = DateTime.Now.ToString("yyyy-MM-dd");
                _emp_Gen_InformationContext.Update(rejecttime);
                await _emp_Gen_InformationContext.SaveChangesAsync();


                await _emp_Gen_InformationContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(rejectEmp);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest(ex);
            }

        }

        [HttpPut("update_emp")]
        public async Task<IActionResult> update_emp([FromBody] Editclass editclass)
        {
            var transaction = _emp_Gen_InformationContext.Database.BeginTransaction();
            try
            {
                if (editclass == null)
                {
                    return BadRequest();
                }
                int id = editclass.id;
                string first_name = editclass.first_name;
                string last_name = editclass.last_name;
                string department = editclass.department;
                string role = editclass.role;
                string phone = editclass.phone;
                string email = editclass.email;
                string nation = editclass.nation;
                string nickname = editclass.nick_name;
                string address = editclass.address;
                bool marry = editclass.married;
                string account_number = editclass.account_number;
                int sick = editclass.sickleave;
                int personalLeave = editclass.personalleave;
                int vacationLeave = editclass.vacationleave;
                string time_start = editclass.time_start;
                string time_end = editclass.time_end;
                
                if (editclass.children == null)
                {
                    editclass.children = 0;
                }

                var update = _emp_Gen_InformationContext.Emp_General_Information.Where(a => a.emp_gen_id == id).FirstOrDefault();
                var update2 = _emp_Gen_InformationContext.Emp_Personal_Informaion.Where(a => a.emp_gen_id == id).FirstOrDefault();
                var update3 = _emp_Gen_InformationContext.Leavedays.Where(a => a.emp_gen_id == id).FirstOrDefault();


                update.first_name = first_name;
                update.last_name = last_name;
                update.nick_name = nickname;
                update.email = email;
                update.phone = phone;
                update.nationality = nation;

                _emp_Gen_InformationContext.Update(update);
                await _emp_Gen_InformationContext.SaveChangesAsync();

                update2.married = marry;
                update2.children = editclass.children;
                update2.bank_account = account_number;
                update2.address = address;

                _emp_Gen_InformationContext.Update(update2);
                await _emp_Gen_InformationContext.SaveChangesAsync();

                update3.sick_leave = sick;
                update3.personal_leave = personalLeave;
                update3.vacation_leave = vacationLeave;

                _emp_Gen_InformationContext.Update(update3);
                await _emp_Gen_InformationContext.SaveChangesAsync();


                Console.WriteLine(update2);
                await _emp_Gen_InformationContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(update);

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest(ex);
            }
        }





    }

}

