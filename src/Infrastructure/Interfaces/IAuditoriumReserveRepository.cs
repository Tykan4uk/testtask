using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface IAuditoriumReserveRepository
    {
        Task<bool> IsBusyAsync(Guid auditoriumId, DateTime start, DateTime end);
        Task<AuditoriumReserve> AddAsync(AuditoriumReserve reserve);
    }
}
