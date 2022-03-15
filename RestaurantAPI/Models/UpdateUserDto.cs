namespace RestaurantAPI.Models
{
    public class UpdateUserDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Nationality { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
    }
}
