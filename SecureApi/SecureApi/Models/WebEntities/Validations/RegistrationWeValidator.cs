using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureApi.Models.WebEntities.Validations
{
	public class RegistrationWeValidator: AbstractValidator<RegistrationWe>
	{
		public RegistrationWeValidator()
		{
			RuleFor(we => we.Email).NotEmpty().WithMessage("Specify value for Email");
			RuleFor(we => we.Password).NotEmpty().WithMessage("Specify value for Password");
			RuleFor(we => we.FirstName).NotEmpty().WithMessage("Specify value for First name");
			RuleFor(we => we.LastName).NotEmpty().WithMessage("Specify value for Last Name");
			RuleFor(we => we.Location).NotEmpty().WithMessage("Specify value for Location");
		}
	}
}
