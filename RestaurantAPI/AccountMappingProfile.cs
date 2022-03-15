using AutoMapper;
using RestaurantAPI.Models;
using RestaurantDAL.Entities;

namespace RestaurantAPI
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<User, ViewUserDto>()
                .ForMember(dest => dest.FirstName, m => m.MapFrom(m => m.FirstName))
                .ForMember(dest => dest.BirthDate, m => m.MapFrom(m => m.BirthDate))
                .ForMember(dest => dest.Nationality, m => m.MapFrom(m => m.Nationality));

            CreateMap<RegisterUserDto, User>();
        }
    }
}
