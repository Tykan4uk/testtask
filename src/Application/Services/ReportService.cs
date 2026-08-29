using Application.Common;
using Application.Dtos;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Resources;
using Infrastructure.Interfaces;

namespace Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public ReportService(IUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<Result<AuditoriumReserveReportDto>> GetReserveReportAsync(AuditoriumReserveReportModel model)
        {
            if (model.From >= model.To)
            {
                return Result<AuditoriumReserveReportDto>.Failure(new Error(400, ErrorReasons.StartTimeBiggerThanEnd, ErrorType.BadRequest));
            }

            var reserves = await _db.AuditoriumReserves.GetByPeriodAsync(model.From, model.To);

            var items = _mapper.Map<List<AuditoriumReserveDto>>(reserves);

            var totalPrice = reserves.Sum(r => r.TotalPrice);

            var result = new AuditoriumReserveReportDto
            {
                Reserves = items,
                TotalPrice = totalPrice
            };

            return Result<AuditoriumReserveReportDto>.Success(result);
        }
    }
}