using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecureApi.Models;
using SecureApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureApi.Database
{
	public class AuthenticationContext: IdentityDbContext
	{
		public AuthenticationContext(DbContextOptions dbContextOptions): base(dbContextOptions)
		{

		}

		public DbSet<DefaultUser> DefaultUsers { get; set; }

		public DbSet<GreatMystery> GreatMystery { get; set; }
	}
}
