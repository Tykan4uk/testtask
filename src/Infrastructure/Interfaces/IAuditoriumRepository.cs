using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface IAuditoriumRepository
    {
        Task<IEnumerable<Auditorium>> GetAllAsync();
        Task<Auditorium?> GetByNameAsync(string name);
        Task<Auditorium?> GetByIdAsync(Guid id);
        Task<List<Auditorium>> GetFreeAuditoriumsAsync(DateTime start, DateTime end, int capacity);
        Task<Auditorium> AddAsync(Auditorium auditorium);
        Task UpdateAsync(Auditorium auditorium);
        Task DeleteAsync(Guid id);
    }
}
