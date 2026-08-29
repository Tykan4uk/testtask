using Application.Common;
using Application.Dtos;
using Application.Models;

namespace Application.Interfaces
{
    public interface IReportService
    {
        Task<Result<AuditoriumReserveReportDto>> GetReserveReportAsync(AuditoriumReserveReportModel model);
    }
}
