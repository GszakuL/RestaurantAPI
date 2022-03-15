using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IDishService
    {
        public IEnumerable<DishDto> GetAllDishesFromRestaurant(int restaurantId);
        public DishDto GetDishByIdFromRestaurant(int restaurantId, int dishId);
        public void AddDishToRestaurant(int restaurantId, CreateDishDto dto);
        public void RemoveDishFromRestaurant(int restaurantId, int dishId);
        public void UpdateDishFromRestaurant(int restaurantId, int dishId, UpdateDishDto updateDishDto);
    }
}
