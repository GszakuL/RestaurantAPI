using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public interface IAccountService
    {
        public void RegisterUser(RegisterUserDto createUserDto);
        public void UpdateUser(int userId, UpdateUserDto updateUserDto);
        public void DeleteUser(int userId);
        public IEnumerable<ViewUserDto> DisplayUsers();
        public string GenerateJwt(LoginDto dto);
    }
}
