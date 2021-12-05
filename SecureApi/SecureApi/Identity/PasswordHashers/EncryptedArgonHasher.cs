using Microsoft.AspNetCore.Identity;
using SecureApi.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureApi.Identity.PasswordHashers
{
	public class EncryptedArgonHasher<TUser> : Argon2PasswordHasher<TUser> where TUser : class
	{
		private readonly IEncryptor encryptor;

		public EncryptedArgonHasher(IEncryptor encryptor)
		{
			this.encryptor = encryptor;
		}

		public override string HashPassword(TUser user, string password)
		{
			var hashedPassword = base.HashPassword(user, password);

			return encryptor.Encrypt(hashedPassword);
		}

		public override PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
		{
			var decryptedHash = encryptor.Decrypt(hashedPassword);
			return base.VerifyHashedPassword(user, decryptedHash, providedPassword);
		}
	}
}
