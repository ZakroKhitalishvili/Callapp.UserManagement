using Callapp.UserManagement.Application.Dtos.Users;
using FluentValidation;

namespace Callapp.UserManagement.Application.Validators;

public class UserUpdateValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateValidator()
    {
        RuleFor(u => u.Username).NotEmpty().MaximumLength(50);
        RuleFor(u => u.Email).NotEmpty().EmailAddress().MaximumLength(50);
        RuleFor(u => u.NewPassword).MinimumLength(6);
        RuleFor(u => u.ConfirmNewPassword).Matches(u => u.NewPassword);
        RuleFor(u => u.PersonalNumber).NotEmpty().Length(11);
        RuleFor(u => u.Firstname).NotEmpty().MaximumLength(50);
        RuleFor(u => u.Lastname).NotEmpty().MaximumLength(50);
        RuleFor(u => u.IsActive).NotEmpty();
    }
}
