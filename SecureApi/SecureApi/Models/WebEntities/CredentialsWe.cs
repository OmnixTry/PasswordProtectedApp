using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureApi.Models.WebEntities
{
	public class CredentialsWe
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}
}
