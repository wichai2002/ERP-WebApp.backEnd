using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Yinnaxs_BackEnd.Context;
using Yinnaxs_BackEnd.Models;

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
            string ppp = "SELECT count(role_id),role_id,emp_gen_id  FROM emp_general_information group by role_id";
            //var payroll = await _payrollContext.Payrolls.GroupBy(a => a.bonus_per_year).CountAsync();
            var bonusCounts = await _payrollContext.Emp_General_Information.FromSqlRaw(ppp).ToListAsync();






            return Ok(bonusCounts); //return 200
        }

    }
}
