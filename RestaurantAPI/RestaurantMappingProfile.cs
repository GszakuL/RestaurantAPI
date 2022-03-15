using AutoMapper;
using RestaurantAPI.Models;
using RestaurantDAL.Entities;

namespace RestaurantAPI
{
    public class RestaurantMappingProfile : Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(dest => dest.City, m => m.MapFrom(source => source.Address.City))
                .ForMember(dest => dest.Street, m => m.MapFrom(source => source.Address.Street))
                .ForMember(dest => dest.PostalCode, m => m.MapFrom(source => source.Address.PostalCode));

            CreateMap<Dish, DishDto>();
            CreateMap<UpdateDishDto, Dish>();
            CreateMap<CreateDishDto, Dish>();

            CreateMap<CreateRestaurantDto, Restaurant>()
                .ForMember(r => r.Address,
                    c => c.MapFrom(dto => new Address() 
                        { City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street }));

            CreateMap<UpdateRestaurantDto, Restaurant>();
                
        }
    }
}
