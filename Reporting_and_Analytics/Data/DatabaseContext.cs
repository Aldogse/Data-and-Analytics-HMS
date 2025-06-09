using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models_and_Enums.Financial;
using Models_and_Enums.patient_and_treatment_statistics;

namespace Reporting_and_Analytics.Data
{
	public class DatabaseContext : IdentityDbContext
	{
		public DatabaseContext(DbContextOptions <DatabaseContext> options) : base(options)
		{
			
		}
		public DbSet <Particular> Particulars { get; set; }
		public DbSet <IncomeStatement> IncomeStatements { get; set; }
        public DbSet<HospitalIncomeRecords> HospitalIncomeRecords { get; set; }
		public DbSet<DailyPatientReport> DailyPatientReports { get; set; }
		public DbSet<MonthlyPatientReport> MonthlyPatientReports { get; set; }
		public DbSet<PatientRecords> PatientRecords { get; set; }
		public DbSet<DailyIncomeReport> DailyIncomeRecords { get; set; }
	}
}
