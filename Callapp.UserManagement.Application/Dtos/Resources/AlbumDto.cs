using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Callapp.UserManagement.Application.Dtos.Resources;

public class AlbumDto
{
    public int UserId { get; init; }
    public int Id { get; init; }
    public string? Title { get; init; }
}
