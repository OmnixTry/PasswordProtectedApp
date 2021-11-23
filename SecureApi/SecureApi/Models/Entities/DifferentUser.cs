using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureApi.Models.Entities
{
	public class DefaultUser
	{
		public int Id { get; set; }
		public string IdentityId { get; set; }
		public User Identity { get; set; }  // navigation property
		public string Location { get; set; }
	}
}
