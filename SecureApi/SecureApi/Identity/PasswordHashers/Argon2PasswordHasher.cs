using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sodium;
using static Sodium.PasswordHash;
using NaCl.Core;
using Amazon.KeyManagementService;
using Amazon.Runtime;

using Amazon;
using Amazon.KeyManagementService.Model;

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
			

			var kmsClient = new AmazonKeyManagementServiceClient("AKIAVD6BDGYODAGN4WXU", "pmexuvNPHzULIMm6eqQ5onYuBimzABArImgkaq+9", RegionEndpoint.GetBySystemName("us-east-2"));;
			//string keyId = "arn:aws:kms:us-east-2:352055408156:key/0ea00b7c-9946-43c7-abca-a4eed2e1a003";
			string keyId = "0ea00b7c-9946-43c7-abca-a4eed2e1a003";
			var aliases = kmsClient.ListAliases(new ListAliasesRequest() { Limit = 10 });

			DescribeKeyRequest describeKeyRequest = new DescribeKeyRequest()
			{
				KeyId = keyId
			};

			var request = new GenerateDataKeyRequest
			{
				KeyId = keyId,
				KeySpec = DataKeySpec.AES_256,
				
			};

			GenerateDataKeyResponse response = kmsClient.GenerateDataKey(request);

			var key = response.Plaintext.ToArray();
			var encryptedKey = response.CiphertextBlob.ToArray();

			ReadOnlyMemory<byte> readOnlyMemory = new ReadOnlyMemory<byte>(key);
			var encryption = new XChaCha20Poly1305(key);
			ReadOnlySpan<byte> span = new ReadOnlySpan<byte>("blah blah".Select(x => (byte)x).ToArray());
			byte[] nonce = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4 };
			var enc = encryption.Encrypt("blah blah".Select(x => (byte)x).ToArray(), nonce: nonce);
			var dec = encryption.Decrypt(enc, null, nonce: nonce);
			var res = String.Concat(dec.Select(x => (char)x).ToList());

			DecryptRequest decryptRequest = new DecryptRequest()
			{
				CiphertextBlob = response.CiphertextBlob
			};
			var decryptedKey = kmsClient.Decrypt(decryptRequest);
			var keyAgain = decryptedKey.Plaintext.ToArray();


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
