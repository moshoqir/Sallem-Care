using FluentValidation;
using SaleemCare.Api.Dtos.Profile;
using System;
using System.Linq;

namespace SaleemCare.Api.Validation;

public class SaveProfileDtoValidator : AbstractValidator<SaveProfileDto>
{
    public SaveProfileDtoValidator()
    {
        RuleFor(x => x.Gender)
            .Must(g => g is null || new[] { "male", "female" }.Contains(g.ToLower()))
            .WithMessage("Invalid gender value.");

        RuleFor(x => x.Dob)
            .Must(d => !d.HasValue ||(d.Value <= DateOnly.FromDateTime(DateTime.Today) &&
            d.Value >= DateOnly.FromDateTime(DateTime.Today.AddYears(120) ) ) )
            .WithMessage("Date of birth must be in the past and not more than 120 years ago.");

        RuleFor(x => x.SurgicalHistory)
            .Must(list => list == null || list.All(item => !string.IsNullOrWhiteSpace(item) ))
            .WithMessage("Surgical history must not contain empty entries.");
    }
}