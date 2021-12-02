using FluentValidation;
using FM21.Core.Model;
using Microsoft.Extensions.Localization;

namespace FM21.Core.Validator
{
    public class FormulaMasterValidator : AbstractValidator<FormulaMasterModel>
    {
        public FormulaMasterValidator(IStringLocalizer localizer)
        {
            RuleSet("New", () =>
            {
                RuleFor(x => x.FormulaTag).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.FormulaReference).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.AltCode).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.CustomerCode).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.AllergenCode).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.Preconditioning).MaximumLength(500).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.BrixRange).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.SyrupAw).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.SyrupPh).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.AltCode).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.FinalDoughTemperature).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.ReleaseAgent).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.PKOPercentage).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.USMavMin).MaximumLength(20).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.USMavMax).MaximumLength(20).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.CanadaMavMin).MaximumLength(20).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.CanadaMavMax).MaximumLength(20).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.BarformatEquipment).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.BarFormatForm).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.BarFormatCore).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.BarFormatTop).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.BarFormatDescription).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.BarFormatTopping).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.BarFormatLocation).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.BarFormatCoating).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.BarFormatDrizzle).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.BarFormatWoody).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.DieNumber).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
            });

            RuleSet("Edit", () =>
            {
                RuleFor(x => x.FormulaID)
                        .NotNull().WithMessage(localizer["msgCouldNotZero"].Value)
                        .GreaterThan(0).WithMessage(localizer["msgMustBeGreaterThenZero"].Value);
            });
        }
    }
}