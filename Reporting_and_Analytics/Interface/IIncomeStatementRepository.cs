using Models_and_Enums.Financial;

namespace Reporting_and_Analytics.Interface
{
	public interface IIncomeStatementRepository
	{
		Task<List<IncomeStatement>> IncomeStatementByMonth(int month);
		Task<List<IncomeStatement>> IncomeStatementsRecords();
		Task<IncomeStatement>GetReportById(int id);
		Task<List<IncomeStatement>>IncomeStatementByYear(int year);
	}
}
