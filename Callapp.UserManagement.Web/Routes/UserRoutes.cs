using Callapp.UserManagement.Application.Dtos.Users;
using Callapp.UserManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;

namespace Callapp.UserManagement.Web.Routes;

public static class UserRoutes
{
    public static IEndpointRouteBuilder MapUserRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users", async ([FromServices] IUserService userService, CancellationToken cancellationToken) =>
        {
            return await userService.GetAllAsync(cancellationToken);
        });

        app.MapGet("api/users/{id}", async (int id, [FromServices] IUserService userService, CancellationToken cancellationToken) =>
        {
            return await userService.GetAsync(id, cancellationToken);
        });

        app.MapPost("api/users", async ([FromBody] UserCreateDto userCreateDto, [FromServices] IUserService userService, CancellationToken cancellationToken) =>
        {
            return await userService.CreateAsync(userCreateDto, cancellationToken);
        });

        app.MapPut("api/users", async ( [FromBody] UserUpdateDto userUpdateDto, [FromServices] IUserService userService, CancellationToken cancellationToken) =>
        {
            return await userService.UpdateAsync(userUpdateDto, cancellationToken);
        });

        app.MapDelete("api/users/{id}", async (int id, [FromServices] IUserService userService, CancellationToken cancellationToken) =>
        {
            return await userService.DeleteAsync(id, cancellationToken);
        });

        return app;
    }
}
