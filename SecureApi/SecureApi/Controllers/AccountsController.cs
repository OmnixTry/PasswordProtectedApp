using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SecureApi.Database;
using SecureApi.Models;
using SecureApi.Models.Entities;
using SecureApi.Models.WebEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly UserManager<User> userManager;
		private readonly AuthenticationContext context;
		private readonly IMapper mapper;
		private readonly IValidator<RegistrationWe> registrationValidator;

		public AccountsController(UserManager<User> userManager, AuthenticationContext context, IMapper mapper, IValidator<RegistrationWe> registrationValidator)
		{
			this.userManager = userManager;
			this.mapper = mapper;
			this.registrationValidator = registrationValidator;
			this.context = context;
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] RegistrationWe registration)
		{
			var validationResult = registrationValidator.Validate(registration);
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult);
			}

			var userEntity = mapper.Map<User>(registration);
			userEntity.UserName = registration.Email;
			var result = await userManager.CreateAsync(userEntity, registration.Password);

			if(!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}

			await context.DefaultUsers.AddAsync(new DefaultUser { IdentityId = userEntity.Id, Location = registration.Location });
			await context.SaveChangesAsync();

			return Ok();
		}
	}
}
