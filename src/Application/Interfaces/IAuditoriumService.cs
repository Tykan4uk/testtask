using Application.Common;
using Application.Dtos;
using Application.Models;

namespace Application.Interfaces
{
    public interface IAuditoriumService
    {
        Task<Result<AuditoriumDto>> CreateAuditoriumAsync(AuditoriumModel auditorium);
    }
}
