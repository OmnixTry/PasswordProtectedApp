using Amazon;
using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using SecureApi.Services.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SecureApi.Services
{
	public class KmsKeyLoader: IKeyLoader
	{
		private readonly AmazonKeyManagementServiceClient client;
		public KmsKeyLoader()
		{
			string awsKeyId = Environment.GetEnvironmentVariable("awsAccessKeyId", EnvironmentVariableTarget.User);
			string awsSecretAccessKey = Environment.GetEnvironmentVariable("awsSecretAccessKey", EnvironmentVariableTarget.User);
			string keyId = Environment.GetEnvironmentVariable("keyId", EnvironmentVariableTarget.User);
			client = new AmazonKeyManagementServiceClient(awsKeyId, awsSecretAccessKey, RegionEndpoint.GetBySystemName("us-east-2"));			 
		}
		public byte[] GetEncryptedKey()
		{
			var key = Environment.GetEnvironmentVariable("EncryptedKey", EnvironmentVariableTarget.User);
			byte[] toBytes = key.Select(x => (byte)x).ToArray();
			return toBytes;
		}

		public byte[] GetDecryptedKey()
		{
			MemoryStream stream = new MemoryStream(GetEncryptedKey());
			DecryptRequest decryptRequest = new DecryptRequest()
			{
				CiphertextBlob = stream
			};
			var decryptedKeyResponse = client.Decrypt(decryptRequest);
			var key = decryptedKeyResponse.Plaintext.ToArray();
			return key;
		}
	}
}
