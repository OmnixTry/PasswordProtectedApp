using Amazon;
using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using System;
using System.Linq;

namespace KeyMaker
{
	class Program
	{
		static void Main(string[] args)
		{					
			string awsKeyId = Environment.GetEnvironmentVariable("awsAccessKeyId", EnvironmentVariableTarget.User);
			string awsSecretAccessKey = Environment.GetEnvironmentVariable("awsSecretAccessKey", EnvironmentVariableTarget.User);
			string keyId = Environment.GetEnvironmentVariable("keyId", EnvironmentVariableTarget.User);
			var kmsClient = new AmazonKeyManagementServiceClient(awsKeyId, awsSecretAccessKey, RegionEndpoint.GetBySystemName("us-east-2")); ;
			

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
			var stringifiedKey = String.Concat(encryptedKey.Select(x => (char)x));
			Environment.SetEnvironmentVariable("EncryptedKey", stringifiedKey, EnvironmentVariableTarget.User);

			/*		
			var gotKey = Environment.GetEnvironmentVariable("EncryptedKey", EnvironmentVariableTarget.User);
			byte[] toBytesAgain = gotKey.Select(x => (byte)x).ToArray();
			Console.WriteLine();
			*/
			/*
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
			*/
		}
	}
}
