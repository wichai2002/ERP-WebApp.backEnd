using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Yinnaxs_BackEnd.Context;
using Yinnaxs_BackEnd.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Yinnaxs_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : Controller
    {
        private readonly ApplicationDbContext _payrollContext;
        public PayrollController(ApplicationDbContext payrollContext)
        {
            _payrollContext = payrollContext;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payroll>>> GetPayroll()
        {
            //string ppp = "SELECT count(role_id),role_id,emp_gen_id  FROM emp_general_information group by last_name";
            var payroll = await _payrollContext.Emp_General_Information.GroupBy(a => a.role_id).Select(a => new
            {
                RoleId = a.Key,
                Count = a.Count()
            }).ToListAsync();

            var roleIds = payroll.Select(p => p.RoleId).ToList();

            var roles = await _payrollContext.Roles.ToListAsync();

            //find count 
            var rolesWithCounts = roles
                .Join(payroll,
                    role => role.role_id,
                    p => p.RoleId,
                    (role, p) => new
                    {
                        Role = role,
                        Count = p.Count,

                    }
                ).Join(_payrollContext.Departments,
                a => a.Role.department_id,
                b => b.department_id,
                (a, b) => new
                {
                    salary = b.base_salary,
                    RoleId = a.Role,
                    conut = a.Count

                }).ToList();

            //find max min
            var check = _payrollContext.Emp_General_Information.Join(_payrollContext.Payrolls,
                a => a.emp_gen_id,
                b => b.emp_gen_id,
                (emp, pay) => new
                {
                    emp,
                    pay
                }).GroupBy(a => a.emp.role_id).Select(g => new
                {
                    RoleId = g.Key,
                    MaxProperty = g.Max(x => x.pay.salary),
                    MinProperty = g.Min(x => x.pay.salary),
                    SumProperty = g.Sum(x => x.pay.salary)
                }).ToList();


            var sortedCheck = check.OrderBy(c => c.RoleId).ToList();

            //find all people 
            var all_people = _payrollContext.Emp_General_Information.Join(_payrollContext.Roles,
                a => a.role_id,
                b => b.role_id,
                (emp, rol) => new
                {
                    rolId = rol.department_id,
                    empId = emp.emp_gen_id,
                    name = emp.first_name + " " + emp.last_name,
                    roleName = rol.position
                }).Join(_payrollContext.Departments,
                a => a.rolId,
                b => b.department_id,
                (one, dep) => new
                {
                    one,
                    dep
                }).Join(_payrollContext.Payrolls,
                a => a.one.empId,
                b => b.emp_gen_id,
                (one, pay) => new
                {
                    ID = one.one.empId,
                    fullname = one.one.name,
                    position = one.one.roleName,
                    baseSalary = one.dep.base_salary,
                    salaryPeo = pay.salary

                }).ToList();

            //final Count all
            var fini = sortedCheck.Join(rolesWithCounts,
                a => a.RoleId,
                b => b.RoleId.role_id,
                (sort, count) => new
                {
                    RoleName = count.RoleId.position,
                    CountPeo = count.conut,
                    BaseSalary = count.salary,
                    MaxSalary = sort.MaxProperty,
                    MinSalary = sort.MinProperty,
                    SumSalary = sort.SumProperty
                }).ToList();


            //return Ok(all_people);
            return Ok(new { count = fini, all = all_people }); //return 200
        }

        //get data employee
        [HttpGet("idPer/{id}")]
        public async Task<ActionResult<IEnumerable<Payroll>>> GetInfo(int id)
        {
            var Info = await _payrollContext.Emp_General_Information.Join(_payrollContext.Roles,
                a => a.role_id,
                b => b.role_id,
                (info,role) => new
                {
                    EmpId = info.emp_gen_id,
                    fullname = info.first_name + " " + info.last_name,
                    roleName = role.position,
                    roleId = role.department_id
                }).Where(a => a.EmpId == id).Join(_payrollContext.Departments,
                a => a.roleId,
                b => b.department_id,
                (role_one,depart) => new
                {
                    departmentName = depart.department_name,
                    EmpId = role_one.EmpId,
                    Fullname = role_one.fullname,
                    RoleName = role_one.roleName
                }).Join(_payrollContext.Payrolls,
                a => a.EmpId,
                b => b.emp_gen_id,
                (Emp_info, payroll) => new 
                { 
                    EmId = Emp_info.Fullname,
                    departName = Emp_info.departmentName,
                    roleName = Emp_info.RoleName,
                    payInfo = payroll.salary
                }).ToListAsync();


            return Ok(Info);
             
        }

        //update salary employee
        [HttpPut("updateEm/{id}/{pay}")]

        public async Task<ActionResult<IEnumerable<Payroll>>> UpdatePayroll(int id,double pay)
        {
            var transaction = _payrollContext.Database.BeginTransaction();
            try
            {
                var update = _payrollContext.Payrolls.Where(a => a.emp_gen_id == id).FirstOrDefault();

                if (update != null)
                {
                    update.salary = pay;
                    _payrollContext.Update(update);
                }

                await _payrollContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(update);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest(ex);
            }
        }

        //getdata role for update
        [HttpGet("getrole/{role_name}")]

        public async Task<ActionResult<IEnumerable<Payroll>>> GetPayRole(string role_name)
        {
            var getrole = _payrollContext.Roles.Where(a => a.position == role_name).Join(_payrollContext.Departments,
                a => a.department_id,
                b => b.department_id,
                (c,d) => new
                {
                    salaryBase = d.base_salary,
                    DepartmentId = d.department_id
                }).FirstOrDefault();
            return Ok(getrole);
        }

        [HttpPut("updatepayRole/{did}/{pay}")]
        public async Task<ActionResult<IEnumerable<Payroll>>> UpdatePayrollRole(int did,double pay)
        {
            var transaction = _payrollContext.Database.BeginTransaction();
            try
            {
                var updatePayRole = _payrollContext.Departments.Where(a => a.department_id == did).FirstOrDefault();

                updatePayRole.base_salary = pay;
                _payrollContext.Update(updatePayRole);

                await _payrollContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(updatePayRole);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest(ex);
            }
        }


    }
}
