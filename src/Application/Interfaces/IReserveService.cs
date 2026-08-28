using Application.Common;
using Application.Dtos;
using Application.Models;

namespace Application.Interfaces
{
    public interface IReserveService
    {
        Task<Result<AuditoriumReserveDto>> CreateReserveAsync(AuditoriumReserveModel model);
    }
}
