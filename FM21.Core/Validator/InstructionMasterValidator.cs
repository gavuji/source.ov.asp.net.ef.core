using FluentValidation;
using FM21.Core.Model;
using Microsoft.Extensions.Localization;

namespace FM21.Core.Validator
{
    public class InstructionMasterValidator : AbstractValidator<InstructionMasterModel>
    {
        public InstructionMasterValidator(IStringLocalizer localizer)
        {
            RuleSet("New", () =>
            {
                RuleFor(x => x.SiteProductMapID)
                        .NotNull().WithMessage(localizer["msgCouldNotZero"].Value)
                        .GreaterThan(0).WithMessage(localizer["msgMustBeGreaterThenZero"].Value);

                RuleFor(x => x.InstructionCategoryID)
                        .NotNull().WithMessage(localizer["msgCouldNotZero"].Value)
                        .GreaterThan(0).WithMessage(localizer["msgMustBeGreaterThenZero"].Value);

                RuleFor(x => x.InstructionGroupID)
                       .NotNull().WithMessage(localizer["msgCouldNotZero"].Value)
                       .GreaterThan(0).WithMessage(localizer["msgMustBeGreaterThenZero"].Value);

                RuleFor(x => x.DescriptionEn)
                    .NotEmpty().WithMessage(localizer["msgRequiredField"].Value)
                    .MaximumLength(150).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);

                RuleFor(x => x.DescriptionFr)
                    //.NotEmpty().WithMessage(localizer["msgRequiredField"].Value)
                    .MaximumLength(150).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);

                RuleFor(x => x.DescriptionEs)
                    //.NotEmpty().WithMessage(localizer["msgRequiredField"].Value)
                    .MaximumLength(150).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
            });

            RuleSet("Edit", () =>
            {
                RuleFor(x => x.InstructionMasterID)
                        .NotNull().WithMessage(localizer["msgCouldNotZero"].Value)
                        .GreaterThan(0).WithMessage(localizer["msgMustBeGreaterThenZero"].Value);
            });
        }
    }
}