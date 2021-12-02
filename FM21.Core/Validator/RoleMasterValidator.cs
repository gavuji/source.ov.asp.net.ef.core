using FluentValidation;
using FM21.Core.Model;
using Microsoft.Extensions.Localization;

namespace FM21.Core.Validator
{
    public class RoleMasterValidator : AbstractValidator<RoleMasterModel>
    {
        public RoleMasterValidator(IStringLocalizer localizer)
        {
            RuleSet("New", () =>
            {
                RuleFor(x => x.RoleName)
                            .NotEmpty().WithMessage(localizer["msgRequiredField"].Value)
                            .MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);

                RuleFor(x => x.RoleDescription).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
            });

            RuleSet("Edit", () =>
            {
                RuleFor(x => x.RoleID)
                        .NotNull().WithMessage(localizer["msgCouldNotZero"].Value)
                        .GreaterThan(0).WithMessage(localizer["msgMustBeGreaterThenZero"].Value);
            });
        }
    }
}