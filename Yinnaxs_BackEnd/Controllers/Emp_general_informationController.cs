using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Yinnaxs_BackEnd.Context;
using Yinnaxs_BackEnd.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Yinnaxs_BackEnd.Controllers
{
    [Route("api/[controller]")]
    public class Emp_general_informationController : ControllerBase
    {
        private readonly ApplicationDbContext _emp_Gen_InformationContext;

        public Emp_general_informationController(ApplicationDbContext emp_Gen_InformationContext)
        {
            _emp_Gen_InformationContext = emp_Gen_InformationContext;
        }
    }
}

