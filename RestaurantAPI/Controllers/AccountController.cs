using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using RestaurantDAL.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public ActionResult<RegisterUserDto> RegisterUser([FromBody] RegisterUserDto registerUser)
        {
            _accountService.RegisterUser(registerUser);
            return Ok(registerUser);
        }

        [HttpDelete("{userId}")]
        public ActionResult DeleteUser([FromRoute]int userId)
        {
            _accountService.DeleteUser(userId);
            return Ok("User has been deleted");
        }

        [HttpPut("{userId}")]
        public ActionResult UpdateUser([FromRoute]int userId, [FromBody] UpdateUserDto updateUserDto)
        {
            _accountService.UpdateUser(userId, updateUserDto);
            return Ok("User has been updated");
        }
        
        [HttpGet]
        public IEnumerable<ViewUserDto> DisplayUser()
        {
            var users = _accountService.DisplayUsers();
            return users;
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginDto dto)
        {
            string token = _accountService.GenerateJwt(dto);
            return Ok(token);
        }
            
            
    }
}
