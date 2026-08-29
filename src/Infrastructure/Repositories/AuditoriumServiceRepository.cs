using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AuditoriumServiceRepository : IAuditoriumServiceRepository
    {
        private readonly TestTaskDbContext _context;

        public AuditoriumServiceRepository(TestTaskDbContext context)
        {
            _context = context;
        }

        public async Task<List<AuditoriumService>> GetByIdsAsync(Guid auditoriumId, IEnumerable<Guid> ids)
        {
            var result = await _context.AuditoriumServices
                .Include(x => x.Service)
                .Where(x => x.AuditoriumId == auditoriumId && ids.Contains(x.Id))
                .ToListAsync();

            return result;
        }
    }
}
