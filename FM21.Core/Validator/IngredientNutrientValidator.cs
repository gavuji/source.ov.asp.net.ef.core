using FluentValidation;
using FM21.Core.Model;
using Microsoft.Extensions.Localization;

namespace FM21.Core.Validator
{
    public class IngredientNutrientValidator : AbstractValidator<NutrientModel>
    {
        public IngredientNutrientValidator(IStringLocalizer localizer)
        {
            RuleSet("New", () =>
            {
                RuleFor(x => x.NutrientId)
                 .GreaterThan(0).WithMessage(localizer["msgMustBeGreaterThenZero"].Value);
                RuleFor(x => x.NutrientValue)
                .GreaterThanOrEqualTo(0).WithMessage(localizer["msgMustBeGreaterThanOrEqualToZero"].Value);
            });
        }
    }
}
