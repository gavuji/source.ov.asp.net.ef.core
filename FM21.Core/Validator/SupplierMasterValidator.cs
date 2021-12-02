using FluentValidation;
using FM21.Core.Model;
using Microsoft.Extensions.Localization;

namespace FM21.Core.Validator
{
    public class SupplierMasterValidator : AbstractValidator<SupplierMasterModel>
    {
        public SupplierMasterValidator(IStringLocalizer localizer)
        {
            RuleSet("New", () =>
            {
                RuleFor(x => x.SupplierName).NotEmpty().WithMessage(localizer["msgRequiredField"].Value).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
            });

            RuleSet("Edit", () =>
            {
                RuleFor(x => x.SupplierId).NotNull().WithMessage(localizer["msgCouldNotZero"].Value)
                         .GreaterThan(0).WithMessage(localizer["msgMustBeGreaterThenZero"].Value);

            });
        }
    }
}
