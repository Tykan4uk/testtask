using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface ITimeRateRepository
    {
        Task<List<TimeRate>> GetListAsync();
    }
}
