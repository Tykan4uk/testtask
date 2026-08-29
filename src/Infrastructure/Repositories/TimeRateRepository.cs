using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TimeRateRepository : ITimeRateRepository
    {
        private readonly TestTaskDbContext _context;

        public TimeRateRepository(TestTaskDbContext context)
        {
            _context = context;
        }

        public async Task<List<TimeRate>> GetListAsync()
        {
            var result = await _context.TimeRates
                .AsNoTracking()
                .ToListAsync();

            return result;
        }
    }
}
