using System;
using Yinnaxs_BackEnd.Models;
using Microsoft.EntityFrameworkCore;


namespace Yinnaxs_BackEnd.Context
{
	public class RoleContext: DbContext
	{
		public RoleContext(DbContextOptions options): base(options)
		{
		}

		public DbSet<Role> Roles { get; set; }
	}
}

