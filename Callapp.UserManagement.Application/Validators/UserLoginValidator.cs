using Callapp.UserManagement.Application.Dtos.Users;
using FluentValidation;

namespace Callapp.UserManagement.Application.Validators;

public class UserLoginValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginValidator()
    {;
        RuleFor(u => u.Email).NotEmpty();
        RuleFor(u => u.Password).NotEmpty();
    }
}
