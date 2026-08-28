using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AuditoriumRepository : IAuditoriumRepository
    {
        private readonly TestTaskDbContext _context;

        public AuditoriumRepository(TestTaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Auditorium>> GetAllAsync()
        {
            var result = await _context.Auditoriums.ToListAsync();

            return result;
        }

        public async Task<Auditorium?> GetByNameAsync(string name)
        {
            var result = await _context.Auditoriums.FirstOrDefaultAsync(a => a.Name == name);

            return result;
        }

        public async Task<Auditorium> AddAsync(Auditorium auditorium)
        {
            await _context.Auditoriums.AddAsync(auditorium);

            return auditorium;
        }

        public Task UpdateAsync(Auditorium auditorium)
        {
            _context.Auditoriums.Update(auditorium);

            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Auditoriums.FindAsync(id);

            if (entity != null)
                _context.Auditoriums.Remove(entity);
        }
    }
}
