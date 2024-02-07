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
    public class RoleController : Controller
    {
        private readonly RoleContext _roleContext;

        public RoleController(RoleContext roleContext)
        {
            _roleContext = roleContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            var roles = await _roleContext.Roles.ToListAsync();

            return Ok(roles);
        }

        [HttpPost]
        public async Task<ActionResult<Role>> CreateRole(Role role)
        {
            _roleContext.Roles.Add(role);
            await _roleContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRoles), new {id = role.role_id, role});
        }
    }
}

