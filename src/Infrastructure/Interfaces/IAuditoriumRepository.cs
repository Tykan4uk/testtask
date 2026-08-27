using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface IAuditoriumRepository
    {
        Task<IEnumerable<Auditorium>> GetAllAsync();
        Task<Auditorium> AddAsync(Auditorium auditorium);
        Task UpdateAsync(Auditorium auditorium);
        Task DeleteAsync(Guid id);
    }
}
