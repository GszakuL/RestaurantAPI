using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using RestaurantAPI.Validators;
using RestaurantDAL.Entities;

namespace RestaurantAPI.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateRestaurantDto> _validatorCreate;
        private readonly IValidator<UpdateRestaurantDto> _validatorUpdate;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, IValidator<CreateRestaurantDto> validatorCreate, IValidator<UpdateRestaurantDto> validatorUpdate)
        {
            _validatorCreate = validatorCreate;
            _validatorUpdate = validatorUpdate;
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public RestaurantDto GetById(int id)
        {
            var restaurant = _dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .FirstOrDefault(x => x.Id == id);

            if (restaurant is null)
                throw new NotFoundException("There is no such restaurant");

            var result = _mapper.Map<RestaurantDto>(restaurant);
            return result;
        }

        public IEnumerable<RestaurantDto> GetAll(string searchPhase)
        {
            var restaurants = _dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .Where(r => searchPhase == null || (r.Name.ToLower().Contains(searchPhase.ToLower()) || r.Category.ToLower().Contains(searchPhase)))
                .ToList();

            var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);

            return restaurantsDto;
        }

        public int CreateRestaurant(CreateRestaurantDto dto)
        {
            var validationResult = _validatorCreate.Validate(dto);

            if (!validationResult.IsValid)
                throw new Exceptions.ValidationException("Invalid data: "+validationResult.Errors.ToString());

            var restaurant = _mapper.Map<Restaurant>(dto);
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();
            return restaurant.Id;
        }

        public void DeleteRestaurant(int id)
        {
            var restaurant = _dbContext.Restaurants.FirstOrDefault(x => x.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant Not Found");

            _dbContext.Remove(restaurant);
            _dbContext.SaveChanges();
        }

        public void UpdateRestaurant(int id, UpdateRestaurantDto updateDto)
        {
            var restaurant = _dbContext.Restaurants.FirstOrDefault(x => x.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var validationResult = _validatorUpdate.Validate(updateDto);

            if (!validationResult.IsValid)
                throw new Exceptions.ValidationException("Invalid data: " + validationResult.Errors.ToString());

            restaurant.Name = updateDto.Name;
            restaurant.Description = updateDto.Description;
            restaurant.HasDelivery = updateDto.HasDelivery;

            _dbContext.SaveChanges();
        }
    }
}
