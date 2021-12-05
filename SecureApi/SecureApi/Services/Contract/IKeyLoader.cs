using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureApi.Services.Contract
{
	public interface IKeyLoader
	{
		byte[] GetEncryptedKey();

		byte[] GetDecryptedKey();
	}
}
