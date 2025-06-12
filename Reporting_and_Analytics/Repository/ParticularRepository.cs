using Microsoft.EntityFrameworkCore;
using Models_and_Enums.Financial;
using Reporting_and_Analytics.Data;
using Reporting_and_Analytics.Interface;

namespace Reporting_and_Analytics.Repository
{
	public class ParticularRepository : IParticularRepository
	{
		private readonly DatabaseContext _databaseContext;
        public ParticularRepository(DatabaseContext database_context)
        {
            _databaseContext = database_context;
        }
        public async Task<List<Particular>> GetAllServices()
		{
			return await _databaseContext.Particulars.ToListAsync();
		}

		public async Task<Particular> GetServiceById(int transaction_id)
		{
			return await _databaseContext.Particulars.Where(s => s.transaction_id == transaction_id).FirstOrDefaultAsync();
		}

		public async Task<List<Particular>> GetServicesByMonth(int month)
		{
			return await _databaseContext.Particulars.Where(m => m.transaction_date.Month == month).ToListAsync();
		}

		public async Task<List<Particular>> GetServicesByYear(int year)
		{
			return await _databaseContext.Particulars.Where(m => m.transaction_date.Year == year).ToListAsync();
		}

	}
}
