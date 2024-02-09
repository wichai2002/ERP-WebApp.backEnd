﻿using System;
using Microsoft.EntityFrameworkCore;
using Yinnaxs_BackEnd.Models;

namespace Yinnaxs_BackEnd.Context
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Department> Departments { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Emp_general_information> Emp_General_Information { get; set; }
	}
}

