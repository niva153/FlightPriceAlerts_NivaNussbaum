
using AutoMapper;
using FlightPriceAlert.Application.DTOs;
using FlightPriceAlert.Domain.Models;

namespace FlightPriceAlert.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Alert, AlertResponseDto>();
            CreateMap<CreateAlertDto, Alert>();
            CreateMap<UpdateAlertDto, Alert>();
        }
    }
}
