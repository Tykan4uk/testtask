using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface IAuditoriumServiceRepository
    {
        Task<List<AuditoriumService>> GetByIdsAsync(Guid auditoriumId, IEnumerable<Guid> ids);
    }
}
