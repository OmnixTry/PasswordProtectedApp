using NaCl.Core;
using SecureApi.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SecureApi.Services.Impl
{
	public class SalsaEncryptionServise : IEncryptor
	{
		const int NonceLength = 24;

		private readonly IKeyLoader keyLoader;
		private byte[] key;

		public SalsaEncryptionServise(IKeyLoader keyLoader)
		{
			this.keyLoader = keyLoader;
			key = keyLoader.GetDecryptedKey();
		}

		public string Encrypt(string data)
		{
			ReadOnlyMemory<byte> readOnlyMemory = new ReadOnlyMemory<byte>(key);
			var encryption = new XChaCha20Poly1305(key);
			var nonce = GetNonce();
			var encrypted = encryption.Encrypt(data.Select(x => (byte)x).ToArray(), nonce: nonce);
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

		private string BytesToString(IEnumerable<byte> data)
		{
			return String.Concat(data.Select(x => (char)x));
		}

		private byte[] ToByteArray(string data)
		{
			return data.Select(x => (byte)x).ToArray();
		}
	}
}
