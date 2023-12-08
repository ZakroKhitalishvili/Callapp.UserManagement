using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Callapp.UserManagement.Application.Dtos.Users;

public class UserCreateDto
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string ConfirmPassword { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Firstname { get; init; } = null!;
    public string Lastname { get; init; } = null!;
    public string PersonalNumber { get; init; } = null!;
}
