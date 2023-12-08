using System;
using System.Collections.Generic;

namespace Callapp.UserManagement.Data.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual UserProfile UserProfile { get; set; } = null!;
}
