using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        public RestaurantDto GetById(int id);
        public PagedResult<RestaurantDto> GetAll(RestaurantQuery query);
        public void CreateRestaurant(CreateRestaurantDto dto);
        public void DeleteRestaurant(int id);
        public void UpdateRestaurant(int id,UpdateRestaurantDto dto);
    }
}
