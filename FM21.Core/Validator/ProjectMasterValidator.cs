using FluentValidation;
using FM21.Core.Model;
using Microsoft.Extensions.Localization;

namespace FM21.Core.Validator
{

    public class ProjectMasterValidator : AbstractValidator<ProjectMasterModel>
    {
        public ProjectMasterValidator(IStringLocalizer localizer)
        {
            RuleSet("New", () =>
            {
                RuleFor(x => x.CustomerId)
                .NotNull().WithMessage(localizer["msgCouldNotZero"].Value)
                .GreaterThan(0).WithMessage(localizer["msgMustBeGreaterThenZero"].Value);

                RuleFor(x => x.ProjectCode)
                .NotEmpty().WithMessage(localizer["msgCouldNotBeNullOrEmpty"].Value)
                .GreaterThan(0).WithMessage(localizer["msgMustBeGreaterThenZero"].Value);

                RuleFor(x => x.ProjectDescription)
                .NotEmpty().WithMessage(localizer["msgCouldNotBeNullOrEmpty"].Value);
            });

            RuleSet("Edit", () =>
            {
                RuleFor(x => x.ProjectId).
                NotNull().WithMessage(localizer["msgCouldNotZero"].Value)
                .GreaterThan(0).WithMessage(localizer["msgMustBeGreaterThenZero"].Value);

            });
        }
    }
}