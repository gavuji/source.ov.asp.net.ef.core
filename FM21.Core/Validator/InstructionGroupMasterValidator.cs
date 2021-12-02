using FluentValidation;
using FM21.Core.Model;
using Microsoft.Extensions.Localization;

namespace FM21.Core.Validator
{
    public class InstructionGroupMasterValidator : AbstractValidator<InstructionGroupMasterModel>
    {
        public InstructionGroupMasterValidator(IStringLocalizer localizer)
        {
            RuleSet("New", () =>
            {
                RuleFor(x => x.InstructionGroupName)
                    .NotEmpty().WithMessage(localizer["msgRequiredField"].Value)
                    .MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
            });

            RuleSet("Edit", () =>
            {
                RuleFor(x => x.InstructionGroupID)
                        .NotNull().WithMessage(localizer["msgCouldNotZero"].Value)
                        .GreaterThan(0).WithMessage(localizer["msgMustBeGreaterThenZero"].Value);
            });
        }
    }
}