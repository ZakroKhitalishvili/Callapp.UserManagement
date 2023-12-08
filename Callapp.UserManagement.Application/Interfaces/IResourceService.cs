using Callapp.UserManagement.Application.Dtos.Resources;
using Callapp.UserManagement.Application.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Callapp.UserManagement.Application.Interfaces;

public interface IResourceService
{
    Task<ServiceResult<IReadOnlyList<AlbumDto>>> GetAlbumsAsync(CancellationToken cancellationToken);
    Task<ServiceResult<IReadOnlyList<PostDto>>> GetPostsAsync(CancellationToken cancellationToken);
    Task<ServiceResult<IReadOnlyList<TodoDto>>> GetTodosAsync(CancellationToken cancellationToken);
}
