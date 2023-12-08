using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Callapp.UserManagement.Application.Dtos.Users;

public class UserLoginDto
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}
