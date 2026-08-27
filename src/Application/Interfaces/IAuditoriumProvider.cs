using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAuditoriumProvider
    {
        Task<Auditorium?> GetByIdAsync(Guid id);
        Task<IReadOnlyCollection<Auditorium>> GetAllAsync();
        Task AddAsync(Auditorium auditorium);
        Task UpdateAsync(Auditorium auditorium);
        Task DeleteAsync(Guid id);
    }
}
