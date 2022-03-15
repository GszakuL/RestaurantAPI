using FluentValidation;
using RestaurantAPI.Models;

namespace RestaurantAPI.Validators
{
    public class CreateRestaurantDtoValidator : AbstractValidator<CreateRestaurantDto>
    {
        public CreateRestaurantDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(25)
                .WithMessage("Must be 25 chars long");

            RuleFor(x => x.City)
                .NotNull()
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Street)
                .NotNull()
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
