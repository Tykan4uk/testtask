using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AuditoriumReserveRepository : IAuditoriumReserveRepository
    {
        private readonly TestTaskDbContext _context;

        public AuditoriumReserveRepository(TestTaskDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsBusyAsync(Guid auditoriumId, DateTime start, DateTime end)
        {
            var result = await _context.AuditoriumReserves.AnyAsync(x =>
                    x.AuditoriumId == auditoriumId &&
                    x.DateTime < end &&
                    x.EndDateTime > start);

            return result;
        }

        public async Task<AuditoriumReserve> AddAsync(AuditoriumReserve reserve)
        {
            await _context.AuditoriumReserves.AddAsync(reserve);

            return reserve;
        }
    }
}
