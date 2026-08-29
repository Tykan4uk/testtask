using Application.Common;
using Application.Dtos;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Resources;
using Infrastructure.Interfaces;

namespace Application.Services
{
    public class ReserveService : IReserveService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public ReserveService(IUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<Result<AuditoriumReserveDto>> CreateReserveAsync(AuditoriumReserveModel model)
        {
            var start = model.Date;
            var end = start.Add(model.Duration);

            if (model.Duration <= TimeSpan.Zero)
            {
                return Result<AuditoriumReserveDto>.Failure(new Error(400, ErrorReasons.DurationMustBeNotZero, ErrorType.BadRequest));
            }

            var auditorium = await _db.Auditoriums.GetByIdAsync(model.AuditoriumId);

            if (auditorium == null)
            {
                return Result<AuditoriumReserveDto>.Failure(new Error(404, ErrorReasons.AuditoriumNotExist, ErrorType.NotFound));
            }

            var isBusy = await _db.AuditoriumReserves.IsBusyAsync(model.AuditoriumId, start, end);

            if (isBusy)
            {
                return Result<AuditoriumReserveDto>.Failure(new Error(409, ErrorReasons.AuditoriumAlreadyReserved, ErrorType.Conflict));
            }

            var serviceIds = model.Services.Select(x => x.Id).Distinct().ToList();

            var auditoriumServices = await _db.AuditoriumServices.GetByIdsAsync(model.AuditoriumId, serviceIds);

            if (auditoriumServices.Count() != serviceIds.Count)
            {
                return Result<AuditoriumReserveDto>.Failure(new Error(400, ErrorReasons.ServicesNotAvailable, ErrorType.BadRequest));
            }

            var rates = await _db.TimeRates.GetListAsync();

            var auditoriumPrice = CalculateAuditoriumPrice(auditorium.BaseRentalPrice, start, end, rates);

            var servicesPrice = auditoriumServices.Sum(x => x.Service.Price);

            var totalPrice = auditoriumPrice + servicesPrice;

            var reserve = _mapper.Map<AuditoriumReserveModel, AuditoriumReserve>(model);

            await _db.AuditoriumReserves.AddAsync(reserve);

            await _db.SaveChangesAsync();

            var dto = _mapper.Map<AuditoriumReserve, AuditoriumReserveDto>(reserve);
            dto.TotalPrice = (int)Math.Round(totalPrice);

            return Result<AuditoriumReserveDto>.Success(dto);
        }

        private decimal CalculateAuditoriumPrice(int hourlyPrice, DateTime start, DateTime end, IEnumerable<TimeRate> rates)
        {
            decimal total = 0;

            var current = start;

            while (current < end)
            {
                var currentTime = TimeOnly.FromDateTime(current);

                var rate = rates.FirstOrDefault(x => IsTimeInRate(currentTime, x));

                var rateEnd = current.Date.Add(rate.EndTime.ToTimeSpan());

                if (rateEnd <= current)
                {
                    rateEnd = rateEnd.AddDays(1);
                }

                var segmentEnd = rateEnd < end
                    ? rateEnd
                    : end;

                var hours = (decimal)Math.Round((segmentEnd - current).TotalHours);

                total += hourlyPrice * hours * rate.Rate;

                current = segmentEnd.AddMinutes(1);
            }

            return total;
        }

        private bool IsTimeInRate(TimeOnly time, TimeRate rate)
        {
            if (rate.StartTime < rate.EndTime)
            {
                return time >= rate.StartTime && time <= rate.EndTime;
            }

            return time >= rate.StartTime || time <= rate.EndTime;
        }
    }
}
