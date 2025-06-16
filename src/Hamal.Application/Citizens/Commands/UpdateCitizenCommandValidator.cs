using System.Text.RegularExpressions;
using FluentValidation;

namespace Hamal.Application.Citizens.Commands;

public class UpdateCitizenCommandValidator : AbstractValidator<UpdateCitizenCommand>
{
    // Regex to match Hebrew characters
    private const string HebrewLettersOnly = @"^[\p{IsHebrew}]+$";
    private const string HebrewLettersDigitsAndDash = @"^[\p{IsHebrew}0-9-]{3,}$";
    private const string DigitsAndOneHebrewLetter = @"^[0-9]*[\p{IsHebrew}]?[0-9]*$";

    public UpdateCitizenCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().Matches(HebrewLettersOnly)
            .WithMessage("First name must contain only Hebrew letters.");
        RuleFor(x => x.LastName).NotEmpty().Matches(HebrewLettersOnly)
            .WithMessage("Last name must contain only Hebrew letters.");
        RuleFor(x => x.FamilyNumber).GreaterThan(0);

        When(x => x.IsAddressWrong, () => {
            RuleFor(x => x.NewStreetName).NotEmpty().Matches(HebrewLettersDigitsAndDash)
                .WithMessage("New street name must be at least 3 characters and contain only Hebrew letters, digits, or dashes.");
            RuleFor(x => x.NewBuildingNumber).NotEmpty().Matches(DigitsAndOneHebrewLetter)
                 .WithMessage("New building number must contain only digits and at most one Hebrew letter.");
            RuleFor(x => x.NewFlatNumber).NotNull().InclusiveBetween(0, 499)
                 .WithMessage("New flat number must be between 0 and 499.");
        });
    }
} 