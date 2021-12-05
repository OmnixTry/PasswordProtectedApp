using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SecureApi.Identity.Jwt;
using SecureApi.Models;
using SecureApi.Models.WebEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SecureApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthorizationController : ControllerBase
	{
        private readonly UserManager<User> _userManager;

        public AuthorizationController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] CredentialsWe credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userToVerify = await _userManager.FindByNameAsync(credentials.UserName);
            if (await _userManager.CheckPasswordAsync(userToVerify, credentials.Password))
            {
                return new OkObjectResult("Login Successfull");
            }

            return BadRequest("Invalid username or password.");
        }
    }
}
