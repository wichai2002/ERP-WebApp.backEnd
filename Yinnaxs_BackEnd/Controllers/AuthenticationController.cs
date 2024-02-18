using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using Yinnaxs_BackEnd.Models;
using Yinnaxs_BackEnd.Context;
using Yinnaxs_BackEnd.Utility;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Yinnaxs_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {

        private class PlayloadAuthen
        {
            public int emp_gen_id { get; set; }
            public string? authen_token { get; set; }
        }

        private IConfiguration _configuration;
        private ApplicationDbContext _applicationDbContext;
        public AuthenticationController(IConfiguration configuration, ApplicationDbContext applicationDbContext)
        {
            _configuration = configuration;
            _applicationDbContext = applicationDbContext;
        }

        // POST api/values
        [HttpPost]
        public IActionResult Authen([FromBody] HrAccount hrAccount)
        {
            try
            {
                HashAlgorithm hashAlgorithm = new HashAlgorithm();
                var account = _applicationDbContext.HrAccounts.Where(s => s.emp_gen_id == hrAccount.emp_gen_id).Single();

                if (account == null)
                {
                    return BadRequest();
                }

                if (hashAlgorithm.VerifyHash(hrAccount.password, account.password) == false)
                {
                    return Unauthorized(hashAlgorithm.VerifyHash(hrAccount.password, account.password));
                }

                var securityKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var Sectoken = new JwtSecurityToken
                    (_configuration["Jwt:Issuer"],
                        _configuration["Jwt:Issuer"],
                        null,
                        //expires: DateTime.Now.AddMinutes(60),
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: credentials
                    );

                var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
                PlayloadAuthen playload = new PlayloadAuthen
                {
                    emp_gen_id = hrAccount.emp_gen_id,
                    authen_token = token.ToString()
                };

                return Ok(playload);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }


}

