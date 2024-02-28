using System;
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
		public DbSet<Emp_personal_informaion> Emp_Personal_Informaion { get; set; }
		public DbSet<HrAccount> HrAccounts { get; set; }

		public DbSet<Emp_training> Emp_Training { get; set; }
		public DbSet<Applicant> Applicants { get; set; }
		public DbSet<Appointment> Appointments { get; set; }

		public DbSet<Leave> Leaves { get; set; }

		public DbSet<Leaveday> Leavedays { get; set;}

		public DbSet<Payroll> Payrolls { get; set; }
	}
}

