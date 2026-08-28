using Application.Dtos;
using Application.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles
{
    public class AuditoriumReserveProfile : Profile
    {
        public AuditoriumReserveProfile()
        {
            CreateMap<AuditoriumReserveModel, AuditoriumReserve>()
                .ForMember(d => d.AuditoriumId, opt => opt.MapFrom(x => x.AuditoriumId))
                .ForMember(d => d.DateTime, opt => opt.MapFrom(x => x.Date))
                .ForMember(d => d.EndDateTime, opt => opt.MapFrom(x => x.Date.Add(x.Duration)))
                .ForMember(d => d.Services, opt => opt.MapFrom(x => x.Services));

            CreateMap<AuditoriumReserve, AuditoriumReserveDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(d => d.AuditoriumId, opt => opt.MapFrom(x => x.AuditoriumId))
                .ForMember(d => d.DateTime, opt => opt.MapFrom(x => x.DateTime))
                .ForMember(d => d.Duration, opt => opt.MapFrom(x => x.EndDateTime.Subtract(x.DateTime)));
        }
    }
}
