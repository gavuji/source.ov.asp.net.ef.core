using FluentValidation;
using FM21.Core.Model;
using Microsoft.Extensions.Localization;

namespace FM21.Core.Validator
{
    public class CustomerValidator : AbstractValidator<CustomerModel>
    {
        public CustomerValidator(IStringLocalizer localizer)
        {
            RuleSet("New", () =>
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["msgCouldNotBeNullOrEmpty"].Value)
                                    .MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);

                RuleFor(x => x.Address).NotEmpty().WithMessage(localizer["msgCouldNotBeNullOrEmpty"].Value)
                                    .MaximumLength(200).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);

                RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(localizer["msgCouldNotBeNullOrEmpty"].Value)
                                    .MaximumLength(20).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);

                RuleFor(x => x.Email).NotEmpty().WithMessage(localizer["msgCouldNotBeNullOrEmpty"].Value)
                                    .MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);

                RuleFor(x => x.CustomerAbbreviation1).NotEmpty().WithMessage(localizer["msgCouldNotBeNullOrEmpty"].Value)
                                    .MaximumLength(40).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);

                RuleFor(x => x.CustomerAbbreviation2).MaximumLength(40).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
            });

            RuleSet("Edit", () =>
            {
                RuleFor(x => x.CustomerId)
                                    .NotNull().WithMessage(localizer["msgCouldNotZero"].Value)
                                    .GreaterThan(0).WithMessage(localizer["msgMustBeGreaterThenZero"].Value);
              
            });
        }
    }
}