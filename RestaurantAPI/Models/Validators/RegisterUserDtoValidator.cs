using FluentValidation;
using RestaurantDAL.Entities;

namespace RestaurantAPI.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(RestaurantDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.PasswordHash)
                .MinimumLength(6);

            RuleFor(x => x.ConfirmPassword)
                .NotNull()
                .NotEmpty()
                .Equal(x => x.PasswordHash);

            RuleFor(x => x.Email)
                .Custom((value, contex) =>
                {
                    var emailInUse = dbContext.Users.Any(u => u.Email == value);

                    if (emailInUse)
                        contex.AddFailure("Email", "Already in use");
                });
        }
    }
}
