using FluentValidation;
using FM21.Core.Model;
using Microsoft.Extensions.Localization;

namespace FM21.Core.Validator
{
    public class IngredientMasterValidator : AbstractValidator<IngredientMasterModel>
    {
        public IngredientMasterValidator(IStringLocalizer localizer)
        {
            RuleSet("New", () =>
            {
                When(x => !string.IsNullOrWhiteSpace(x.ONTResearchCode), () => {
                    RuleFor(x => x.ONTResearchCode)
                        .MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value)
                        .Matches(@"J\d{4}|K\d{4}|V\d{4}|RF\d{4}").WithMessage(localizer["msgInvalidCodeFormat"].Value);
                });

                When(x => !string.IsNullOrWhiteSpace(x.JDECode), () => {
                    RuleFor(x => x.JDECode)
                        .MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value)
                        .Matches(@"R\d{5}").WithMessage(localizer["msgInvalidCodeFormat"].Value);
                });

                When(x => !string.IsNullOrWhiteSpace(x.S30SubAssemblyCode), () => {
                    RuleFor(x => x.S30SubAssemblyCode)
                        .MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value)
                        .Matches(@"S30\d{5}").WithMessage(localizer["msgInvalidCodeFormat"].Value);
                });

                RuleFor(x => x.IngredientCategoryID)
                        .NotNull().WithMessage(localizer["msgCouldNotZero"].Value)
                        .GreaterThan(0).WithMessage(localizer["msgMustBeGreaterThenZero"].Value);

                RuleFor(x => x.IRWPart).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.S30SubAssemblyCode).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.JDECode).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.IngredientUsed).MaximumLength(30).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.RMDescription).MaximumLength(30).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.OriginCountry).MaximumLength(200).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.InternalXReference).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.ExternalXReference).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.OptimumStorageCondition).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.AltCode).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.NutrientLink).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.StorageCode).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.PreConditionCode).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.IPPBagSize).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.AlertReview).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.UsageAlert).MaximumLength(150).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.ExclusivityAlert).MaximumLength(200).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.AlertCustomerAbbr).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.Vegetarian).MaximumLength(10).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.Vegan).MaximumLength(10).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.RSPOCertificateNumber).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.GMOStatus).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.CFRReferenceNo).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.GRASNo).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.HTSNo).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.ShakleeCode).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.CASNumber).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.Biological).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.Chemical).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.Physical).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.ControlMechanismBiological).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.ControlMechanismPhysical).MaximumLength(300).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.ControlMachanismChemical).MaximumLength(300).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.Micro).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.ForeignMatter).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.TGApproval).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.PackageOption).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.SupplierPackage).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.ProcurementDetail).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.HandSort).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.UsageBarLimit).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.UsagePowderLimit).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.UsageCanadaLimit).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.Viscosity).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.Microns).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.ActiveNutrient).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.NutrientActivity).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.ClaimAmountUnit).MaximumLength(10).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.UsageRangeLimit).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.Pro65Chemical).MaximumLength(1000).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.GeneralRMDescription).MaximumLength(100).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
                RuleFor(x => x.SortingPart).MaximumLength(50).WithMessage(localizer["msgMustBeLessThenOrEqual"].Value);
            });

            RuleSet("Edit", () =>
            {
                RuleFor(x => x.IngredientID)
                        .NotNull().WithMessage(localizer["msgCouldNotZero"].Value)
                        .GreaterThan(0).WithMessage(localizer["msgMustBeGreaterThenZero"].Value);
            });
        }
    }
}