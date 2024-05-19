using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace AspTemplate.API.Contracts.Users;

public record LoginUserRequest(
	[Required] string Email,
	[Required] string Password);

internal sealed class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Invalid email");

        RuleFor(x => x.Password)
            .MinimumLength(1)
            .WithMessage("Invalid password");
    }
}