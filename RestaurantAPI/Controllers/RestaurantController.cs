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
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Manager")]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {    
            var id = _restaurantService.CreateRestaurant(dto);
            return Ok("Restaurant created");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Restaurant>> GetAll([FromQuery]string searchPhase)
        {
            var restaurants = _restaurantService.GetAll(searchPhase);
            return Ok(restaurants);
        }
        
        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<Restaurant> GetById([FromRoute] int id)
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
