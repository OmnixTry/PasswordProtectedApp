using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sodium;
using static Sodium.PasswordHash;

namespace SecureApi.Identity.PasswordHashers
{
	public class Argon2PasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
	{
		const StrengthArgon Strength = StrengthArgon.Sensitive;
		public string HashPassword(TUser user, string password)
		{
			if(password == null)
			{
				throw new ArgumentException("Password can't be null");
			}
			return PasswordHash.ArgonHashString(password, Strength).TrimEnd('\0');
		}

		public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
		{
			if(hashedPassword == null)
			{
				throw new ArgumentNullException("Hash Can't be null");
			}
			if (providedPassword == null)
			{
				throw new ArgumentNullException("providedPassword Can't be null");
			}

			var isValid = PasswordHash.ArgonHashStringVerify(hashedPassword, providedPassword);
			if (isValid && PasswordHash.ArgonPasswordNeedsRehash(hashedPassword, Strength))
			{
				return PasswordVerificationResult.SuccessRehashNeeded;
			}

			return isValid ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
		}
	}
}
