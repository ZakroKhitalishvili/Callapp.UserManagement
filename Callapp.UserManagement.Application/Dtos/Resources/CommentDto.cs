using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Callapp.UserManagement.Application.Dtos.Resources;

public class CommentDto
{
    public int PostId { get; init; }
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? Body { get; init; }
}
