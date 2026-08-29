using Application.Dtos;
using Application.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles
{
    internal class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<ServiceModel, AuditoriumService>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AuditoriumId, opt => opt.Ignore())
                .ForMember(dest => dest.Auditorium, opt => opt.Ignore())
                .ForMember(dest => dest.Service, opt => opt.Ignore());

            CreateMap<AuditoriumServiceModel, AuditoriumReserveService>()
                .ForMember(dest => dest.AuditoriumServiceId, opt => opt.MapFrom(src => src.Id));


            CreateMap<AuditoriumService, AuditoriumServiceDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceId))
                .ForMember(dest => dest.AuditoriumId, opt => opt.MapFrom(src => src.AuditoriumId));
        }
    }
}
