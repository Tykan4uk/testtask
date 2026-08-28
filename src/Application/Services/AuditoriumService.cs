using Application.Common;
using Application.Dtos;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services
{
    public class AuditoriumService : IAuditoriumService
    {
        // TO DO: 28.08.2026 - It`s best to move all errors to resource file with application growth
        private const string _auditoriumExist = "An audience with that name already exists";

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
                return Result<AuditoriumDto>.Failure(new Error(400,_auditoriumExist));
            }

            var entity = _mapper.Map<AuditoriumModel, Auditorium>(auditorium);

            await _db.Auditoriums.AddAsync(entity);
            await _db.SaveChangesAsync();

            var dto = _mapper.Map<Auditorium, AuditoriumDto>(entity);

            return Result<AuditoriumDto>.Success(dto);
        }
    }
}
