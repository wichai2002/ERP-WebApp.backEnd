using System;
using Microsoft.EntityFrameworkCore;
using Yinnaxs_BackEnd.Models;

namespace Yinnaxs_BackEnd.Context
{
	public class DepartmentContext : DbContext
	{
		public DepartmentContext(DbContextOptions options) : base(options) {
		}

		public DbSet<Department> Departments { get; set; }

	}
}

