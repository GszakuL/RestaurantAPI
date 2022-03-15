using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using RestaurantDAL.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurants/{restaurantId}/dishes")]
    public class DishController : ControllerBase
    {
        private IDishService _service;

        public DishController(IDishService service)
        {
            _service = service;
        }
        [HttpGet]
        public ActionResult<IEnumerable<DishDto>> GetAllDishesFromRestaurant([FromRoute]int restaurantId)
        {
            var dishes = _service.GetAllDishesFromRestaurant(restaurantId);
            return Ok(dishes);
        }

        [HttpGet("{dishId}")]
        public ActionResult<DishDto> GetDish([FromRoute]int restaurantId, [FromRoute]int dishId)
        {
            var dish = _service.GetDishByIdFromRestaurant(restaurantId,dishId);
            return Ok(dish);
        }

        [HttpPost]
        public ActionResult<CreateDishDto> AddDishToRestaurant([FromRoute]int restaurantId, [FromBody]CreateDishDto dto)
        {
            _service.AddDishToRestaurant(restaurantId,dto);
            return Ok(dto);
        }
        
        [HttpDelete("{dishId}")]
        public ActionResult DeleteDishFromRestaurant([FromRoute]int restaurantId, [FromRoute] int dishId)
        {
            _service.RemoveDishFromRestaurant(restaurantId,dishId);
            return Ok("Dish deleted");
        }
        
        [HttpPut("{dishId}")]
        public ActionResult<UpdateDishDto> UpdateDishFromRestaurant([FromRoute]int restaurantId, [FromRoute]int dishId, [FromBody]UpdateDishDto dto)
        {
            _service.UpdateDishFromRestaurant(restaurantId,dishId,dto);
            return Ok(dto);
        }
    }
}
