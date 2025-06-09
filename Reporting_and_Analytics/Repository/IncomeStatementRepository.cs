using Microsoft.EntityFrameworkCore;
using Models_and_Enums.Financial;
using Reporting_and_Analytics.Data;
using Reporting_and_Analytics.Interface;

namespace Reporting_and_Analytics.Repository
{
	public class IncomeStatementRepository : IIncomeStatementRepository
	{
		private readonly DatabaseContext _databaseContext;
        public IncomeStatementRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

		public async Task<IncomeStatement> GetReportById(int id)
		{
			return await _databaseContext.IncomeStatements.Where(i => i.report_id == id).FirstOrDefaultAsync();
		}

		public Task<List<IncomeStatement>> IncomeStatementByMonth(int month)
		{
			return _databaseContext.IncomeStatements.Where(m => m.Month == month).ToListAsync();
		}

		public async Task<List<IncomeStatement>> IncomeStatementByYear(int year)
		{
			return await _databaseContext.IncomeStatements.Where(i => i.year == year).ToListAsync();
		}

		public async Task<List<IncomeStatement>> IncomeStatementsRecords()
		{
			return await _databaseContext.IncomeStatements.ToListAsync();
		}
	}
}
