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
        private readonly ApplicationDbContext _roleContext;

        public RoleController(ApplicationDbContext roleContext)
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRoleById(int id)
        {
            var role_one = await _roleContext.Roles.FindAsync(id);

            if (role_one == null)
            {
                return NotFound();
            }

            return Ok(role_one);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, Role role)
        {
            if (id != role.role_id)
            {
                return BadRequest();
            }

            _roleContext.Entry(role).State = EntityState.Modified;

            try
            {
                await _roleContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExits(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var delete_role = await _roleContext.Roles.FindAsync(id);

            if (delete_role == null)
            {
                return BadRequest();
            }

            _roleContext.Roles.Remove(delete_role);
            await _roleContext.SaveChangesAsync();

            return NoContent();
        }

        private bool RoleExits(int id)
        {
            return _roleContext.Roles.Any(e => e.role_id == id);
        }
    }
}

