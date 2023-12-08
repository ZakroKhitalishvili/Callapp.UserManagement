using Callapp.UserManagement.Application.Dtos.Users;
using FluentValidation;

namespace Callapp.UserManagement.Application.Validators;

public class UserCreateValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateValidator()
    {
        RuleFor(u => u.Username).NotEmpty().MaximumLength(50);
        RuleFor(u => u.Email).NotEmpty().EmailAddress().MaximumLength(50);
        RuleFor(u => u.PersonalNumber).NotEmpty().Length(11);
        RuleFor(u => u.Password).NotEmpty().MinimumLength(6);
        RuleFor(u => u.ConfirmPassword).NotEmpty().Matches(u => u.Password);
        RuleFor(u => u.Firstname).NotEmpty().MaximumLength(50);
        RuleFor(u => u.Lastname).NotEmpty().MaximumLength(50);
    }
}
