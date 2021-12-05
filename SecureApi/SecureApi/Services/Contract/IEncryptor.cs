using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureApi.Services.Contract
{
	public interface IEncryptor
	{
		string Encrypt(string data);
		string Decrypt(string data);
	}
}
