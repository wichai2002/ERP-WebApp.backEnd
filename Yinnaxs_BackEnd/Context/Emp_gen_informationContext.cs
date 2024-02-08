using System;
using Microsoft.EntityFrameworkCore;
using Yinnaxs_BackEnd.Models;

namespace Yinnaxs_BackEnd.Context
{
	public class Emp_gen_informationContext: DbContext
	{
		public Emp_gen_informationContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Emp_general_information> Emp_General_Informations { get; set; }



	}
}

