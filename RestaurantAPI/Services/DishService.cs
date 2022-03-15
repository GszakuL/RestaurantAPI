using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using RestaurantDAL.Entities;

namespace RestaurantAPI.Services
{
    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;

        public DishService(RestaurantDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        private Restaurant GetRestaurant(int restaurantId)
        {
            var restaurant = _dbContext
                            .Restaurants
                            .Include(d => d.Dishes)
                            .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            return restaurant;
        }

        public void AddDishToRestaurant(int restaurantId, CreateDishDto dto)
        {
            var restaurant = GetRestaurant(restaurantId);
            var newDish = _mapper.Map<Dish>(dto);

            restaurant.Dishes.Add(newDish);
            _dbContext.Add(newDish);
            _dbContext.SaveChanges();
        }

        public IEnumerable<DishDto> GetAllDishesFromRestaurant(int restaurantId)
        {
            var restaurant = GetRestaurant(restaurantId);
            var dishes = restaurant.Dishes.ToList();
            var dishesDto = _mapper.Map<List<DishDto>>(dishes);

            if (dishes is null)
                throw new NotFoundException("Dish not found");

            return dishesDto;
        }

        public DishDto GetDishByIdFromRestaurant(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurant(restaurantId);

            var dish = restaurant
                       .Dishes
                       .FirstOrDefault(d => d.Id == dishId);

            if (dish is null)
                throw new NotFoundException("Dish not found");

            var dishDto = _mapper.Map<DishDto>(dish); 

            return dishDto;
        }

        public void RemoveDishFromRestaurant(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurant(restaurantId);
            var dish = _dbContext.Dishes.FirstOrDefault(dish => dish.Id == dishId);

            if (dish is null)
                throw new NotFoundException("Dish not found");

            _dbContext.Remove(dish);
            _dbContext.SaveChanges();
        }

        public void UpdateDishFromRestaurant(int restaurantId, int dishId, UpdateDishDto updateDishDto)
        {
            var restaurant = GetRestaurant(restaurantId);
            var dishToUpdate = restaurant.Dishes.FirstOrDefault(dish => dish.Id == dishId);

            if (dishToUpdate is null)
                throw new NotFoundException("Dish not found");
            
            dishToUpdate.Name = updateDishDto.Name;
            dishToUpdate.Description = updateDishDto.Description;
            dishToUpdate.Price = updateDishDto.Price;

            _dbContext.SaveChanges();
        }
    }
}
