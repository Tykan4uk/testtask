using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface IAuditoriumReserveRepository
    {
        Task<AuditoriumReserve> AddAsync(AuditoriumReserve auditoriumReserve);
    }
}
