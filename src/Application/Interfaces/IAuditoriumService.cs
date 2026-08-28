using Application.Common;
using Application.Dtos;
using Application.Models;

namespace Application.Interfaces
{
    public interface IAuditoriumService
    {
        Task<Result<AuditoriumDto>> CreateAuditoriumAsync(AuditoriumModel auditorium);
        Task<Result<AuditoriumDto>> UpdateAuditoriumAsync(AuditoriumModel auditorium);
        Task<Result> RemoveAuditoriumAsync(Guid id);
        Task<Result<List<AuditoriumDto>>> SearchFreeAsync(AuditoriumSearchFreeModel model);
    }
}
