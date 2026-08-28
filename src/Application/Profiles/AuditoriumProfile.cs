using Application.Dtos;
using Application.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles
{
    internal class AuditoriumProfile : Profile
    {
        public AuditoriumProfile()
        {
            CreateMap<AuditoriumModel, Auditorium>()
                .ForMember(d => d.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(d => d.Capacity, opt => opt.MapFrom(x => x.Capacity))
                .ForMember(d => d.BaseRentalPrice, opt => opt.MapFrom(x => x.BaseRentalPrice))
                .ForMember(d => d.AuditoriumServices, opt => opt.MapFrom(x => x.Services));

            CreateMap<Auditorium, AuditoriumDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(d => d.Capacity, opt => opt.MapFrom(x => x.Capacity))
                .ForMember(d => d.BaseRentalPrice, opt => opt.MapFrom(x => x.BaseRentalPrice))
                .ForMember(d => d.AuditoriumServices, opt => opt.MapFrom(x => x.AuditoriumServices));
        }
    }
}
