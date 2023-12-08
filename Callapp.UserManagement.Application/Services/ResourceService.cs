using Callapp.UserManagement.Application.Dtos.Resources;
using Callapp.UserManagement.Application.Dtos.Users;
using Callapp.UserManagement.Application.Interfaces;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Callapp.UserManagement.Application.Services;

public class ResourceService : IResourceService
{
    private readonly HttpClient _httpClient;

    public ResourceService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("Resources");
    }

    public async Task<ServiceResult<IReadOnlyList<PostDto>>> GetPostsAsync(CancellationToken cancellationToken)
    {
        var postsTask = _httpClient.GetFromJsonAsync<List<PostDto>>("/posts", cancellationToken);

        var commentsTask = _httpClient.GetFromJsonAsync<List<CommentDto>>("/comments", cancellationToken);

        await Task.WhenAll([postsTask, commentsTask]);

        var posts = await postsTask;
        var comments = await commentsTask;

        if (posts is null || comments is null)
        {
            return ServiceResult<IReadOnlyList<PostDto>>.Error("Failed to load posts");
        }

        foreach (var post in posts)
        {
            post.Comments = comments.Where(x => x.PostId == post.Id).ToList();
        }

        return ServiceResult<IReadOnlyList<PostDto>>.Success(posts);
    }

    public async Task<ServiceResult<IReadOnlyList<TodoDto>>> GetTodosAsync(CancellationToken cancellationToken)
    {
        var todos = await _httpClient.GetFromJsonAsync<List<TodoDto>>("/todos", cancellationToken);

        if (todos is null)
        {
            return ServiceResult<IReadOnlyList<TodoDto>>.Error("Failed to load todos");
        }

        return ServiceResult<IReadOnlyList<TodoDto>>.Success(todos);
    }

    public async Task<ServiceResult<IReadOnlyList<AlbumDto>>> GetAlbumsAsync(CancellationToken cancellationToken)
    {
        var albums = await _httpClient.GetFromJsonAsync<List<AlbumDto>>("/albums", cancellationToken);

        if (albums is null)
        {
            return ServiceResult<IReadOnlyList<AlbumDto>>.Error("Failed to load albums");
        }

        return ServiceResult<IReadOnlyList<AlbumDto>>.Success(albums);
    }
}
