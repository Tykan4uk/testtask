using Domain.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class AuditoriumReserveRepository : IAuditoriumReserveRepository
    {
        private readonly TestTaskDbContext _context;

        public AuditoriumReserveRepository(TestTaskDbContext context)
        {
            _context = context;
        }
        public async Task<AuditoriumReserve> AddAsync(AuditoriumReserve auditoriumReserve)
        {
            await _context.AuditoriumReserves.AddAsync(auditoriumReserve);

            return auditoriumReserve;
        }
    }
}
