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
    public class AuditoriumService : IAuditoriumService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public AuditoriumService(IUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<Result<AuditoriumDto>> CreateAuditoriumAsync(AuditoriumModel auditorium)
        {
            var existingAuditorium = await _db.Auditoriums.GetByNameAsync(auditorium.Name);

            if (existingAuditorium != null)
            {
                return Result<AuditoriumDto>.Failure(new Error(400,ErrorReasons.AuditoriumExist, ErrorType.BadRequest));
            }

            var entity = _mapper.Map<AuditoriumModel, Auditorium>(auditorium);

            await _db.Auditoriums.AddAsync(entity);
            await _db.SaveChangesAsync();

            var dto = _mapper.Map<Auditorium, AuditoriumDto>(entity);

            return Result<AuditoriumDto>.Success(dto);
        }

        public async Task<Result<AuditoriumDto>> UpdateAuditoriumAsync(AuditoriumModel auditorium)
        {
            if (auditorium.Id == null)
            {
                return Result<AuditoriumDto>.Failure(new Error(404, ErrorReasons.AuditoriumNotExist, ErrorType.NotFound));
            }

            var existingAuditorium = await _db.Auditoriums.GetByIdAsync(auditorium.Id.Value);

            if (existingAuditorium == null)
            {
                return Result<AuditoriumDto>.Failure(new Error(404, ErrorReasons.AuditoriumNotExist, ErrorType.NotFound));
            }

            var auditoriumWithSameName = await _db.Auditoriums.GetByNameAsync(auditorium.Name);

            if (auditoriumWithSameName != null && auditoriumWithSameName.Id != auditorium.Id)
            {
                return Result<AuditoriumDto>.Failure(new Error(400, ErrorReasons.AuditoriumExist, ErrorType.BadRequest));
            }

            _mapper.Map(auditorium, existingAuditorium);

            await _db.Auditoriums.UpdateAsync(existingAuditorium);
            await _db.SaveChangesAsync();

            var dto = _mapper.Map<Auditorium, AuditoriumDto>(existingAuditorium);

            return Result<AuditoriumDto>.Success(dto);
        }

        public async Task<Result> RemoveAuditoriumAsync(Guid id)
        {
            var existingAuditorium = await _db.Auditoriums.GetByIdAsync(id);

            if (existingAuditorium == null)
            {
                return Result<AuditoriumDto>.Failure(new Error(404, ErrorReasons.AuditoriumNotExist, ErrorType.NotFound));
            }

            await _db.Auditoriums.DeleteAsync(id);
            await _db.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<List<AuditoriumDto>>> SearchFreeAsync(AuditoriumSearchFreeModel model)
        {
            if (model.StartTime >= model.EndTime)
            {
                return Result<List<AuditoriumDto>>.Failure(new Error(400, ErrorReasons.StartTimeBiggerThanEnd, ErrorType.BadRequest));
            }
            var dateTimeStart = DateTime.SpecifyKind(model.Date.ToDateTime(model.StartTime), DateTimeKind.Utc);

            var dateTimeEnd = DateTime.SpecifyKind(model.Date.ToDateTime(model.EndTime),DateTimeKind.Utc);

            var auditoriums = await _db.Auditoriums.GetFreeAuditoriumsAsync(
                    dateTimeStart,
                    dateTimeEnd,
                    model.Capacity);

            var dto = _mapper.Map<List<AuditoriumDto>>(auditoriums);

            return Result<List<AuditoriumDto>>.Success(dto);
        }
    }
}
