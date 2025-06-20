using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models_and_Enums.Financial;
using Models_and_Enums.patient_and_treatment_statistics;
using Models_and_Enums.Staff;

namespace Reporting_and_Analytics.Data
{
	public class DatabaseContext : IdentityDbContext
	{
		public DatabaseContext(DbContextOptions <DatabaseContext> options) : base(options)
		{
			 
		}
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<Employee>()
				   .Property(i => i.shift_start)
				   .HasColumnType("time");

			builder.Entity<Employee>()
				   .Property(i => i.shift_end)
				   .HasColumnType("time");
		}

		public DbSet <Particular> Particulars { get; set; }
		public DbSet <IncomeStatement> IncomeStatements { get; set; }
        public DbSet<HospitalIncomeRecords> HospitalIncomeRecords { get; set; }
		public DbSet<DailyPatientReport> DailyPatientReports { get; set; }
		public DbSet<MonthlyPatientReport> MonthlyPatientReports { get; set; }
		public DbSet<PatientRecords> PatientRecords { get; set; }
		public DbSet<DailyIncomeReport> DailyIncomeRecords { get; set; }
        public DbSet<Employee> Employees { get; set; }
		public DbSet<AdheranceReport> AdheranceReports { get; set; }

		
    }
}
