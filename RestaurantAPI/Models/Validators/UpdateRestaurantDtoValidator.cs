using FluentValidation;
using RestaurantAPI.Models;

namespace RestaurantAPI.Validators
{
    public class UpdateRestaurantDtoValidator : AbstractValidator<UpdateRestaurantDto>
    {
        public UpdateRestaurantDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(25)
                .WithMessage("Must be 25 chars long");
        }
    }
}
