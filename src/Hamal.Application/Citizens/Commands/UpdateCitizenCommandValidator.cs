using System.Text.RegularExpressions;
using FluentValidation;

namespace Hamal.Application.Citizens.Commands;

public class UpdateCitizenCommandValidator : AbstractValidator<UpdateCitizenCommand>
{
    // Hebrew letters, spaces, apostrophes, and dashes only
    private const string HebrewLettersOnly = @"^[\p{IsHebrew}'\- ]+$";

    // Hebrew letters, digits, dashes, apostrophes, and spaces — minimum 3 characters
    private const string HebrewLettersDigitsAndDash = @"^[\p{IsHebrew}0-9'\- ]{3,}$";

    // Digits with optional one Hebrew letter, allow optional spaces
    private const string DigitsAndOneHebrewLetter = @"^[0-9\s]*[\p{IsHebrew}]?[0-9\s]*$";
    
    private const string DigitsOnly = @"^\d+$";

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
            RuleFor(x => x.NewFlatNumber).NotEmpty().Matches(DigitsOnly)
                 .WithMessage("New flat number must contain only digits.");
        });
    }
} 