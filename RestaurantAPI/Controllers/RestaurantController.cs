using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using RestaurantAPI.Validators;
using RestaurantDAL.Entities;

namespace RestaurantAPI.Controllers
{
    
    [Route("api/restaurants")]
    [ApiController]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPost]
        //[Authorize(Roles = "Administrator,Manager")]
        [AllowAnonymous]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {    
            _restaurantService.CreateRestaurant(dto);
            return Ok("Restaurant created");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll([FromQuery]RestaurantQuery query)
        {
            var restaurants = _restaurantService.GetAll(query);
            return Ok(restaurants);
        }
        
        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<RestaurantDto> GetById([FromRoute] int id)
        {
            var restaurant = _restaurantService.GetById(id);

            return Ok(restaurant);
        }

        [HttpDelete("delete/{id}")]
        public ActionResult DeleteRestaurantById([FromRoute] int id)
        {
            _restaurantService.DeleteRestaurant(id);

            return NoContent();
        }

        [HttpPut("update/{id}")]
        public ActionResult UpdateRestaurantInformations([FromRoute] int id,[FromBody]UpdateRestaurantDto updateDto)
        {
            _restaurantService.UpdateRestaurant(id,updateDto);

             return Ok($"Restaurant with id:{id} has been updated");
        }
    }
}
