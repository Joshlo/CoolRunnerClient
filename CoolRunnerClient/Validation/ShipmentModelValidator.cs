using CRClient.Models;
using FluentValidation;

namespace CRClient.Validation
{
    public class ShipmentModelValidator : AbstractValidator<ShipmentModel>
    {
        public ShipmentModelValidator()
        {
            RuleFor(x => x.ReceiverName)
                .NotEmpty()
                .WithMessage("{PropertyName} can't be empty");

            RuleFor(x => x.ReceiverStreet1)
                .NotEmpty()
                .WithMessage("{PropertyName} can't be empty");

            RuleFor(x => x.ReceiverZipcode)
                .NotEmpty()
                .WithMessage("{PropertyName} can't be empty");

            RuleFor(x => x.ReceiverCity)
                .NotEmpty()
                .WithMessage("{PropertyName} can't be empty");

            RuleFor(x => x.ReceiverCountry)
                .NotEmpty()
                .WithMessage("{PropertyName} can't be empty")
                .Matches("[a-zæøåA-ZÆØÅ]{2}")
                .WithMessage("{PropertyName} must be 2 letter ISO code");

            RuleFor(x => x.SenderName)
                .NotEmpty()
                .WithMessage("{PropertyName} can't be empty");

            RuleFor(x => x.SenderStreet1)
                .NotEmpty()
                .WithMessage("{PropertyName} can't be empty");

            RuleFor(x => x.SenderZipcode)
                .NotEmpty()
                .WithMessage("{PropertyName} can't be empty");

            RuleFor(x => x.SenderCity)
                .NotEmpty()
                .WithMessage("{PropertyName} can't be empty");

            RuleFor(x => x.SenderCountry)
                .NotEmpty()
                .WithMessage("{PropertyName} can't be empty")
                .Matches("[a-zæøåA-ZÆØÅ]{2}")
                .WithMessage("{PropertyName} must be 2 letter ISO code");

            RuleFor(x => x.Description)
                .Length(0, 50)
                .WithMessage("{PropertyName} can't be longer than {MaxLength}");

            When(x => x.Insurance, () =>
            {
                RuleFor(x => x.InsuranceValue)
                    .GreaterThan(0)
                    .WithMessage("{PropertyName} must be greater than ZERO or set Insurance to false");
                RuleFor(x => x.InsuranceCurrency)
                    .NotEmpty()
                    .WithMessage("{PropertyName} must be declared to know which currency the insurance is in")
                    .Matches("[a-zA-Z]{3}")
                    .WithMessage("{PropertyName} is in a wrong format");
            });
        }
    }
}
