using FluentValidation;
using FM21.Core.Model;
using Microsoft.Extensions.Localization;

namespace FM21.Core.Validator
{
    public class AllergenMasterValidator : AbstractValidator<AllergenMasterModel>
    {
        public AllergenMasterValidator(IStringLocalizer localizer)
        {
            RuleSet("New", () =>
            {
                RuleFor(x => x.AllergenCode).NotEmpty().WithMessage(localizer["msgRequiredField"].Value).MaximumLength(500).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.AllergenName).NotEmpty().WithMessage(localizer["msgRequiredField"].Value).MaximumLength(500).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.AllergenDescription_En).NotEmpty().WithMessage(localizer["msgRequiredField"].Value).MaximumLength(500).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
            });

            RuleSet("Edit", () =>
            {
                RuleFor(x => x.AllergenID).NotNull().WithMessage(localizer["msgCouldNotZero"].Value)
                         .GreaterThan(0).WithMessage(localizer["msgMustBeGreaterThenZero"].Value);
            });
        }
    }
}