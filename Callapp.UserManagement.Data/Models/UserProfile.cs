using System;
using System.Collections.Generic;

namespace Callapp.UserManagement.Data.Models;

public class UserProfile
{
    public int Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string PersonalNumber { get; set; } = null!;

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
