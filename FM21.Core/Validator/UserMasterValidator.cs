using FluentValidation;
using FM21.Core.Model;
using Microsoft.Extensions.Localization;

namespace FM21.Core.Validator
{
    public class UserMasterValidator : AbstractValidator<UserMasterModel>
    {
        public UserMasterValidator(IStringLocalizer localizer)
        {
            RuleSet("New", () =>
            {
                RuleFor(x => x.DomainFullName)
                            .NotEmpty().WithMessage(localizer["msgRequiredField"].Value)
                            .MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
            });

            RuleSet("Edit", () =>
            {

            });
        }
    }
}