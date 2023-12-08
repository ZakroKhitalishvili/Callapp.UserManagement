using Callapp.UserManagement.Application;
using Callapp.UserManagement.Application.Dtos;
using Callapp.UserManagement.Application.Dtos.Resources;
using Callapp.UserManagement.Application.Interfaces;
using Callapp.UserManagement.Application.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading;

namespace Callapp.UserManagement.Web.Routes;

public static class ResourcesRoutes
{
    public static IEndpointRouteBuilder MapResourceRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/{id}/posts", async (int id, [FromServices] IResourceService resourceService, CancellationToken cancellationToken) =>
        {
            var result = await resourceService.GetPostsAsync(cancellationToken);

            if (result.Successful)
            {
                var userPosts = result.Data!.Where(x => x.UserId == id).ToList();

                if (userPosts.Any())
                {
                    return Results.Ok(ServiceResult<List<PostDto>>.Success(userPosts));
                }
                else
                {
                    return Results.Ok(ServiceResult<List<PostDto>>.Error("Records not found"));
                }

            }

            return Results.Ok(result);
        });

        app.MapGet("/api/users/{id}/todos", async (int id, [FromServices] IResourceService resourceService, CancellationToken cancellationToken) =>
        {
            var result = await resourceService.GetTodosAsync(cancellationToken);

            if (result.Successful)
            {
                var userTodos = result.Data!.Where(x => x.UserId == id).ToList();

                if (userTodos.Any())
                {
                    return Results.Ok(ServiceResult<List<TodoDto>>.Success(userTodos));
                }
                else
                {
                    return Results.Ok(ServiceResult<List<TodoDto>>.Error("Records not found"));
                }

            }

            return Results.Ok(result);
        });

        app.MapGet("/api/users/{id}/albums", async (int id, [FromServices] IResourceService resourceService, CancellationToken cancellationToken) =>
        {
            var result = await resourceService.GetAlbumsAsync(cancellationToken);

            if (result.Successful)
            {
                var userAlbums = result.Data!.Where(x => x.UserId == id).ToList();

                if (userAlbums.Any())
                {
                    return Results.Ok(ServiceResult<List<AlbumDto>>.Success(userAlbums));
                }
                else
                {
                    return Results.Ok(ServiceResult<List<AlbumDto>>.Error("Records not found"));
                }

            }

            return Results.Ok(result);
        });


        return app;
    }
}
