using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureApi.Identity.PasswordHashers
{
	public class BCryptPasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
	{
		public const int WorkFactor = 12;
		public string HashPassword(TUser user, string password)
		{
			return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
		}

		public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
		{
			var isValid = BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);

			if (isValid)
			{
				if(BCrypt.Net.BCrypt.PasswordNeedsRehash(hashedPassword, WorkFactor))
				{
					return PasswordVerificationResult.SuccessRehashNeeded;
				}

				return PasswordVerificationResult.Success;
			}

			return PasswordVerificationResult.Failed;
		}
	}
}
