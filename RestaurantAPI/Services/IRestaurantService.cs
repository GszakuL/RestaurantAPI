using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        public RestaurantDto GetById(int id);
        public IEnumerable<RestaurantDto> GetAll(string searchPhase);
        public int CreateRestaurant(CreateRestaurantDto dto);
        public void DeleteRestaurant(int id);
        public void UpdateRestaurant(int id,UpdateRestaurantDto dto);
    }
}
