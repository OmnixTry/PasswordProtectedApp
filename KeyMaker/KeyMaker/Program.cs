using Amazon;
using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using NaCl.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace KeyMaker
{
	class Program
	{
		static void Main(string[] args)
		{					
			string awsKeyId = Environment.GetEnvironmentVariable("awsAccessKeyId", EnvironmentVariableTarget.User);
			string awsSecretAccessKey = Environment.GetEnvironmentVariable("awsSecretAccessKey", EnvironmentVariableTarget.User);
			string keyId = Environment.GetEnvironmentVariable("keyId", EnvironmentVariableTarget.User);
			var kmsClient = new AmazonKeyManagementServiceClient(awsKeyId, awsSecretAccessKey, RegionEndpoint.GetBySystemName("us-east-2"));
			

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

			var encS = new SalsaEncryptionServise(new KmsKeyLoader());

			var rand = RandomNumberGenerator.Create();
			byte[] byres = new byte[5];
			rand.GetBytes(byres);
			var encryptedString = encS.Encrypt(encS.BytesToString(byres));

			var decryptedString = encS.Decrypt(encryptedString);
			var second = encS.ToByteArray(decryptedString);
			Console.WriteLine();




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

	public class SalsaEncryptionServise
	{
		const int NonceLength = 24;

		private readonly KmsKeyLoader keyLoader;
		private byte[] key;

		public SalsaEncryptionServise(KmsKeyLoader keyLoader)
		{
			this.keyLoader = keyLoader;
			key = keyLoader.GetDecryptedKey();
		}

		public string Encrypt(string data)
		{
			ReadOnlyMemory<byte> readOnlyMemory = new ReadOnlyMemory<byte>(key);
			var encryption = new XChaCha20Poly1305(key);
			var nonce = GetNonce();
			var encrypted = encryption.Encrypt(data.Select(x => (byte)x).ToArray(), aad: null, nonce: nonce);
			var encryptedWithNonce = encrypted.Concat(nonce);
			return BytesToString(encryptedWithNonce);
		}
		public string Decrypt(string data)
		{
			ReadOnlyMemory<byte> readOnlyMemory = new ReadOnlyMemory<byte>(key);
			var encryption = new XChaCha20Poly1305(key);

			var bytes = ToByteArray(data);
			var nonce = bytes.TakeLast(NonceLength).ToArray();
			var encryptedData = bytes.Take(data.Length - NonceLength).ToArray();

			var decryptedData = encryption.Decrypt(encryptedData, null, nonce: nonce);
			return BytesToString(decryptedData);
		}

		private byte[] GetNonce()
		{
			var rand = RandomNumberGenerator.Create();

			var nonce = new byte[NonceLength];
			rand.GetBytes(nonce);
			return nonce;
		}

		public string BytesToString(IEnumerable<byte> data)
		{
			return String.Concat(data.Select(x => (char)x));
		}

		public byte[] ToByteArray(string data)
		{
			return data.Select(x => (byte)x).ToArray();
		}
	}

	public class KmsKeyLoader
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
