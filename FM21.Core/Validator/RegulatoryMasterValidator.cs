using FluentValidation;
using FM21.Core.Model;
using Microsoft.Extensions.Localization;

namespace FM21.Core.Validator
{
    public class RegulatoryMasterValidator : AbstractValidator<RegulatoryModel>
    {
        public RegulatoryMasterValidator(IStringLocalizer localizer)
        {
            RuleSet("New", () =>
            {
                RuleFor(x => x.NutrientId)
                .NotNull().WithMessage(localizer["msgCouldNotZero"].Value)
                .GreaterThan(0).WithMessage(localizer["msgMustBeGreaterThenZero"].Value);

                RuleFor(x => x.Unit)
                .NotEmpty().WithMessage(localizer["msgCouldNotBeNullOrEmpty"].Value);
                RuleFor(x => x.UnitPerMg)
                .NotEmpty().WithMessage(localizer["msgCouldNotBeNullOrEmpty"].Value);
                RuleFor(x => x.NewUsRdi)
                .NotEmpty().WithMessage(localizer["msgCouldNotBeNullOrEmpty"].Value);
            });

            RuleSet("Edit", () =>
            {
                RuleFor(x => x.RegulatoryId).
                NotNull().WithMessage(localizer["msgCouldNotZero"].Value)
                .GreaterThan(0).WithMessage(localizer["msgMustBeGreaterThenZero"].Value);
            
            });
        }
    }
}