using SecureApi.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureApi.Models.Entities
{
	[]
	public class GreatMystery
	{
		public int Id { get; set; }

		[Encrypt]
		public string MysteriousMessage { get; set; }
	}
}
